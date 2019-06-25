using System.Xml.Serialization;

namespace AIEnterprise.Feature.SiteMap.Models
{
    public class Url
    {
        ///
        ///Change Frequency
        ///
        [XmlElement("changefreq")]
        public SitemapChangeFrequency Changefrequency { get; set; }

        ///
        /// Last modified on
        /// 

        [XmlElement("lastmod")]
        public string Lastmod { get; set; }

        ///
        /// Location Parameter
        /// 

        [XmlElement("loc")]
        public string Loc { get; set; }

        ///
        /// Location Parameter
        /// 

        [XmlElement("priority")]
        public string Priority { get; set; }
    }
}