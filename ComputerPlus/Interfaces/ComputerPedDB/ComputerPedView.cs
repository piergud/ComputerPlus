using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage.Forms;
using Gwen;
using Gwen.Control;
using Rage;
using LSPD_First_Response;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus.Interfaces.ComputerPedDB
{
    sealed class ComputerPedView : GwenForm
    {
        TextBox text_first_name, text_last_name, 
                text_home_address, text_dob, text_license_status, 
                text_wanted_status,  text_times_stopped, text_age;
        Label lbl_alert;
        Button btn_ped_image_holder;

        Persona Persona;
        Ped Ped;

        internal ComputerPedView(Persona persona, Ped ped) : base(typeof(ComputerPedViewTemplate))
        {
            Persona = persona;
            Ped = ped;
        }
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            Game.LogVerboseDebug("ComputerPedView InitializeLayout");
            btn_ped_image_holder.SetImage(DetermineImagePath(Ped), true);
            this.Position = this.GetLaunchPosition();
            if (Persona == null || Ped == null) return;            
            text_first_name.KeyboardInputEnabled = false;
            text_last_name.KeyboardInputEnabled = false;
            text_home_address.KeyboardInputEnabled = false;
            text_dob.KeyboardInputEnabled = false;
            text_license_status.KeyboardInputEnabled = false;
            text_wanted_status.KeyboardInputEnabled = false;
            text_times_stopped.KeyboardInputEnabled = false;
            
            BindData();

        }

        private String DetermineImagePath(Ped ped)
        {

            String modelName = ped.Model.Name;
            int headDrawableIndex, headDrawableTextureIndex;

            ped.GetVariation(0, out headDrawableIndex, out headDrawableTextureIndex);

            String _model = String.Format(@"{0}__0_{1}_{2}", modelName, headDrawableIndex, headDrawableTextureIndex).ToLower();
            try
            {
                var path = Function.GetPedImagePath(_model);
                Game.LogVerboseDebug(String.Format("Loading image for model from  {0}", path));
                return path;
            }
            catch
            {
                Game.LogVerboseDebug("DetermineImagePath Error");
                return null;
            }
        }

        private void BindData()
        {
            if(Ped.Metadata.HomeAddress == null)
            {
                Ped.Metadata.HomeAddress = ComputerPedController.GetRandomStreetAddress();
            }
            switch (Persona.LicenseState)
            {
                case ELicenseState.Expired:
                    text_license_status.Text = "Expired";
                    break;
                case ELicenseState.None:
                    text_license_status.Text = "None";
                    break;
                case ELicenseState.Suspended:
                    text_license_status.Text = "Suspended";
                    break;
                case ELicenseState.Valid:
                    text_license_status.Text = "Valid";
                    break;
            }

            if(Persona.IsAgent)
            {
                lbl_alert.SetText("Individual is a federal agent");
            }
            else if (Persona.IsCop)
            {
                lbl_alert.SetText("Individual is a police officer");
            }

            var age = (DateTime.Today - Persona.BirthDay).Days / 365.25m;
            text_age.Text = ((int)age).ToString();
            text_first_name.Text = Persona.Forename;
            text_last_name.Text = Persona.Surname;
            text_home_address.Text = Ped.Metadata.HomeAddress;
            text_dob.Text = Persona.BirthDay.ToString("MM/dd/yyyy");
            text_dob.SetToolTipText("MM/dd/yyyy");
            text_wanted_status.Text = (Persona.Wanted) ? "Wanted" : "None";
            text_times_stopped.Text = Persona.TimesStopped.ToString();
            
        }
    }
}
