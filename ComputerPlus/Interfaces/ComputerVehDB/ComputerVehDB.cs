using System;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using Rage;
using Rage.Forms;
using Gwen.Control;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;

namespace ComputerPlus
{
    public class ComputerVehDB : GwenForm
    {
        private Button btn_search, btn_main;
        private MultilineTextBox output_info;
        private TextBox input_name;
        public static GameFiber form_main = new GameFiber(OpenMainMenuForm);
        public static GameFiber search_fiber = new GameFiber(OpenMainMenuForm);
        private BackgroundWorker veh_search;
        private bool _initial_clear = false;
        private SynchronizationContext sc;

        public ComputerVehDB() : base(typeof(ComputerVehDBTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_search.Clicked += this.SearchButtonClickedHandler;
            this.btn_main.Clicked += this.MainMenuButtonClickedHandler;
            this.input_name.Clicked += this.InputNameFieldClickedHandler;
            this.input_name.SubmitPressed += this.InputNameSubmitHandler;
            veh_search = new BackgroundWorker();
            veh_search.DoWork += new DoWorkEventHandler(VehSearchProcess);
            veh_search.RunWorkerCompleted += new RunWorkerCompletedEventHandler(VehSearchCompleted);
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            output_info.KeyboardInputEnabled = false;
            if (Functions.GetCurrentPullover() != null)
            {
                Ped ped = Functions.GetPulloverSuspect(Functions.GetCurrentPullover());
                if (ped.LastVehicle != null)
                {
                    input_name.SetText(ped.LastVehicle.LicensePlate);
                    _initial_clear = true;
                }
            }

            sc = SynchronizationContext.Current;
        }

        private void InputNameSubmitHandler(Base sender, EventArgs e)
        {
            SearchForVehicle();
        }

        private void SearchButtonClickedHandler(Base sender, ClickedEventArgs e) 
        {
            SearchForVehicle();
        }

        private void MainMenuButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            this.Window.Close();
            form_main = new GameFiber(OpenMainMenuForm);
            form_main.Start();
        }

        private void InputNameFieldClickedHandler(Base sender, ClickedEventArgs e)
        {
            if (!_initial_clear)
            {
                input_name.Text = "";
                _initial_clear = true;
            }
        }

        private void VehSearchProcess(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2500);
            Vehicle[] vehs = World.GetAllVehicles();
            for (int i = 0; i < vehs.Length; i++)
            {
                if (vehs[i].LicensePlate.ToLower() == ((string)e.Argument).ToLower())
                {
                    e.Result = vehs[i];
                    break;
                }
                else if (i == vehs.Length - 1)
                    e.Result = null;
            }
        }

        private void VehSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Vehicle veh = (Vehicle)e.Result;
            if (veh != null)
            {
                sc.Post(UpdateResult, GetFormattedInfoForVehicle(veh));
                Function.AddVehicleToRecents(veh);
            }
            else
            {             
                sc.Post(UpdateResult, "No record for the specified license plate was found.");
            }
        }

        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        private void SearchForVehicle() 
        {
            if (!veh_search.IsBusy)
            {
                output_info.Text = "Searching. Please wait...";
                veh_search.RunWorkerAsync(input_name.Text);
            }
        }

        private string GetFormattedInfoForVehicle(Vehicle veh)
        {
            string info = "";;
            string veh_name = Function.GetVehicleDisplayName(veh);
            string veh_owner = Functions.GetVehicleOwnerName(veh);
            info = String.Format("Information found for license plate \"{0}\":\nVehicle: {1}\nOwner: {2}", veh.LicensePlate, 
                veh_name, veh_owner);

            if (Function.IsLSPDFRPluginRunning("Traffic Policer"))
            {
                string insurance_text = "None";
                if (TrafficPolicerFunction.GetVehicleInsured(veh))
                {
                    insurance_text = "Insured";
                }
                info += String.Format("\nInsurance Status: {0}", insurance_text);
            }

            if (veh.IsStolen)
            {
                info += "\nThis vehicle has been reported as stolen.";
            }

            Ped ped = null;
            Persona p = null;
            float min = -1f;
            foreach (Ped pd in World.GetAllPeds())
            {
                 p = Functions.GetPersonaForPed(pd);
                 if (p.FullName == veh_owner)
                 {
                     if (min == -1f)
                     {
                         min = Vector3.Distance(Game.LocalPlayer.Character.Position, pd.Position);
                         ped = pd;
                     }
                     else
                     {
                         float val = Vector3.Distance(Game.LocalPlayer.Character.Position, pd.Position);
                         if (val < min)
                         {
                             ped = pd;
                         }
                     }
                 }
            }
            if (ped != null)
            {
                p = Functions.GetPersonaForPed(ped);
                string wanted_text = "No active warrant(s)", leo_text = "";
                if (p.Wanted)
                    wanted_text = "Suspect has an active warrant";
                if (p.IsCop)
                    leo_text = "Note: Suspect is an off-duty police officer";
                else if (p.IsAgent)
                    leo_text = "Note: Suspect is a federal agent";
                info += String.Format("\n\nInformation found about vehicle owner \"{0}\":\nDOB: {1}\nCitations: {2}\nGender: {3}\nLicense: {4}\n"
                    + "Times Stopped: {5}\nWanted: {6}\n{7}", p.FullName, String.Format("{0:dddd, MMMM dd, yyyy}", p.BirthDay), p.Citations, p.Gender, p.LicenseState,
                    p.TimesStopped, wanted_text, leo_text);
            }
            return info;
        }

        public void UpdateResult(object state)
        {
            string result = state as string;
            output_info.Text = result;
        }
    }
}
