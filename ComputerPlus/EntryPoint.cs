using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Rage;
using Rage.Forms;
using LSPD_First_Response.Mod.API;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Interfaces.ComputerVehDB;
using ComputerPlus.Controllers.Models;

namespace ComputerPlus
{
    public sealed class EntryPoint : Plugin
    {
        internal static GwenForm login = null, main = null;
        internal delegate void VehicleStoppedEvent(object sender, Vehicle veh);
        internal static VehicleStoppedEvent OnVehicleStopped;
        static Stopwatch sw = new Stopwatch();
        internal static bool _arrestFirstLogin = true;

        private static bool _opened, _prompted;
        internal static bool HasBackground
        {
            get;
            private set;
        } = false;

        internal static bool IsOpen
        {
            get;
            private set;
        } = false;
        internal static List<string> recent_text = new List<string>();
        internal static GameFiber fCheckIfCalloutActive = new GameFiber(CheckIfCalloutActive);
        private static GameFiber RunComputerPlus =  new GameFiber(ShowPoliceComputer);

        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += DutyStateChangedHandler;
            OnVehicleStopped += VehicleStoppedHandler;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
            Configs.RunConfigCheck();
            Function.checkForRageVersionClass.checkForRageVersion(0.41f);
        }

        public override void Finally()
        {
            if (login != null)
            {
                if (login.Window.IsVisible)
                {
                    login.Window.Close();
                }
            }
            if (Game.IsPaused)
            {
                Function.LogDebug("Pause false EntryPoint.Finally");
                PauseGame(false);
            }
            ShowBackground(false);
        }

        private static void DutyStateChangedHandler(bool on_duty)
        {
            Globals.IsPlayerOnDuty = on_duty;

            if (on_duty) 
            {
                Game.FrameRender += Process;
                Function.LogDebug("Successfully loaded LSPDFR Computer+.");

                Function.MonitorAICalls();
                fCheckIfCalloutActive = new GameFiber(CheckIfCalloutActive);
                fCheckIfCalloutActive.Start();

                Function.CheckForUpdates();
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
        static bool alprRunning = false;
        private static void Process(object sender, GraphicsEventArgs e)
        {
            var fiber = EntryPoint.RunComputerPlus;

            Vehicle curr_veh = Game.LocalPlayer.Character.Exists() ? Game.LocalPlayer.Character.LastVehicle : null;
            if (curr_veh && curr_veh.Driver == Game.LocalPlayer.Character)
            {
                if (curr_veh.Speed <= 1)
                {
                    OnVehicleStopped.Invoke(null, curr_veh);

                    if (Game.IsControlPressed(0, GameControl.Context) && Function.IsPoliceVehicle(curr_veh) && !IsOpen)
                    {
                        if (!sw.IsRunning)
                        {
                            sw.Start();
                        }
                        else if (sw.ElapsedMilliseconds > 250)
                        {
                            sw.Stop();
                            sw.Reset();
                            if (fiber.IsHibernating) fiber.Wake();
                            else if (!fiber.IsAlive) fiber.Start();
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
                }
            }          

           /* 
            @TODO @ainesophaur discuss whether to include a vanilla ALPR in C+ and optimize the vehicle enumeration calls
            if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.LControlKey) && Game.IsKeyDown(System.Windows.Forms.Keys.U))
            {
                if (!alprRunning) ComputerVehicleController.RunVanillaAlpr();
                else ComputerVehicleController.StopVanillaAlpr();
                alprRunning = !alprRunning;
            }
           */
        }
        //@TODO put all game fibers in an array and filter through to find any that are alive
        private static bool AreGameFibersRunning
        {
            get
            {
                return
                    (ComputerLogin.ComputerLoginGameFiber.IsAlive && !ComputerLogin.ComputerLoginGameFiber.IsHibernating)
                    || (ComputerMain.ComputerMainGameFiber.IsAlive && !ComputerMain.ComputerMainGameFiber.IsHibernating)
                    || (ComputerPedController.PedSearchGameFiber.IsAlive && !ComputerPedController.PedSearchGameFiber.IsHibernating)
                    || (ComputerPedController.PedViewGameFiber.IsAlive && !ComputerPedController.PedViewGameFiber.IsHibernating)
                    || (ComputerVehicleController.VehicleSearchGameFiber.IsAlive && !ComputerVehicleController.VehicleSearchGameFiber.IsHibernating)
                    || (ComputerVehicleController.VehicleDetailsGameFiber.IsAlive && !ComputerVehicleController.VehicleDetailsGameFiber.IsHibernating)
                    || (ComputerMain.form_active_calls.IsAlive && !ComputerMain.form_active_calls.IsHibernating)
                    || (ComputerMain.form_backup.IsAlive && !ComputerMain.form_backup.IsHibernating);
                   // || ComputerMain.form_backup.IsAlive || ComputerMain.form_active_calls.IsAlive
                   //|| ComputerPedDB.form_main.IsAlive || ComputerVehDB.form_main.IsAlive || ComputerRequestBackup.form_main.IsAlive;
                   //|| ComputerCurrentCallDetails.form_main.IsAlive;
            }
        }

        internal static void OpenMain()
        {
            var fiber = ComputerMain.ComputerMainGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if (!fiber.IsAlive) fiber.Start();
        }

        internal static void OpenLogin()
        {
            var fiber = ComputerLogin.ComputerLoginGameFiber;
            if (fiber.IsHibernating) fiber.Wake();
            else if (!fiber.IsAlive) fiber.Start();
        }

        private static void ShowPoliceComputer()
        {
            while (true)
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
                while (AreGameFibersRunning);
                PauseGame(false, true);
                ShowBackground(false, true);
                IsOpen = false;
                GameFiber.Hibernate();
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
            if(!gameOnlyChange) Globals.ShowBackgroundWhenOpen = visible;
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

            while(Globals.IsPlayerOnDuty)
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
