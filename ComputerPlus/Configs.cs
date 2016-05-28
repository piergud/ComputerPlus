using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace ComputerPlus
{
    public static class Configs
    {
        static InitializationFile ini_file = new InitializationFile(@"Plugins\LSPDFR\ComputerPlus.ini");
        public static Dictionary<uint,string> bgs = new Dictionary<uint,string>();
        static string user, pass;

        public static void RunConfigCheck()
        {
            if (!ini_file.Exists())
            {
                CreateINIFile();
            }

            user = ini_file.ReadString("SETTINGS", "LoginName");
            pass = ini_file.ReadString("SETTINGS", "LoginPass");
            if (String.IsNullOrWhiteSpace(user))
                user = "OfficerHotStuff";
            if (String.IsNullOrWhiteSpace(pass))
                pass = "DoNuTz";

            foreach (string key in ini_file.GetKeyNames("VEHICLE BACKGROUNDS"))
            {
                bgs.Add(Game.GetHashKey(key), ini_file.ReadString("VEHICLE BACKGROUNDS", key));
            }
        }

        public static void CreateINIFile()
        {
            ini_file.Create();
            ini_file.Write("SETTINGS", "LoginName", "OfficerHotStuff");
            ini_file.Write("SETTINGS", "LoginPass", "DoNuTz");
        }

        public static string Username
        {
            get { return user; }
        }

        public static string Password
        {
            get { return pass; }
        }
    }
}
