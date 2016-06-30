using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Rage;
using Rage.Forms;
using LSPD_First_Response.Mod.API;

namespace ComputerPlus
{
    public sealed class EntryPoint : Plugin
    {
        internal static GwenForm login = null, main = null;
        internal delegate void VehicleStoppedEvent(object sender, Vehicle veh);
        internal static VehicleStoppedEvent OnVehicleStopped;
        static Stopwatch sw = new Stopwatch();
        private static float _stored_speed;
        private static bool _opened = false;
        internal static List<string> recent_text = new List<string>();
        internal static GameFiber fCheckIfCalloutActive = new GameFiber(CheckIfCalloutActive);

        public override void Initialize()
        {
            LSPD_First_Response.Mod.API.Functions.OnOnDutyStateChanged += DutyStateChangedHandler;
            OnVehicleStopped += VehicleStoppedHandler;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
            Configs.RunConfigCheck();
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
                Game.IsPaused = false;
            Function.DisableBackground();
        }

        private static void DutyStateChangedHandler(bool on_duty)
        {
            Globals.IsPlayerOnDuty = on_duty;

            if (on_duty) 
            {
                Game.FrameRender += Process;
                Game.LogTrivial("Successfully loaded LSPDFR Computer+.");

                Function.MonitorAICalls();
                fCheckIfCalloutActive = new GameFiber(CheckIfCalloutActive);
                fCheckIfCalloutActive.Start();

                Function.CheckForUpdates();
            }
        }

        private static void VehicleStoppedHandler(object sender, Vehicle veh)
        {
            if (veh) 
            {
                if (Function.IsPoliceVehicle(veh)
                    && LSPD_First_Response.Mod.API.Functions.GetCurrentPullover() != null)
                {
                    Game.DisplayHelp("Hold ~INPUT_CONTEXT~ to open ~b~LSPDFR Computer+~w~.");
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

        private static void Process(object sender, GraphicsEventArgs e)
        {
            Vehicle curr_veh = Game.LocalPlayer.Character.CurrentVehicle;
            if (curr_veh)
            {
                if (curr_veh.Speed != _stored_speed)
                {
                    _stored_speed = curr_veh.Speed;
                    if (_stored_speed == 0)
                        OnVehicleStopped.Invoke(null, curr_veh);
                }
                if (Game.IsControlPressed(0, GameControl.Context) && Function.IsPoliceVehicle(curr_veh) && curr_veh.Speed == 0 && !_opened)
                {
                    if (!sw.IsRunning)
                    {
                        sw.Start();
                    }
                    else if (sw.ElapsedMilliseconds > 250)
                    {
                        sw.Stop();
                        sw.Reset();
                        GameFiber.StartNew(() => ShowPoliceComputer());
                        _opened = true;
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

        private static void ShowPoliceComputer()
        {
            Game.IsPaused = true;
            if (!Configs.SkipLogin)
            {
                login = new ComputerLogin();
                login.Show();

                while (login.Window.IsVisible || ComputerLogin.next_form.IsAlive || ComputerMain.form_ped_db.IsAlive
                    || ComputerMain.form_veh_db.IsAlive || ComputerMain.form_backup.IsAlive || ComputerMain.form_active_calls.IsAlive
                    || ComputerPedDB.form_main.IsAlive || ComputerVehDB.form_main.IsAlive || ComputerRequestBackup.form_main.IsAlive 
                    || ComputerCurrentCallDetails.form_main.IsAlive)
                {
                    GameFiber.Yield();
                }
            }
            else
            {
                main = new ComputerMain();
                main.Show();

                while (main.Window.IsVisible || ComputerMain.form_ped_db.IsAlive || ComputerMain.form_veh_db.IsAlive 
                    || ComputerMain.form_backup.IsAlive || ComputerMain.form_active_calls.IsAlive || ComputerPedDB.form_main.IsAlive
                    || ComputerPedDB.form_main.IsAlive || ComputerVehDB.form_main.IsAlive || ComputerRequestBackup.form_main.IsAlive
                    || ComputerCurrentCallDetails.form_main.IsAlive)
                {
                    GameFiber.Yield();
                }
            }

            Function.DisableBackground();

            _opened = false;
            Game.IsPaused = false;
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