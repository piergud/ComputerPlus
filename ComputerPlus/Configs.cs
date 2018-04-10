using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using ComputerPlus.Interfaces;
using System.Windows.Forms;

namespace ComputerPlus
{
    internal static class Configs
    {
        static InitializationFile ini_file = new InitializationFile(@"Plugins\LSPDFR\ComputerPlus.ini");
        internal static Dictionary<uint,string> bgs = new Dictionary<uint,string>();
        static string user, pass, unit;
        static bool skip;
        static bool randomHistoryRecords;
        static bool displayPedImage;
        static bool displayVehicleImage;
        static bool enableLSPDFRPlusIntegration;
        internal static int FontSize
        {
            get;
            private set;
        } = 16;
        internal static String FontName
        {
            get;
            private set;
        } = "Microsoft Sans Serif";

        private static List<KeyBinder> OpenComputerPlusKeys = new List<KeyBinder>();
        private static List<KeyBinder> OpenSimpleNotepadKeys = new List<KeyBinder>();
        private static List<KeyBinder> CloseComputerPlusKeys = new List<KeyBinder>();
        private static List<KeyBinder> GiveCitationsToPedKeys = new List<KeyBinder>();
        private static List<KeyBinder> OpenComputerPlusOnFootKeys = new List<KeyBinder>();

        internal static void RunConfigCheck()
        {
            if (!ini_file.Exists())
            {
                CreateINIFile();
            }
            user = ini_file.ReadString("SETTINGS", "LoginName");
            pass = ini_file.ReadString("SETTINGS", "LoginPass");
            skip = ini_file.ReadBoolean("SETTINGS", "SkipLogin");
            unit = ini_file.ReadString("SETTINGS", "UnitNumber");
            FontSize = ini_file.ReadInt32("SETTINGS", "FontSize");
            FontName = ini_file.ReadString("SETTINGS", "FontName");
            randomHistoryRecords = ini_file.ReadBoolean("SETTINGS", "RandomHistoryRecords", true);

            displayPedImage = ini_file.ReadBoolean("SETTINGS", "DisplayPedImage", true);
            displayVehicleImage = ini_file.ReadBoolean("SETTINGS", "DisplayVehicleImage", true);

            enableLSPDFRPlusIntegration = ini_file.ReadBoolean("SETTINGS", "EnableLSPDFRPlusIntegration", true);

            if (String.IsNullOrWhiteSpace(user))
                user = "Officer";
            if (String.IsNullOrWhiteSpace(pass))
                pass = "DoNuTz";
            if (String.IsNullOrWhiteSpace(unit))
                unit = "1-A-12";

            FontSize = FontSize > 0 ? FontSize : 16;
            FontName = !String.IsNullOrWhiteSpace(FontName) ? FontName : "Microsoft Sans Serif";

            foreach (string key in ini_file.GetKeyNames("VEHICLE BACKGROUNDS"))
            {
                bgs.Add(Game.GetHashKey(key), ini_file.ReadString("VEHICLE BACKGROUNDS", key));
            }

            ParseKeybindings();


        }

        private static void ParseKeybindings()
        {
            if (!ini_file.DoesSectionExist("KEYBINDINGS"))
            {
                Game.DisplayHelp("Your ComputerPlus.ini file is missing the KEYBINDINGS section. Please verify your config");
                Function.Log("Your Computer+ config file is missing the KEYBINDINGS section. Please verify your config");
                OpenComputerPlusKeys.Add(new KeyBinder(GameControl.Context));
            }
            else {
                ParseKeybindings(OpenComputerPlusKeys, "OpenComputerPlus");
                ParseKeybindings(CloseComputerPlusKeys, "CloseComputerPlus");
                ParseKeybindings(OpenSimpleNotepadKeys, "OpenSimpleNotepad");
                ParseKeybindings(GiveCitationsToPedKeys, "GiveCitationsToPed");
                ParseKeybindings(OpenComputerPlusOnFootKeys, "OpenComputerPlusOnFoot");

                if (OpenComputerPlusKeys.Count == 0) //Fail safe for opening computer by holding the context secondary (E / DPadRight)
                    OpenComputerPlusKeys.Add(new KeyBinder(GameControl.Context));
            }
        }

        private static void ParseKeybindings(List<KeyBinder> toList, String forKey)
        {
            try
            {
                var key = ini_file.ReadEnum<Keys>("KEYBINDINGS", forKey + "Key", Keys.None);
                var keyModifier = ini_file.ReadEnum<Keys>("KEYBINDINGS", forKey + "ModifierKey", Keys.None);
                var controllerButton = ini_file.ReadEnum<ControllerButtons>("KEYBINDINGS", forKey + "ControllerButton", ControllerButtons.None);
                var controllerButtonModifier = ini_file.ReadEnum<ControllerButtons>("KEYBINDINGS", forKey + "ControllerModifierButton", ControllerButtons.None);

                if (key != Keys.None)
                    toList.Add(new KeyBinder(key, keyModifier));
                if (controllerButton != ControllerButtons.None)
                    toList.Add(new KeyBinder(controllerButton, controllerButtonModifier));               
            }
            catch
            {
            }
        }

      

        internal static void CreateINIFile()
        {
            ini_file.Create();
            ini_file.Write("SETTINGS", "LoginName", "OfficerHotStuff");
            ini_file.Write("SETTINGS", "LoginPass", "DoNuTz");
            ini_file.Write("SETTINGS", "SkipLogin", "false");
            ini_file.Write("SETTINGS", "UnitNumber", "1-A-12");
            ini_file.Write("SETTINGS", "FontSize", 16);
            ini_file.Write("SETTINGS", "FontName", "Microsoft Sans Serif");
            ini_file.Write("SETTINGS", "RandomHistoryRecords", "true");
            ini_file.Write("SETTINGS", "DisplayPedImage", "true");
            ini_file.Write("SETTINGS", "DisplayVehicleImage", "true");
            ini_file.Write("SETTINGS", "EnableLSPDFRPlusIntegration", "true");

            ini_file.Write("KEYBINDINGS", "OpenComputerPlusKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusModifierKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusControllerButton", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusControllerModifierButton", "None");

            ini_file.Write("KEYBINDINGS", "CloseComputerPlusKey", "None");
            ini_file.Write("KEYBINDINGS", "CloseComputerPlusModifierKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusControllerModifierButton", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusControllerModifierButton", "None");

            ini_file.Write("KEYBINDINGS", "OpenSimpleNotepadKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenSimpleNotepadModifierKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusControllerModifierButton", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusControllerModifierButton", "None");

            ini_file.Write("KEYBINDINGS", "GiveCitationsToPedKey", "None");
            ini_file.Write("KEYBINDINGS", "GiveCitationsToPedModifierKey", "None");
            ini_file.Write("KEYBINDINGS", "GiveCitationsToPedControllerButton", "None");
            ini_file.Write("KEYBINDINGS", "GiveCitationsToPedControllerModifierButton", "None");

            ini_file.Write("KEYBINDINGS", "OpenComputerPlusOnFootKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusOnFootModifierKey", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusOnFootControllerButton", "None");
            ini_file.Write("KEYBINDINGS", "OpenComputerPlusOnFootControllerModifierButton", "None");

        }

        internal static string Username
        {
            get { return user; }
        }

        internal static string Password
        {
            get { return pass; }
        }

        internal static bool SkipLogin
        {
            get { return skip; }
        }

        internal static bool RandomHistoryRecords
        {
            get { return randomHistoryRecords; }
        }

        internal static bool DisplayPedImage
        {
            get { return displayPedImage; }
        }

        internal static bool DisplayVehicleImage
        {
            get { return displayVehicleImage; }
        }

        internal static bool EnableLSPDFRPlusIntegration
        {
            get { return enableLSPDFRPlusIntegration; }
        }

        internal static string UnitNumber
        {
            get { return unit; }
        }

        internal static KeyBinder[] OpenComputerPlus
        {
            get
            {
                return OpenComputerPlusKeys.ToArray();
            }
        }

        internal static KeyBinder[] CloseComputerPlus
        {
            get
            {
                return CloseComputerPlusKeys.ToArray();
            }
        }

        internal static KeyBinder[] OpenSimpleNotepad
        {
            get
            {
                return OpenSimpleNotepadKeys.ToArray();
            }
        }

        internal static KeyBinder[] GiveTicketsToPed
        {
            get
            {
                return GiveCitationsToPedKeys.ToArray();
            }
        }

        internal static KeyBinder[] OpenComputerPlusOnFoot
        {
            get
            {
                return OpenComputerPlusOnFootKeys.ToArray();
            }
        }

        internal static int BaseFormWidth
        {
            get { return 700; }
        }

        internal static int BaseFormHeight
        {
            get { return 300; }
        }

        internal static int BaseFormControlSpacingHalf
        {
            get { return BaseFormControlSpacing / 2; }
        }

        internal static int BaseFormControlSpacing
        {
            get { return 15; }
        }

        internal static int BaseFormControlSpacingDouble
        {
            get { return BaseFormControlSpacing * 2; }
        }

        internal static int BaseFormControlSpacingTriple
        {
            get { return BaseFormControlSpacing * 3; }
        }

        
        internal static readonly String RegularBoldFontName = String.Format("Microsoft Sans Serif, {0}px, style=Bold", FontSize + 4);


        /*public static Keys NotebookKey { get { return ini_file.ReadEnum<Keys>("General", "NotebookKey", Keys.D2); } }
        public static Keys NotebookKeyModifier { get { return ini_file.ReadEnum<Keys>("General", "NotebookKeyModifier", Keys.LControlKey); } }

        public static class RandomNumber
        {
            public static Random r = new Random();
        }

        public static string OfficerNumber { get { return ini_file.ReadString("Officer Information", "Officer Number", "1-Adam-12"); } }
        public static string OfficerName { get { return ini_file.ReadString("Officer Information", "Officer Name", "Pete Malloy"); } }*/
    }
}
