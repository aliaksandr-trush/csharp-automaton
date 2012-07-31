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

        public Registrant()
        {
            this.Email = string.Format("selenium{0}@regonline.com", DateTime.Now.Ticks.ToString());
        }

        public Registrant(string email)
        {
            this.Email = email;
        }

        public static Registrant Default = Registrant.GetDefault();

        public static Registrant GetDefault()
        {
            return new Registrant
            {
                FirstName = "Selenium",
                MiddleName = "M",
                JobTitle = "Regression meister",
                Company = "Regression, Inc",
                AddressLineOne = "4750 Walnut Street",
                AddressLineTwo = "suite 100",
                City = "Boulder",
                State = "Colorado",
                ZipCode = "99701",
                WorkPhone = "303.555.1212",
                Extension = "113",
                Fax = "303.987.3524",
                Password = ConfigurationProvider.XmlConfig.AccountConfiguration.Password
            };
        }
    }
}
