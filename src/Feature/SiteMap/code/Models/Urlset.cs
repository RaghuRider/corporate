using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AIEnterprise.Feature.SiteMap.Models
{
    [XmlRoot("urlset")]
    public class Urlset
    {
        ///

        /// Constructor to initialize Url Object
        /// 

        public Urlset() { Urls = new List<Url>(); }

        ///

        /// Urls collection
        /// 

        [XmlElement("url")]
        public List<Url> Urls { get; set; }
    }
}