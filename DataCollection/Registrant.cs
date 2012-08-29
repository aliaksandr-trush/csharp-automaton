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

    public class FeeSummary
    {
        public double Total { get; set; }
    }

    public class UpdateRegistrant : Registrant
    {
        public Registrant OldRegistration;

        public UpdateRegistrant(Registrant reg) : base(reg.Event)
        {
            this.OldRegistration = reg;
        }
    }

    public class Registrant
    {
        public int Id;
        public Event Event;
        public string Email;
        public EventFeeResponse EventFee_Response;
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
        public PaymentMethod Payment_Method;
        public RegisterMethod Register_Method = RegisterMethod.EventId;
        public List<CustomFieldResponse> CustomField_Responses = new List<CustomFieldResponse>();
        public List<MerchandiseResponse> Merchandise_Responses = new List<MerchandiseResponse>();
        public FormData.Gender? Gender;
        public DateTime? BirthDate;

        public bool WhetherToVerifyFeeOnCheckoutPage { get; set; }
        public FeeSummary Fee_Summary { get; set; }

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

        public void ReCalculateFee()
        {
            this.Fee_Summary = new FeeSummary();
            this.Fee_Summary.Total = 0;

            if (this.EventFee_Response != null)
            {
                this.Fee_Summary.Total += (this.EventFee_Response.Code == null) ? this.EventFee_Response.Fee : this.EventFee_Response.Code.CalculateDiscountedPrice(this.EventFee_Response.Fee);
            }

            foreach (CustomFieldResponse responses in this.CustomField_Responses)
            {
                if (responses is AgendaResponse)
                {
                    AgendaResponse response = responses as AgendaResponse;

                    switch (response.AgendaItem.Type)
                    {
                        case FormData.CustomFieldType.AlwaysSelected:
                            {
                                AgendaResponse_AlwaysSelected resp = response as AgendaResponse_AlwaysSelected;
                                this.Fee_Summary.Total += (resp.Code == null) ? resp.Fee : resp.Code.CalculateDiscountedPrice(resp.Fee);
                            }
                            break;

                        case FormData.CustomFieldType.CheckBox:
                            {
                                AgendaResponse_Checkbox resp = response as AgendaResponse_Checkbox;
                                this.Fee_Summary.Total += (resp.Code == null) ? resp.Fee : resp.Code.CalculateDiscountedPrice(resp.Fee);
                            }
                            break;

                        case FormData.CustomFieldType.RadioButton:
                            {
                                AgendaResponse_MultipleChoice_RadioButton resp = response as AgendaResponse_MultipleChoice_RadioButton;
                                this.Fee_Summary.Total += (resp.Code == null) ? resp.Fee : resp.Code.CalculateDiscountedPrice(resp.Fee);
                            }
                            break;

                        case FormData.CustomFieldType.Dropdown:
                            {
                                AgendaResponse_MultipleChoice_DropDown resp = response as AgendaResponse_MultipleChoice_DropDown;
                                this.Fee_Summary.Total += (resp.Code == null) ? resp.Fee : resp.Code.CalculateDiscountedPrice(resp.Fee);
                            }
                            break;
                        
                        case FormData.CustomFieldType.Contribution:
                            {
                                AgendaResponse_Contribution resp = response as AgendaResponse_Contribution;
                                this.Fee_Summary.Total += resp.ContributionAmount;
                            }
                            break;

                        case FormData.CustomFieldType.FileUpload:
                            {
                                AgendaResponse_FileUpload resp = response as AgendaResponse_FileUpload;
                                this.Fee_Summary.Total += (resp.Code == null) ? resp.Fee : resp.Code.CalculateDiscountedPrice(resp.Fee);
                            }
                            break;

                        default:
                            throw new ArgumentException(string.Format("Agenda item of specified type has no price: {0}", response.AgendaItem.Type));
                    }
                }
            }

            foreach (MerchandiseResponse response in this.Merchandise_Responses)
            {
                switch (response.Merchandise_Item.Type)
                {
                    case FormData.MerchandiseType.Fixed:
                        {
                            MerchResponse_FixedPrice resp = response as MerchResponse_FixedPrice;
                            double price = (resp.Merchandise_Item.Price.HasValue ? resp.Merchandise_Item.Price.Value : 0);
                            this.Fee_Summary.Total += ((resp.Discount_Code == null) ? price : resp.Discount_Code.CalculateDiscountedPrice(price)) * resp.Quantity;
                        }
                        break;

                    case FormData.MerchandiseType.Variable:
                        {
                            MerchResponse_VariableAmount resp = response as MerchResponse_VariableAmount;
                            this.Fee_Summary.Total += resp.Amount;
                        }
                        break;

                    default:
                        throw new ArgumentException(string.Format("Merchandise item of specified type has no price: {0}", response.Merchandise_Item.Type));
                }
            }
        }
    }
}
