using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rage;

namespace ComputerPlus.Interfaces
{
    interface IKeyBinder
    {
        void AddMonitorKeyBinding(Keys key);
        void AddMonitorControllerBinding(ControllerButtons button);
        void StartEventMonitoring();
        void StopEventMonitoring();
    }
    enum KeyBinderInput { Controller, Keyboard, Game };
    internal class KeyBinder
    {
        internal readonly ControllerButtons ControllerButton;
        internal readonly ControllerButtons ModifierControllerButton;
        internal readonly Keys Key;
        internal readonly Keys ModifierKey;
        internal readonly GameControl GameControlArg;
        readonly bool UseRageKeyPressBuffers;
        internal KeyBinderInput Input
        {
            get
            {
                return GameControlArg != GameControl.InteractionMenu ? KeyBinderInput.Game :
                    Key != Keys.None ? KeyBinderInput.Keyboard : KeyBinderInput.Controller;
            }
        }
        internal bool HasModifier
        {
            get
            {
                var input = Input;
                switch(input)
                {
                    case KeyBinderInput.Keyboard:
                        return ModifierKey != Keys.None;
                    case KeyBinderInput.Controller:
                        return ModifierControllerButton != ControllerButtons.None;
                    default: return false;
                }
             }
        }

        internal String FriendlyName
        {
            get
            {
                switch(Input)
                {                    
                    case KeyBinderInput.Game:
                        return String.Format("~INPUT_{0}~", GameControlArg.ToString().ToUpper());
                    case KeyBinderInput.Controller:
                        return HasModifier ? String.Format("~r~~h~{0}~h~ ~s~+ ~r~~h~{1}~h~~s~", ControllerButton.ToString(), ModifierControllerButton.ToString()) : ControllerButton.ToString();
                    case KeyBinderInput.Keyboard:
                        return HasModifier ? String.Format("{0} + {1}", FriendlyKeys.GetFriendlyName(Key), FriendlyKeys.GetFriendlyName(ModifierKey)) : FriendlyKeys.GetFriendlyName(Key);
                    default:
                        return String.Empty;
                }
            }
        }
        
        internal bool WasTriggeredOnce;
        internal bool IsPressed
        {
            get
            {
                bool result;
                bool hasModifier = HasModifier;
                if (Input == KeyBinderInput.Game)
                {
                    result = UseRageKeyPressBuffers ? Game.IsControlPressed(0, GameControlArg) : Game.IsControlJustPressed(0, GameControlArg);
                }
                else if (UseRageKeyPressBuffers)
                {
                    if (hasModifier)
                        result = Input == KeyBinderInput.Keyboard ? Game.IsKeyDownRightNow(ModifierKey) && Game.IsKeyDownRightNow(Key) : Game.IsControllerButtonDownRightNow(ModifierControllerButton) && Game.IsControllerButtonDownRightNow(ControllerButton);
                    else
                    result = Input == KeyBinderInput.Keyboard ? Game.IsKeyDown(Key) : Game.IsControllerButtonDown(ControllerButton);
                }
                else
                {
                    if (hasModifier)
                        result = Input == KeyBinderInput.Keyboard ? Game.IsKeyDownRightNow(ModifierKey) && Game.IsKeyDownRightNow(Key) : Game.IsControllerButtonDownRightNow(ModifierControllerButton) && Game.IsControllerButtonDownRightNow(ControllerButton);
                    else
                    result = Input == KeyBinderInput.Keyboard ? Game.IsKeyDownRightNow(Key) : Game.IsControllerButtonDownRightNow(ControllerButton);
                }
                if (!WasTriggeredOnce && result) WasTriggeredOnce = true;
                return result;
            }
        }
        private KeyBinder(bool useRageKeyPressBuffers = true, GameControl gameControl = GameControl.InteractionMenu)
        {
            UseRageKeyPressBuffers = useRageKeyPressBuffers;
            GameControlArg = gameControl;
        }
        internal KeyBinder(Keys key, Keys modifierKey = Keys.None, bool useRageKeyPressBuffers = true) : this(useRageKeyPressBuffers)
        {
            ControllerButton = ControllerButtons.None;
            ModifierKey = modifierKey;
            Key = key;
        }
        internal KeyBinder(ControllerButtons controllerButton, ControllerButtons modifierButton = ControllerButtons.None, bool useRageKeyPressBuffers = true) : this(useRageKeyPressBuffers)
        {
            Key = Keys.None;
            ModifierControllerButton = modifierButton;
            ControllerButton = controllerButton;
        }
        internal KeyBinder(GameControl control, bool useRageKeyPressBuffers = true) : this(useRageKeyPressBuffers, control)
        {
            Key = Keys.None;
            ControllerButton = ControllerButtons.None;
        }
    }
    class KeyBinderMonitor : IKeyBinder
    {
        bool FiberCanRun = true;
        int? MaxEvents;
        int EventsTriggered;
        GameFiber KeyListenerFiber;
        List<KeyBinder> BoundKeys;
        OnInputPressed EventTriggeredCallback;
        KeyBinderMonitor()
        {
            KeyListenerFiber = new GameFiber(CheckForBinderEvents);
            BoundKeys = new List<KeyBinder>();
        }

        KeyBinderMonitor(List<KeyBinder> binders)
        {
            BoundKeys = binders;
        }

        KeyBinderMonitor(List<KeyBinder> binders, OnInputPressed callback, int? maxEvents) : this(binders)
        {
            EventTriggeredCallback = callback;
            this.MaxEvents = maxEvents;
        }

        ~KeyBinderMonitor()
        {
            StopEventMonitoring();
        }
        void CheckForBinderEvents()
        {
            while (FiberCanRun)
            {
                if (EventTriggeredCallback != null && BoundKeys.Any(x => x.IsPressed))
                {
                    if (MaxEvents.HasValue)
                        EventsTriggered += 1;
                    EventTriggeredCallback();
                }
                GameFiber.Yield();
                if (MaxEvents.HasValue && MaxEvents.Value <= EventsTriggered)
                    GameFiber.Hibernate();
            }

        }
        public void StopEventMonitoring()
        {
            if (KeyListenerFiber == null) return;
            try
            {
                KeyListenerFiber.Abort();
            }
            catch
            {
                KeyListenerFiber = null;
            }
        }
        public void StartEventMonitoring()
        {
            if (FiberCanRun)
            {
                if (KeyListenerFiber.IsHibernating) KeyListenerFiber.Wake();
                else KeyListenerFiber.Start();
            }
            else if (KeyListenerFiber != null)
            {
                StopEventMonitoring();   
            }
        }

        public void AddMonitorBoundKey(KeyBinder binder)
        {
            BoundKeys.Add(binder);
        }
      
        public void AddMonitorKeyBinding(Keys key)
        {
            AddMonitorKeyBinding(key, Keys.None);
        }

        public void AddMonitorControllerBinding(ControllerButtons button)
        {
            AddMonitorControllerBinding(button, ControllerButtons.None);
        }

        public void AddMonitorKeyBinding(Keys key, Keys modifier = Keys.None)
        {
            BoundKeys.Add(new KeyBinder(key, modifier));
        }

        public void AddMonitorControllerBinding(ControllerButtons button, ControllerButtons modifier = ControllerButtons.None)
        {
            BoundKeys.Add(new KeyBinder(button, modifier));
        }

        internal static GameFiber MonitorForBinderListFactory(List<KeyBinder> binders, OnInputPressed callback, int? maxEvents)
        {

            var monitor = CreateNew(binders, callback, maxEvents);
            return new GameFiber(monitor.StartEventMonitoring);
        }

        internal static KeyBinderMonitor CreateNew(List<KeyBinder> binders, OnInputPressed callback, int? maxEvents)
        {
            return new KeyBinderMonitor(binders, callback, maxEvents);
        }

        internal static KeyBinderMonitor CreateNew(KeyBinder binder, OnInputPressed callback, int? maxEvents)
        {
            return new KeyBinderMonitor(new List<KeyBinder>() { binder }, callback, maxEvents);
        }
    }
    internal delegate void OnInputPressed();
    internal delegate void StopRequested(object sender);
   
}
