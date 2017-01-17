namespace Notsolethalpolicing.MDT
{
    partial class CitationViolationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CitationAreaBox = new System.Windows.Forms.ComboBox();
            this.CitationSubmitButton = new System.Windows.Forms.Button();
            this.CitationExtraInfoBox = new System.Windows.Forms.TextBox();
            this.ExtraInfo = new System.Windows.Forms.Label();
            this.Violations = new System.Windows.Forms.Label();
            this.StreetBox = new System.Windows.Forms.TextBox();
            this.CitationTrafficConditionBox = new System.Windows.Forms.ComboBox();
            this.CitationLightConditionBox = new System.Windows.Forms.ComboBox();
            this.CitationStreetConditionBox = new System.Windows.Forms.ComboBox();
            this.CitationWeatherBox = new System.Windows.Forms.ComboBox();
            this.AccidentCheck = new System.Windows.Forms.CheckBox();
            this.VehPlateBox = new System.Windows.Forms.TextBox();
            this.VehInfoBox = new System.Windows.Forms.TextBox();
            this.SpeedBox = new System.Windows.Forms.TextBox();
            this.CitationSpeedDeviceBox = new System.Windows.Forms.ComboBox();
            this.CitationViolationBox = new System.Windows.Forms.TextBox();
            this.CitationStreet = new System.Windows.Forms.Label();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.Operated = new System.Windows.Forms.Label();
            this.VehCheck1 = new System.Windows.Forms.CheckBox();
            this.VehCheck2 = new System.Windows.Forms.CheckBox();
            this.VehCheck3 = new System.Windows.Forms.CheckBox();
            this.VehCheck4 = new System.Windows.Forms.CheckBox();
            this.VehCheck5 = new System.Windows.Forms.CheckBox();
            this.VehInfo = new System.Windows.Forms.Label();
            this.VehPlate = new System.Windows.Forms.Label();
            this.CitationCity = new System.Windows.Forms.Label();
            this.CityBox = new System.Windows.Forms.TextBox();
            this.Conditions = new System.Windows.Forms.Label();
            this.CommittedOffenses = new System.Windows.Forms.Label();
            this.Defendent = new System.Windows.Forms.Label();
            this.SpeedCheck = new System.Windows.Forms.CheckBox();
            this.Area = new System.Windows.Forms.Label();
            this.CourtInfo = new System.Windows.Forms.Label();
            this.Fail = new System.Windows.Forms.Label();
            this.InABox = new System.Windows.Forms.TextBox();
            this.Ina2 = new System.Windows.Forms.Label();
            this.offense_other_check = new System.Windows.Forms.CheckBox();
            this.offense_other_box = new System.Windows.Forms.TextBox();
            this.speed_type_lbl = new System.Windows.Forms.Label();
            this.summons_box = new System.Windows.Forms.TextBox();
            this.weather_label = new System.Windows.Forms.Label();
            this.street_lbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.veh_other_box = new System.Windows.Forms.TextBox();
            this._areaOtherBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CitationAreaBox
            // 
            this.CitationAreaBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitationAreaBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationAreaBox.FormattingEnabled = true;
            this.CitationAreaBox.Items.AddRange(new object[] {
            "Business",
            "Industrial",
            "Residential",
            "Other"});
            this.CitationAreaBox.Location = new System.Drawing.Point(580, 172);
            this.CitationAreaBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationAreaBox.Name = "CitationAreaBox";
            this.CitationAreaBox.Size = new System.Drawing.Size(112, 23);
            this.CitationAreaBox.TabIndex = 144;
            // 
            // CitationSubmitButton
            // 
            this.CitationSubmitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationSubmitButton.ForeColor = System.Drawing.Color.Blue;
            this.CitationSubmitButton.Location = new System.Drawing.Point(586, 434);
            this.CitationSubmitButton.Margin = new System.Windows.Forms.Padding(2);
            this.CitationSubmitButton.Name = "CitationSubmitButton";
            this.CitationSubmitButton.Size = new System.Drawing.Size(175, 97);
            this.CitationSubmitButton.TabIndex = 133;
            this.CitationSubmitButton.Text = "Submit Citation";
            this.CitationSubmitButton.UseVisualStyleBackColor = true;
            // 
            // CitationExtraInfoBox
            // 
            this.CitationExtraInfoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationExtraInfoBox.Location = new System.Drawing.Point(143, 434);
            this.CitationExtraInfoBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationExtraInfoBox.Multiline = true;
            this.CitationExtraInfoBox.Name = "CitationExtraInfoBox";
            this.CitationExtraInfoBox.Size = new System.Drawing.Size(438, 97);
            this.CitationExtraInfoBox.TabIndex = 126;
            // 
            // ExtraInfo
            // 
            this.ExtraInfo.AutoSize = true;
            this.ExtraInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ExtraInfo.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ExtraInfo.Location = new System.Drawing.Point(140, 417);
            this.ExtraInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExtraInfo.Name = "ExtraInfo";
            this.ExtraInfo.Size = new System.Drawing.Size(161, 15);
            this.ExtraInfo.TabIndex = 132;
            this.ExtraInfo.Text = "Additional Information/Notes";
            // 
            // Violations
            // 
            this.Violations.AutoSize = true;
            this.Violations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Violations.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Violations.Location = new System.Drawing.Point(140, 347);
            this.Violations.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Violations.Name = "Violations";
            this.Violations.Size = new System.Drawing.Size(68, 15);
            this.Violations.TabIndex = 129;
            this.Violations.Text = "Violation(s)";
            // 
            // StreetBox
            // 
            this.StreetBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.StreetBox.Location = new System.Drawing.Point(143, 172);
            this.StreetBox.Margin = new System.Windows.Forms.Padding(2);
            this.StreetBox.Name = "StreetBox";
            this.StreetBox.Size = new System.Drawing.Size(288, 21);
            this.StreetBox.TabIndex = 116;
            // 
            // CitationTrafficConditionBox
            // 
            this.CitationTrafficConditionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitationTrafficConditionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationTrafficConditionBox.FormattingEnabled = true;
            this.CitationTrafficConditionBox.Items.AddRange(new object[] {
            "Heavy",
            "Medium",
            "Light",
            "None",
            "N/A"});
            this.CitationTrafficConditionBox.Location = new System.Drawing.Point(481, 235);
            this.CitationTrafficConditionBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationTrafficConditionBox.Name = "CitationTrafficConditionBox";
            this.CitationTrafficConditionBox.Size = new System.Drawing.Size(95, 23);
            this.CitationTrafficConditionBox.TabIndex = 120;
            // 
            // CitationLightConditionBox
            // 
            this.CitationLightConditionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitationLightConditionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationLightConditionBox.FormattingEnabled = true;
            this.CitationLightConditionBox.Items.AddRange(new object[] {
            "1 - Direct Sunlight",
            "2 - Dimmed Sunlight/Moonlight",
            "3 - Shaded",
            "4 - Dusk/Dawn",
            "5 - No/Limited Moonlight"});
            this.CitationLightConditionBox.Location = new System.Drawing.Point(371, 235);
            this.CitationLightConditionBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationLightConditionBox.Name = "CitationLightConditionBox";
            this.CitationLightConditionBox.Size = new System.Drawing.Size(106, 23);
            this.CitationLightConditionBox.TabIndex = 119;
            // 
            // CitationStreetConditionBox
            // 
            this.CitationStreetConditionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitationStreetConditionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationStreetConditionBox.FormattingEnabled = true;
            this.CitationStreetConditionBox.Items.AddRange(new object[] {
            "Paved - Good",
            "Paved - Potholes",
            "Dirt - Good",
            "Dirt - Potholes",
            "Other",
            "N/A"});
            this.CitationStreetConditionBox.Location = new System.Drawing.Point(248, 235);
            this.CitationStreetConditionBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationStreetConditionBox.Name = "CitationStreetConditionBox";
            this.CitationStreetConditionBox.Size = new System.Drawing.Size(119, 23);
            this.CitationStreetConditionBox.TabIndex = 118;
            // 
            // CitationWeatherBox
            // 
            this.CitationWeatherBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitationWeatherBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationWeatherBox.FormattingEnabled = true;
            this.CitationWeatherBox.Items.AddRange(new object[] {
            "Clear",
            "Light Rain",
            "Heavy Rain",
            "Thunderstorm",
            "Snow",
            "Other"});
            this.CitationWeatherBox.Location = new System.Drawing.Point(143, 235);
            this.CitationWeatherBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationWeatherBox.Name = "CitationWeatherBox";
            this.CitationWeatherBox.Size = new System.Drawing.Size(101, 23);
            this.CitationWeatherBox.TabIndex = 117;
            // 
            // AccidentCheck
            // 
            this.AccidentCheck.AutoSize = true;
            this.AccidentCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.AccidentCheck.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.AccidentCheck.Location = new System.Drawing.Point(143, 277);
            this.AccidentCheck.Margin = new System.Windows.Forms.Padding(2);
            this.AccidentCheck.Name = "AccidentCheck";
            this.AccidentCheck.Size = new System.Drawing.Size(72, 19);
            this.AccidentCheck.TabIndex = 135;
            this.AccidentCheck.TabStop = false;
            this.AccidentCheck.Text = "Accident";
            this.AccidentCheck.UseVisualStyleBackColor = true;
            // 
            // VehPlateBox
            // 
            this.VehPlateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehPlateBox.Location = new System.Drawing.Point(467, 132);
            this.VehPlateBox.Margin = new System.Windows.Forms.Padding(2);
            this.VehPlateBox.Name = "VehPlateBox";
            this.VehPlateBox.Size = new System.Drawing.Size(109, 21);
            this.VehPlateBox.TabIndex = 124;
            // 
            // VehInfoBox
            // 
            this.VehInfoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehInfoBox.Location = new System.Drawing.Point(143, 132);
            this.VehInfoBox.Margin = new System.Windows.Forms.Padding(2);
            this.VehInfoBox.Name = "VehInfoBox";
            this.VehInfoBox.Size = new System.Drawing.Size(320, 21);
            this.VehInfoBox.TabIndex = 123;
            // 
            // SpeedBox
            // 
            this.SpeedBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.SpeedBox.Location = new System.Drawing.Point(209, 298);
            this.SpeedBox.Margin = new System.Windows.Forms.Padding(2);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(66, 21);
            this.SpeedBox.TabIndex = 122;
            this.SpeedBox.Text = "Value";
            // 
            // CitationSpeedDeviceBox
            // 
            this.CitationSpeedDeviceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitationSpeedDeviceBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationSpeedDeviceBox.FormattingEnabled = true;
            this.CitationSpeedDeviceBox.Items.AddRange(new object[] {
            "N/A",
            "Radar",
            "Laser",
            "Pace",
            "Other"});
            this.CitationSpeedDeviceBox.Location = new System.Drawing.Point(359, 298);
            this.CitationSpeedDeviceBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationSpeedDeviceBox.Name = "CitationSpeedDeviceBox";
            this.CitationSpeedDeviceBox.Size = new System.Drawing.Size(85, 23);
            this.CitationSpeedDeviceBox.TabIndex = 121;
            // 
            // CitationViolationBox
            // 
            this.CitationViolationBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationViolationBox.Location = new System.Drawing.Point(143, 364);
            this.CitationViolationBox.Margin = new System.Windows.Forms.Padding(2);
            this.CitationViolationBox.Multiline = true;
            this.CitationViolationBox.Name = "CitationViolationBox";
            this.CitationViolationBox.Size = new System.Drawing.Size(433, 51);
            this.CitationViolationBox.TabIndex = 125;
            // 
            // CitationStreet
            // 
            this.CitationStreet.AutoSize = true;
            this.CitationStreet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationStreet.ForeColor = System.Drawing.SystemColors.Desktop;
            this.CitationStreet.Location = new System.Drawing.Point(140, 155);
            this.CitationStreet.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CitationStreet.Name = "CitationStreet";
            this.CitationStreet.Size = new System.Drawing.Size(180, 15);
            this.CitationStreet.TabIndex = 152;
            this.CitationStreet.Text = "Upon a public highway, namely:";
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.InfoLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.InfoLabel.Location = new System.Drawing.Point(56, 45);
            this.InfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(136, 17);
            this.InfoLabel.TabIndex = 163;
            this.InfoLabel.Text = "Violation Information";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(298, 536);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(196, 20);
            this.ProgressBar.Step = 5;
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressBar.TabIndex = 182;
            this.ProgressBar.Value = 100;
            // 
            // Operated
            // 
            this.Operated.AutoSize = true;
            this.Operated.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Operated.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Operated.Location = new System.Drawing.Point(113, 77);
            this.Operated.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Operated.Name = "Operated";
            this.Operated.Size = new System.Drawing.Size(173, 15);
            this.Operated.TabIndex = 183;
            this.Operated.Text = "You operated/parked/walked a";
            // 
            // VehCheck1
            // 
            this.VehCheck1.AutoSize = true;
            this.VehCheck1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehCheck1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.VehCheck1.Location = new System.Drawing.Point(143, 94);
            this.VehCheck1.Margin = new System.Windows.Forms.Padding(2);
            this.VehCheck1.Name = "VehCheck1";
            this.VehCheck1.Size = new System.Drawing.Size(85, 19);
            this.VehCheck1.TabIndex = 184;
            this.VehCheck1.TabStop = false;
            this.VehCheck1.Text = "Passenger";
            this.VehCheck1.UseVisualStyleBackColor = true;
            // 
            // VehCheck2
            // 
            this.VehCheck2.AutoSize = true;
            this.VehCheck2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehCheck2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.VehCheck2.Location = new System.Drawing.Point(227, 94);
            this.VehCheck2.Margin = new System.Windows.Forms.Padding(2);
            this.VehCheck2.Name = "VehCheck2";
            this.VehCheck2.Size = new System.Drawing.Size(93, 19);
            this.VehCheck2.TabIndex = 185;
            this.VehCheck2.TabStop = false;
            this.VehCheck2.Text = "Commercial";
            this.VehCheck2.UseVisualStyleBackColor = true;
            // 
            // VehCheck3
            // 
            this.VehCheck3.AutoSize = true;
            this.VehCheck3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehCheck3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.VehCheck3.Location = new System.Drawing.Point(324, 94);
            this.VehCheck3.Margin = new System.Windows.Forms.Padding(2);
            this.VehCheck3.Name = "VehCheck3";
            this.VehCheck3.Size = new System.Drawing.Size(55, 19);
            this.VehCheck3.TabIndex = 186;
            this.VehCheck3.TabStop = false;
            this.VehCheck3.Text = "Cycle";
            this.VehCheck3.UseVisualStyleBackColor = true;
            // 
            // VehCheck4
            // 
            this.VehCheck4.AutoSize = true;
            this.VehCheck4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehCheck4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.VehCheck4.Location = new System.Drawing.Point(383, 94);
            this.VehCheck4.Margin = new System.Windows.Forms.Padding(2);
            this.VehCheck4.Name = "VehCheck4";
            this.VehCheck4.Size = new System.Drawing.Size(66, 19);
            this.VehCheck4.TabIndex = 187;
            this.VehCheck4.TabStop = false;
            this.VehCheck4.Text = "Service";
            this.VehCheck4.UseVisualStyleBackColor = true;
            // 
            // VehCheck5
            // 
            this.VehCheck5.AutoSize = true;
            this.VehCheck5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehCheck5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.VehCheck5.Location = new System.Drawing.Point(453, 94);
            this.VehCheck5.Margin = new System.Windows.Forms.Padding(2);
            this.VehCheck5.Name = "VehCheck5";
            this.VehCheck5.Size = new System.Drawing.Size(56, 19);
            this.VehCheck5.TabIndex = 188;
            this.VehCheck5.TabStop = false;
            this.VehCheck5.Text = "Other";
            this.VehCheck5.UseVisualStyleBackColor = true;
            // 
            // VehInfo
            // 
            this.VehInfo.AutoSize = true;
            this.VehInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehInfo.ForeColor = System.Drawing.SystemColors.Desktop;
            this.VehInfo.Location = new System.Drawing.Point(140, 115);
            this.VehInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VehInfo.Name = "VehInfo";
            this.VehInfo.Size = new System.Drawing.Size(222, 15);
            this.VehInfo.TabIndex = 189;
            this.VehInfo.Text = "Vehicle Make, Model, Color, Body Type:";
            // 
            // VehPlate
            // 
            this.VehPlate.AutoSize = true;
            this.VehPlate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.VehPlate.ForeColor = System.Drawing.SystemColors.Desktop;
            this.VehPlate.Location = new System.Drawing.Point(464, 115);
            this.VehPlate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VehPlate.Name = "VehPlate";
            this.VehPlate.Size = new System.Drawing.Size(84, 15);
            this.VehPlate.TabIndex = 190;
            this.VehPlate.Text = "License Plate:";
            // 
            // CitationCity
            // 
            this.CitationCity.AutoSize = true;
            this.CitationCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CitationCity.ForeColor = System.Drawing.SystemColors.Desktop;
            this.CitationCity.Location = new System.Drawing.Point(432, 155);
            this.CitationCity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CitationCity.Name = "CitationCity";
            this.CitationCity.Size = new System.Drawing.Size(95, 15);
            this.CitationCity.TabIndex = 192;
            this.CitationCity.Text = "in the City/Town:";
            // 
            // CityBox
            // 
            this.CityBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CityBox.Location = new System.Drawing.Point(435, 172);
            this.CityBox.Margin = new System.Windows.Forms.Padding(2);
            this.CityBox.Name = "CityBox";
            this.CityBox.Size = new System.Drawing.Size(141, 21);
            this.CityBox.TabIndex = 191;
            // 
            // Conditions
            // 
            this.Conditions.AutoSize = true;
            this.Conditions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Conditions.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Conditions.Location = new System.Drawing.Point(140, 195);
            this.Conditions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Conditions.Name = "Conditions";
            this.Conditions.Size = new System.Drawing.Size(151, 15);
            this.Conditions.TabIndex = 193;
            this.Conditions.Text = "In the following conditions:";
            // 
            // CommittedOffenses
            // 
            this.CommittedOffenses.AutoSize = true;
            this.CommittedOffenses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CommittedOffenses.ForeColor = System.Drawing.SystemColors.Desktop;
            this.CommittedOffenses.Location = new System.Drawing.Point(140, 260);
            this.CommittedOffenses.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CommittedOffenses.Name = "CommittedOffenses";
            this.CommittedOffenses.Size = new System.Drawing.Size(213, 15);
            this.CommittedOffenses.TabIndex = 194;
            this.CommittedOffenses.Text = "And committed the following offenses:";
            // 
            // Defendent
            // 
            this.Defendent.AutoSize = true;
            this.Defendent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Defendent.ForeColor = System.Drawing.Color.Red;
            this.Defendent.Location = new System.Drawing.Point(113, 62);
            this.Defendent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Defendent.Name = "Defendent";
            this.Defendent.Size = new System.Drawing.Size(143, 15);
            this.Defendent.TabIndex = 195;
            this.Defendent.Text = "To Defendent: Complaint";
            // 
            // SpeedCheck
            // 
            this.SpeedCheck.AutoSize = true;
            this.SpeedCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.SpeedCheck.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.SpeedCheck.Location = new System.Drawing.Point(143, 300);
            this.SpeedCheck.Margin = new System.Windows.Forms.Padding(2);
            this.SpeedCheck.Name = "SpeedCheck";
            this.SpeedCheck.Size = new System.Drawing.Size(62, 19);
            this.SpeedCheck.TabIndex = 196;
            this.SpeedCheck.TabStop = false;
            this.SpeedCheck.Text = "Speed";
            this.SpeedCheck.UseVisualStyleBackColor = true;
            // 
            // Area
            // 
            this.Area.AutoSize = true;
            this.Area.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Area.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Area.Location = new System.Drawing.Point(577, 155);
            this.Area.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(35, 15);
            this.Area.TabIndex = 198;
            this.Area.Text = "Area:";
            // 
            // CourtInfo
            // 
            this.CourtInfo.AutoSize = true;
            this.CourtInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.CourtInfo.ForeColor = System.Drawing.Color.Red;
            this.CourtInfo.Location = new System.Drawing.Point(590, 264);
            this.CourtInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CourtInfo.Name = "CourtInfo";
            this.CourtInfo.Size = new System.Drawing.Size(176, 15);
            this.CourtInfo.TabIndex = 200;
            this.CourtInfo.Text = "YOU ARE SUMMONED ON";
            // 
            // Fail
            // 
            this.Fail.AutoSize = true;
            this.Fail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Fail.ForeColor = System.Drawing.Color.Red;
            this.Fail.Location = new System.Drawing.Point(602, 327);
            this.Fail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Fail.Name = "Fail";
            this.Fail.Size = new System.Drawing.Size(147, 30);
            this.Fail.TabIndex = 202;
            this.Fail.Text = "IF YOU FAIL TO APPEAR \r\nYOU MAY BE ARRESTED";
            // 
            // InABox
            // 
            this.InABox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.InABox.Location = new System.Drawing.Point(279, 298);
            this.InABox.Margin = new System.Windows.Forms.Padding(2);
            this.InABox.Name = "InABox";
            this.InABox.Size = new System.Drawing.Size(74, 21);
            this.InABox.TabIndex = 203;
            // 
            // Ina2
            // 
            this.Ina2.AutoSize = true;
            this.Ina2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Ina2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Ina2.Location = new System.Drawing.Point(278, 281);
            this.Ina2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Ina2.Name = "Ina2";
            this.Ina2.Size = new System.Drawing.Size(78, 15);
            this.Ina2.TabIndex = 204;
            this.Ina2.Text = "Posted Limit:";
            // 
            // offense_other_check
            // 
            this.offense_other_check.AutoSize = true;
            this.offense_other_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.offense_other_check.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.offense_other_check.Location = new System.Drawing.Point(143, 327);
            this.offense_other_check.Margin = new System.Windows.Forms.Padding(2);
            this.offense_other_check.Name = "offense_other_check";
            this.offense_other_check.Size = new System.Drawing.Size(56, 19);
            this.offense_other_check.TabIndex = 205;
            this.offense_other_check.TabStop = false;
            this.offense_other_check.Text = "Other";
            this.offense_other_check.UseVisualStyleBackColor = true;
            // 
            // offense_other_box
            // 
            this.offense_other_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.offense_other_box.Location = new System.Drawing.Point(209, 325);
            this.offense_other_box.Margin = new System.Windows.Forms.Padding(2);
            this.offense_other_box.Name = "offense_other_box";
            this.offense_other_box.Size = new System.Drawing.Size(235, 21);
            this.offense_other_box.TabIndex = 206;
            this.offense_other_box.Text = "Please specify...";
            // 
            // speed_type_lbl
            // 
            this.speed_type_lbl.AutoSize = true;
            this.speed_type_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.speed_type_lbl.ForeColor = System.Drawing.SystemColors.Desktop;
            this.speed_type_lbl.Location = new System.Drawing.Point(357, 281);
            this.speed_type_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.speed_type_lbl.Name = "speed_type_lbl";
            this.speed_type_lbl.Size = new System.Drawing.Size(42, 15);
            this.speed_type_lbl.TabIndex = 207;
            this.speed_type_lbl.Text = "Using:";
            // 
            // summons_box
            // 
            this.summons_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.summons_box.Location = new System.Drawing.Point(591, 281);
            this.summons_box.Margin = new System.Windows.Forms.Padding(2);
            this.summons_box.Multiline = true;
            this.summons_box.Name = "summons_box";
            this.summons_box.Size = new System.Drawing.Size(170, 40);
            this.summons_box.TabIndex = 199;
            // 
            // weather_label
            // 
            this.weather_label.AutoSize = true;
            this.weather_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.weather_label.ForeColor = System.Drawing.SystemColors.Desktop;
            this.weather_label.Location = new System.Drawing.Point(140, 218);
            this.weather_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.weather_label.Name = "weather_label";
            this.weather_label.Size = new System.Drawing.Size(111, 15);
            this.weather_label.TabIndex = 208;
            this.weather_label.Text = "Weather Condition:";
            // 
            // street_lbl
            // 
            this.street_lbl.AutoSize = true;
            this.street_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.street_lbl.ForeColor = System.Drawing.SystemColors.Desktop;
            this.street_lbl.Location = new System.Drawing.Point(248, 218);
            this.street_lbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.street_lbl.Name = "street_lbl";
            this.street_lbl.Size = new System.Drawing.Size(97, 15);
            this.street_lbl.TabIndex = 209;
            this.street_lbl.Text = "Street Condition:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(368, 218);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 15);
            this.label1.TabIndex = 210;
            this.label1.Text = "Light Condition:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(478, 218);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 15);
            this.label2.TabIndex = 211;
            this.label2.Text = "Traffic Condition:";
            // 
            // veh_other_box
            // 
            this.veh_other_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.veh_other_box.Location = new System.Drawing.Point(513, 92);
            this.veh_other_box.Margin = new System.Windows.Forms.Padding(2);
            this.veh_other_box.Name = "veh_other_box";
            this.veh_other_box.Size = new System.Drawing.Size(109, 21);
            this.veh_other_box.TabIndex = 212;
            this.veh_other_box.Text = "Please specify...";
            // 
            // _areaOtherBox
            // 
            this._areaOtherBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this._areaOtherBox.Location = new System.Drawing.Point(580, 199);
            this._areaOtherBox.Margin = new System.Windows.Forms.Padding(2);
            this._areaOtherBox.Name = "_areaOtherBox";
            this._areaOtherBox.Size = new System.Drawing.Size(112, 21);
            this._areaOtherBox.TabIndex = 213;
            this._areaOtherBox.Text = "Please specify...";
            this._areaOtherBox.Visible = false;
            // 
            // CitationViolationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._areaOtherBox);
            this.Controls.Add(this.veh_other_box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.street_lbl);
            this.Controls.Add(this.weather_label);
            this.Controls.Add(this.speed_type_lbl);
            this.Controls.Add(this.offense_other_box);
            this.Controls.Add(this.offense_other_check);
            this.Controls.Add(this.Ina2);
            this.Controls.Add(this.InABox);
            this.Controls.Add(this.Fail);
            this.Controls.Add(this.CourtInfo);
            this.Controls.Add(this.summons_box);
            this.Controls.Add(this.Area);
            this.Controls.Add(this.SpeedCheck);
            this.Controls.Add(this.Defendent);
            this.Controls.Add(this.CommittedOffenses);
            this.Controls.Add(this.Conditions);
            this.Controls.Add(this.CitationCity);
            this.Controls.Add(this.CityBox);
            this.Controls.Add(this.VehPlate);
            this.Controls.Add(this.VehInfo);
            this.Controls.Add(this.VehCheck5);
            this.Controls.Add(this.VehCheck4);
            this.Controls.Add(this.VehCheck3);
            this.Controls.Add(this.VehCheck2);
            this.Controls.Add(this.VehCheck1);
            this.Controls.Add(this.Operated);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.CitationStreet);
            this.Controls.Add(this.CitationAreaBox);
            this.Controls.Add(this.CitationSubmitButton);
            this.Controls.Add(this.ExtraInfo);
            this.Controls.Add(this.Violations);
            this.Controls.Add(this.StreetBox);
            this.Controls.Add(this.CitationTrafficConditionBox);
            this.Controls.Add(this.CitationLightConditionBox);
            this.Controls.Add(this.CitationStreetConditionBox);
            this.Controls.Add(this.CitationWeatherBox);
            this.Controls.Add(this.AccidentCheck);
            this.Controls.Add(this.VehPlateBox);
            this.Controls.Add(this.VehInfoBox);
            this.Controls.Add(this.SpeedBox);
            this.Controls.Add(this.CitationSpeedDeviceBox);
            this.Controls.Add(this.CitationViolationBox);
            this.Controls.Add(this.CitationExtraInfoBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "CitationViolationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDT V0.0.3.0 by Fiskey111 -- Developed for Non-lethal policing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CitationAreaBox;
        private System.Windows.Forms.Button CitationSubmitButton;
        private System.Windows.Forms.TextBox CitationExtraInfoBox;
        private System.Windows.Forms.Label ExtraInfo;
        private System.Windows.Forms.Label Violations;
        private System.Windows.Forms.TextBox StreetBox;
        private System.Windows.Forms.ComboBox CitationTrafficConditionBox;
        private System.Windows.Forms.ComboBox CitationLightConditionBox;
        private System.Windows.Forms.ComboBox CitationStreetConditionBox;
        private System.Windows.Forms.ComboBox CitationWeatherBox;
        private System.Windows.Forms.CheckBox AccidentCheck;
        private System.Windows.Forms.TextBox VehPlateBox;
        private System.Windows.Forms.TextBox VehInfoBox;
        private System.Windows.Forms.TextBox SpeedBox;
        private System.Windows.Forms.ComboBox CitationSpeedDeviceBox;
        private System.Windows.Forms.TextBox CitationViolationBox;
        private System.Windows.Forms.Label CitationStreet;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label Operated;
        private System.Windows.Forms.CheckBox VehCheck1;
        private System.Windows.Forms.CheckBox VehCheck2;
        private System.Windows.Forms.CheckBox VehCheck3;
        private System.Windows.Forms.CheckBox VehCheck4;
        private System.Windows.Forms.CheckBox VehCheck5;
        private System.Windows.Forms.Label VehInfo;
        private System.Windows.Forms.Label VehPlate;
        private System.Windows.Forms.Label CitationCity;
        private System.Windows.Forms.TextBox CityBox;
        private System.Windows.Forms.Label Conditions;
        private System.Windows.Forms.Label CommittedOffenses;
        private System.Windows.Forms.Label Defendent;
        private System.Windows.Forms.CheckBox SpeedCheck;
        private System.Windows.Forms.Label Area;
        private System.Windows.Forms.Label CourtInfo;
        private System.Windows.Forms.Label Fail;
        private System.Windows.Forms.TextBox InABox;
        private System.Windows.Forms.Label Ina2;
        private System.Windows.Forms.CheckBox offense_other_check;
        private System.Windows.Forms.TextBox offense_other_box;
        private System.Windows.Forms.Label speed_type_lbl;
        private System.Windows.Forms.TextBox summons_box;
        private System.Windows.Forms.Label weather_label;
        private System.Windows.Forms.Label street_lbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox veh_other_box;
        private System.Windows.Forms.TextBox _areaOtherBox;
    }
}