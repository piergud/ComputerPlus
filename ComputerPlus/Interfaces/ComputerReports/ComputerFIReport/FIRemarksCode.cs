using Rage;
using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Notsolethalpolicing.MDT;
using LSPD_First_Response;
using LSPD_First_Response.Mod.API;
using System.IO;
using LSPD_First_Response.Engine.Scripting.Entities;
using Gwen.ControlInternal;
using System.Media;

namespace ComputerPlus
{
    public class FIRemarksCode2 : GwenForm
    {
        DateTime BDay;
        Persona PedPersona;
        Persona PedPersona1;
        Persona PedPersona2;
        Persona PedPersona3;
        Persona PedPersona4;
        string PedName1;
        string PedName2;
        string PedName3;
        string PedName4;
        string PedName5;
        string FITotal;
        int FINumber;
        string Address;
        string Occupation;
        string SSN;

        // Housekeeping
        private Label FiskeyLabel;
        private Label MDTLabel2;
        private Label MenuLabel1;
        private Label MenuLabel2;
        private Label MenuLabel3;
        private Label MenuLabel4;
        private Label MenuLabel5;
        private ProgressBar ProgressBar;

        /// <summary>
        /// FI Section
        /// </summary>
        // Labels
        private Label FIInfo;
        private Label FIPersonalInfo;
        private Label CreditLabel;
        private Label RemarkLabel;
        private Label SuspectLast;
        private Label SuspectFirst;
        private Label CurrentStoppedLabel;
        // Boxes
        private TextBox SuspectLastBox;
        private TextBox SuspectFirstBox;
        private TextBox CurrentStoppedBox1;
        private TextBox CurrentStoppedBox2;
        private TextBox CurrentStoppedBox3;
        private TextBox CurrentStoppedBox4;
        private MultilineTextBox RemarkBox;
        // Button
        private Button FISubmitButton;
        private Button FIBackButton;

        public FIRemarksCode2()
            : base(typeof(FIRemarksForm))
        {

        }

        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing FI Main");
                FISubmitButton.Clicked += OnFISubmitButtonClick;
                FIBackButton.Clicked += OnFIBackButtonClick;
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true)
                        {
                            PedPersona1 = pers;
                            PedName1 = pers.FullName;
                            CurrentStoppedBox1.Text = PedName1;
                        }
                    }
                }
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true && pers.FullName != PedPersona1.FullName)
                        {
                            PedPersona2 = pers;
                            PedName2 = pers.FullName;
                            CurrentStoppedBox2.Text = PedName2;
                        }
                    }
                }
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true && pers.FullName != PedPersona1.FullName && pers.FullName != PedPersona2.FullName)
                        {
                            PedPersona3 = pers;
                            PedName3 = pers.FullName;
                            CurrentStoppedBox3.Text = PedName3;
                        }
                    }
                }
                foreach (Ped ped in World.GetAllPeds())
                {
                    if (ped.Exists())
                    {
                        Persona pers = Functions.GetPersonaForPed(ped);
                        if (Functions.IsPedStoppedByPlayer(ped) == true && pers.FullName != PedPersona1.FullName && pers.FullName != PedPersona2.FullName && pers.FullName != PedPersona3.FullName)
                        {
                            PedPersona4 = pers;
                            PedName4 = pers.FullName;
                            CurrentStoppedBox4.Text = PedName4;
                        }
                    }
                }
                GameFiber.Yield();
            });
        }

        private void OnFISubmitButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Game.LogTrivial("FI remarks saving for: " + SuspectLastBox.Text.ToString() + " " + SuspectFirstBox.Text.ToString());
            using (StreamWriter Information = new StreamWriter("LSPDFR/MDT/FIs/" + SuspectLastBox.Text.ToLower() + SuspectFirstBox.Text.ToLower() + ".txt", true))
            {
                // 3 lines
                Information.WriteLine("---REMARKS---");
                Information.WriteLine("Remarks: " + RemarkBox.Text);
                Information.WriteLine("Date & Time Submitted: " + System.DateTime.Now.ToShortDateString() + System.DateTime.Now.ToShortTimeString());
            }
            Game.DisplayNotification("Page 3 of 3 saved.  Field interaction for: ~r~" + SuspectLastBox.Text.ToString() + ", " + SuspectFirstBox.Text.ToString() + " ~w~ completed successfully!");
            Window.Close();
        }

        private void OnFIBackButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            Window.Close();
        }
    }
}
