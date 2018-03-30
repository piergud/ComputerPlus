using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ComputerPlus.Interfaces.Reports.Models
{
    [XmlRoot(ElementName ="CitationCategories")]
    public class CitationCategories
    {
       
        [XmlElement(ElementName = "Citation")]
        public List<CitationCategory> Categories;
    }

    [Serializable]
    public class CitationCategory
    {
        [XmlAttribute("name")]
        public String Name
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Citation")]
        public List<CitationDefinition> Citations;
    }

    [Serializable]
    public class CitationDefinition
    {

        [XmlAttribute("name")]
        public String Name
        {
            get;
            set;
        }

        [XmlAttribute("arrestable")]
        public bool IsArrestable
        {
            get;
            set;
        }

        public bool IsPublic
        {
            get;
            set;
        }

        [XmlAttribute("fine")]
        public double FineAmount
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Citation")]
        public List<CitationDefinition> Children
        {
            get;
            set;
        }

        public CitationDefinition()
        {

        }

        public CitationDefinition(String name, double fineAmount, bool isArrestable)
        {
            this.Name = name;
            this.FineAmount = fineAmount;
            this.IsArrestable = isArrestable;
        }

        public bool IsContainer
        {
            get
            {
                return Children != null && Children.Count > 0;
            }
        }

    }
}
