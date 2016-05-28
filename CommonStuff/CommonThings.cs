using LSPD_First_Response.Engine.Scripting;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Notsolethalpolicing.MDT;

namespace ComputerPlus.CommonStuff
{
    public static class Settings
    {
        public static InitializationFile INIFile = new InitializationFile(@"Plugins\LSPDFR\PolicingMDT.ini");

        public static Keys MDTKey { get { return Settings.INIFile.ReadEnum<Keys>("General", "MDTKey", Keys.D1); } }
        public static Keys MDTKeyModifier { get { return Settings.INIFile.ReadEnum<Keys>("General", "MDTKeyModifier", Keys.LControlKey); } }
        public static Keys NotebookKey { get { return Settings.INIFile.ReadEnum<Keys>("General", "NotebookKey", Keys.D2); } }
        public static Keys NotebookKeyModifier { get { return Settings.INIFile.ReadEnum<Keys>("General", "NotebookKeyModifier", Keys.LControlKey); } }

        public static string Beat { get { return Settings.INIFile.ReadString("Audio", "Beat", "BEAT_12"); } }
        public static string Div { get { return Settings.INIFile.ReadString("Audio", "Division", "DIV_01"); } }
        public static string Unit { get { return Settings.INIFile.ReadString("Audio", "Unit Type", "ADAM"); } }

        public static string Username { get { return Settings.INIFile.ReadString("Login", "Username", "admin"); } }
        public static string Password { get { return Settings.INIFile.ReadString("Login", "Password", "password"); } }

        public static string OfficerNumber { get { return Settings.INIFile.ReadString("Officer Information", "Officer Number", "1-Adam-12"); } }
        public static string OfficerName { get { return Settings.INIFile.ReadString("Officer Information", "Officer Name", "Pete Malloy"); } }

        public static string CommunityPolicingName { get { return Settings.INIFile.ReadString("Callout Names", "Community Policing", "Community Policing"); } }
    }

    public static class RandomNumber
    {
        public static Random r = new Random();
    }
}