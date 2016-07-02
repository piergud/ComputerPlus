using ComputerPlus.API;
using ComputerPlus.Extensions;
using System;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Rage;
using Rage.Forms;
using Gwen.Control;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPD_First_Response.Mod.API;

namespace ComputerPlus
{
    internal class ComputerCurrentCallDetails : GwenForm
    {
        private Button btn_main, btn_help;
        private ListBox list_calls;
        private Label lbl_c_unit, lbl_c_time, lbl_c_status, lbl_c_call;
        private Label lbl_a_id, lbl_a_time, lbl_a_call, lbl_a_loc, lbl_a_stat, lbl_a_unit, lbl_a_resp,
            lbl_a_desc, lbl_a_peds, lbl_a_vehs;
        private TextBox out_id, out_date, out_time, out_call, out_loc, out_stat, out_unit, out_resp;
        private MultilineTextBox out_desc, out_peds, out_vehs;
        private Base base_calls, base_active;
        private TabControl tc_main;
        //internal static GameFiber form_main = new GameFiber(OpenMainMenuForm),
       //     diag_help = new GameFiber(OpenHelpDialog);

        public ComputerCurrentCallDetails() : base(typeof(ComputerCurrentCallDetailsTemplate))
        {

        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            CreateComponents();
            this.btn_main.Clicked += this.MainMenuButtonClickedHandler;
            this.btn_help.Clicked += this.HelpButtonClickedHandler;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            FillCallDetails();
        }

        public void CreateComponents()
        {
            /***** Main Tab Control *****/
            tc_main = new TabControl(this);
            tc_main.SetPosition(15, 12);
            tc_main.SetSize(625, 389);

            /***** Call List Tab *****/
            // base container
            base_calls = new Base(this);
            base_calls.SetPosition(0, 0);
            base_calls.SetSize(617, 358);

            // calls listbox
            list_calls = new ListBox(base_calls);
            list_calls.SetPosition(0, 18);
            list_calls.SetSize(613, 333);

            // "Unit" label
            lbl_c_unit = new Label(base_calls);
            lbl_c_unit.Text = "Unit";
            lbl_c_unit.SetPosition(3, 1);
            lbl_c_unit.SetSize(26, 13);

            // "Time" label
            lbl_c_time = new Label(base_calls);
            lbl_c_time.Text = "Time";
            lbl_c_time.SetPosition(50, 1);
            lbl_c_time.SetSize(30, 13);

            // "Status" label
            lbl_c_status = new Label(base_calls);
            lbl_c_status.Text = "Status";
            lbl_c_status.SetPosition(120, 1);
            lbl_c_status.SetSize(37, 13);

            // "Call" label
            lbl_c_call = new Label(base_calls);
            lbl_c_call.Text = "Call";
            lbl_c_call.SetPosition(230, 1);
            lbl_c_call.SetSize(24, 13);

            /***** Active Call Tab *****/
            // base container
            base_active = new Base(this);
            base_active.SetPosition(0, 0);
            base_active.SetSize(617, 358);

            // "ID No." label
            lbl_a_id = new Label(base_active);
            lbl_a_id.Text = "ID No.";
            lbl_a_id.SetPosition(26, 6);
            lbl_a_id.SetSize(38, 13);
            // "ID No." textbox
            out_id = new TextBox(base_active);
            out_id.SetPosition(70, 3);
            out_id.SetSize(306, 20);
            out_id.KeyboardInputEnabled = false;

            // "Time" label
            lbl_a_time = new Label(base_active);
            lbl_a_time.Text = "Time";
            lbl_a_time.SetPosition(422, 7);
            lbl_a_time.SetSize(30, 13);
            // "Time" date textbox
            out_date = new TextBox(base_active);
            out_date.SetPosition(455, 3);
            out_date.SetSize(66, 20);
            out_date.KeyboardInputEnabled = false;
            // "Time" time textbox
            out_time = new TextBox(base_active);
            out_time.SetPosition(527, 3);
            out_time.SetSize(66, 20);
            out_time.KeyboardInputEnabled = false;

            // "Situation" label
            lbl_a_call = new Label(base_active);
            lbl_a_call.Text = "Situation";
            lbl_a_call.SetPosition(17, 33);
            lbl_a_call.SetSize(48, 13);
            // "Situation" textbox
            out_call = new TextBox(base_active);
            out_call.SetPosition(70, 29);
            out_call.SetSize(523, 20);
            out_call.KeyboardInputEnabled = false;

            // "Location" label
            lbl_a_loc = new Label(base_active);
            lbl_a_loc.Text = "Location";
            lbl_a_loc.SetPosition(18, 58);
            lbl_a_loc.SetSize(48, 13);
            // "Location" textbox
            out_loc = new TextBox(base_active);
            out_loc.SetPosition(70, 55);
            out_loc.SetSize(523, 20);
            out_loc.KeyboardInputEnabled = false;

            // "Status" label
            lbl_a_stat = new Label(base_active);
            lbl_a_stat.Text = "Status";
            lbl_a_stat.SetPosition(27, 84);
            lbl_a_stat.SetSize(37, 13);
            // "Status" textbox
            out_stat = new TextBox(base_active);
            out_stat.SetPosition(70, 81);
            out_stat.SetSize(106, 20);
            out_stat.KeyboardInputEnabled = false;

            // "Unit" label
            lbl_a_unit = new Label(base_active);
            lbl_a_unit.Text = "Unit";
            lbl_a_unit.SetPosition(258, 84);
            lbl_a_unit.SetSize(26, 13);
            // "Unit" textbox
            out_unit = new TextBox(base_active);
            out_unit.SetPosition(290, 81);
            out_unit.SetSize(86, 20);
            out_unit.KeyboardInputEnabled = false;

            // "Response" label
            lbl_a_resp = new Label(base_active);
            lbl_a_resp.Text = "Response";
            lbl_a_resp.SetPosition(454, 84);
            lbl_a_resp.SetSize(55, 13);
            // "Response" textbox
            out_resp = new TextBox(base_active);
            out_resp.SetPosition(514, 81);
            out_resp.SetSize(79, 20);
            out_resp.KeyboardInputEnabled = false;

            // "Comments" label
            lbl_a_desc = new Label(base_active);
            lbl_a_desc.Text = "Comments";
            lbl_a_desc.SetPosition(8, 113);
            lbl_a_desc.SetSize(56, 13);
            // "Comments" textbox
            out_desc = new MultilineTextBox(base_active);
            out_desc.SetPosition(70, 110);
            out_desc.SetSize(523, 103);
            out_desc.KeyboardInputEnabled = false;

            // "Persons" label
            lbl_a_peds = new Label(base_active);
            lbl_a_peds.Text = "Persons";
            lbl_a_peds.SetPosition(19, 226);
            lbl_a_peds.SetSize(45, 13);
            // "Persons" textbox
            out_peds = new MultilineTextBox(base_active);
            out_peds.SetPosition(70, 226);
            out_peds.SetSize(523, 57);
            out_peds.KeyboardInputEnabled = false;

            // "Vehicles" label
            lbl_a_vehs = new Label(base_active);
            lbl_a_vehs.Text = "Vehicles";
            lbl_a_vehs.SetPosition(19, 298);
            lbl_a_vehs.SetSize(47, 13);
            // "Vehicles" textbox
            out_vehs = new MultilineTextBox(base_active);
            out_vehs.SetPosition(70, 295);
            out_vehs.SetSize(523, 57);
            out_vehs.KeyboardInputEnabled = false;

            // Add tabs and their corresponding containers
            // Active Call tab is hidden when no callout is active
            if (Globals.ActiveCallout != null)
                tc_main.AddPage("Active Call", base_active);
            else
                base_active.Hide();
            tc_main.AddPage("Call List", base_calls);

            List<CalloutData> mActiveCalls = (from CalloutData x in Globals.CallQueue orderby x.TimeReceived descending select x).ToList();

            foreach (CalloutData x in mActiveCalls)
            {
                list_calls.AddRow(String.Format("{0}{1}{2}{3}", x.PrimaryUnit.PadRight(12), x.TimeReceived.ToLocalTime().ToString("HH:mm").PadRight(21), x.Status.ToFriendlyString().PadRight(31), x.Name.ToUpper()));
            }
        }

        private void MainMenuButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            //this.Window.Close();
            //form_main = new GameFiber(OpenMainMenuForm);
            //form_main.Start();
        }

        private void HelpButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            /*diag_help = new GameFiber(OpenHelpDialog);
            diag_help.Start();*/
        }
        /*
        private static void OpenMainMenuForm()
        {
            GwenForm main = new ComputerMain();
            main.Show();
            while (main.Window.IsVisible)
                GameFiber.Yield();
        }
        */
        private static void OpenHelpDialog()
        {
            GwenForm help = new ComputerHelpDialog();
            help.Show();
            while (help.Window.IsVisible)
                GameFiber.Yield();
        }

        private void FillCallDetails()
        {
            if (Globals.ActiveCallout != null)
            {
                DateTime dt = DateTime.UtcNow;

                out_id.Text = Globals.ActiveCallout.ID.ToString();
                out_date.Text = dt.ToLocalTime().ToString("MM/dd/yyyy");
                out_time.Text = dt.ToLocalTime().ToString("HH:mm:ss");
                out_call.Text = Globals.ActiveCallout.Name;
                out_loc.Text = World.GetStreetName(Globals.ActiveCallout.Location);
                out_stat.Text = Globals.ActiveCallout.Status.ToFriendlyString();
                out_unit.Text = Configs.UnitNumber;
                out_resp.Text = Globals.ActiveCallout.ResponseType.ToFriendlyString();

                string mDescription = Globals.ActiveCallout.Description.WordWrap(450, out_desc.Font.FaceName.ToString()) + Environment.NewLine;
                DescriptionBoxText = mDescription;

                foreach (CalloutUpdate u in Globals.ActiveCallout.Updates)
                {
                    DescriptionBoxText += String.Format("{0}{1} {2}", Environment.NewLine, Function.GetFormattedDateTime(u.TimeAdded), u.Text);
                }

                List<Ped> peds = Globals.ActiveCallout.Peds;
                if (peds != null)
                {
                    if (peds.Count > 0)
                    {
                        for (int i = 0; i < peds.Count; i++)
                        {
                            if (peds[i])
                            {
                                if (i != 0)
                                    PedBoxText += ", ";
                                PedBoxText += LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(peds[i]).FullName;
                            }
                        }
                    }
                    else
                    {
                        PedBoxText = "No information available at this time.";
                    }
                }

                List<Vehicle> vehs = Globals.ActiveCallout.Vehicles;
                if (vehs != null)
                {
                    if (vehs.Count > 0)
                    {
                        for (int i = 0; i < vehs.Count; i++)
                        {
                            if (vehs[i])
                            {
                                if (i != 0)
                                    VehicleBoxText += ", ";
                                VehicleBoxText += vehs[i].LicensePlate;
                            }
                        }
                    }
                    else
                    {
                        VehicleBoxText = "No information available at this time.";
                    }
                }
            }
        }

        #region Properties

        internal string DescriptionBoxText
        {
            get
            {
                return out_desc.Text;
            }
            set
            {
                out_desc.Text = value;
                //out_desc.Text = out_desc.Text.Replace(Environment.NewLine, "");
                //out_desc.WordWrap(450);
            }
        }

        internal string PedBoxText
        {
            get
            {
                return out_peds.Text;
            }
            set
            {
                out_peds.Text = value.WordWrap(400, out_peds.Font.FaceName.ToString());
                //out_peds.Text = out_peds.Text.Replace(Environment.NewLine, "");
                //out_peds;
            }
        }

        internal string VehicleBoxText
        {
            get
            {
                return out_vehs.Text;
            }
            set
            {
                out_vehs.Text = value.WordWrap(400, out_vehs.Font.FaceName.ToString());
                //out_vehs.Text = out_vehs.Text.Replace(Environment.NewLine, "");
                //out_vehs.WordWrap(350);
            }
        }

        #endregion
    }
}