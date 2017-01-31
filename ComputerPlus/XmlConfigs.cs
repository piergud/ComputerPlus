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
        }
        public static class Configs
        {
            public static ChargeCategories ChargeCategories
            {
                get;
                internal set;
            }
        }
        public static class ConfigPaths
        {
            public static readonly String ChargeCategories = @"Plugins\LSPDFR\ComputerPlus\ChargeDefinitions.xml";
        }

        static ChargeCategories Parse(Stream stream)
        {
            if (stream == null)
            {
                Function.ShowError("Tried to parse ChargeCategories XML, but no file was found. Please check or redownload");
                return null;
            }
            try
            {
                Configs.ChargeCategories = (ChargeCategories)Seralizers.ChargeCategories.Deserialize(stream);
                return Configs.ChargeCategories;
            }
            catch(Exception e)
            {
               if (e is InvalidOperationException)
                {
                    Function.Log(e.ToString());
                    Function.ShowWarning("Could not parse ChargeCategories XML. Please check or redownload");
                    return null;
                }
                throw;
            }
        }
        public static ChargeCategories ReadChargeCategories()
        {
            try
            {
                FileStream stream = new FileStream(ConfigPaths.ChargeCategories, FileMode.Open);
                return Parse(stream);
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
        static void WriteConfig<O> (String path, XmlSerializer seralizer, O data )
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                Function.LogDebug("XmlConfig WriteConfig attempting to write data to an empty path");
                return;
            }
            else if(data == null || seralizer == null)
            {
                Function.LogDebug("XmlConfig WriteConfig attempting to write null data");
                return;
            }
            try
            {
                FileStream stream = new FileStream(path, FileMode.Append);
                seralizer.Serialize(stream, data);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Function.ShowError(String.Format(@"Could not write ChargeCategories. File not found: {0}", path));                    
                }
                throw;
            }
            
        }
        public static void WriteChargeCategories(ChargeCategories categories)
        {
            WriteConfig(ConfigPaths.ChargeCategories, Seralizers.ChargeCategories, categories);
        }
    }
}
