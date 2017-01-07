using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.API;
using Rage.Forms;
using System.Globalization;
using System.IO;

namespace ComputerPlus
{
    internal static class GwenFormExtension
    {
        internal static Point GetLaunchPosition(this GwenForm form)
        {
            return new Point(Game.Resolution.Width / 2 - form.Window.Width / 2, Game.Resolution.Height / 2 - form.Window.Height / 2);
        }
    }
    internal static class Function
    {
        private static Texture _bg;
        private static bool _bg_enabled = false;
        private static RectangleF taskbar = new RectangleF();
        private static Color taskbar_col = Color.FromArgb(160, 0, 0, 0);
        private static int width, height;
        private static string update_text = "";

        /// <summary>
        /// Gets whether the computer background is enabled or not.
        /// </summary>
        internal static bool IsBackgroundEnabled()
        {
            return _bg_enabled;
        }

        internal static async void CheckForUpdates()
        {
            await System.Threading.Tasks.Task.Factory.StartNew(() => SendAPIWebRequest(Globals.WebAPIFileId, false));
        }

        private static async void SendAPIWebRequest(int fileId, bool beta = false)
        {
            string apiURIFormat = "http://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId={0}&beta={1}&textOnly=true";
            string apiURI = String.Format(apiURIFormat, Globals.WebAPIFileId, beta.ToString().ToLower());
            string webVersionStr = "";

            try
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    webVersionStr = await wc.DownloadStringTaskAsync(apiURI);
                }
            }
            catch (Exception e)
            {
                webVersionStr = "";
                Game.LogTrivial("Error checking for updates -- " + e.ToString());
            }

            if (webVersionStr != "")
            {
                try
                {
                    Version webVersion = null;
                    Version installedVersion = null;

                    if (!Version.TryParse(webVersionStr, out webVersion))
                    {
                        webVersion = null;
                    }

                    if (!Version.TryParse(FileVersionInfo.GetVersionInfo(@"Plugins\LSPDFR\ComputerPlus.dll").FileVersion, out installedVersion))
                    {
                        installedVersion = null;
                    }

                    if (installedVersion != null && webVersion != null)
                    {
                        if (webVersion.CompareTo(installedVersion) > 0)
                        {
                            update_text = String.Format("Version: {0} | Update Available: ({1})", installedVersion.ToString(), webVersionStr);
                        }
                        else
                        {
                            update_text = String.Format("Version: {0} | Up to Date", installedVersion.ToString());
                        }
                    }
                    else
                    {
                        update_text = "System Update Check Failed";
                    }
                }
                catch (Exception e)
                {
                    update_text = "System Update Check Failed";
                    Game.LogTrivial("Error comparing version numbers -- " + e.ToString());
                }
            }
            else
            {
                update_text = "System Update Check Failed";
            }
        }

        internal static class checkForRageVersionClass
        {
            private static bool correctVersion;

            /// <summary>
            /// Checks whether the person has the specified minimum version or higher. 
            /// </summary>
            /// <param name="minimumVersion">Provide in the format of a float i.e.: 0.22</param>
            /// <returns></returns>
            public static bool checkForRageVersion(float minimumVersion)
            {

                var versionInfo = FileVersionInfo.GetVersionInfo("RAGEPluginHook.exe");
                float Rageversion;
                try
                {
                    //If you decide to use this in your plugin, I would appreciate some credit :)
                    Rageversion = float.Parse(versionInfo.ProductVersion.Substring(0, 4), CultureInfo.InvariantCulture);
                    Game.LogTrivial("ComputerPlus detected RAGEPluginHook version: " + Rageversion.ToString());

                    //If user's RPH version is older than the minimum
                    if (Rageversion < minimumVersion)
                    {
                        correctVersion = false;
                        GameFiber.StartNew(delegate
                        {
                            while (Game.IsLoading)
                            {
                                GameFiber.Yield();
                            }
                            //If you decide to use this in your plugin, I would appreciate some credit :)
                            Game.DisplayNotification("RPH ~r~v" + Rageversion.ToString() + " ~s~detected. ~b~ComputerPlus ~s~requires ~b~v" + minimumVersion.ToString() + " ~s~or higher.");
                            GameFiber.Sleep(5000);
                            Game.LogTrivial("RAGEPluginHook version " + Rageversion.ToString() + " detected. ComputerPlus requires v" + minimumVersion.ToString() + " or higher.");
                            Game.LogTrivial("Preparing redirect...");
                            Game.DisplayNotification("You are being redirected to the RagePluginHook website so you can download the latest version.");
                            Game.DisplayNotification("Press Backspace to cancel the redirect.");

                            int count = 0;
                            while (true)
                            {
                                GameFiber.Sleep(10);
                                count++;
                                if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Back))
                                {
                                    break;
                                }
                                if (count >= 300)
                                {
                                    //URL to the RPH download page.
                                    //I use bit.ly to track the number of times this is called: at the moment, it has been called 327 times over the past 2 days! What a timesaver for me.
                                    System.Diagnostics.Process.Start("http://bit.ly/RPHDownload");
                                    break;
                                }
                            }

                        });
                    }
                    //If user's RPH version is (above) the specified minimum
                    else
                    {
                        correctVersion = true;
                    }
                }
                catch (Exception e)
                {
                    //If for whatever reason the version couldn't be found.
                    Game.LogTrivial(e.ToString());
                    Game.LogTrivial("Unable to detect your Rage installation.");
                    if (File.Exists("RAGEPluginHook.exe"))
                    {
                        Game.LogTrivial("RAGEPluginHook.exe exists");
                    }
                    else { Game.LogTrivial("RAGEPluginHook doesn't exist."); }
                    Game.LogTrivial("Rage Version: " + versionInfo.ProductVersion.ToString());
                    Game.DisplayNotification("ComputerPlus unable to detect RPH installation. Please send me your logfile.");
                    correctVersion = false;

                }

                return correctVersion;

            }
        }

        /// <summary>
        /// Gets whether a vehicle is a police vehicle or not.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        /// <returns>True if it is a police vehicle, false if not.</returns>
        internal static bool IsPoliceVehicle(Vehicle veh)
        {
            if (veh.HasSiren
                && veh.Model.Hash != 0x73920F8E // Ambulance
                && veh.Model.Hash != 0x45D56ADA // Fire Truck
                && veh.Model.Hash != 0x1BF8D381 // Lifeguard SUV
                && veh.Model.Hash != 0x91EFE36F) // Merryweather Patriot (RDE)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enables the police computer's background.
        /// </summary>
        internal static void EnableBackground() 
        {
            _bg = LoadBackground(GetBackgroundFileNameForVehicle(Game.LocalPlayer.Character.CurrentVehicle));
            if (_bg == null) 
            {
                Game.LogTrivial(@"Failed to load LSPDFR Computer+ background. Please ensure all backgrounds are present in Plugins\LSPDFR\ComputerPlus\backgrounds\.");
                Game.LogTrivial(@"Ensure your ComputerPlus.ini contains entries for [VEHICLE BACKGROUNDS] in the format of vehicleModel=backgroundImage.jpg");
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "LSPDFR Computer+", "~r~Error", @"Failed to load background in Plugins\LSPDFR\ComputerPlus\backgrounds\.");
                _bg = LoadBackground(Globals.DefaultBackgroundImage);
            }
            else 
            {
                Game.RawFrameRender += OnRawFrameRender;
                _bg_enabled = true;
            }
        }

        /// <summary>
        /// Disables the police computer's background.
        /// </summary>
        internal static void DisableBackground()
        {
            Game.RawFrameRender -= OnRawFrameRender;
            _bg_enabled = false;
        }

        private static void OnRawFrameRender(object sender, GraphicsEventArgs e) 
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            float length = Rage.Graphics.MeasureText(time, "Arial", 18).Width;
            taskbar.Size = new SizeF(Game.Resolution.Width, Game.Resolution.Height / 25);
            taskbar.Location = new PointF(1, 1 + Game.Resolution.Height - (Game.Resolution.Height / 25));
            
            e.Graphics.DrawTexture(_bg, 0f, 0f, Game.Resolution.Width, Game.Resolution.Height);
            //e.Graphics.DrawRectangle(taskbar, taskbar_col);
            e.Graphics.DrawText(update_text, "Arial", 18,
                new PointF(taskbar.X + (taskbar.Width / 150), taskbar.Y + (taskbar.Height / 4)), 
                Color.White);
            e.Graphics.DrawText(time, "Arial", 18,
                new PointF(taskbar.Width - length - taskbar.Width / 150, taskbar.Y + (taskbar.Height / 4)),
                Color.White);
        }

        /// <summary>
        /// Loads a given background from the backgrounds folder.
        /// </summary>
        /// <param name="bg_name"></param>
        /// <returns>The background file. Returns null if background failed to load.</returns>
        private static Texture LoadBackground(String bg_name)
        {
            return Game.CreateTextureFromFile(String.Format(@"Plugins\LSPDFR\ComputerPlus\backgrounds\{0}", bg_name));
        }

        /// <summary>
        /// Gets the background file for a given vehicle.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        /// <returns>The background file. Returns the LSPD background if this vehicle's department is not defined.</returns>
        private static string GetBackgroundFileNameForVehicle(Vehicle veh)
        {
            string file;
            try
            {
                file = Configs.bgs[veh.Model.Hash];
            }
            catch (KeyNotFoundException)
            {
                file = Globals.DefaultBackgroundImage;
            }
            return file;
        }

        internal static String GetPedImagePath(String model, String variant = "front")
        {
            return String.Format(@"Plugins\LSPDFR\ComputerPlus\images\peds\{0}_{1}.jpg", model, variant);
        }

        internal static String GetVehicleImagePath(String model, String variant = "f")
        {
            return String.Format(@"Plugins\LSPDFR\ComputerPlus\images\vehicles\{0}{1}.jpg", model, variant);
        }

        internal static String DefaultVehicleImagePath
        {
            get
            {
                return String.Format(@"Plugins\LSPDFR\ComputerPlus\{0}.jpg", Globals.EmptyImageVehicle);
            }
        }

        internal static String DefaultPedImagePath
        {
            get
            {
                return String.Format(@"Plugins\LSPDFR\ComputerPlus\{0}.jpg", Globals.EmptyImagePed);
            }
        }

        internal static Texture LoadPedImage(String model)
        {
            var path = GetPedImagePath(model);
            Game.LogVerboseDebug(String.Format("LoadPedImage: {0}", path));
            return Game.CreateTextureFromFile(path);
        }

        private static void MakeSpaceForNewRecent()
        {
            if (EntryPoint.recent_text.Count == 7)
                EntryPoint.recent_text.RemoveAt(0);
        }

        /// <summary>
        /// Adds a specific ped to the recent actions screen.
        /// </summary>
        /// <param name="ped">The ped</param>
        internal static void AddPedToRecents(Ped ped)
        {
            MakeSpaceForNewRecent();
            string name = Functions.GetPersonaForPed(ped).FullName;
            EntryPoint.recent_text.Add(String.Format("Looked up person: {0}", name));
        }

        /// <summary>
        /// Adds a specified vehicle to the recent actions screen.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        internal static void AddVehicleToRecents(Vehicle veh)
        {
            MakeSpaceForNewRecent();
            string lp = veh.LicensePlate;
            EntryPoint.recent_text.Add(String.Format("Looked up vehicle: {0} ({1})", lp, GetVehicleDisplayName(veh)));
        }

        /// <summary>
        /// Adds a specified backup request to the recent actions screen.
        /// </summary>
        /// <param name="response">The response type</param>
        /// <param name="unit">The unit type</param>
        internal static void AddBackupRequestToRecents(string response, string unit)
        {
            MakeSpaceForNewRecent();
            EntryPoint.recent_text.Add(String.Format("Requested backup: {0} ({1})", unit, response));
        }

        /// <summary>
        /// Gets whether a specific DLC is installed.
        /// </summary>
        /// <param name="dlc">The DLC's code name</param>
        /// <returns>True if installed, false if not.</returns>
        private static bool IsDLCPresent(String dlc)
        {
            return NativeFunction.CallByName<bool>("IS_DLC_PRESENT", Game.GetHashKey(dlc));
        }

        /// <summary>
        /// Gets whether a specific LSPDFR plugin is running or not.
        /// Credits to Albo1125 for this function.
        /// </summary>
        /// <param name="plugin">The name of the plugin</param>
        /// <returns></returns>
        internal static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool IsTrafficPolicerRunning()
        {
            return IsLSPDFRPluginRunning("Traffic Policer", new Version(6, 9, 8, 1));
        }

        internal static bool IsAlprPlusRunning()
        {
            return IsLSPDFRPluginRunning("ALPRPlus", new Version(0, 2, 0, 0));
        }

        internal static string GetFormattedDateTime(DateTime? date = null)
        {
            if (date == null || !date.HasValue)
                date = DateTime.UtcNow;

            return String.Format("[{0:MM/dd/yyyy HH:mm:ss}]", date.Value.ToLocalTime());
        }

        internal static void ClearActiveCall(Guid? callID = null)
        {
            // Specifying callID only clears the active call if the active call's ID is equal to callID
            // Shouldn't be an issue, but is a safety check in case the user gets a call while they are already on a call
            if (callID == null || Globals.ActiveCallID == callID.Value)
            {
                if (Globals.ActiveCallout != null)
                {
                    foreach (var x in Globals.CallQueue.Where(x => x.ID == Globals.ActiveCallID && x.IsPlayerAssigned == true))
                    {
                        x.ConcludeCallout();
                    }
                }

                Globals.ActiveCallID = Guid.Empty;
            }
        }

        internal static string GetRandomAIUnit()
        {
            char[] unitTypes = { 'A', 'L', 'X' };

            int division = Globals.Random.Next(1, 8);
            char uType = unitTypes[Globals.Random.Next(unitTypes.Length)];
            int beatNum = Globals.Random.Next(25, 60);

            return String.Format("{0}-{1}-{2}", division, uType, beatNum);
        }

        internal static void MonitorAICalls()
        {
            GameFiber.StartNew(
                delegate
                {
                    while(Globals.IsPlayerOnDuty)
                    {
                        foreach(var x in Globals.CallQueue.Where(x => x.IsPlayerAssigned == false))
                        {
                            switch(x.Status)
                            {
                                case ECallStatus.Created:
                                    x.UpdateStatus(ECallStatus.Dispatched);
                                    break;

                                case ECallStatus.Dispatched:
                                    x.UpdateStatus(ECallStatus.Unit_Responding);
                                    x.AddUpdate(String.Format("Unit {0} is responding.", x.PrimaryUnit));
                                    break;

                                case ECallStatus.Unit_Responding:
                                    TimeSpan drivingTime = (DateTime.UtcNow - x.LastUpdated);
                                    if (drivingTime.TotalMinutes >= x.AIUnitResponseTime)
                                    {
                                        x.UpdateStatus(ECallStatus.At_Scene);
                                        x.AddUpdate(String.Format("Unit {0} is on scene.", x.PrimaryUnit));
                                    }

                                    break;

                                case ECallStatus.At_Scene:
                                    TimeSpan timeAtScene = (DateTime.UtcNow - x.LastUpdated);
                                    if (timeAtScene.TotalMinutes >= x.AIUnitMinutesAtScene)
                                    {
                                        x.ConcludeCallout();
                                    }

                                    break;

                                default:
                                    break;
                            }
                        }

                        // Clean up abandoned/orphan calls that were assigned to the player
                        // If 'stuck' for more than 5 minutes, assign to AI
                        foreach (var x in Globals.CallQueue.Where(x => x.IsPlayerAssigned == true && (x.Status == ECallStatus.Created | x.Status == ECallStatus.Dispatched)))
                        {
                            TimeSpan ts = (DateTime.UtcNow - x.LastUpdated);
                            if (ts.TotalMinutes > 5)
                            {
                                x.AssignCallToAIUnit();
                            }
                        }

                        GameFiber.Yield();
                    }
                });
        }

        /// <summary>
        /// Gets the in-game display name of a specified vehicle.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        /// <returns>The in-game display name of the vehicle</returns>
        internal static string GetVehicleDisplayName(Vehicle veh)
        {
            string model = GetStringFromNative("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", veh.Model.Hash);
            string name = GetStringFromNative(0x7B5280EBA9840C72, model);
            return name;
        }

        /// <summary>
        /// Gets the returned string from a native.
        /// </summary>
        /// <param name="native_hash">Hash of the native</param>
        /// <param name="args">Arguments</param>
        /// <returns>A string of the native's return</returns>
        internal static string GetStringFromNative(ulong native_hash, params NativeArgument[] args)
        {
            IntPtr ptr = NativeFunction.CallByHash<IntPtr>(native_hash, args);
            String str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }

        /// <summary>
        /// Gets the returned string from a native.
        /// </summary>
        /// <param name="native_name">Name of the native</param>
        /// <param name="args">Arguments</param>
        /// <returns>A string of the native's return</returns>
        internal static string GetStringFromNative(String native_name, params NativeArgument[] args)
        {
            IntPtr ptr = NativeFunction.CallByName<IntPtr>(native_name, args);
            String str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }

        
    }
}
