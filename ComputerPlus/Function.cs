using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.API;

namespace ComputerPlus
{
    public static class Function
    {
        private static Texture _bg;
        private static bool _bg_enabled = false;

        /// <summary>
        /// Gets whether Realism Dispatch Enhanced is installed or not.
        /// </summary>
        private static bool HasRDEInstalled
        {
            get { return IsDLCPresent("rde"); }
        }

        /// <summary>
        /// Gets whether the RDE LSPDFR addon is installed or not.
        /// </summary>
        private static bool HasRDELSPDFRAddonInstalled
        {
            get { return IsDLCPresent("bcso") && IsDLCPresent("rdesheriffaddon"); }
        }

        /// <summary>
        /// Gets whether the computer background is enabled or not.
        /// </summary>
        public static bool IsBackgroundEnabled
        {
            get { return _bg_enabled; }
        }

        /// <summary>
        /// Returns whether a vehicle is a police vehicle or not.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        /// <returns>True if it is a police vehicle, false if not.</returns>
        public static bool IsPoliceVehicle(Vehicle veh)
        {
            if (veh.HasSiren
                && veh.Model.Hash != 0x73920F8E // Ambulance
                && veh.Model.Hash != 0x45D56ADA // Fire Truck
                && veh.Model.Hash != 0x1BF8D381) // Lifeguard SUV
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enables the police computer's background.
        /// </summary>
        public static void EnableBackground() 
        {
            _bg = LoadBackground(GetBackgroundFileNameForVehicle(Game.LocalPlayer.Character.CurrentVehicle));
            if (_bg == null) 
            {
                Game.LogTrivial(@"Failed to load LSPDFR Computer+ background. Please ensure all backgrounds are present in Plugins\LSPDFR\ComputerPlus\backgrounds\.");
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "LSPDFR Computer+", "~r~Error", @"Failed to load background in Plugins\LSPDFR\ComputerPlus\backgrounds\.");
            }
            else 
            {
                Game.RawFrameRender += _OnRawFrameRender;
                _bg_enabled = true;
            }
        }

        /// <summary>
        /// Disables the police computer's background.
        /// </summary>
        public static void DisableBackground()
        {
            Game.RawFrameRender -= _OnRawFrameRender;
            _bg_enabled = false;
        }

        private static void _OnRawFrameRender(object sender, GraphicsEventArgs e) 
        {
            e.Graphics.DrawTexture(_bg, 0f, 0f, Game.Resolution.Width, Game.Resolution.Height);
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
                file = "lspd.jpg";
            }
            return file;
        }

        private static void _MakeSpaceForNewRecent()
        {
            if (EntryPoint.recent_text.Count == 7)
                EntryPoint.recent_text.RemoveAt(0);
        }

        /// <summary>
        /// Adds a specific ped to the recent actions screen.
        /// </summary>
        /// <param name="ped">The ped</param>
        public static void AddPedToRecents(Ped ped)
        {
            _MakeSpaceForNewRecent();
            string name = Functions.GetPersonaForPed(ped).FullName;
            EntryPoint.recent_text.Add(String.Format("Looked up person: {0}", name));
        }

        /// <summary>
        /// Adds a specified vehicle to the recent actions screen.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        public static void AddVehicleToRecents(Vehicle veh)
        {
            _MakeSpaceForNewRecent();
            string lp = veh.LicensePlate;
            EntryPoint.recent_text.Add(String.Format("Looked up vehicle: {0} ({1})", lp, Function.GetVehicleDisplayName(veh)));
        }

        /// <summary>
        /// Adds a specified backup request to the recent actions screen.
        /// </summary>
        /// <param name="response">The response type</param>
        /// <param name="unit">The unit type</param>
        public static void AddBackupRequestToRecents(string response, string unit)
        {
            _MakeSpaceForNewRecent();
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
        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
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

        /// <summary>
        /// Gets the in-game display name of a specified vehicle.
        /// </summary>
        /// <param name="veh">The vehicle</param>
        /// <returns>The in-game display name of the vehicle</returns>
        public static string GetVehicleDisplayName(Vehicle veh)
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
        public static string GetStringFromNative(ulong native_hash, params NativeArgument[] args)
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
        public static string GetStringFromNative(String native_name, params NativeArgument[] args)
        {
            IntPtr ptr = NativeFunction.CallByName<IntPtr>(native_name, args);
            String str = Marshal.PtrToStringAnsi(ptr);
            return str;
        }
    }
}
