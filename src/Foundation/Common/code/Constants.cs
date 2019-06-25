using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIEnterprise.Foundation.Common
{
    public class BaseModelConstants
    {
        public const string Title = "Title";
        public const string Subtitle = "SubTitle";
    }

    public class CarouselConstants
    {
        public const string Label = "Label";
    }

    public class HotSpotConstants
    {
        //public const string Title = "Title";

        public const string DataLeft = "DataLeft";
        public const string DataTop = "DataTop";
        public const string Description = "Description";
    }


    public class TestimonialConstants
    {
        public const string Body = "Body";
        public const string CssClass = "CSS Class";
        public const string CssClassTemplateID = "{70062471-C78F-49CC-991B-7FE5BA15DF8A}";
    }

    public class HeaderConstants
    {
        public const string PhoneNumber = "Phone Number";
        public const string HeaderLink = "HeaderLink";
        public const string HeaderLinks = "HeaderLinks";
        public const string IsSearch = "IsSearch";
        public const string Logo = "Logo";
        public const string FranchiseeText = "Franchisee Header Text";
    }

    public class NaturalLanguageConstants
    {
        public const string audienceType = "AudienceType";
        public const string serviceType = "ServiceType";
    }

    public class FranchiseeConstants
    {
        public const string FRANCHISEE_LOGGER = "FranchiseeLogFileAppender";
        public const string LOCATION_ID = "Location Id";
        public const string SERVICE_LINE_TYPE = "Service Line Type";
        public const string ENTERPRISE_NUMBER = "Enterprise Number";
        public const string LICENSE_NUMBER = "License Number";
        public const string ENTERPRISE_DBA = "Enterprise DBA";
        public const string LICENSE_DBA = "License DBA";
        public const string ADDRESS_1 = "Address 1";
        public const string ADDRESS_2 = "Address 2";
        public const string CITY = "City";
        public const string BUSINESS_DESCRIPTION = "Business Description";
        public const string YEARS_IN_BUSINESS = "Years in Business";
        public const string STATE = "State";
        public const string COUNTRY = "Country";
        public const string PHONE = "Phone";
        public const string EMAIL = "Email";
        public const string SERVICE_NAME_DESCRIPTION = "Service Name and Description";
        public const string FACEBOOK_LINK = "Facebook Link";
        public const string BBB_URL = "BBB url";
        public const string HOME_ADVISOR_URL = "Home Advisor url";
        public const string ZIP_CODES_COVERED = "Zip Codes Covered";
        public const string CITIES_COVERED = "Cities Covered";
        public const string STATES_COVERED = "States Covered";
        public const string SERVICE_NAME = "Service Name";
        public const string SERVICE_DESCRIPTION = "Service Description";
        public const string BIO_NAME = "Bio Name";
        public const string BIO_DESCRIPTION = "Bio Description";
        public const string BIO_IMAGE = "Bio Image";
        public const string TESTIMONIAL_NAME = "Testimonial Name";
        public const string TESTIMONIAL_TITLE = "Testimonial Title";
        public const string TESTIMONIAL_DESCRIPTION = "Testimonial Description";
        public const string VIDEOURL = "Video URL";
        public const string FRANCHISEEDATA = "FranchiseeData";

        private const string COMMON_FRANCHISEE_TEMPLATEPATH = "/sitecore/templates/Feature/SVMBrands/Global/FranchiseeSearch/Franchisee/";

        public const string FRANCHISEE_ROOTPATH = "/sitecore/content/SVMBrands/FurnitureMedic/External Data/Franchisee Store";
        public const string FRANCHISEE_DATA_ROOTPATH = "/sitecore/content/SVMBrands/FurnitureMedic/External Data/Franchisee Data";
        public const string FRANCHISEE_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee";
        public const string FRANCHISEE_DATA_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Folder";
        public const string FRANCHISEESERVICE_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Service";
        public const string FRANCHISEEBIOS_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Bio";
        public const string FRANCHISEETESTIMONIAL_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Testimonial";
        public const string FRANCHISEEVIDEOS_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Video";

        public const string FRANCHISEESERVICEFOLDER_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Service Folder";
        public const string FRANCHISEEBIOSFOLDER_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Bio Folder";
        public const string FRANCHISEETESTIMONIALFOLDER_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Testimonial Folder";
        public const string FRANCHISEEVIDEOSFOLDER_TEMPLATEPATH = COMMON_FRANCHISEE_TEMPLATEPATH + "Franchisee Video Folder";
        public const string FRANCHISEEBIOS_MEDIA_FOLDERPATH = "/sitecore/media library/Feature/FranchiseeBios/";

        public const string FRANCHISEESERVICEFOLDER_ID = "{15E51FF2-C3EA-45F0-B1B3-AEA85C753A02}";
        public const string FRANCHISEEBIOSFOLDER_ID = "{C6542247-91E9-4952-8EE2-5583A5E6F0B3}";
        public const string FRANCHISEETESTIMONIALFOLDER_ID = "{1A2F3821-AF75-4D2E-9F47-C4C117EC3119}";
        public const string FRANCHISEEVIDEOSFOLDER_ID = "{1A27D507-B84E-4AF5-A7FA-111942844AA8}";
        public const string FRANCHISEE_TEMPLATEID = "{A366E46E-1CD2-4A81-8FC9-2C26755E09BE}";
        
    }


    public class GlobalConstants
    {
        public const string frachiseeCookie = "sxa";
        public const string renderingParametersPath = "/sitecore/content/SVMBrands/FurnitureMedic/External Data/RenderingParameters Data";
    }
}
