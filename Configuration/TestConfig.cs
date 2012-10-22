namespace RegOnline.RegressionTest.Configuration
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "TestConfig")]
    public class TestConfig
    {
        [XmlElement(ElementName = "Environments")]
        public Environments Environments_Config { get; set; }

        [XmlElement(ElementName = "WebServices")]
        public WebServices WebServices_Config { get; set; }

        [XmlElement(ElementName = "Browsers")]
        public Browsers Browsers_Config { get; set; }

        [XmlElement(ElementName = "NUnitAddin")]
        public NUnitAddin NUnitAddin_Config { get; set; }

        [XmlElement(ElementName = "ScreenResolution")]
        public ScreenResolution ScreenResolution_Config { get; set; }
    }

    public class Environments
    {
        [XmlAttribute("EnvironmentChosen")]
        public string Environment_Chosen { get; set; }

        [XmlAttribute("AccountChosen")]
        public string Account_Chosen { get; set; }

        [XmlAttribute("CurrentMachineTimeZoneOffset")]
        public int CurrentMachine_TimeZoneOffset { get; set; }

        [XmlElement(ElementName = "Environment")]
        public Environment[] Environment_Config { get; set; }
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
        public Account[] Account_Config { get; set; }
    }

    public class Account
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

        [XmlElement(ElementName = "TimeZoneOffset")]
        public int TimeZoneOffset { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        public string BaseUrl
        {
            get
            {
                return string.Format("http://{0}/", this.DomainName);
            }
        }

        public string BaseUrlWithHttps
        {
            get
            {
                return string.Format("https://{0}/", this.DomainName);
            }
        }
    }

    public class WebServices
    {
        [XmlElement(ElementName = "WebService")]
        public WebService[] WebService_Config { get; set; }
    }

    public class WebService
    {
        [XmlElement(ElementName = "Url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "EndpointConfigName")]
        public string EndpointConfigName { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("HTTPS")]
        public bool HTTPS { get; set; }
    }

    public class Browsers
    {
        [XmlElement(ElementName = "Browser")]
        public Browser[] Browser_Config { get; set; }

        [XmlAttribute("Current")]
        public string Current { get; set; }

        [XmlAttribute("DirectStartup")]
        public bool DirectStartup { get; set; }
    }

    public class Browser
    {
        [XmlElementAttribute(ElementName = "Server")]
        public Server Server_Config { get; set; }

        [XmlElementAttribute(ElementName = "BinaryPath")]
        public BinaryPath Binary_Path { get; set; }

        [XmlElementAttribute(ElementName = "ProfilePath")]
        public ProfilePath Profile_Path { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    public class Server
    {
        [XmlAttributeAttribute("Path")]
        public string Path { get; set; }

        [XmlAttributeAttribute("Host")]
        public string Host { get; set; }

        [XmlAttributeAttribute("Port")]
        public int Port { get; set; }
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
        [XmlAttributeAttribute("ReportBack")]
        public bool ReportBack { get; set; }
    }

    public class ScreenResolution
    {
        [XmlAttributeAttribute("Enable")]
        public bool Enable { get; set; }

        [XmlAttributeAttribute("Width")]
        public int Width { get; set; }

        [XmlAttributeAttribute("Height")]
        public int Height { get; set; }
    }
}