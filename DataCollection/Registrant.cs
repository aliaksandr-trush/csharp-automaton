namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Configuration;

    public enum RegisterMethod
    {
        EventId,
        Shortcut,
        RegTypeDirectUrl,
        EventWebsite,
        EventCalendar,
        Admin
    }

    public abstract class DefaultPaymentInfo
    {
        public const string CCType = "Visa";
        public const string CCNumber = "4444444444444448";
        public readonly static string CCNumber_Encrypted = Utility.GetEncryptedCCNumber("4444444444444448");
        public const string CCNumberAlternative = "4012888888881881";
        public const string CVV = "123";// CVV is the namely 'Security Code'
        public const string ExpirationMonth = "12 - Dec";
        public const string ExpirationYear = "2019";
        public const string HolderName = "test test";
        public const string HolderCountry = "United States";
        public const string BillingPhone = "3035775100";
        public const string BillingAddressLineOne = "4750 Walnut Street";
        public const string BillingAddressLineTwo = "suite 100";
        public const string BillingCity = "Boulder";
        public const string BillingState = "CO";
        public const string ZipCode = "99701";
        public const string Country = "United States";
    }

    public abstract class DefaultPersonalInfo
    {
        public const string FirstName = "Selenium";
        public const string MiddleName = "M";
        public const string JobTitle = "Regression meister";
        public const string Company = "Regression, Inc";
        public const string AddressLineOne = "4750 Walnut Street";
        public const string AddressLineTwo = "suite 100";
        public const string City = "Boulder";
        public const string State = "Colorado";
        public const string NonUSState = "Canada";
        public const string ZipCode = "99701";
        public const string WorkPhone = "303.555.1212";
        public const string Extension = "113";
        public const string Fax = "303.987.3524";
        public readonly static string Password = ConfigurationProvider.XmlConfig.AccountConfiguration.Password;
    }

    public class Registrant
    {
        public int Id;
        public Event Event;
        public string Email;
        public RegType RegType;
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public string JobTitle;
        public string Company;
        public string AddressLineOne;
        public string AddressLineTwo;
        public string City;
        public FormData.Countries? Country;
        public string State;
        public string NonUSState;
        public string ZipCode;
        public string WorkPhone;
        public string Extension;
        public string Fax;
        public string Password;
        public PaymentMethod PaymentMethod;
        public RegisterMethod RegisterMethod = RegisterMethod.EventId;
        public List<CustomFieldResponse> CustomFieldResponses = new List<CustomFieldResponse>();
        public FormData.Gender? Gender;
        public DateTime? BirthDate;

        public Registrant(Event evt)
        {
            this.Email = string.Format("selenium{0}@regonline.com", DateTime.Now.Ticks.ToString());
            this.Event = evt;
            this.SetWithDefaultPersonalInfo();
        }

        public Registrant(Event evt, string email)
        {
            this.Email = email;
            this.Event = evt;
            this.SetWithDefaultPersonalInfo();
        }

        public void SetWithDefaultPersonalInfo()
        {
            switch (this.Event.FormType)
            {
                case FormData.FormType.ProEvent:
                    this.SetWithDefaultPersonalInfo_ProEvent();
                    break;

                case FormData.FormType.ActiveEuropeEvent:
                    this.SetWithDefaultPersonalInfo_ProEvent();
                    this.State = null;
                    this.NonUSState = DefaultPersonalInfo.NonUSState;
                    break;

                case FormData.FormType.ExpressEvent:
                    break;
                case FormData.FormType.LiteEvent:
                    break;
                case FormData.FormType.Membership:
                    break;
                case FormData.FormType.WebEvent:
                    break;
                case FormData.FormType.Survey:
                    break;
                case FormData.FormType.DonationForm:
                    break;
                case FormData.FormType.CreateFromTemplate:
                    break;
                default:
                    break;
            }
        }

        private void SetWithDefaultPersonalInfo_ProEvent()
        {
            this.FirstName = DefaultPersonalInfo.FirstName;
            this.MiddleName = DefaultPersonalInfo.MiddleName;
            this.JobTitle = DefaultPersonalInfo.JobTitle;
            this.Company = DefaultPersonalInfo.Company;
            this.AddressLineOne = DefaultPersonalInfo.AddressLineOne;
            this.AddressLineTwo = DefaultPersonalInfo.AddressLineTwo;
            this.City = DefaultPersonalInfo.City;
            this.State = DefaultPersonalInfo.State;
            this.ZipCode = DefaultPersonalInfo.ZipCode;
            this.WorkPhone = DefaultPersonalInfo.WorkPhone;
            this.Extension = DefaultPersonalInfo.Extension;
            this.Fax = DefaultPersonalInfo.Fax;
            this.Password = DefaultPersonalInfo.Password;
        }

        public void SetCurrentRegistrantLastName()
        {
            int check = Convert.ToInt32(System.DateTime.Now.Ticks.ToString().Substring(0, 5)) % 9;

            this.LastName =
                "Rgrssn-"
                + check.ToString()
                + System.DateTime.Now.Ticks.ToString().Substring(6);
        }
    }
}
