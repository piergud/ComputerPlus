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
using System.Drawing;
using ComputerPlus.Interfaces.ComputerReports;

namespace ComputerPlus
{
    internal class FIRemarksCode : GwenForm
    {
        string FITotal;

        private Persona PedPersona;

        private PedData _data;

        private Label FIInfo,FIPersonalInfo, CreditLabel, RemarkLabel, SuspectLast, SuspectFirst;
        private bool isPullover = false;

        private MultilineTextBox RemarkBox;

        private Button FISubmitButton;

        private TextBox first_box, last_box;

        public FIRemarksCode()
            : base(typeof(FIRemarksForm))
        {

        }
        
        public override void InitializeLayout()
        {
            GameFiber.StartNew(delegate
            {
                base.InitializeLayout();
                Game.LogTrivial("Initializing FI Remarks");
                this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);

                FISubmitButton.Clicked += OnFISubmitButtonClick;

                FillBoxes();

                GameFiber.Yield();
            });
        }

        private void FillBoxes()
        {
            _data = Carryover.Data;

            first_box.Text = _data.FirstName;
            last_box.Text = _data.LastName;
        }

        private void OnFISubmitButtonClick(Gwen.Control.Base sender, Gwen.Control.ClickedEventArgs e)
        {
            WriteData();

            Carryover.continuecarry = false;
            
            this.Window.Close();
        }

        private void WriteData()
        {
            Game.LogTrivial("FI remarks saving for: " + last_box.Text.ToString() + " " + first_box.Text.ToString());
            using (StreamWriter Information = new StreamWriter("Plugins/LSPDFR/ComputerPlus/field interviews/" + last_box.Text.ToLower() + first_box.Text.ToLower() + ".txt", true))
            {
                // 3 lines
                Information.WriteLine("---REMARKS---");
                Information.WriteLine("Remarks: " + RemarkBox.Text);
                Information.WriteLine("Date & Time Submitted: " + System.DateTime.Now.ToShortDateString() + System.DateTime.Now.ToShortTimeString());
            }
        }
    }
}
