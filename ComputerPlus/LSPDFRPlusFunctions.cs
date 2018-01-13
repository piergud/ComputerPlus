using System;
using System.Collections.Generic;

using Rage;
using LSPD_First_Response.Engine.Scripting.Entities;
using LSPDFR_;
using LSPDFR_.API;

namespace ComputerPlus
{

    public static class LSPDFRPlusFunctions
    {

        /// <summary>
        /// Adds a new court case to the court system.
        /// </summary>
        /// <param name="DefendantPersona">LSPDFR persona of the defendant</param>
        /// <param name="Crime">String describing the crime committed, e.g. 'stealing a police vehicle'.</param>
        /// <param name="GuiltyChance">Percentage based chance of the suspect being found guilty. 100 = always guilty, 0 = never guilty.</param>
        /// <param name="CourtVerdict">The decision the court will come to, e.g. 'Sentenced to 5 months in prison'</param>
        public static void CreateNewCourtCase(Persona DefendantPersona, string Crime, int GuiltyChance, string CourtVerdict)
        {
            Functions.CreateNewCourtCase(DefendantPersona, Crime, GuiltyChance, CourtVerdict);
            //CreateNewCourtCase(DefendantPersona, Crime, GuiltyChance, CourtVerdict);
        }

        /// <summary>
        /// Adds a new court case to the court system (this overload is recommended only for debugging by setting an instant publish time).
        /// </summary>
        /// <param name="DefendantPersona">LSPDFR persona of the defendant</param>
        /// <param name="Crime">String describing the crime committed, e.g. 'stealing a police vehicle'.</param>
        /// <param name="GuiltyChance">Percentage based chance of the suspect being found guilty. 100 = always guilty, 0 = never guilty.</param>
        /// <param name="CourtVerdict">The decision the court will come to, e.g. 'Sentenced to 5 months in prison'</param>
        /// <param name="ResultsPublishTime">The DateTime when the results will become available to the player (use not recommended in release builds).</param>
        public static void CreateNewCourtCase(Persona DefendantPersona, string Crime, int GuiltyChance, string CourtVerdict, DateTime ResultsPublishTime)
        {
            Functions.CreateNewCourtCase(DefendantPersona, Crime, GuiltyChance, CourtVerdict, ResultsPublishTime);
        }

        /// <summary>
        /// Returns a court verdict for a prison sentence depending on the parameters.
        /// </summary>
        /// <param name="MinMonths"></param>
        /// <param name="MaxMonths"></param>
        /// <param name="SuspendedChance">Percentage based chance of the sentence being suspended. 100 = always suspended, 0 = never suspended.</param>
        /// <returns></returns>
        public static string DeterminePrisonSentence(int MinMonths, int MaxMonths, int SuspendedChance)
        {
            return Functions.DeterminePrisonSentence(MinMonths, MaxMonths, SuspendedChance);
        }

        /// <summary>
        /// Returns a court verdict for a fine depending on the parameters.
        /// </summary>
        /// <param name="MinFine"></param>
        /// <param name="MaxFine"></param>
        /// <returns></returns>
        public static string DetermineFineSentence(int MinFine, int MaxFine)
        {
            return Functions.DetermineFineSentence(MinFine, MaxFine);
        }

        /// <summary>
        /// Adds a custom question to the traffic stop questioning section.
        /// </summary>
        /// <param name="Suspect">The ped for whom the question should appear (must have this ped stopped for the question to appear).</param>
        /// <param name="Question"></param>
        /// <param name="Answer"></param>
        public static void AddQuestionToTrafficStop(Ped Suspect, string Question, string Answer)
        {
            Functions.AddQuestionToTrafficStop(Suspect, Question, Answer);
        }

        /// <summary>
        /// Adds a custom question to the traffic stop questioning section.
        /// </summary>
        /// <param name="Suspect">The ped for whom the question should appear (must have this ped stopped for the question to appear).</param>
        /// <param name="Question"></param>
        /// <param name="Answers">A list of possible answers. A random one will be selected.</param>
        public static void AddQuestionToTrafficStop(Ped Suspect, string Question, List<string> Answers)
        {
            Functions.AddQuestionToTrafficStop(Suspect, Question, Answers);
        }
        /// <summary>
        /// Adds a custom question to the traffic stop questioning section.
        /// </summary>
        /// <param name="Suspect">The ped for whom the question should appear (must have this ped stopped for the question to appear).</param>
        /// <param name="Questions">A list of possible questions. A random one will be selected.</param>
        /// <param name="Answers">A list of possible answers. A random one will be selected.</param>
        public static void AddQuestionToTrafficStop(Ped Suspect, List<string> Questions, List<string> Answers)
        {
            Functions.AddQuestionToTrafficStop(Suspect, Questions, Answers);
        }

        /// <summary>
        /// Adds a custom question to the traffic stop questioning section.
        /// </summary>
        /// <param name="Suspect">The ped for whom the question should appear (must have this ped stopped for the question to appear).</param>
        /// <param name="Question"></param>
        /// <param name="CallbackAnswer">The function passed will be called when the question is asked. The suspect will be passed to the function. It must return a string, which will be used as an answer.</param>
        public static void AddQuestionToTrafficStop(Ped Suspect, string Question, Func<Ped, string> CallbackAnswer)
        {
            Functions.AddQuestionToTrafficStop(Suspect, Question, CallbackAnswer);
        }

        /// <summary>
        /// Adds a custom question to the traffic stop questioning section.
        /// </summary>
        /// <param name="Suspect">The ped for whom the question should appear (must have this ped stopped for the question to appear).</param>
        /// <param name="Question"></param>
        /// <param name="Answers">A list of possible answers. A random one will be selected.</param>
        /// <param name="Callback">The function passed will be called when the question is asked. The suspect and the chosen answer will be passed to the function as parameters.</param>
        public static void AddQuestionToTrafficStop(Ped Suspect, string Question, List<string> Answers, Action<Ped, string> Callback)
        {
            Functions.AddQuestionToTrafficStop(Suspect, Question, Answers, Callback);
        }

        /// <summary>
        /// Hides the standard traffic stop questions for the specified ped.
        /// </summary>
        /// <param name="Suspect">Traffic stop questions will be hidden if this ped is the current suspect.</param>
        /// <param name="Hide">If true, hides standard questions. If false, shows standard questions.</param>
        public static void HideStandardTrafficStopQuestions(Ped Suspect, bool Hide)
        {
            Functions.HideStandardTrafficStopQuestions(Suspect, Hide);
        }

        /// <summary>
        /// Resets the traffic stop questions to default for the specified suspect.
        /// </summary>
        /// <param name="Suspect"></param>
        public static void ResetTrafficStopQuestions(Ped Suspect)
        {
            Functions.ResetTrafficStopQuestions(Suspect);
        }

        /// <summary>
        /// Raised whenever the player orders a ped out of a vehicle on a traffic stop.
        /// </summary>
        public static event PedEvent PedOrderedOutOfVehicle;

        /// <summary>
        /// Raised whenever the player initiates a traffic stop on a suspect.
        /// </summary>
        public static event PedEvent TrafficStopInitiated;

        /// <summary>
        /// Raised whenever the player joins a pursuit that's active.
        /// </summary>
        public static event Action PlayerJoinedActivePursuit;

        /// <summary>
        /// Returns the current pursuit tactic.
        /// </summary>
        /// <returns></returns>
        public static PursuitTactics GetCurrentPursuitTactics()
        {
            return Functions.GetCurrentPursuitTactics();
        }

        /// <summary>
        /// Returns true if automatic tactics are enabled for pursuits, and false if not.
        /// </summary>
        /// <returns></returns>
        public static bool ArePursuitTacticsAutomatic()
        {
            return Functions.ArePursuitTacticsAutomatic();
        }
    }
}
