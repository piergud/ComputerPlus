using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace ComputerPlus
{
    internal static class Configs
    {
        static InitializationFile ini_file = new InitializationFile(@"Plugins\LSPDFR\ComputerPlus.ini");
        internal static Dictionary<uint,string> bgs = new Dictionary<uint,string>();
        static string user, pass, unit;
        static bool skip;

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
            if (String.IsNullOrWhiteSpace(user))
                user = "Officer";
            if (String.IsNullOrWhiteSpace(pass))
                pass = "DoNuTz";
            if (String.IsNullOrWhiteSpace(unit))
                unit = "1-A-12";

            foreach (string key in ini_file.GetKeyNames("VEHICLE BACKGROUNDS"))
            {
                bgs.Add(Game.GetHashKey(key), ini_file.ReadString("VEHICLE BACKGROUNDS", key));
            }
        }

        internal static void CreateINIFile()
        {
            ini_file.Create();
            ini_file.Write("SETTINGS", "LoginName", "OfficerHotStuff");
            ini_file.Write("SETTINGS", "LoginPass", "DoNuTz");
            ini_file.Write("SETTINGS", "SkipLogin", "false");
            ini_file.Write("SETTINGS", "UnitNumber", "1-A-12");
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

        internal static string UnitNumber
        {
            get { return unit; }
        }

        internal static int BaseFormWidth
        {
            get { return 700; }
        }

        internal static int BaseFormHeight
        {
            get { return 300; }
        }

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
