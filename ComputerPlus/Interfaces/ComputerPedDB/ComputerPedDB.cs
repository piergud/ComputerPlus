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
    internal class ComputerPedDB : GwenForm
    {
        private Button btn_search, btn_main;
        private MultilineTextBox output_info;
        private TextBox input_name;
        internal static GameFiber form_main = new GameFiber(OpenMainMenuForm);
        internal static GameFiber search_fiber = new GameFiber(OpenMainMenuForm);
        private BackgroundWorker ped_search;
        private bool _initial_clear = false;
        private SynchronizationContext sc;

        public ComputerPedDB() : base(typeof(ComputerPedDBTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.btn_search.Clicked += this.SearchButtonClickedHandler;
            this.btn_main.Clicked += this.MainMenuButtonClickedHandler;
            this.input_name.Clicked += this.InputNameFieldClickedHandler;
            this.input_name.SubmitPressed += this.InputNameSubmitHandler;
            ped_search = new BackgroundWorker();
            ped_search.DoWork += new DoWorkEventHandler(PedSearchProcess);
            ped_search.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PedSearchCompleted);
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            output_info.KeyboardInputEnabled = false;
            if (Functions.GetCurrentPullover() != null)
            {
                input_name.SetText(Functions.GetPersonaForPed(Functions.GetPulloverSuspect(Functions.GetCurrentPullover())).FullName);
                _initial_clear = true;
            }

            sc = SynchronizationContext.Current;
        }

        private void InputNameSubmitHandler(Base sender, EventArgs e)
        {
            SearchForSuspect();
        }

        private void SearchButtonClickedHandler(Base sender, ClickedEventArgs e) 
        {
            SearchForSuspect();
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

        private void PedSearchProcess(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2500);
            Ped[] peds = World.GetAllPeds();
            for (int i = 0; i < peds.Length; i++)
            {
                Persona persona = Functions.GetPersonaForPed(peds[i]);
                if (persona.FullName.ToLower() == ((string)e.Argument).ToLower())
                {
                    e.Result = peds[i];
                    break;
                }
                else if (i == peds.Length - 1)
                    e.Result = null;
            }
        }

        private void PedSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Ped ped = (Ped)e.Result;
            if (ped != null)
            {
                sc.Post(UpdateResult, GetFormattedInfoForPersona(Functions.GetPersonaForPed(ped)));
                Function.AddPedToRecents(ped);
            }
            else
            {
                sc.Post(UpdateResult, "No record for the specified name was found.");
            }
        }

        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        private void SearchForSuspect() 
        {
            if (!ped_search.IsBusy)
            {
                output_info.SetText("Searching. Please wait...");
                ped_search.RunWorkerAsync(input_name.Text);
            }
        }

        private string GetFormattedInfoForPersona(Persona p)
        {
            string wanted_text = "No active warrant(s)", leo_text = "";
            if (p.Wanted)
                wanted_text = "Suspect has an active warrant";
            if (p.IsCop)
                leo_text = "Note: Suspect is an off-duty police officer";
            else if (p.IsAgent)
                leo_text = "Note: Suspect is a federal agent";
            return String.Format("Information found about \"{0}\":\nDOB: {1}\nCitations: {2}\nGender: {3}\nLicense: {4}\n"
                + "Times Stopped: {5}\nWanted: {6}\n{7}", p.FullName, String.Format("{0:dddd, MMMM dd, yyyy}", p.BirthDay), p.Citations, p.Gender, p.LicenseState,
                p.TimesStopped, wanted_text, leo_text);
        }

        internal void UpdateResult(object state)
        {
            string result = state as string;
            output_info.Text = result;
        }
    }
}
