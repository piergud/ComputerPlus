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
        readonly ControllerButtons ControllerButton;
        readonly Keys Key;
        readonly GameControl GameControlArg;
        readonly bool UseRageKeyPressBuffers;
        KeyBinderInput Input
        {
            get
            {
                return GameControlArg != GameControl.InteractionMenu ? KeyBinderInput.Game :
                    Key != Keys.None ? KeyBinderInput.Keyboard : KeyBinderInput.Controller;
            }
        }
        internal bool WasTriggeredOnce;
        internal bool IsPressed
        {
            get
            {
                bool result;
                if (Input == KeyBinderInput.Game)
                    result = UseRageKeyPressBuffers ? Game.IsControlPressed(0, GameControlArg) : Game.IsControlJustPressed(0, GameControlArg);
                //else if (Input == KeyBinderInput.Controller && !Game.IsControllerConnected)
                //    result = false;
                else if (UseRageKeyPressBuffers)
                    result = Input == KeyBinderInput.Keyboard ? Game.IsKeyDown(Key) : Game.IsControllerButtonDown(ControllerButton);
                else
                    result = Input == KeyBinderInput.Keyboard ? Game.IsKeyDownRightNow(Key) : Game.IsControllerButtonDownRightNow(ControllerButton);
                if (!WasTriggeredOnce && result) WasTriggeredOnce = true;
                return result;
            }
        }
        private KeyBinder(bool useRageKeyPressBuffers = true, GameControl gameControl = GameControl.InteractionMenu)
        {
            UseRageKeyPressBuffers = useRageKeyPressBuffers;
            GameControlArg = gameControl;
        }
        internal KeyBinder(Keys key, bool useRageKeyPressBuffers = true) : this(useRageKeyPressBuffers)
        {
            ControllerButton = ControllerButtons.None;
            Key = key;
        }
        internal KeyBinder(ControllerButtons controllerButton, bool useRageKeyPressBuffers = true) : this(useRageKeyPressBuffers)
        {
            Key = Keys.None;
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
      
        public void AddMonitorKeyBinding(Keys key)
        {
            BoundKeys.Add(new KeyBinder(key));
        }

        public void AddMonitorControllerBinding(ControllerButtons button)
        {
            BoundKeys.Add(new KeyBinder(button));
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
