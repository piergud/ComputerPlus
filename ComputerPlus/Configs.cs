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
        static string user, pass;
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
            if (String.IsNullOrWhiteSpace(user))
                user = "Officer";
            if (String.IsNullOrWhiteSpace(pass))
                pass = "DoNuTz";

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
