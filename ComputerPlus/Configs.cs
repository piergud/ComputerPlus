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
                user = "OfficerHotStuff";
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
    }
}
