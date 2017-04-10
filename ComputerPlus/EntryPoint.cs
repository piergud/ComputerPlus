using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Rage;
using Rage.Forms;
using LSPD_First_Response.Mod.API;
using ComputerPlus.Interfaces;
using ComputerPlus.Interfaces.ComputerPedDB;
using ComputerPlus.Interfaces.ComputerVehDB;
using ComputerPlus.Controllers.Models;
using ComputerPlus.DB;
using ComputerPlus.DB.Models;
using ComputerPlus.Controllers;
using ComputerPlus.Extensions.Rage;
using System.Linq;
using ComputerPlus.Extensions.Gwen;
using ComputerPlus.Extensions;
using System.Windows.Forms;
using ComputerPlus.Interfaces.SimpleNotepad;
using ComputerPlus.Interfaces.Reports.Models;
using System.Threading;
using System.Text;
using System.Globalization;

namespace ComputerPlus
{
    public sealed class EntryPoint : Plugin
    {
        internal static GwenForm login = null, main = null;
        internal delegate void VehicleStoppedEvent(object sender, Vehicle veh);
        internal static VehicleStoppedEvent OnVehicleStopped;

        internal delegate void FacingPedWithPendingTicketsEvent(object sender, Ped ped, List<TrafficCitation> citations);
        internal static FacingPedWithPendingTicketsEvent OnFacingPedWithPendingTickets;

        static Stopwatch sw = new Stopwatch();

        private static bool _prompted;
        internal static bool HasBackground
        {
            get;
            private set;
        } = false;

        internal bool IsMainComputerOpen = false;

        internal static List<string> recent_text = new List<string>();
        internal GameFiber CheckIfCalloutActiveFiber;
        private GameFiber DetectOpenCloseRequestedFiber;
        private GameFiber DetectOpenSimpleNotepadFiber;
        private GameFiber RunComputerPlusFiber;




        public override void Initialize()
        {
            DetectOpenCloseRequestedFiber = new GameFiber(ComputerPlusMain);
            RunComputerPlusFiber = new GameFiber(RunPoliceComputer);
            CheckIfCalloutActiveFiber = new GameFiber(CheckIfCalloutActive);
            DetectOpenSimpleNotepadFiber = new GameFiber(CheckOpenSimpleNotepad);
            Functions.OnOnDutyStateChanged += DutyStateChangedHandler;
            OnVehicleStopped += VehicleStoppedHandler;
            OnFacingPedWithPendingTickets += PedFacingPlayerWithPendingTickets;
            Globals.Navigation.OnFormAdded += NavOnFormAdded;
            Globals.Navigation.OnFormRemoved += NavOnFormRemoved;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
            Configs.RunConfigCheck();
            Function.checkForRageVersionClass.checkForRageVersion(0.41f);

        }

        public override void Finally()
        {
            Globals.Navigation.OnFormAdded -= NavOnFormAdded;
            Globals.Navigation.OnFormRemoved -= NavOnFormRemoved;
            if (RunComputerPlusFiber.IsRunning()) RunComputerPlusFiber.Abort();
            if (CheckIfCalloutActiveFiber.IsRunning()) CheckIfCalloutActiveFiber.Abort();
            Globals.Store.Close();
        }


        private void DutyStateChangedHandler(bool on_duty)
        {
            Globals.IsPlayerOnDuty = on_duty;

            if (on_duty)
            {
                XmlConfigs.ReadDefinitionsAndGlobalize();
                Function.MonitorAICalls();
                Function.CheckForUpdates();
                CheckIfCalloutActiveFiber.Resume();
                DetectOpenSimpleNotepadFiber.Resume();
                DetectOpenCloseRequestedFiber.Resume();
                Function.LogDebug("Successfully loaded LSPDFR Computer+.");
                InitStorage();
                if (Function.IsAlprPlusRunning())
                {
                    Function.LogDebug("C+: Registering for ALPR+ Events");
                    ALPRPlusFunctions.OnAlprPlusMessage += ALPRPlusFunctions_OnAlprPlusMessage;
                    ALPRPlusFunctions.RegisterForEvents();
                }
                else {
                    Function.LogDebug("C+: ALPR+ Not Detected");
                }
                if (Function.IsBPSRunning())
                {
                    // @TODO put this back once Albo tests integration
                    ComputerPlusEntity.PersonaType = PersonaTypes.BPS;
                }                
                
            }
            else
            {
                if (Function.IsAlprPlusRunning())
                {
                    ALPRPlusFunctions.OnAlprPlusMessage -= ALPRPlusFunctions_OnAlprPlusMessage;
                }
            }
        }

        private async void InitStorage()
        {
            try
            {
                await Globals.OpenStore();
                if (Globals.Store != null)
                {
                    Function.Log("Store was opened");
                }
                else
                {
                    Function.Log("Store was not opened");
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.Message);
            }
        }
        private static void ALPRPlusFunctions_OnAlprPlusMessage(object sender, ALPR_Arguments e)
        {
            ComputerVehicleController.AddAlprScan(e);
        }

        private static void VehicleStoppedHandler(object sender, Vehicle veh)
        {
            if (veh && !_prompted)
            {
                if (Function.IsPoliceVehicle(veh))
                //&& LSPD_First_Response.Mod.API.Functions.GetCurrentPullover() != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var friendlyName in Configs.OpenComputerPlus.Select(x => x.FriendlyName))
                    {
                        if (sb.Length == 0) sb.Append(friendlyName);
                        else sb.AppendFormat(" or {0}", friendlyName);
                    }


                    Game.DisplayHelp(String.Format("Hold {0} to open ~b~LSPDFR Computer+~w~.", sb.ToString()));
                    _prompted = true;
                }
            }
        }
        //Yes, this is ugly.. this will be removed once @redux is added
        private static List<List<TrafficCitation>> mPromptedCitations = new List<List<TrafficCitation>>();
        private static void PedFacingPlayerWithPendingTickets(object sender, Ped ped, List<TrafficCitation> citations)
        {
            if (mPromptedCitations.Contains(citations)) return;
            mPromptedCitations.Add(citations);
            StringBuilder sb = new StringBuilder();
            foreach (var friendlyName in Configs.GiveTicketsToPed.Select(x => x.FriendlyName))
            {
                if (sb.Length == 0) sb.Append(friendlyName);
                else sb.AppendFormat(" or {0}", friendlyName);
            }
            if (citations.Any(x => x.IsArrestable)) Game.DisplayNotification("~y~~h~WARNING~h~: ~s~One or more citations is arrestable. If you give the citations to the ped, they will be ROR");
            var message = String.Format("Press {0} to give {1} tickets to {2} totalling {3} and release {2}.",
                sb.ToString(),
                citations.Count,
                citations.First().FullName,
                citations.Sum(x => x.CitationAmount).ToString("c", CultureInfo.CurrentCulture));

            Game.DisplayNotification(message);
        }

        internal static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }


        private bool CheckOpenCloseDebouncedTrigger(KeyBinder[] binder)
        {
            if (binder.Any(x => x.IsPressed))
            {
                if (!sw.IsRunning)
                {
                    sw.Start();
                }
                else if (sw.ElapsedMilliseconds > 250)
                {
                    sw.Stop();
                    sw.Reset();
                    return true;
                }
            }
            else
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                    sw.Reset();
                }
            }
            return false;
        }

        private DateTime ToggleCooldown = DateTime.Now;

        private void CheckForOpenTrigger()
        {
            Vehicle curr_veh = Game.LocalPlayer.Character.Exists() ? Game.LocalPlayer.Character.LastVehicle : null;

            if (curr_veh && curr_veh.Driver == Game.LocalPlayer.Character && curr_veh.Speed <= 1 && Function.IsPoliceVehicle(curr_veh))
            {
                var currPullover = Functions.GetCurrentPullover();
                if (currPullover != null) {
                    var ped = Functions.GetPulloverSuspect(currPullover);
                    if (ped && ped.LastVehicle && ped.LastVehicle.Speed <= 1)
                    {
                        OnVehicleStopped(null, curr_veh);
                    }
                    
                }

                if (CheckOpenCloseDebouncedTrigger(Configs.OpenComputerPlus))
                {
                    ShowPoliceComputer();
                }
            }

        }

        private void CheckForCloseTrigger()
        {
            if (!Globals.CloseRequested && Configs.CloseComputerPlus.Any(x => x.IsPressed))
            {
                Globals.Navigation.Clear();
                ClosePoliceComputer();
            }
        }

        private bool? ShouldEndPullover;
        private void CheckForDeliverTicketTrigger()
        {
           
          if (!Globals.HasTrafficTicketsInHand() && (ShouldEndPullover.HasValue && ShouldEndPullover.Value) && !Game.LocalPlayer.Character.HasScenario())
            {
                ShouldEndPullover = null;

                GameFiber.StartNew(() =>
                {
                    if (Game.LocalPlayer.LastVehicle && !Game.LocalPlayer.LastVehicle.HasDriver) Game.DisplayNotification("The driver will wait until you are back in your vehicle before taking off");
                    while (Game.LocalPlayer.LastVehicle && !Game.LocalPlayer.LastVehicle.HasDriver) GameFiber.Yield(); //Wait for the player to enter their vehicle
                    Function.Log("Starting Ending pull over wait timer for ped to leave");
                    var stopAt = DateTime.Now.AddMilliseconds(5000); //have the sadPed drive off in 5 seconds if the traffic stop isnt over
                    while (DateTime.Now < stopAt) GameFiber.Yield();
                    try
                    {
                        
                        lock (mPromptedCitations) mPromptedCitations.Clear();
                        var handle = Functions.GetCurrentPullover();
                        if (handle != null)
                        {
                            Functions.ForceEndCurrentPullover();
                        }
                    }
                    catch (Exception e)
                    {
                        Function.LogCatch(e.Message);
                    }

                });

            }
            else if (Globals.HasTrafficTicketsInHand() && !Game.LocalPlayer.Character.HasScenario()) //only run when we have tickets and we're not already doing WORLD_HUMAN_CLIPBOARD
            {
                var stopped = World.GetEntities(Game.LocalPlayer.Character.Position, 2.5f, GetEntitiesFlags.ConsiderAllPeds);
                if (stopped != null && stopped.Count() > 0)
                {
                    var pedsAboutToGetTheSmackDown = stopped.Select(x => x as Ped)
                        .Where(x => x.DistanceTo(Game.LocalPlayer.Character.FrontPosition) < 2f && Globals.GetTrafficCitationsInHandForPed(x) != null); //may have to add ordering by distance
                    foreach (var sadPed in pedsAboutToGetTheSmackDown)
                    {
                        if (Configs.GiveTicketsToPed.Any(x => x.IsPressed))
                        {
                            //The user wants to give the sad ped the ticket now..
                            GameFiber.StartNew(() =>
                            {
                                var item = new Rage.Object(new Model("prop_cs_documents_01"), Game.LocalPlayer.Character.Position);
                                item.AttachTo(Game.LocalPlayer.Character, Game.LocalPlayer.Character.GetBoneIndex(PedBoneId.RightThumb1), new Vector3(item.Model.Dimensions.Length() * 0.4f, 0, 0), Rotator.Zero);
                                Game.LocalPlayer.Character.Tasks.PlayAnimation("mp_common", "givetake1_b", 3f, AnimationFlags.None).WaitForCompletion();
                                item.Detach();
                                item.Delete();
                            });
                            ShouldEndPullover = true;
                            Globals.RemoveTrafficCitationsInHandForPed(sadPed);
                            break;
                        }
                        else
                        {
                            
                            //Prompt the user that they can deliver the ticket
                            OnFacingPedWithPendingTickets(null, sadPed, Globals.GetTrafficCitationsInHandForPed(sadPed));
                        }

                    }
                }                
            }
            else if (Functions.GetCurrentPullover() == null && Globals.HasTrafficTicketsInHand())
            {
                Globals.ClearTrafficCitationsInHand();
                return;
            }
        }


        private void ComputerPlusMain()
        {
            do
            {
                if (!Globals.CloseRequested && Globals.OpenRequested)
                {
                    CheckForCloseTrigger();
                }
                else
                {
                    CheckForOpenTrigger();
                }

                CheckForDeliverTicketTrigger();

                //Function.Log(String.Format("{0} {1} {2}", Game.TimeScale, Game.GameTime, World.DateTime.ToString("hh:mm:ss.f")));
                //World.TimeOfDay = DateTime.Now.TimeOfDay;
                GameFiber.Yield();
            }
            while (Globals.IsPlayerOnDuty);
            GameFiber.Hibernate();
        }



        internal static void OpenMain()
        {
            if (Configs.SkipLogin) Globals.Navigation.Push(new ComputerMain());
            else Globals.Navigation.Replace(new ComputerMain());
        }

        internal static void OpenLogin()
        {
            Globals.Navigation.Push(new ComputerLogin());
        }

        private void ShowPoliceComputer()
        {
            Globals.CloseRequested = false;
            Globals.OpenRequested = true;
            if (RunComputerPlusFiber.IsHibernating) RunComputerPlusFiber.Wake();
            else if (!RunComputerPlusFiber.IsAlive) RunComputerPlusFiber.Start();
        }
        private void ClosePoliceComputer()
        {
            Globals.CloseRequested = true;
            Globals.OpenRequested = false;
        }

        private void RunPoliceComputer()
        {
            do
            {

                ShowBackground(Globals.ShowBackgroundWhenOpen);
                GameFiber.Yield();
                PauseGame(Globals.PauseGameWhenOpen);
                IsMainComputerOpen = true;
                if (!Configs.SkipLogin)
                {
                    OpenLogin();
                }
                else
                {
                    OpenMain();
                }


                do
                {
                    EnsurePaused();
                    GameFiber.Yield();
                }
                while (Globals.Navigation.Head != null);
                ClosePoliceComputer();
                Globals.Navigation.Clear();
                IsMainComputerOpen = false;
                PauseGame(false, true);
                ShowBackground(false, true);
                GameFiber.Yield(); //Yield to allow form fibers to close out
                GameFiber.Hibernate();

            }
            while (Globals.IsPlayerOnDuty);
            GameFiber.Hibernate();
        }


        private void NavOnFormAdded(object sender, NavigationController.NavigationEntry entry)
        {
            try
            {
                GameFiber.StartNew(() =>
                {
                    try
                    {
                        entry.form.Show();
                        do
                        {
                            GameFiber.Yield();
                        }
                        while (!Globals.CloseRequested && entry.form.IsOpen());

                        Globals.Navigation.RemoveEntry(entry, false);
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() != typeof(ThreadAbortException))
                            Function.LogCatch(e.ToString());
                    }
                    //NavOnFormRemoved(sender, entry);
                });
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
        }

        private void NavOnFormRemoved(object sender, NavigationController.NavigationEntry entry)
        {
            try
            {
                if (entry.form.Window == null || !entry.form.IsOpen()) return;
                GameFiber.StartNew(() =>
                {
                    entry.form.Window.Close();
                });
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(ThreadAbortException))
                    Function.LogCatch(e.ToString());
            }
        }

        private static void EnsurePaused()
        {
            if (Globals.PauseGameWhenOpen && !Game.IsPaused)
            {
                Function.Log("EnsurePaused forced");
                PauseGame(true, true);
            }
            BlockInputIfNeeded();
        }

        private static void PauseGame(bool pause, bool gameOnlyChange = false)
        {
            Function.Log("Pause");
            if (!gameOnlyChange) Globals.PauseGameWhenOpen = pause;
            Game.IsPaused = pause;
        }

        private static void ShowBackground(bool visible, bool gameOnlyChange = false)
        {
            if (!gameOnlyChange) Globals.ShowBackgroundWhenOpen = visible;
            if (visible)
                Function.EnableBackground();
            else
                Function.DisableBackground();
        }

        internal static void TogglePause()
        {
            PauseGame(!Globals.PauseGameWhenOpen);
        }

        internal static void ToggleBackground()
        {
            ShowBackground(!Globals.ShowBackgroundWhenOpen);
        }

        private static readonly List<GameControl> InputsToBlock = new List<GameControl> { GameControl.Context, GameControl.ContextSecondary, GameControl.Sprint, GameControl.FrontendPause, GameControl.FrontendPauseAlternate };
        internal static void BlockInputIfNeeded()
        {

            if (Globals.BlockInputNeeded.HasValue && Globals.BlockInputNeeded.Value)
            {
                Game.AlwaysReceiveKeyEvents = false;
                InputsToBlock.ForEach(x =>
                {
                    Game.DisableControlAction(0, x, true);
                    Game.DisableControlAction(1, x, true);
                });

            }
            else if (Globals.BlockInputNeeded.HasValue && !Globals.BlockInputNeeded.Value)
            {
                Game.AlwaysReceiveKeyEvents = true;
                Globals.BlockInputNeeded = null;
                InputsToBlock.ForEach(x =>
                {
                    Game.DisableControlAction(0, x, false);
                    Game.DisableControlAction(1, x, false);
                });
            }
        }

        private void CheckIfCalloutActive()
        {
            //set active callout to null whenever a callout ends

            while (Globals.IsPlayerOnDuty)
            {
                GameFiber.Yield();

                if (Globals.IsCalloutActive == true && Functions.IsCalloutRunning() == false && Globals.ActiveCallout != null)
                {
                    Function.ClearActiveCall();
                }
            }
        }

        public static void ShowNotepad(bool showPauseButton = true)
        {
            SimpleNotepad notepad = new SimpleNotepad();
            try
            {
                var navPushResult = Globals.Navigation.Push(notepad);
                notepad.Show();

                do
                {
                    EnsurePaused();
                    if (showPauseButton)
                    {
                        notepad.ShowPause(showPauseButton);
                        notepad.SetPauseState(Globals.PauseGameWhenOpen);
                    }

                    GameFiber.Yield();

                }
                while ((Globals.Navigation.Head == notepad || notepad.IsOpen()));
                notepad.Close();

            }
            catch { }
        }



        private void CheckOpenSimpleNotepad()
        {

            while (Globals.IsPlayerOnDuty)
            {
                try
                {
                    GameFiber.Yield();
                    if (!IsMainComputerOpen && Configs.OpenSimpleNotepad.Any(x => x.IsPressed))
                    {
                        EntryPoint.PauseGame(Globals.PauseGameWhenOpen, true);
                        ShowNotepad();
                        if (!IsMainComputerOpen) //Make sure game isnt paused if this is opened by itself
                            EntryPoint.PauseGame(false, true);
                    }
                    else
                    {

                    }
                }
                catch (Exception e)
                {
                    //Function.Log(e.ToString());
                }
            }
        }

    }
}
