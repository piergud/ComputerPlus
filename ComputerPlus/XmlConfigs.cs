using ComputerPlus.Interfaces.Reports.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputerPlus
{
    class XmlConfigs
    {
        static class Seralizers
        {
            internal static XmlSerializer ChargeCategories = new XmlSerializer(typeof(ChargeCategories));
            internal static XmlSerializer CitationCategories = new XmlSerializer(typeof(CitationCategories));
            internal static XmlSerializer VehicleDefinitions = new XmlSerializer(typeof(VehicleDefinitions));
        }
        public static class Configs
        {
            public static ChargeCategories ChargeCategories
            {
                get;
                internal set;
            }

            public static CitationCategories CitationCategories
            {
                get;
                internal set;
            }

            public static VehicleDefinitions VehicleDefinitions
            {
                get;
                internal set;
            }
        }
        public static class ConfigPaths
        {
            public static readonly String ChargeCategories = @"Plugins\LSPDFR\ComputerPlus\ChargeDefinitions.xml";
            public static readonly String CitationDefinitions = @"Plugins\LSPDFR\ComputerPlus\CitationDefinitions.xml";
            public static readonly String VehicleDefinitions = @"Plugins\LSPDFR\ComputerPlus\VehicleDefinitions.xml";

        }

        static T Parse<T>(Stream stream, XmlSerializer serializer)
        {
            if (stream == null)
            {
                Function.ShowError("Tried to parse ChargeCategories XML, but no file was found. Please check or redownload");
                return default(T);
            }
            try
            {
                return (T)serializer.Deserialize(stream); ;
            }
            catch(Exception e)
            {
               if (e is InvalidOperationException)
                {
                    Function.Log(e.ToString());
                    Function.ShowWarning("Could not parse ChargeCategories XML. Please check or redownload");
                    return default(T);
                }
                throw;
            }
        }

        public static ChargeCategories ReadArrestChargeDefinitions()
        {
            try
            {
                FileStream stream = new FileStream(ConfigPaths.ChargeCategories, FileMode.Open);
                return Parse<ChargeCategories>(stream, Seralizers.ChargeCategories);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Function.ShowError(String.Format(@"Could not parse ChargeCategories. File not found: {0}", ConfigPaths.ChargeCategories));
                    return null;
                }
                throw;
            }
        }

        public static CitationCategories ReadCitationDefinitions()
        {
            try
            {
                FileStream stream = new FileStream(ConfigPaths.CitationDefinitions, FileMode.Open);
                return Parse<CitationCategories>(stream, Seralizers.CitationCategories);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Function.ShowError(String.Format(@"Could not parse CitationDefinitions. File not found: {0}", ConfigPaths.CitationDefinitions));
                    return null;
                }
                throw;
            }
        }

        public static VehicleDefinitions ReadVehicleDefinitions()
        {
            try
            {
                FileStream stream = new FileStream(ConfigPaths.VehicleDefinitions, FileMode.Open);
                return Parse<VehicleDefinitions>(stream, Seralizers.VehicleDefinitions);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Function.ShowError(String.Format(@"Could not parse VehicleDefinitions. File not found: {0}", ConfigPaths.VehicleDefinitions));
                    return null;
                }
                throw;
            }
        }

        public static void ReadDefinitionsAndGlobalize()
        {
            var arrestChargeDefinitions = ReadArrestChargeDefinitions();
            var citationDefinitions = ReadCitationDefinitions();
            var vehicleDefinitions = ReadVehicleDefinitions();
            Globals.ChargeDefinitions = arrestChargeDefinitions;
            Globals.CitationDefinitions = citationDefinitions;
            Globals.VehicleDefinitions = vehicleDefinitions;
            PopulateWarrantCharges(arrestChargeDefinitions);
            PopulateCitationCategories(citationDefinitions);
        }

        private static void TransverseCharges(String parentName, Charge charge)
        {
            if (charge.IsContainer)
            {
                String chargeName = parentName + "/" + charge.Name;
                charge.Children.ForEach(childCharge => TransverseCharges(chargeName, childCharge));
            }
            else
            {
                String chargeName = parentName + ":\n => " + String.Format("{0}{1}", charge.Name, charge.IsFelony ? " (F)" : String.Empty);
                Globals.WantedReasons.Add(chargeName);
            }
        }

        private static void PopulateWarrantCharges(ChargeCategories categories)
        {
            if (categories == null) return;
            if (Globals.WantedReasons == null) Globals.WantedReasons = new List<String>();
            categories.Categories.ForEach(x =>
            {
                String categoryName = x.Name;
                x.Charges.ForEach(charge => TransverseCharges(categoryName, charge));
            });
        }


        private static void TransverseCitations(CitationDefinition citation, bool isPublic)
        {
            if (citation.IsContainer)
            {
                citation.Children.ForEach(childCitation => TransverseCitations(childCitation, isPublic));
            }
            else
            {
                citation.IsPublic = isPublic;
                Globals.CitationList.Add(citation);
            }
        }

        private static void PopulateCitationCategories(CitationCategories categories)
        {
            if (categories == null) return;
            if (Globals.CitationList == null) Globals.CitationList = new List<CitationDefinition>();
            categories.Categories.ForEach(x =>
            {
                bool isPublic = false;
                if (x.Name.ToLower().Equals("public")) isPublic = true;
                x.Citations.ForEach(citation => TransverseCitations(citation, isPublic));
            });
        }


        //static void WriteConfig<O> (String path, XmlSerializer seralizer, O data )
        //{
        //    if (String.IsNullOrWhiteSpace(path))
        //    {
        //        Function.LogDebug("XmlConfig WriteConfig attempting to write data to an empty path");
        //        return;
        //    }
        //    else if(data == null || seralizer == null)
        //    {
        //        Function.LogDebug("XmlConfig WriteConfig attempting to write null data");
        //        return;
        //    }
        //    try
        //    {
        //        FileStream stream = new FileStream(path, FileMode.Append);
        //        seralizer.Serialize(stream, data);
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is FileNotFoundException)
        //        {
        //            Function.ShowError(String.Format(@"Could not write ChargeCategories. File not found: {0}", path));                    
        //        }
        //        throw;
        //    }

        //}
        //public static void WriteChargeCategories(ChargeCategories categories)
        //{
        //    WriteConfig(ConfigPaths.ChargeCategories, Seralizers.ChargeCategories, categories);
        //}
    }
}
