using System;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
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
        internal static GameFiber search_fiber = new GameFiber(null);
        private bool _initial_clear = false;

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
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            output_info.KeyboardInputEnabled = false;
            if (Functions.GetCurrentPullover() != null)
            {
                input_name.Text = Functions.GetPersonaForPed(Functions.GetPulloverSuspect(Functions.GetCurrentPullover())).FullName;
                _initial_clear = true;
            }
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

        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }

        private void SearchForSuspect() 
        {
            if (!search_fiber.IsAlive)
            {
                output_info.SetText("Searching. Please wait...");

                search_fiber = GameFiber.StartNew(delegate 
                {
                    GameFiber.Sleep(2500);
                    string name = input_name.Text.ToLower();
                    List<Ped> peds = World.GetAllPeds().ToList();
                    peds.RemoveAll(p => !p);
                    peds.OrderBy(p => p.DistanceTo(Game.LocalPlayer.Character.Position));
                    Ped ped = peds.Where(p => p && Functions.GetPersonaForPed(p).FullName.ToLower() == name).FirstOrDefault();

                    if (ped)
                    {
                        output_info.Text = GetFormattedInfoForPed(ped);
                        Function.AddPedToRecents(ped);
                    }
                    else
                    {
                        output_info.Text = "No record for the specified name was found.";
                    }
                });
            }
        }

        private string GetFormattedInfoForPed(Ped ped)
        {
            Persona p = Functions.GetPersonaForPed(ped);
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
    }
}
