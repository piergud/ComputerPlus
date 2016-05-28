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

        public static string OfficerNumber { get { return Settings.INIFile.ReadString("Officer Information", "Officer Number", "1-Adam-12"); } }
        public static string OfficerName { get { return Settings.INIFile.ReadString("Officer Information", "Officer Name", "Pete Malloy"); } }
    }

    public static class RandomNumber
    {
        public static Random r = new Random();
    }
}
