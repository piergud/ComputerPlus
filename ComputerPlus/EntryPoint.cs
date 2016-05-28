using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Media;
using Rage;
using Rage.Forms;
using Rage.Native;
using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus
{
    public sealed class EntryPoint : Plugin
    {
        public static GwenForm login = null;
        public static EventHandler OnVehicleStopped;
        static Stopwatch sw = new Stopwatch();
        private static float _stored_speed;
        private static bool _opened = false;
        public static List<string> recent_text = new List<string>();

        public static float StoredSpeed
        {
            set
            {
                if (_stored_speed != value && value == 0f)
                {
                    if (OnVehicleStopped != null)
                        OnVehicleStopped.Invoke(null, new EventArgs());
                }
                _stored_speed = value;
            }
            get
            {
                return Game.LocalPlayer.Character.CurrentVehicle.Speed;
            }
        }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += DutyStateChangedHandler;
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
            if (on_duty) 
            {
                Game.FrameRender += Process;
                Game.LogTrivial("Successfully loaded LSPDFR Computer+ by PieRGud.");
            }
        }

        private static void VehicleStoppedHandler(object sender, EventArgs e)
        {
            Vehicle curr_veh = Game.LocalPlayer.Character.CurrentVehicle;
            if (curr_veh) 
            {
                if (Function.IsPoliceVehicle(curr_veh)
                    && Functions.GetCurrentPullover() != null)
                {
                    Game.DisplayHelp("Hold ~INPUT_CONTEXT~ to open LSPDFR Computer+.");
                }
            }
        }

        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
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
                StoredSpeed = curr_veh.Speed;
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
            login = new ComputerLogin();
            login.Show();

            while (login.Window.IsVisible || ComputerLogin.next_form.IsAlive || ComputerMain.form_ped_db.IsAlive
                || ComputerMain.form_veh_db.IsAlive  || ComputerMain.form_backup.IsAlive || ComputerPedDB.form_main.IsAlive
                || ComputerVehDB.form_main.IsAlive || ComputerRequestBackup.form_main.IsAlive)
            {
                GameFiber.Yield();
            }

            Function.DisableBackground();

            _opened = false;
            Game.IsPaused = false;
        }
    }
}
