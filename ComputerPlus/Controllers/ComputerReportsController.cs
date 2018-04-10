using System;
using System.Collections.Generic;
using ComputerPlus.Interfaces.Reports.Arrest;
using ComputerPlus.Interfaces.Reports.Citation;
using ComputerPlus.Interfaces.Reports.Models;
using ComputerPlus.Controllers.Models;
using LSPD_First_Response.Mod.API;
using ComputerPlus.Interfaces.ComputerPedDB;
using System.Linq;
using Rage;
using ComputerPlus.DB;
using static ComputerPlus.Interfaces.Reports.Models.ArrestReportAdditionalParty;
using LiteDB;
using System.Globalization;
using static ComputerPlus.Extensions.Gwen.TextBoxExtensions;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace ComputerPlus.Controllers
{
    class ComputerReportsController
    {
        private static int SECONDS_IN_A_DAY = 60 * 60 * 24;

        public static void ShowArrestReportCreate()
        {
            if (Globals.PendingArrestReport == null)
                Globals.PendingArrestReport = new ArrestReport();
            Globals.Navigation.Push(new ArrestReportContainer(Globals.PendingArrestReport, null));
        }

        public static void ShowArrestReportCreate(ComputerPlusEntity entity, ArrestReportContainer.ArrestReportActionEvent callbackDelegate)
        {
            ArrestReport report;
            var container = ArrestReportContainer.CreateForPed(entity, out report);
            if (callbackDelegate != null) container.OnArrestReportAction += callbackDelegate;
            Globals.PendingArrestReport = report;
            Globals.Navigation.Push(container);
        }

        public static void ShowArrestReportView(ArrestReport report)
        {
            PopulateArrestLineItems(report);
            PopulateArrestParties(report);
            //Globals.Navigation.Push(new ArrestReportContainer(report));
            Globals.Navigation.Push(new ArrestReportViewContainer(report));
        }

        public static void ShowArrestReportList()
        {
            var reports = GetAllArrestReports(0, 256);
            if (reports == null) Function.Log("Reports is null");
            else if (Globals.Navigation == null) Function.Log("Global nav is null");
            else
            Globals.Navigation.Push(new ArrestReportListContainer(reports));
        }

        private static void insertArrestReport(ArrestReport report)
        {
            var arrestReportDoc = new ArrestReportDoc
            {
                Id = report.id,
                ArrestTimeDate = report.ArrestTimeDate.ToString(Function.DateFormatForPart(DateOutputPart.ISO)),
                FirstName = report.FirstName,
                LastName = report.LastName,
                DOB = report.DOB,
                HomeAddress = report.HomeAddress,
                ArrestStreetAddress = report.ArrestStreetAddress,
                ArrestCity = report.ArrestCity,
                Details = report.Details
            };
            Globals.Store.arrestReportCollection.Insert(arrestReportDoc);
        }

        private static void updateArrestReport(ArrestReport report)
        {
            var arrestReportDoc = Globals.Store.arrestReportCollection.FindById(report.id);
            arrestReportDoc.ArrestTimeDate = report.ArrestTimeDate.ToString(Function.DateFormatForPart(DateOutputPart.ISO));
            arrestReportDoc.FirstName = report.FirstName;
            arrestReportDoc.LastName = report.LastName;
            arrestReportDoc.DOB = report.DOB;
            arrestReportDoc.HomeAddress = report.HomeAddress;
            arrestReportDoc.ArrestStreetAddress = report.ArrestStreetAddress;
            arrestReportDoc.ArrestCity = report.ArrestCity;
            arrestReportDoc.Details = report.Details;

            Globals.Store.arrestReportCollection.Update(arrestReportDoc); ;
        }

        private static void insertArrestReportLineItem(ArrestChargeLineItem charge, Guid arrestReportId)
        {
            var arrestReportChargeDoc = new ArrestReportChargeDoc
            {
                Charge = charge.Charge,
                IsFelony = charge.IsFelony,
                Note = charge.Note,
                ArrestReportId = arrestReportId
            };
            Globals.Store.arrestReportChargeCollection.Insert(arrestReportChargeDoc);
        }

        private static void insertArrestReportParties(ArrestReportAdditionalParty party, Guid arrestReportId)
        {
            var arrestReportPartyDoc = new ArrestReportPartyDoc
            {
                PartyType = (int)party.PartyType,
                FirstName = party.FirstName,
                LastName = party.LastName,
                DOB = party.DOB,
                ArrestReportId = arrestReportId
            };
            Globals.Store.arrestReportPartyCollection.Insert(arrestReportPartyDoc);
        }

        private static void clearArrestReportLineItem(Guid arrestReportId)
        {
            Globals.Store.arrestReportChargeCollection.Delete(x => x.ArrestReportId == arrestReportId);
        }

        private static void clearArrestReportParties(Guid arrestReportId)
        {
            Globals.Store.arrestReportPartyCollection.Delete(x => x.ArrestReportId == arrestReportId);
        }

        public static ArrestReport SaveArrestRecord(ArrestReport report)
        {
            try
            {
                if (report.IsNew)
                {
                    report.id = Guid.NewGuid();

                    insertArrestReport(report);
                    if (report.Charges != null && report.Charges.Count > 0)
                    {
                        foreach (var charge in report.Charges)
                        {
                            insertArrestReportLineItem(charge, report.id);
                        }
                    }
                    if (report.AdditionalParties != null && report.AdditionalParties.Count > 0)
                    {
                        foreach (var party in report.AdditionalParties)
                        {
                            insertArrestReportParties(party, report.id);
                        }
                    }
                }
                else
                {
                    updateArrestReport(report);
                    if (report.Charges != null && report.Charges.Count > 0)
                    {
                        clearArrestReportLineItem(report.id);
                        foreach (var charge in report.Charges)
                        {
                            insertArrestReportLineItem(charge, report.id);
                        }
                    }
                    if (report.AdditionalParties != null && report.AdditionalParties.Count > 0)
                    {
                        clearArrestReportParties(report.id);
                        foreach (var party in report.AdditionalParties)
                        {
                            insertArrestReportParties(party, report.id);
                        }
                    }

                }
                return report;
            }
            catch(Exception e)
            {
                Function.LogCatch(e.ToString());
                return null;
            }
        }

        private static void PopulateArrestReportCharges(ArrestReport arrestReport)
        {
            try
            {
                List<ArrestReportChargeDoc> chargeDocs = Globals.Store.arrestReportChargeCollection.Find(x => x.ArrestReportId == arrestReport.id).ToList();
                foreach (var chargeDoc in chargeDocs)
                {
                    var charge = new ArrestChargeLineItem();
                    charge.id = chargeDoc.Id;
                    charge.Charge = chargeDoc.Charge;
                    charge.IsFelony = chargeDoc.IsFelony;
                    charge.Note = chargeDoc.Note == null ? String.Empty : chargeDoc.Note;
                    charge.ReportId = chargeDoc.ArrestReportId;
                    arrestReport.Charges.Add(charge);
                }
             }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
        }

        private static void PopulateArrestParties(ArrestReport arrestReport)
        {
            try
            {
                List<ArrestReportPartyDoc> partyDocs = Globals.Store.arrestReportPartyCollection.Find(x => x.ArrestReportId == arrestReport.id).ToList();
                foreach (var partyDoc in partyDocs)
                {
                    var party = new ArrestReportAdditionalParty();
                    party.id = partyDoc.Id;
                    party.PartyType = (PartyTypes)partyDoc.PartyType;
                    party.FirstName = partyDoc.FirstName == null ? String.Empty : partyDoc.FirstName;
                    party.LastName = partyDoc.LastName == null ? String.Empty : partyDoc.LastName;
                    party.DOB = partyDoc.DOB == null ? String.Empty : partyDoc.DOB;
                    party.ReportId =partyDoc.ArrestReportId;
                    arrestReport.AdditionalParties.Add(party);
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
        }

        private static ArrestReport convertArrestReportDoc(ArrestReportDoc arrestReportDoc)
        {
            var arrestReport = new ArrestReport();
            arrestReport.id = arrestReportDoc.Id;
            arrestReport.ArrestTimeDate = DateTime.ParseExact(arrestReportDoc.ArrestTimeDate, Function.DateFormatForPart(DateOutputPart.ISO), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal);
            arrestReport.FirstName = arrestReportDoc.FirstName;
            arrestReport.LastName = arrestReportDoc.LastName;
            arrestReport.DOB = arrestReportDoc.DOB;
            arrestReport.HomeAddress = arrestReportDoc.HomeAddress;
            arrestReport.ArrestStreetAddress = arrestReportDoc.ArrestStreetAddress;
            arrestReport.ArrestCity = arrestReportDoc.ArrestCity;
            arrestReport.Details = arrestReportDoc.Details == null ? String.Empty : arrestReportDoc.Details;

            return arrestReport;
        }

        public static List<ArrestReport> GetAllArrestReports(int skip = 0, int limit = 100)
        {
            var arrestReports = new List<ArrestReport>();
            try
            {
                List<ArrestReportDoc> arrestReportDocs = Globals.Store.arrestReportCollection.Find(Query.All("ArrestTimeDate", Query.Descending), skip, limit).ToList();
                foreach (var arrestReportDoc in arrestReportDocs)
                {
                    var arrestReport = convertArrestReportDoc(arrestReportDoc);
                    arrestReports.Add(arrestReport);
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            return arrestReports;
        }

        private static ArrestReport generateRandomArrestReport(ComputerPlusEntity entity)
        {
            ArrestReport newArrest = ArrestReport.CreateForPed(entity);
            int randomSeconds = Globals.Random.Next(SECONDS_IN_A_DAY * 7, SECONDS_IN_A_DAY * 1200) * -1;
            newArrest.ArrestTimeDate = DateTime.Now.AddSeconds(randomSeconds);
            Vector3 randomLocation = Rage.World.GetRandomPositionOnStreet();
            newArrest.ArrestStreetAddress = Rage.World.GetStreetName(randomLocation);
            newArrest.ArrestCity = Functions.GetZoneAtPosition(randomLocation).RealAreaName;
            newArrest.Details = String.Empty;

            string randomChargeName = ComputerPedController.GetRandomWantedReason();
            bool isFelony = false;
            randomChargeName = randomChargeName.Substring(randomChargeName.LastIndexOf("=>") + 3);
            if (randomChargeName.EndsWith("(F)"))
            {
                isFelony = true;
                randomChargeName = randomChargeName.Substring(0, randomChargeName.Length - 3);
            }
            ArrestChargeLineItem newCharge = new ArrestChargeLineItem();
            newCharge.id = new Guid();
            newCharge.Charge = randomChargeName;
            newCharge.IsFelony = isFelony;
            newCharge.Note = String.Empty;
            newArrest.Charges.Add(newCharge);

            return SaveArrestRecord(newArrest);
        }

        private static void generatePastArrestNum(ComputerPlusEntity entity)
        {
            if (entity.Ped.Metadata.pastArrestNum == null)
            {
                int randomNum = Globals.Random.Next(1, 12);
                int pastArrestNum;
                if (entity.PedPersona.Wanted)
                {
                    if (randomNum > 5)
                        pastArrestNum = 0;
                    else
                        pastArrestNum = randomNum;
                }
                else
                {
                    if (randomNum > 2)
                        pastArrestNum = 0;
                    else
                        pastArrestNum = randomNum;
                }
                entity.Ped.Metadata.pastArrestNum = pastArrestNum;
            }
        }

        public static List<ArrestReport> GetArrestReportsForPed(ComputerPlusEntity entity)
        {
            // check if ped has past arrest reports
            List<ArrestReport> pastArrestFromDB = GetArrestReportsForPed(entity.FirstName, entity.LastName, entity.DOBString);
            return pastArrestFromDB.OrderByDescending(o => o.ArrestTimeDate).ToList();
        }

        public static List<ArrestReport> GetArrestReportsForPed(String firstName = "", String lastName = "", String dob = "")
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            dob = dob.Trim();
            var arrestReports = new List<ArrestReport>();
            try
            {
                List<ArrestReportDoc> arrestReportDocs = Globals.Store.arrestReportCollection.Find(x => x.DOB.Equals(dob) && x.FirstName.Equals(firstName) && x.LastName.Equals(lastName)).ToList();
                foreach (var arrestReportDoc in arrestReportDocs)
                {
                    var arrestReport = convertArrestReportDoc(arrestReportDoc);
                    PopulateArrestReportCharges(arrestReport);
                    PopulateArrestParties(arrestReport);
                    arrestReports.Add(arrestReport);
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            return arrestReports.OrderByDescending(o => o.ArrestTimeDate).ToList();
        }

        public static bool PopulateArrestLineItems(ArrestReport report)
        {
            PopulateArrestReportCharges(report);
            return true;
        }

        /** Traffic Citations **/

        public static void ShowTrafficCitationList()
        {
            var citations = GetAllTrafficCitations(0, 256);
            if (citations == null) Function.Log("Citations are null");
            else if (Globals.Navigation == null) Function.Log("Global nav is null");
            else
                Globals.Navigation.Push(new TrafficCitationListContainer(citations));
        }

        public static void ShowTrafficCitationView(TrafficCitation citation)
        {
            Globals.Navigation.Push(new TrafficCitationCreateContainer(citation, TrafficCitationView.ViewTypes.VIEW)); //Switch to view
        }

        public static void ShowTrafficCitationCreate(TrafficCitation citation, ComputerPlusEntity entity = null, TrafficCitationView.TrafficCitationActionEvent callbackDelegate = null)
        {
            TrafficCitation mCitation = null;
            if (citation == null && entity == null)
            {
                mCitation = Globals.PendingTrafficCitation != null ? Globals.PendingTrafficCitation : new TrafficCitation();
            }
            else if (citation == null && entity.Validate())
            {
                Function.Log("Creating Traffic Citation for ped in vehicle");
                mCitation = TrafficCitation.CreateForPedInVehicle(entity);
                Globals.PendingTrafficCitation = mCitation;
                Function.Log("Created Traffic Citation for ped  " + mCitation.FirstName);
            }
            else if (citation != null && citation == Globals.PendingTrafficCitation && !citation.FullName.Equals(entity.FullName))
            {
                if (entity.Validate()) mCitation = TrafficCitation.CreateForPedInVehicle(entity);
                else mCitation = new TrafficCitation();
                Globals.PendingTrafficCitation = mCitation;
            }
            else
            {
                mCitation = citation;
            }
            Globals.Navigation.Push(new TrafficCitationCreateContainer(mCitation, TrafficCitationView.ViewTypes.CREATE, callbackDelegate));
        }

        private static void insertTrafficCitation(TrafficCitation citation)
        {
            var citationDoc = new TrafficCitationDoc
            {
                Id = citation.id,
                CitationTimeDate = citation.CitationTimeDate.ToString(Function.DateFormatForPart(DateOutputPart.ISO)),
                FirstName = citation.FirstName,
                LastName = citation.LastName,
                DOB = citation.DOB,
                HomeAddress = citation.HomeAddress,
                CitationStreetAddress = citation.CitationStreetAddress,
                CitationCity = citation.CitationCity,
                CitationPosX = (float) citation.CitationPosX,
                CitationPosY = (float) citation.CitationPosY,
                CitationPosZ = (float) citation.CitationPosZ,
                VehicleType = citation.VehicleType,
                VehicleModel = citation.VehicleModel,
                VehicleTag = citation.VehicleTag,
                VehicleColor = citation.VehicleColor,
                CitationReason = citation.CitationReason,
                CitationAmount = citation.CitationAmount,
                Details = citation.Details,
                IsArrestable = citation.IsArrestable
            };
            Globals.Store.citationCollection.Insert(citationDoc);
        }

        private static void updateTrafficCitation(TrafficCitation citation)
        {
            var citationDoc = Globals.Store.citationCollection.FindOne(x => x.Id == citation.id);
            citationDoc.CitationTimeDate = citation.CitationTimeDate.ToString(Function.DateFormatForPart(DateOutputPart.ISO));
            citationDoc.FirstName = citation.FirstName;
            citationDoc.LastName = citation.LastName;
            citationDoc.DOB = citation.DOB;
            citationDoc.HomeAddress = citation.HomeAddress;
            citationDoc.CitationStreetAddress = citation.CitationStreetAddress;
            citationDoc.CitationCity = citation.CitationCity;
            citationDoc.CitationPosX = (float)citation.CitationPosX;
            citationDoc.CitationPosY = (float)citation.CitationPosY;
            citationDoc.CitationPosZ = (float)citation.CitationPosZ;
            citationDoc.VehicleType = citation.VehicleType;
            citationDoc.VehicleModel = citation.VehicleModel;
            citationDoc.VehicleTag = citation.VehicleTag;
            citationDoc.VehicleColor = citation.VehicleColor;
            citationDoc.CitationReason = citation.CitationReason;
            citationDoc.CitationAmount = citation.CitationAmount;
            citationDoc.Details = citation.Details;
            citationDoc.IsArrestable = citation.IsArrestable;

            Globals.Store.citationCollection.Update(citationDoc);
        }


        public static TrafficCitation SaveTrafficCitation(TrafficCitation citation)
        {
            try
            {
                if (citation.IsNew)
                {
                    citation.id = Guid.NewGuid();
                    insertTrafficCitation(citation);
                }
                else
                {
                    updateTrafficCitation(citation);
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            if (Globals.PendingTrafficCitation != null && Globals.PendingTrafficCitation == citation) Globals.PendingTrafficCitation = null;
            return citation;
        }

        private static TrafficCitation convertCitationDoc(TrafficCitationDoc citationDoc)
        {
            var citation = new TrafficCitation();
            citation.id = citationDoc.Id;
            citation.CitationTimeDate = DateTime.ParseExact(citationDoc.CitationTimeDate, Function.DateFormatForPart(DateOutputPart.ISO), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal);
            citation.FirstName = citationDoc.FirstName;
            citation.LastName = citationDoc.LastName;
            citation.DOB = citationDoc.DOB;
            citation.HomeAddress = citationDoc.HomeAddress;
            citation.CitationStreetAddress = citationDoc.CitationStreetAddress;
            citation.CitationCity = citationDoc.CitationCity;
            var posX = citationDoc.CitationPosX;
            var posY = citationDoc.CitationPosY;
            var posZ = citationDoc.CitationPosZ;
            citation.CitationPos = new Vector3(posX, posY, posZ);
            citation.VehicleType = citationDoc.VehicleType == null ? String.Empty : citationDoc.VehicleType;
            citation.VehicleModel = citationDoc.VehicleModel == null ? String.Empty : citationDoc.VehicleModel; 
            citation.VehicleTag = citationDoc.VehicleTag == null ? String.Empty : citationDoc.VehicleTag;
            citation.VehicleColor = citationDoc.VehicleColor == null ? String.Empty : citationDoc.VehicleColor;
            citation.CitationReason = citationDoc.CitationReason;
            citation.CitationAmount = citationDoc.CitationAmount;
            citation.Details = citationDoc.Details == null ? String.Empty : citationDoc.Details;
            citation.IsArrestable = citationDoc.IsArrestable;

            return citation;
        }

        public static List<TrafficCitation> GetAllTrafficCitations(int skip = 0, int limit = 100)
        {
            var citations = new List<TrafficCitation>();
            try
            {
                List<TrafficCitationDoc> citationDocs = Globals.Store.citationCollection.Find(Query.All("CitationTimeDate", Query.Descending), skip, limit).ToList();
                foreach (var citationDoc in citationDocs)
                {
                    citations.Add(convertCitationDoc(citationDoc));
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            return citations;
        }

        private static TrafficCitation generateRandomCitation(ComputerPlusEntity entity)
        {
            TrafficCitation newCitation = TrafficCitation.CreateForPedInVehicle(entity);
            newCitation.VehicleType = "N/A";
            int randomSeconds = Globals.Random.Next(SECONDS_IN_A_DAY * 7, SECONDS_IN_A_DAY * 1200) * -1;
            newCitation.CitationTimeDate = DateTime.Now.ToUniversalTime().AddSeconds(randomSeconds);
            newCitation.CitationPos = Rage.World.GetRandomPositionOnStreet();
            newCitation.CitationStreetAddress = Rage.World.GetStreetName(newCitation.CitationPos);
            newCitation.CitationCity = Functions.GetZoneAtPosition(newCitation.CitationPos).RealAreaName;
            newCitation.Citation = ComputerPedController.GetRandomCitation();
            return SaveTrafficCitation(newCitation);
        }

        public static List<TrafficCitation> GetTrafficCitationsForPed(ComputerPlusEntity entity)
        {
            // check if ped has past citations
            List<TrafficCitation> pastCitationFromDB = GetTrafficCitationsForPed(entity.FirstName, entity.LastName, entity.DOBString);
            return pastCitationFromDB.OrderByDescending(o => o.CitationTimeDate).ToList();
        }

        public static List<TrafficCitation> GetTrafficCitationsForPed(String firstName, String lastName, String dob)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            dob = dob.Trim();
            var citations = new List<TrafficCitation>();
            try
            {
                List<TrafficCitationDoc> citationDocs = Globals.Store.citationCollection.Find(x => x.DOB.Equals(dob) && x.FirstName.Equals(firstName) && x.LastName.Equals(lastName)).ToList();
                foreach (var citationDoc in citationDocs)
                {
                    citations.Add(convertCitationDoc(citationDoc));
                }
            }
            catch (Exception e)
            {
                Function.LogCatch(e.ToString());
            }
            return citations;
        }
       
        public static DetailedEntity GetAllReportsForPed(ComputerPlusEntity entity)
        {            
            var arrests = GetArrestReportsForPed(entity);
            var traffic = GetTrafficCitationsForPed(entity);
            return new DetailedEntity(entity, arrests, traffic);
        }

        public static void generateRandomHistory(ComputerPlusEntity entity) {
            if (Configs.RandomHistoryRecords && entity.Ped != null && entity.Ped.Metadata.randomReportsGenerated == null)
            {
                entity.Ped.Metadata.randomReportsGenerated = true;

                // generate random Citation history
                if (entity.PedPersona.Citations > 0)
                {
                    for (var i = 0; i < entity.PedPersona.Citations; i++)
                    {
                        generateRandomCitation(entity);
                    }
                }

                // generate random Arrest history
                generatePastArrestNum(entity);
                if (entity.Ped.Metadata.pastArrestNum > 0)
                {
                    // generate past arrest history
                    for (var i = 0; i < entity.Ped.Metadata.pastArrestNum; i++)
                    {
                        generateRandomArrestReport(entity);
                    }
                }
            }
        }

        public static void createCourtCaseForArrest(ArrestReport report, ComputerPlusEntity entity)
        {
            if (Configs.EnableLSPDFRPlusIntegration && Function.IsLSPDFRPlusRunning() && entity != null)
            {
                string crimeStr = String.Empty;
                string courtVerdictStr = String.Empty;
                int guiltyChance = Globals.Random.Next(75, 101);
                int verdictMonths = 0;
                int verdictYears = 0;
                bool trafficFelonyCharged = false;

                foreach (var charge in report.Charges)
                {
                    if (crimeStr.Equals(String.Empty)) crimeStr = charge.Charge;
                    else crimeStr += ", " + charge.Charge;

                    if (charge.IsTraffic)
                    {
                        if (charge.IsFelony)
                        {
                            if (!trafficFelonyCharged)
                            {
                                courtVerdictStr = "License revoked";
                                verdictMonths = Globals.Random.Next(3, 13);
                                trafficFelonyCharged = true;
                            }
                        }
                        else
                        {
                            if (!trafficFelonyCharged)
                            {
                                courtVerdictStr = "Fined $" + Globals.Random.Next(500, 1001) + ". License suspended for " + Globals.Random.Next(6, 13) + " months";
                            }
                        }
                    }
                    else
                    {
                        if (charge.IsFelony) verdictYears += Globals.Random.Next(4, 11);
                        else verdictYears += Globals.Random.Next(1, 4);
                    }
                }

                if (verdictYears > 0 && verdictMonths > 0) verdictYears++;
                string sentencePrisonStr = String.Empty;

                if (verdictYears > 0) sentencePrisonStr = "Sentenced to " + verdictYears + " years in prison";
                else if (verdictMonths > 0) sentencePrisonStr = "Sentenced to " + verdictMonths + " months in prison";

                if (courtVerdictStr.Equals(String.Empty)) courtVerdictStr = sentencePrisonStr;
                else courtVerdictStr += ". " + sentencePrisonStr;

                LSPDFRPlusFunctions.CreateNewCourtCase(entity.PedPersona, crimeStr, guiltyChance, courtVerdictStr);
            }
        }

        public static void createCourtCaseForCitations(List<TrafficCitation> citations, Ped ped)
        {
            if (Configs.EnableLSPDFRPlusIntegration && Function.IsLSPDFRPlusRunning() && ped != null && ped.IsValid())
            {
                Persona pedPersona = Functions.GetPersonaForPed(ped);
                string citationStr = String.Empty;
                string courtVerdictStr = String.Empty;
                int guiltyChance = Globals.Random.Next(75, 101);
                int totalFine = 0;
                bool containTrafficCitation = false;
                bool isSuspended = false;
                bool isRevoked = false;

                foreach (var citation in citations)
                {
                    if (citation.CreateCourtCase)
                    {
                        if (citationStr.Equals(String.Empty)) citationStr = citation.Citation.Name;
                        else citationStr += ", " + citation.Citation.Name;

                        int maxFine = (int)citation.Citation.FineAmount;
                        int fine = 0;
                        int randumNum = Globals.Random.Next(0, 100);
                        if (randumNum < 25)
                            fine = (int)(maxFine * 0.80f);
                        else if (randumNum < 50)
                            fine = (int)(maxFine * 0.65f);
                        else if (randumNum < 75)
                            fine = (int)(maxFine * 0.5f);
                        else
                            fine = maxFine;

                        totalFine += fine;

                        if (!citation.Citation.IsPublic)
                        {
                            containTrafficCitation = true;
                            if (citation.Citation.IsArrestable) isRevoked = true;
                        }
                    }
                }

                if (!citationStr.Equals(String.Empty))
                {
                    if (containTrafficCitation && !isRevoked && totalFine > 500) isSuspended = true;
                    courtVerdictStr = "Fined $" + totalFine;

                    if (isRevoked)
                        courtVerdictStr += ". License revoked";
                    else if (isSuspended)
                        courtVerdictStr += ". License suspended for " + Globals.Random.Next(6, 13) + " months";

                    LSPDFRPlusFunctions.CreateNewCourtCase(pedPersona, citationStr, guiltyChance, courtVerdictStr);
                }
            }
        }
    }
}
