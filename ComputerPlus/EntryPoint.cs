using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Rage;
using Rage.Forms;
using LSPD_First_Response.Mod.API;
using ComputerPlus.Interfaces;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Interfaces.ComputerVehDB;
using ComputerPlus.Controllers.Models;
using ComputerPlus.Controllers;
using ComputerPlus.Extensions.Rage;
using System.Linq;
using ComputerPlus.Extensions.Gwen;
using System.Windows.Forms;
using ComputerPlus.Interfaces.SimpleNotepad;

namespace ComputerPlus
{
    public sealed class EntryPoint : Plugin
    {
        internal delegate void VehicleStoppedEvent(object sender, Vehicle veh);
        internal static VehicleStoppedEvent OnVehicleStopped;
        static Stopwatch sw = new Stopwatch();

        private static bool _prompted;
        internal static bool HasBackground
        {
            get;
            private set;
        } = false;

        internal bool IsMainComputerOpen = false;

        internal static List<string> recent_text = new List<string>();
        internal GameFiber CheckIfCalloutActiveFiber;
        private GameFiber DetectOpenCloseRequestedFiber;
        private GameFiber DetectOpenSimpleNotepadFiber;
        private GameFiber RunComputerPlusFiber;


        private static KeyBinder CloseComputerPlusWindow;
        private KeyBinder OpenSimpleNotepad;
        private static KeyBinder OpenCloseComputerPlusBinder;


        public override void Initialize()
        {
            Function.Log("Computer+ is loading");
            DetectOpenCloseRequestedFiber = new GameFiber(CheckToggleComputer);
            RunComputerPlusFiber = new GameFiber(RunPoliceComputer);
            CheckIfCalloutActiveFiber = new GameFiber(CheckIfCalloutActive);
            DetectOpenSimpleNotepadFiber = new GameFiber(CheckOpenSimpleNotepad);
            Functions.OnOnDutyStateChanged += DutyStateChangedHandler;
            OnVehicleStopped += VehicleStoppedHandler;
            Globals.Navigation.OnFormAdded += NavOnFormAdded;
            Globals.Navigation.OnFormRemoved += NavOnFormRemoved;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
            Configs.RunConfigCheck();
            if (Game.IsControllerConnected)
                CloseComputerPlusWindow = new KeyBinder(ControllerButtons.X);
            else
                CloseComputerPlusWindow = new KeyBinder(Keys.PageDown);
            OpenCloseComputerPlusBinder = new KeyBinder(GameControl.Context);
            OpenSimpleNotepad = new KeyBinder(Keys.End);
            Function.checkForRageVersionClass.checkForRageVersion(0.41f);

        }

        public override void Finally()
        {
            Globals.Navigation.OnFormAdded -= NavOnFormAdded;
            Globals.Navigation.OnFormRemoved -= NavOnFormRemoved;
            if (RunComputerPlusFiber.IsRunning()) RunComputerPlusFiber.Abort();
            if (CheckIfCalloutActiveFiber.IsRunning()) CheckIfCalloutActiveFiber.Abort();
        }


        private void DutyStateChangedHandler(bool on_duty)
        {
            Globals.IsPlayerOnDuty = on_duty;

            if (on_duty)
            {                               
                Function.MonitorAICalls();
                Function.CheckForUpdates();
                CheckIfCalloutActiveFiber.Resume();
                DetectOpenSimpleNotepadFiber.Resume();
                DetectOpenCloseRequestedFiber.Resume();
                Function.LogDebug("Successfully loaded LSPDFR Computer+.");

                if (Function.IsAlprPlusRunning())
                {
                    Function.LogDebug("C+: Registering for ALPR+ Events");
                    ALPRPlusFunctions.OnAlprPlusMessage += ALPRPlusFunctions_OnAlprPlusMessage;
                    ALPRPlusFunctions.RegisterForEvents();
                }
                else {
                    Function.LogDebug("C+: ALPR+ Not Detected");
                }
            }
            else
            {
                if (Function.IsAlprPlusRunning())
                {
                    ALPRPlusFunctions.OnAlprPlusMessage -= ALPRPlusFunctions_OnAlprPlusMessage;
                }
            }
        }


        private static void ALPRPlusFunctions_OnAlprPlusMessage(object sender, ALPR_Arguments e)
        {
            ComputerVehicleController.AddAlprScan(e);
        }

        private static void VehicleStoppedHandler(object sender, Vehicle veh)
        {
            if (veh && !_prompted)
            {
                if (Function.IsPoliceVehicle(veh)
                    && LSPD_First_Response.Mod.API.Functions.GetCurrentPullover() != null)
                {
                    Game.DisplayHelp("Hold ~INPUT_CONTEXT~ to open ~b~LSPDFR Computer+~w~.");
                    _prompted = true;
                }
            }
        }

        internal static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }


        private bool CheckOpenCloseDebouncedTrigger(KeyBinder binder)
        {
            if (binder.IsPressed)
            {
                if (!sw.IsRunning)
                {
                    sw.Start();
                }
                else if (sw.ElapsedMilliseconds > 250)
                {
                    sw.Stop();
                    sw.Reset();
                    return true;
                }
            }
            else
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                    sw.Reset();
                }
            }
            return false;
        }

        private DateTime ToggleCooldown = DateTime.Now;

        private void CheckForOpenTrigger()
        {
            
            if (CheckOpenCloseDebouncedTrigger(OpenCloseComputerPlusBinder))
            {
                Vehicle curr_veh = Game.LocalPlayer.Character.Exists() ? Game.LocalPlayer.Character.LastVehicle : null;
                if (curr_veh && curr_veh.Driver == Game.LocalPlayer.Character && curr_veh.Speed <= 1 && Function.IsPoliceVehicle(curr_veh))
                {
                    OnVehicleStopped(null, curr_veh);
                    ShowPoliceComputer();
                }
            }
        }

        private void CheckForCloseTrigger()
        {
            if(!Globals.CloseRequested && CloseComputerPlusWindow.IsPressed)
            {
                //if (!Globals.Navigation.Pop()) //@TODO one day we'll try this again.. for now we'll just let it close
                ClosePoliceComputer();
            }
        }


        private void CheckToggleComputer()
        {
            do
            {
                if (!Globals.CloseRequested && Globals.OpenRequested)
                {
                    CheckForCloseTrigger();
                }                                
                else
                {
                    CheckForOpenTrigger();
                }
                GameFiber.Yield();
            }
            while (Globals.IsPlayerOnDuty);
            GameFiber.Hibernate();
        }



        internal static void OpenMain()
        {
            if (Configs.SkipLogin) Globals.Navigation.Push(new ComputerMain());
            else Globals.Navigation.Replace(new ComputerMain());
        }

        internal static void OpenLogin()
        {
            Globals.Navigation.Push(new ComputerLogin());
        }

        private void ShowPoliceComputer()
        {
            Globals.CloseRequested = false;
            Globals.OpenRequested = true;
            if (RunComputerPlusFiber.IsHibernating) RunComputerPlusFiber.Wake();
            else if (!RunComputerPlusFiber.IsAlive) RunComputerPlusFiber.Start();
        }
        private void ClosePoliceComputer()
        {
            Globals.CloseRequested = true;
            Globals.OpenRequested = false;
        }

        private void RunPoliceComputer()
        {
            do
            {
                
                ShowBackground(Globals.ShowBackgroundWhenOpen);
                GameFiber.Yield();
                PauseGame(Globals.PauseGameWhenOpen);
                IsMainComputerOpen = true;
                if (!Configs.SkipLogin)
                {
                    OpenLogin();
                }
                else
                {
                    OpenMain();
                }


                do
                {                    
                    GameFiber.Yield();                    
                }
                while (!CloseComputerPlusWindow.IsPressed && Globals.Navigation.Head != null);
                ClosePoliceComputer();
                Globals.Navigation.Clear();
                IsMainComputerOpen = false;
                PauseGame(false, true);
                ShowBackground(false, true);
                GameFiber.Yield(); //Yield to allow form fibers to close out
                GameFiber.Hibernate();

            }
            while (Globals.IsPlayerOnDuty);
            GameFiber.Hibernate();
        }


        private void NavOnFormAdded(object sender, NavigationController.NavigationEntry entry)
        {
            try
            {
                GameFiber.StartNew(() =>
                {
                    entry.form.Show();
                    do
                    {
                        GameFiber.Yield();
                    }
                    while (!Globals.CloseRequested && entry.form.IsOpen());

                    Globals.Navigation.RemoveEntry(entry, false);
                    //NavOnFormRemoved(sender, entry);
                });
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }

        private void NavOnFormRemoved(object sender, NavigationController.NavigationEntry entry)
        {
            try
            {
                if (entry.form.Window == null || !entry.form.IsOpen()) return;
                GameFiber.StartNew(() =>
                {
                    entry.form.Window.Close();
                });
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }

        private static void PauseGame(bool pause, bool gameOnlyChange = false)
        {
            if (!gameOnlyChange) Globals.PauseGameWhenOpen = pause;
            Game.IsPaused = pause;
        }


        internal static void TogglePause()
        {
            PauseGame(!Globals.PauseGameWhenOpen);
        }

        private static void ShowBackground(bool visible, bool gameOnlyChange = false)
        {
            if (!gameOnlyChange) Globals.ShowBackgroundWhenOpen = visible;
            if (visible)
                Function.EnableBackground();
            else
                Function.DisableBackground();
        }

        internal static void ToggleBackground()
        {
            ShowBackground(!Globals.ShowBackgroundWhenOpen);
        }

        private void CheckIfCalloutActive()
        {
            //set active callout to null whenever a callout ends

            while (Globals.IsPlayerOnDuty)
            {
                GameFiber.Yield();

                if (Globals.IsCalloutActive == true && Functions.IsCalloutRunning() == false && Globals.ActiveCallout != null)
                {
                    Function.ClearActiveCall();
                }
            }
        }

        public static void ShowNotepad(bool showPauseButton = true)
        {
            SimpleNotepad notepad = new SimpleNotepad();
            try
            {
                var navPushResult = Globals.Navigation.Push(notepad);
                notepad.Show();

                do
                {

                    if (showPauseButton)
                    {
                        notepad.ShowPause(showPauseButton);
                        notepad.SetPauseState(Globals.PauseGameWhenOpen);
                    }

                    GameFiber.Yield();

                }
                while (!CloseComputerPlusWindow.IsPressed && (Globals.Navigation.Head == notepad || notepad.IsOpen()));
                notepad.Close();

            }
            catch { }
        }

       

        private void CheckOpenSimpleNotepad()
        {
            
            while (Globals.IsPlayerOnDuty)
            {
                try {
                    GameFiber.Yield();
                    if (!IsMainComputerOpen && OpenSimpleNotepad.IsPressed)
                    {
                        EntryPoint.PauseGame(Globals.PauseGameWhenOpen, true);
                        ShowNotepad();
                        if (!IsMainComputerOpen) //Make sure game isnt paused if this is opened by itself
                            EntryPoint.PauseGame(false, true);
                    }
                    else
                    {
                        
                    }
                }
                catch(Exception e)
                {
                    Function.Log(e.ToString());
                }
            }
        }

    }
}
