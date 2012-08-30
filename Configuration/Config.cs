﻿namespace RegOnline.RegressionTest.Configuration
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "ArrayOfRegOnlineRegressionTestConfig")]
    public class TestConfig
    {
        [XmlElement(ElementName = "Environments")]
        public Environments Environments { get; set; }

        [XmlElement(ElementName = "WebServices")]
        public WebServices WebServices { get; set; }

        [XmlElement(ElementName = "Browsers")]
        public Browsers Browsers { get; set; }

        [XmlElement(ElementName = "TimeZoneDifference")]
        public int TimeZoneDifference { get; set; }

        [XmlElement(ElementName = "NUnitAddin")]
        public NUnitAddin NUnitAddin { get; set; }
    }

    public class Environments
    {
        [XmlElement(ElementName = "Preferred")]
        public Preferred Preferred { get; set; }

        [XmlElement(ElementName = "Environment")]
        public Environment[] Environment { get; set; }
    }

    public class Preferred
    {
        [XmlAttribute("Environment")]
        public string Environment { get; set; }

        [XmlAttribute("PrivateLabel")]
        public string PrivateLabel { get; set; }
    }

    public class Environment
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ClientDbConnection")]
        public string ClientDbConnection { get; set; }

        [XmlAttribute("ROMasterConnection")]
        public string ROMasterConnection { get; set; }

        [XmlAttribute("ROWarehouseConnection")]
        public string ROWarehouseConnection { get; set; }

        [XmlAttribute("DataPath")]
        public string DataPath { get; set; }

        [XmlElement(ElementName = "Account")]
        public Account[] Account { get; set; }
    }

    public partial class Account
    {
        [XmlElement(ElementName = "DomainName")]
        public string DomainName { get; set; }

        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Login")]
        public string Login { get; set; }

        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }

        [XmlElement(ElementName = "Folder")]
        public string Folder { get; set; }

        [XmlElement(ElementName = "XAuthVersion")]
        public string XAuthVersion { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    public class WebServices
    {
        [XmlElement(ElementName = "WebService")]
        public WebService[] WebService { get; set; }
    }

    public class WebService
    {
        [XmlElement(ElementName = "Url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "EndpointConfigName")]
        public string EndpointConfigName { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    public class Browsers
    {
        [XmlElement(ElementName = "ChromeDriverPath")]
        public string ChromeDriverPath { get; set; }

        [XmlElement(ElementName = "Browser")]
        public Browser[] Browser { get; set; }

        [XmlAttribute("Current")]
        public string Current { get; set; }
    }

    public class Browser
    {
        [XmlElementAttribute(ElementName = "BinaryPath")]
        public BinaryPath BinaryPath { get; set; }

        [XmlElementAttribute(ElementName = "ProfilePath")]
        public ProfilePath ProfilePath { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    [XmlTypeAttribute]
    public class BinaryPath
    {
        [XmlAttributeAttribute("Enable")]
        public bool Enable { get; set; }

        [XmlText]
        public string Path { get; set; }
    }

    public class ProfilePath
    {
        [XmlAttributeAttribute("Enable")]
        public bool Enable { get; set; }

        [XmlText]
        public string Path { get; set; }
    }

    public class NUnitAddin
    {
        [XmlAttributeAttribute(ElementName = "ReportBack")]
        public bool ReportBack { get; set; }
    }
}