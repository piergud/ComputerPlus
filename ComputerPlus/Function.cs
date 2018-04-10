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
using static ComputerPlus.Extensions.Gwen.TextBoxExtensions;

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
        private static bool mDrawBackground = false;
        private static bool DrawBackground {
            get
            {
                return mDrawBackground;
            }
            set
            {
                if (value)
                    Game.RawFrameRender += OnRawFrameRender;
                else
                    Game.RawFrameRender -= OnRawFrameRender;
                mDrawBackground = value;
            }
        }
        private static String CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }
        private static RectangleF taskbar = new RectangleF();
        private static Color taskbar_col = Color.FromArgb(160, 0, 0, 0);
        // private static int width, height;
        private static string update_text = "";

        /// <summary>
        /// Gets whether the computer background is enabled or not.
        /// </summary>
        internal static bool IsBackgroundEnabled()
        {
            return DrawBackground;
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
                Function.Log("Error checking for updates -- " + e.ToString());
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
                    Function.Log("Error comparing version numbers -- " + e.ToString());
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
                    Function.LogDebug("ComputerPlus detected RAGEPluginHook version: " + Rageversion.ToString());

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
                            Function.Log("RAGEPluginHook version " + Rageversion.ToString() + " detected. ComputerPlus requires v" + minimumVersion.ToString() + " or higher.");
                            Function.Log("Preparing redirect...");
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
                    Function.Log(e.ToString());
                    Function.Log("Unable to detect your Rage installation.");
                    if (File.Exists("RAGEPluginHook.exe"))
                    {
                        Function.Log("RAGEPluginHook.exe exists");
                    }
                    else { Function.Log("RAGEPluginHook doesn't exist."); }
                    Function.Log("Rage Version: " + versionInfo.ProductVersion.ToString());
                    Function.Log("ComputerPlus unable to detect RPH installation. Please send me your logfile.");
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
                Function.Log(@"Failed to load LSPDFR Computer+ background. Please ensure all backgrounds are present in Plugins\LSPDFR\ComputerPlus\backgrounds\.");
                Function.Log(@"Ensure your ComputerPlus.ini contains entries for [VEHICLE BACKGROUNDS] in the format of vehicleModel=backgroundImage.jpg");
                _bg = LoadBackground(Globals.DefaultBackgroundImage);
            }
            else 
            {
                DrawBackground = true;
            }
        }

        /// <summary>
        /// Disables the police computer's background.
        /// </summary>
        internal static void DisableBackground()
        {
            DrawBackground = false;
        }
        private static void OnRawFrameRender(object sender, GraphicsEventArgs e) 
        {
            
            if (!DrawBackground) return;
            try {
                string time = CurrentTime;
                Size gameResolution = Game.Resolution;
                e.Graphics.DrawTexture(_bg, 0f, 0f, gameResolution.Width, gameResolution.Height);
                float length = Rage.Graphics.MeasureText(time, "Arial", 18).Width;
                float taskbarHeight = gameResolution.Height / 25;
                float textWidth = taskbar.Width / 150;
                float textHeight = taskbar.Height / 4;
                taskbar.Size = new SizeF(gameResolution.Width, taskbarHeight);
                taskbar.Location = new PointF(1, 1 + gameResolution.Height - taskbarHeight);


                e.Graphics.DrawText(update_text, "Arial", 18,
                    new PointF(taskbar.X + textWidth, taskbar.Y + textHeight),
                    Color.White);
                e.Graphics.DrawText(time, "Arial", 18,
                    new PointF(taskbar.Width - length - textWidth, taskbar.Y + textHeight),
                    Color.White);
            } catch(Exception err)
            {
                Function.Log("Exception in OnRawFrameRender");
                throw err;
            }
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
                if (veh != null && veh.IsValid())
                    file = Configs.bgs[veh.Model.Hash];
                else
                    file = Globals.DefaultBackgroundImage;
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
                return String.Format(@"Plugins\LSPDFR\ComputerPlus\{0}", Globals.EmptyImageVehicle);
            }
        }

        internal static String DefaultPedImagePath
        {
            get
            {
                return String.Format(@"Plugins\LSPDFR\ComputerPlus\{0}", Globals.EmptyImagePed);
            }
        }

        internal static Texture LoadPedImage(String model)
        {
            var path = GetPedImagePath(model);
            Function.LogDebug(String.Format("LoadPedImage: {0}", path));
            return Game.CreateTextureFromFile(path);
        }

        internal static String GetPedImagePath(String modelName, int headDrawableIndex, int headDrawableTextureIndex)
        {            
            var path = String.Format(@"Plugins\LSPDFR\ComputerPlus\images\peds\{0}__0_{1}_{2}_front.jpg", modelName.ToLower(), headDrawableIndex, headDrawableTextureIndex);
            if (!File.Exists(path))
            {
                if (headDrawableIndex != 0 && headDrawableTextureIndex != 0)
                {
                    // if not found, fallback to 0 index and 0 texture 
                    path = String.Format(@"Plugins\LSPDFR\ComputerPlus\images\peds\{0}__0_0_0_front.jpg", modelName.ToLower());
                    if (!File.Exists(path))
                    {
                        path = Function.DefaultPedImagePath;
                    }
                } else
                {
                    path = Function.DefaultPedImagePath;
                }
            }
            return path;
        }

        internal static String GetVehicleImagePath(String model)
        {
            var path = String.Format(@"Plugins\LSPDFR\ComputerPlus\images\vehicles\{0}f.jpg", model);
            return File.Exists(path) ? path : Function.DefaultVehicleImagePath;
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
            EntryPoint.AddRecentText(String.Format("Looked up person: {0}", name));
            if (ped != null && !ped.IsPersistent)
            {
                ped.IsPersistent = true;
                Globals.persistedRageEntities.Add(ped);
            }
        }

        /// <summary>
        /// Adds a specified vehicle to the recent actions screen.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        internal static void AddVehicleToRecents(Vehicle veh)
        {
            MakeSpaceForNewRecent();
            string lp = veh.LicensePlate;
            EntryPoint.AddRecentText(String.Format("Looked up vehicle: {0} ({1})", lp, GetVehicleDisplayName(veh)));
            if (veh != null && !veh.IsPersistent)
            {
                veh.IsPersistent = true;
                Globals.persistedRageEntities.Add(veh);
            }
        }

        /// <summary>
        /// Adds a specified backup request to the recent actions screen.
        /// </summary>
        /// <param name="response">The response type</param>
        /// <param name="unit">The unit type</param>
        internal static void AddBackupRequestToRecents(string response, string unit)
        {
            MakeSpaceForNewRecent();
            EntryPoint.AddRecentText(String.Format("Requested backup: {0} ({1})", unit, response));
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
            return IsLSPDFRPluginRunning("ALPRPlus", new Version(1, 0, 0, 0));
        }

        internal static bool IsBPSRunning()
        {
            return IsLSPDFRPluginRunning("British Policing Script", new Version(0, 9, 0, 0));
        }

        internal static bool IsLSPDFRPlusRunning()
        {
            return IsLSPDFRPluginRunning("LSPDFR+", new Version(1, 6, 5, 0));
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

        internal static void Log(String message)
        {
            if (!String.IsNullOrWhiteSpace(message)) Game.LogTrivial(String.Format("C+: {0}", message));
        }

        internal static void LogCatch(String message)
        {
            if (!String.IsNullOrWhiteSpace(message)) Game.LogTrivial(String.Format("C+ Minor Exception: {0}", message));
        }


        internal static void LogDebug(String message)
        {
            if (!String.IsNullOrWhiteSpace(message)) Game.LogTrivial(String.Format("C+ DEV: {0}", message));
        }

        internal static void ShowError(String message, int duration = 5000)
        {
            if (!String.IsNullOrWhiteSpace(message)) Game.DisplaySubtitle(String.Format("~r~C+: ~w~{0}", message), duration);
        }

        internal static void ShowWarning(String message, int duration = 5000)
        {
            if (!String.IsNullOrWhiteSpace(message)) Game.DisplaySubtitle(String.Format("~y~C+: ~w~{0}", message), duration);
        }

        internal static String GetAssetPath(String fileName, bool fullPath = false)
        {
            if (fullPath)
                return String.Format(@"{0}\{1}{2}", Directory.GetCurrentDirectory(), Globals.DefaultAssetPath, fileName);
            return Globals.DefaultAssetPath + fileName;
        }

        internal static String GetIconPath(String iconName)
        {
            return String.Format(@"{0}icons\{1}", Globals.DefaultAssetPath, iconName);
        }

        internal static int GetIconSize()
        {
            return 26;
        }

        internal static String GetPedCurrentStreetName(Ped ped = null)
        {
            ped = ped != null ? ped : Game.LocalPlayer.Character;            
            return World.GetStreetName(ped.Position);
        }

        internal static String GetPedCurrentZoneName(Ped ped = null)
        {
            ped = ped != null ? ped : Game.LocalPlayer.Character;
            return Functions.GetZoneAtPosition(ped.Position).RealAreaName;
        }

        internal static Vector3 GetPedCurrentPos(Ped ped = null)
        {
            ped = ped != null ? ped : Game.LocalPlayer.Character;
            return ped.Position;
        }

        internal static String SimpleNotepadCut()
        {
            var s = SimpleNotepadCopy();
            Globals.SimpleNotepadText = String.Empty;
            return s;
        }

        internal static String SimpleNotepadCopy()
        {
            var s = Globals.SimpleNotepadText;
            return s == null ? String.Empty : s;
        }

        internal static String DetermineImagePath(Ped ped)
        {
            try
            {
                // if (ped == null || !ped.Exists()) return Function.DefaultPedImagePath;
                String modelName = String.Empty;
                if (ped != null && ped.IsValid()) modelName = ped.Model.Name;
                int headDrawableIndex = 0, headDrawableTextureIndex = 0;

                if (ped != null && ped.IsValid()) ped.GetVariation(0, out headDrawableIndex, out headDrawableTextureIndex);

                // String _model = String.Format(@"{0}__0_{1}_{2}", modelName, headDrawableIndex, headDrawableTextureIndex).ToLower();
                var path = Function.GetPedImagePath(modelName, headDrawableIndex, headDrawableTextureIndex);
                Function.LogDebug(String.Format("Loading image for model from {0}", path));
                return path;
            }
            catch
            {
                Function.LogDebug("DetermineImagePath Error");
                return Function.DefaultPedImagePath;
            }
        }

        internal static String DateFormatForPart(DateOutputPart part)
        {
            switch (part)
            {
                case DateOutputPart.DATE: return "d";
                case DateOutputPart.TIME: return "t";
                case DateOutputPart.ISO: return "o";
                default: return "g";
            }
        }

        internal static String ToLocalDateString(DateTime date, DateOutputPart output, bool convertToLocal = true)
        {
            var local = date;
            if (convertToLocal) local = date.ToLocalTime();
            switch (output)
            {
                case DateOutputPart.DATE: return local.ToShortDateString();
                case DateOutputPart.TIME: return local.ToShortTimeString();
                case DateOutputPart.ISO: return local.ToString("g");
                default: return local.ToString("f");
            }
        }

        /*
        internal static String ToLocalDateString(String date, DateOutputPart input = DateOutputPart.ALL, DateOutputPart output = DateOutputPart.ALL, bool convertToLocal = true)
        {
            DateTime parsed;

            if (DateTime.TryParseExact(date, DateFormatForPart(input), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsed))
                return ToLocalDateString(parsed, output, convertToLocal);
            else
                return date;
        }
        */

    }
}
