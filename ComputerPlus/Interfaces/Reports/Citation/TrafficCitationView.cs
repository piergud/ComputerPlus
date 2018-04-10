using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gwen.Control;
using Gwen.Control.Layout;
using Rage.Forms;
using ComputerPlus.Controllers;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Extensions.Gwen;
using GwenSkin = Gwen.Skin;
using ComputerPlus.Interfaces.Common;
using Gwen;
using SystemDrawing = System.Drawing;
using Rage;
using System.Globalization;

namespace ComputerPlus.Interfaces.Reports.Citation
{
    class TrafficCitationView : Base
    {
        public enum TrafficCitationSaveResult { SAVE, SAVE_FAILED, SAVE_ERROR, CLEAR }
        public delegate void TrafficCitationActionEvent(object sender, TrafficCitationSaveResult action, TrafficCitation citation);
        internal event TrafficCitationActionEvent OnTrafficCitationAction;

        public enum ViewTypes { CREATE = 0, VIEW = 1 };

        internal static int DefaultWidth = 539;
        internal static int DefaultHeight = 810;

        bool ReadOnly;

        TrafficCitation mCitation;
        TrafficCitation Citation
        {
            get { return mCitation;  }
            set
            {
                mCitation = value;
                if (mCitation != null)
                {
                    if (mCitation.CitationTimeDate == DateTime.MinValue || mCitation.CitationTimeDate == null) mCitation.CitationTimeDate = DateTime.Now;
                    if (mCitation.CitationPos == Vector3.Zero) mCitation.CitationPos = Function.GetPedCurrentPos();
                    if (String.IsNullOrWhiteSpace(mCitation.CitationStreetAddress)) mCitation.CitationStreetAddress = Function.GetPedCurrentStreetName();
                    if (String.IsNullOrWhiteSpace(mCitation.CitationCity)) mCitation.CitationCity = Function.GetPedCurrentZoneName();
                }
            }
        }
        SystemDrawing.Color labelColor = SystemDrawing.Color.Black;

        Font labelFont, valueFont;

        LabeledComponent<ComboBox> labeled_vehicle_type;
        LabeledComponent<StateControlledTextbox> labeled_citation_report_id, labeled_first_name, labeled_last_name,
            labeled_dob, labeled_home_address, labeled_citation_street_address,
            labeled_citation_city, labeled_citation_time, labeled_citation_date,
            labeled_vehicle_model, labeled_vehicle_color, labeled_vehicle_tag;

        LabeledComponent<StateControlledMultilineTextbox> labeled_citation_details;

        Base headerSection, citationInformationContent, citationLocationContent, vehicleInformationContent, violationContent;
        FormSection citationeeInformationSection, citationLocationSection, vehicleInformationSection, violationSection;

        LabeledComponent<TreeControl> labeled_available_citation_reasons;

        LabeledComponent<Button> btn_finish, btn_finish_new;
        LabeledCheckBox court_case_flag;

        List<LabeledComponent<StateControlledTextbox>> LabeledInputs = new List<LabeledComponent<StateControlledTextbox>>();

        bool BindNeeded, DataBound;

        ViewTypes ViewType;

        TrafficCitationCreateContainer Container = null;

        public TrafficCitationView(Base parent, TrafficCitation citation, ViewTypes viewType = ViewTypes.CREATE, TrafficCitationActionEvent actionCallback = null) : this(null, parent, citation, viewType, actionCallback)
        {
        }

        public TrafficCitationView(TrafficCitationCreateContainer container, Base parent, TrafficCitation citation, ViewTypes viewType = ViewTypes.CREATE, TrafficCitationActionEvent actionCallback = null) : base(parent)
        {
            Container = container;
            Citation = citation;
            ViewType = viewType;
            ReadOnly = ViewType != ViewTypes.CREATE;
            if (actionCallback != null) OnTrafficCitationAction += actionCallback;
            if (ViewType == ViewTypes.CREATE && (citation.CitationTimeDate == null || citation.CitationTimeDate == DateTime.MinValue)) citation.CitationTimeDate = DateTime.Now;
            else Function.Log(String.Format("Citation time is {0}", citation.CitationTimeDate.ToString()));
            BindNeeded = true;
            InitializeLayout();
        }

        public void ChangeCitation(TrafficCitation citation, bool readOnly = false)
        {
            Function.LogDebug("ChangeCitation from " + Citation.Id() + " to " + citation.Id());
            Citation = citation;
            BindNeeded = true;
            if (!citation.IsNew) PopulateCitationCategories(null);
            labeled_available_citation_reasons.Component.UnselectAll();
            BindDataFromCitation();
        }

        public void InitializeLayout()
        {
            labelFont = this.Skin.DefaultFont.Copy();
            labelFont.Size = 14;
            labelFont.Smooth = true;

            headerSection = new Base(this);
            labeled_citation_report_id = LabeledComponent.StatefulTextbox(headerSection, "Citation", RelationalPosition.LEFT, Configs.BaseFormControlSpacing);
            LabeledInputs.Add(labeled_citation_report_id);


            citationeeInformationSection = new FormSection(this, "Personal Information");
            citationInformationContent = (new Base(this) { });

            labeled_first_name = LabeledComponent.StatefulTextbox(citationInformationContent, "First Name", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_last_name = LabeledComponent.StatefulTextbox(citationInformationContent, "Last Name", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_dob = LabeledComponent.StatefulTextbox(citationInformationContent, "DOB", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_home_address = LabeledComponent.StatefulTextbox(citationInformationContent, "Home Address", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            LabeledInputs.AddRange(new LabeledComponent<StateControlledTextbox>[] { labeled_first_name, labeled_last_name, labeled_dob, labeled_home_address });


            vehicleInformationSection = new FormSection(this, "Vehicle Information");
            vehicleInformationContent = new Base(this);
            labeled_vehicle_type = new LabeledComponent<ComboBox>(vehicleInformationContent, "Type", new ComboBox(vehicleInformationContent), RelationalPosition.TOP, RelationalSize.NONE, Configs.BaseFormControlSpacingHalf, labelFont, labelColor);
            labeled_vehicle_type.Component.AddItem("Select One", "PlaceHolder", String.Empty);
            labeled_vehicle_type.Component.AddItem("N/A", "N/A", "N/A");
            Globals.VehicleDefinitions.Types.Categories.ForEach(x => {
                labeled_vehicle_type.Component.AddItem(x.Value, x.Value, x.Value);
            });

            labeled_vehicle_model = LabeledComponent.StatefulTextbox(vehicleInformationContent, "Model", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_vehicle_color = LabeledComponent.StatefulTextbox(vehicleInformationContent, "Color", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_vehicle_tag = LabeledComponent.StatefulTextbox(vehicleInformationContent, "Tag", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            LabeledInputs.AddRange(new LabeledComponent<StateControlledTextbox>[] { labeled_vehicle_model, labeled_vehicle_color, labeled_vehicle_tag });

            citationLocationSection = new FormSection(this, "Violation Location");
            citationLocationContent = (new Base(this) { });

            labeled_citation_street_address = LabeledComponent.StatefulTextbox(citationLocationContent, "Street Address", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_citation_city = LabeledComponent.StatefulTextbox(citationLocationContent, "City", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_citation_date = LabeledComponent.StatefulTextbox(citationLocationContent, "Date", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
            labeled_citation_time = LabeledComponent.StatefulTextbox(citationLocationContent, "Time", RelationalPosition.TOP, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);

            LabeledInputs.AddRange(new LabeledComponent<StateControlledTextbox>[] { labeled_citation_street_address, labeled_citation_city, labeled_citation_date, labeled_citation_time });


            violationSection = new FormSection(this, "Violation");
            violationContent = new Base(this);

            labeled_available_citation_reasons = new LabeledComponent<TreeControl>(violationContent, "Citation", new TreeControl(violationContent), RelationalPosition.TOP, RelationalSize.NONE, Configs.BaseFormControlSpacingHalf, labelFont, labelColor);
            labeled_available_citation_reasons.Component.AllowMultiSelect = false;
            if (ViewType == ViewTypes.CREATE)
            {
                btn_finish = LabeledComponent.Button(headerSection, "Finish", RelationalPosition.BOTTOM, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
                btn_finish.Component.SaveIcon();
                btn_finish.Component.Clicked += ButtonClicked;

                btn_finish_new = LabeledComponent.Button(headerSection, "Add New", RelationalPosition.BOTTOM, Configs.BaseFormControlSpacingHalf, labelColor, labelFont);
                btn_finish_new.Component.PlusOneIcon();
                btn_finish_new.Component.Clicked += ButtonClicked;

                court_case_flag = new LabeledCheckBox(citationInformationContent);
                court_case_flag.Text = "Notice to Appear";
                if (Configs.EnableLSPDFRPlusIntegration && Function.IsLSPDFRPlusRunning())
                    court_case_flag.IsChecked = true;
                else
                    court_case_flag.IsChecked = false;

                PopulateCitationCategories(Globals.CitationDefinitions);
            }
            else PopulateCitationCategories(null);

            labeled_citation_details = new LabeledComponent<StateControlledMultilineTextbox>(violationContent, "Details", new StateControlledMultilineTextbox(violationContent), RelationalPosition.TOP, RelationalSize.NONE, Configs.BaseFormControlSpacingHalf, labelFont, labelColor);

            labeled_vehicle_type.Component.ItemSelected += ComponentPropChanged;
            labeled_available_citation_reasons.Component.SelectionChanged += ComponentPropChanged;
            LabeledInputs.ForEach(x => x.Component.TextChanged += ComponentPropChanged);
            labeled_citation_details.Component.TextChanged += ComponentPropChanged;

            LockControls(ReadOnly);
            //The below components should always be "read only"
            labeled_citation_report_id.Component.Disable();
            labeled_citation_date.Component.Disable();
            labeled_citation_time.Component.Disable();

        }


        private void ComponentPropChanged(Base sender, EventArgs arguments)
        {
            if (DataBound) UpdateCitationFromFields();
        }

        private void OnValidationError(object sender, Dictionary<String, String> errors)
        {
            ClearErrorState();
            if (errors != null && errors.Count > 0)
            {
                new MessageBox(this, "Citation is missing required information");
                foreach (KeyValuePair<String, String> kvp in errors)
                {
                    if (!String.IsNullOrEmpty(kvp.Key))
                    {
                        switch (kvp.Key)
                        {
                            case "First Name":
                                {
                                    labeled_first_name.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Last Name":
                                {
                                    labeled_last_name.Label.Error(kvp.Value);
                                    break;
                                }
                            case "DOB":
                                {
                                    labeled_dob.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Home Address":
                                {
                                    labeled_home_address.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Citation Street Address":
                                {
                                    labeled_citation_street_address.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Citation City":
                                {
                                    labeled_citation_city.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Citation Date":
                                {
                                    labeled_citation_date.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Citation Time":
                                {
                                    labeled_citation_time.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Citation Reason":
                                {
                                    labeled_available_citation_reasons.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Vehicle Type":
                                {
                                    labeled_vehicle_type.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Vehicle Model":
                                {
                                    labeled_vehicle_model.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Vehicle Color":
                                {
                                    labeled_vehicle_color.Label.Error(kvp.Value);
                                    break;
                                }
                            case "Vehicle Tag":
                                {
                                    labeled_vehicle_tag.Label.Error(kvp.Value);
                                    break;
                                }
                        }
                    }
                }
            }
        }

        private void ClearErrorState()
        {
            var labelsWithState = new Label[] {
                labeled_first_name.Label,
                labeled_last_name.Label,
                labeled_dob.Label,
                labeled_home_address.Label,
                labeled_vehicle_type.Label,
                labeled_vehicle_model.Label,
                labeled_vehicle_color.Label,
                labeled_vehicle_tag.Label,
                labeled_citation_street_address.Label,
                labeled_citation_city.Label,
                labeled_available_citation_reasons.Label,

            };            
            foreach (var label in labelsWithState) label.ClearError();
        }


        private void ButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender == btn_finish.Component || sender == btn_finish_new.Component)
            {
                Function.LogDebug("ButtonClicked");
                if (labeled_vehicle_type.Component.SelectedItem.UserData is String && String.IsNullOrEmpty(labeled_vehicle_type.Component.SelectedItem.UserData as String))
                {
                    OnValidationError(sender, new Dictionary<string, string>() { { "Vehicle Type", "Cannot be Select One" } });
                    NotifyForEvent(TrafficCitationSaveResult.SAVE_ERROR);
                    return;
                }

                UpdateCitationFromFields();

                Dictionary<String, String> failReasons;
                if (!Citation.Validate(out failReasons))
                {
                    OnValidationError(sender, failReasons);
                    NotifyForEvent(TrafficCitationSaveResult.SAVE_ERROR);
                }
                else
                {
                    try
                    {
                        Citation = ComputerReportsController.SaveTrafficCitation(Citation);                        
                        //if (Globals.PendingTrafficCitation == Citation) Globals.PendingTrafficCitation = null;
                        NotifyForEvent(TrafficCitationSaveResult.SAVE);
                        
                        if (sender == btn_finish_new.Component)
                        {
                            Globals.PendingTrafficCitation = TrafficCitation.CloneFromCitation(Citation);
                            ChangeCitation(Globals.PendingTrafficCitation);
                            new MessageBox(this, "Saved citation. Creating new");
                            BindNeeded = true;
                            BindDataFromCitation();
                        }
                        else
                        {
                            if (this.Container != null) this.Container.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        NotifyForEvent(TrafficCitationSaveResult.SAVE_FAILED);
                        new MessageBox(this, String.Format("Failed to save citation.. {0}", e.Message), "Failure saving");
                    }
                }
            }
        }

        private void NotifyForEvent(TrafficCitationSaveResult action)
        {
            if (OnTrafficCitationAction != null)
            {
                OnTrafficCitationAction(this, action, Citation);
            }
        }

        protected override void Layout(GwenSkin.Base skin)
        {
            base.Layout(skin);
            if (this.Parent == null && headerSection == null)
            {
                return;
            }            

            BindDataFromCitation();

            labeled_citation_report_id.Component.SmallSize();
            if (btn_finish != null) btn_finish.AlignRightWith().AlignTopWith(labeled_citation_report_id);
            if (btn_finish_new != null) btn_finish_new.PlaceLeftOf(btn_finish, Configs.BaseFormControlSpacingDouble * 2).AlignTopWith(btn_finish);
            headerSection.SizeWidthWith().AlignTopWith().AlignLeftWith().SizeToChildrenBlock();

            /* Persons Information */

            citationeeInformationSection
             .AddContentChild(citationInformationContent)
             .PlaceBelowOf(headerSection, 0)
             .SizeWidthWith();

            labeled_last_name.PlaceRightOf(labeled_first_name, Configs.BaseFormControlSpacingHalf);
            labeled_dob.PlaceRightOf(labeled_last_name, Configs.BaseFormControlSpacingHalf);
            labeled_dob.Component.SmallSize();
            labeled_home_address.PlaceBelowOf(labeled_first_name);
            if (ViewType == ViewTypes.CREATE)
            {
                court_case_flag
                    .PlaceBelowOf(labeled_last_name, Configs.BaseFormControlSpacingDouble + 7)
                    .AlignLeftWith(labeled_last_name);
                court_case_flag.X = court_case_flag.X + Configs.BaseFormControlSpacingDouble * 3;
            }

            citationInformationContent.SizeToChildren(false, true);
            citationeeInformationSection.SizeToChildren(false, true);

            /* Vehicle Information */

            vehicleInformationSection
             .AddContentChild(vehicleInformationContent)
             .PlaceBelowOf(citationeeInformationSection)
             .SizeWidthWith();

            labeled_vehicle_model.Component.SmallSize();
            labeled_vehicle_color.Component.SmallSize();
            labeled_vehicle_tag.Component.SmallSize();

            labeled_vehicle_model.PlaceRightOf(labeled_vehicle_type, Configs.BaseFormControlSpacingHalf);
            labeled_vehicle_color.PlaceRightOf(labeled_vehicle_model, Configs.BaseFormControlSpacingHalf);
            labeled_vehicle_tag.PlaceRightOf(labeled_vehicle_color, Configs.BaseFormControlSpacingHalf);

            vehicleInformationContent.SizeToChildren(false, true);
            vehicleInformationSection.SizeToChildren(false, true);

            /* Location Information */

            citationLocationSection
             .AddContentChild(citationLocationContent)
             .PlaceBelowOf(vehicleInformationSection)
             .SizeWidthWith();            

            labeled_citation_date.Component.SmallSize();
            labeled_citation_time.Component.SmallSize();
            labeled_citation_city.Component.MediumSize();

            labeled_citation_date.PlaceRightOf(labeled_citation_street_address, Configs.BaseFormControlSpacingTriple * 2);
            labeled_citation_city.PlaceBelowOf(labeled_citation_street_address);
            labeled_citation_time.Align(labeled_citation_date, labeled_citation_city);

            citationLocationContent.SizeToChildren(false, true);
            citationLocationSection.SizeToChildren(false, true);

            /* Citation Details */

            violationSection
             .AddContentChild(violationContent)
             .PlaceBelowOf(citationLocationSection)
             .SizeWidthWith();

            labeled_available_citation_reasons
                .SizeWidthWith()
                .SetHeight(225)
                .SizeChildrenWidth()
                .SizeChildrenHeight(null, 30);

            labeled_citation_details
                .PlaceBelowOf(labeled_available_citation_reasons, 5)
                .SizeWidthWith()
                .SetHeight(68)
                .SizeChildrenWidth()
                .SizeChildrenHeight(null, 30);

            violationContent.SizeToChildren(false, true);
            violationSection.SizeToChildren(false, true);
            
        }

        private void TransverseTrafficCitations(TreeNode parent, CitationDefinition charge)
        {
            if (charge.IsContainer)
            {
                var container = parent.AddNode(charge.Name);
                container.IsSelectable = false;
                charge.Children.ForEach(childCharge =>
                TransverseTrafficCitations(container, childCharge));
            }
            else {
                var child = parent.AddNode(String.Format("{0}{1}", charge.Name, charge.IsArrestable ? " (A)" : String.Empty, charge));
                child.UserData = charge;
                child.LabelPressed += CitationTreeItemSelected;
            }
        }

        private void PopulateCitationCategories(CitationCategories categories)
        {
            labeled_available_citation_reasons.Component.RemoveAll();
            if (categories == null)
            {
                try
                {
                    var parent = labeled_available_citation_reasons.Component.AddNode(Citation.CitationReason);
                    try
                    {
                        parent.AddNode(String.Format("Fine: {0}", Citation.CitationAmount.ToString("c", CultureInfo.CurrentCulture)));
                        parent.AddNode(String.Format("Arrestable: {0}", Citation.IsArrestable));
                    }
                    catch
                    {
                    }
                }
                catch
                {
                    labeled_available_citation_reasons.Component.AddNode("Unknown");
                }
            }
            else {
                categories.Categories.ForEach(x =>
                {
                    var parent = labeled_available_citation_reasons.Component.AddNode(x.Name);
                    x.Citations.ForEach(charge => TransverseTrafficCitations(parent, charge));
                });
            }
        }

        private void CitationTreeItemSelected(Base sender, EventArgs arguments)
        {
            if (sender == null || ViewType != ViewTypes.CREATE)
            {
            }
            else if (sender.UserData is CitationDefinition)
            {
                var selectedCharge = sender.UserData as CitationDefinition;
                if (selectedCharge.IsContainer) return;
                Citation.Citation = selectedCharge;
            }

        }

        private void LockControls(bool disable)
        {
            StateControlledTextbox[] textboxes = new StateControlledTextbox[]
            {
                labeled_first_name.Component,
                labeled_last_name.Component,
                labeled_dob.Component,
                labeled_home_address.Component,
                labeled_citation_street_address.Component,
                labeled_citation_city.Component,
                labeled_vehicle_color.Component,
                labeled_vehicle_model.Component,
                labeled_vehicle_tag.Component,                               
            };

            if (disable)
            {
                foreach (var textbox in textboxes) textbox.Disable();
                labeled_vehicle_type.Component.Disable();
                labeled_available_citation_reasons.Component.Disable();
                labeled_citation_details.Component.Disable();
            }
            else
            {
                foreach (var textbox in textboxes) textbox.Enable();
                labeled_vehicle_type.Component.Enable();
                labeled_available_citation_reasons.Component.Enable();
                labeled_citation_details.Component.Enable();
            }           

        }

        private void UpdateCitationFromFields()
        {
            if (!ReadOnly)
            {
                Citation.FirstName = labeled_first_name.Component.Text;
                Citation.LastName = labeled_last_name.Component.Text;
                Citation.DOB = labeled_dob.Component.Text;
                Citation.HomeAddress = labeled_home_address.Component.Text;
                Citation.VehicleType = labeled_vehicle_type.Component.SelectedItem.Name;
                Citation.VehicleModel = labeled_vehicle_model.Component.Text;
                Citation.VehicleColor = labeled_vehicle_color.Component.Text;
                Citation.VehicleTag = labeled_vehicle_tag.Component.Text;
                Citation.CitationStreetAddress = labeled_citation_street_address.Component.Text;
                Citation.CitationCity = labeled_citation_city.Component.Text;
                Citation.Details = labeled_citation_details.Component.Text;
                Citation.CreateCourtCase = court_case_flag.IsChecked;
            }
        }

        private void BindDataFromCitation()
        {
            if (Citation == null)
            {
                Function.Log("Citation is null");
                return;
            }
            if (!BindNeeded)
            {
                Function.Log("Bind not needed " + Citation.FirstName);
                return;
            }
            BindNeeded = false;
            
            lock (Citation)
            {
                labeled_citation_report_id.SetValueText(Citation.ShortId());
                labeled_first_name.SetValueText(Citation.FirstName);
                labeled_last_name.SetValueText(Citation.LastName);
                labeled_dob.SetValueText(Citation.DOB);
                labeled_home_address.SetValueText(Citation.HomeAddress);

                labeled_vehicle_type.Component.Enable();
                if (Citation.VehicleModel.Equals("N/A"))
                    labeled_vehicle_type.Component.SelectByText(Citation.VehicleModel);
                else
                    labeled_vehicle_type.Component.SelectByText(Citation.VehicleType);
                if (ReadOnly) labeled_vehicle_type.Component.Disable();

                labeled_vehicle_model.SetValueText(Citation.VehicleModel);
                labeled_vehicle_color.SetValueText(Citation.VehicleColor);
                labeled_vehicle_tag.SetValueText(Citation.VehicleTag);

                labeled_citation_street_address.SetValueText(Citation.CitationStreetAddress);
                labeled_citation_city.SetValueText(Citation.CitationCity);
                labeled_citation_date.SetValueText(Function.ToLocalDateString(Citation.CitationTimeDate, TextBoxExtensions.DateOutputPart.DATE));
                labeled_citation_time.SetValueText(Function.ToLocalDateString(Citation.CitationTimeDate, TextBoxExtensions.DateOutputPart.TIME));

                labeled_citation_details.Component.ClearText();
                labeled_citation_details.SetValueText(Citation.Details);
            }

            DataBound = true;
        }
     
    }

    class TrafficCitationCreateContainer : GwenForm
    {
        TrafficCitationView trafficCitationCreate;
        TrafficCitation Citation;
        TrafficCitationView.ViewTypes ViewType;
        TrafficCitationView.TrafficCitationActionEvent Callback;

        internal TrafficCitationCreateContainer(TrafficCitation citation, TrafficCitationView.ViewTypes viewType = TrafficCitationView.ViewTypes.CREATE, TrafficCitationView.TrafficCitationActionEvent actionCallback = null) : base("Citation", TrafficCitationView.DefaultWidth, TrafficCitationView.DefaultHeight)
        {
            Citation = citation;
            ViewType = viewType;
            Callback = actionCallback;
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            this.Position = this.GetLaunchPosition();
            trafficCitationCreate = new TrafficCitationView(this, this, Citation, ViewType, Callback);
            trafficCitationCreate.Dock = Gwen.Pos.Fill;
        }

        public void ChangeCitation(TrafficCitation citation)
        {
            Citation = citation;
            trafficCitationCreate.ChangeCitation(Citation);
        }
    }
}
