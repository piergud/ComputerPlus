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

        private static bool IsOpen = false;
        internal static List<string> recent_text = new List<string>();
        internal GameFiber fCheckIfCalloutActive = new GameFiber(CheckIfCalloutActive);
        private GameFiber DetectOpenCloseRequestedFiber;
        private GameFiber RunComputerPlusFiber;

        //private static KeyBinder CloseComputerPlusController = new KeyBinder(ControllerButtons.X, false);
        //private static KeyBinder CloseComputerPlusKeyboard = new KeyBinder(Keys.Escape, false);

        private static KeyBinder OpenCloseComputerPlusBinder = new KeyBinder(GameControl.Context, true);

        //private static GameFiber KeyPressFiber = new GameFiber(Process);
        //Try to just run Process without being in a fiber

        KeyBinderMonitor BinderMonitor;


        internal EntryPoint()
        {
            DetectOpenCloseRequestedFiber = new GameFiber(CheckToggleComputer);
            RunComputerPlusFiber = new GameFiber(RunPoliceComputer);
        }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += DutyStateChangedHandler;
            OnVehicleStopped += VehicleStoppedHandler;
            Globals.Navigation.OnFormAdded += NavOnFormAdded;
            Globals.Navigation.OnFormRemoved += NavOnFormRemoved;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
            Configs.RunConfigCheck();
            Function.checkForRageVersionClass.checkForRageVersion(0.41f);
   
        }

        public override void Finally()
        {
            Globals.Navigation.OnFormAdded -= NavOnFormAdded;
            Globals.Navigation.OnFormRemoved -= NavOnFormRemoved;
            if (RunComputerPlusFiber.IsRunning()) RunComputerPlusFiber.Abort();
            if (fCheckIfCalloutActive.IsRunning()) fCheckIfCalloutActive.Abort();
            //if (KeyPressFiber.IsRunning()) KeyPressFiber.Abort();
        }


        private void DutyStateChangedHandler(bool on_duty)
        {
            Globals.IsPlayerOnDuty = on_duty;

            if (on_duty)
            {

               // BinderMonitor = KeyBinderMonitor.CreateNew(OpenCloseComputerPlusBinder, () => OpenClosePressed = true, null);
                if (fCheckIfCalloutActive.IsHibernating) fCheckIfCalloutActive.Wake();
                else fCheckIfCalloutActive.Start();
                Function.MonitorAICalls();
                Function.CheckForUpdates();

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


        private bool CheckOpenCloseDebouncedTrigger()
        {
            if (OpenCloseComputerPlusBinder.IsPressed)
            {
                if (!sw.IsRunning)
                {
                    sw.Start();
                }
                else if (sw.ElapsedMilliseconds > 250)
                {
                    Function.Log("Show PC");
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

        private void CheckForOpenTrigger()
        {

            if (!IsOpen && CheckOpenCloseDebouncedTrigger())
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
            if (IsOpen)
            CheckOpenCloseDebouncedTrigger();
        }


        private void CheckToggleComputer()
        {
            do
            {
                
                if (!Globals.CloseRequested)
                {
                    if (!IsOpen)
                    {
                        CheckForOpenTrigger();
                    }
                    else
                    {
                        CheckForCloseTrigger();
                    }
                }
                else
                {
                    Function.Log("Process doing nothing CloseRequested");
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
            if (RunComputerPlusFiber.IsHibernating) RunComputerPlusFiber.Wake();
            else if (!RunComputerPlusFiber.IsAlive) RunComputerPlusFiber.Start();
        }
        private static void ClosePoliceComputer()
        {
            Globals.CloseRequested = true;
        }

        private static void RunPoliceComputer()
        {
            do
            {                
                IsOpen = true;
                PauseGame(Globals.PauseGameWhenOpen);
                ShowBackground(Globals.ShowBackgroundWhenOpen);


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
                while (!Globals.CloseRequested && Globals.Navigation.Head != null);
                Function.Log("------BREAKING OUT-------");
                if (Globals.CloseRequested)
                {
                    Globals.CloseRequested = false;
                    Function.Log("Close requested");
                    Globals.Navigation.Clear();
                }
                IsOpen = false;                
                PauseGame(false, true);
                ShowBackground(false, true);
                GameFiber.Yield(); //Yield to allow form fibers to close out
                GameFiber.Hibernate();

            }
            while (true);
        }

        private static void NavOnFormRemoved(object sender, NavigationController.NavigationEntry entry)
        {
            try
            {
                Function.Log("Removing form");
                GameFiber.StartNew(() =>
                {
                    entry.form.Window.Close();
                    var nav = sender as NavigationController;
                    if (nav is NavigationController)
                    {
                        if (nav.HasOpenForms)
                        {
                            Function.Log("Nav has OpenForms");
                        } else
                        {
                          
                        }
                    }
                    
                });
                Function.Log("Removed form");
            }
            catch (Exception e)
            {
                Function.Log(e.ToString());
            }
        }
        
        private static void NavOnFormAdded(object sender, NavigationController.NavigationEntry entry)
        {
            try
            {

                GameFiber.StartNew(() =>
                {
                    Function.Log("Showing form");
                    entry.form.Show();
                    do
                    {
                        GameFiber.Yield();
                    }
                    while (!Globals.CloseRequested && entry.form.IsOpen());
                    Function.Log("Removing stack form");
                    Globals.Navigation.RemoveEntry(entry);
                    NavOnFormRemoved(sender, entry);
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

        private static void CheckIfCalloutActive()
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


    }
}
