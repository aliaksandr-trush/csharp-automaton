namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Utilities;

    public class Event
    {
        public int Id;
        public string Title;
        public string Shortcut;
        public bool IsActive;
        public FormData.FormType FormType;
        public TaxRate TaxRateOne;
        public TaxRate TaxRateTwo;
        
        public StartPage StartPage = new StartPage();
        public PersonalInfoPage PersonalInfoPage = new PersonalInfoPage();
        public AgendaPage AgendaPage;
        public LodgingTravelPage LodgingTravelPage;
        public MerchandisePage MerchandisePage;
        public CheckoutPage CheckoutPage = new CheckoutPage();

        public EventWebsite EventWebsite;

        public Event(string title)
        {
            this.Title = title;
            this.ReSetShortcut();
            FormType = FormData.FormType.ProEvent;
        }

        public void ReSetShortcut()
        {
            this.Shortcut = Guid.NewGuid().ToString();
        }
    }

    public class EventFee
    {
        public double StandardPrice { get; set; }
        public string Name { get; set; }
        public EarlyPrice Early_Price { get; set; }
        public LatePrice Late_Price { get; set; }
        public List<CustomFieldCode> DiscountCodes { get; set; }
        public bool RequireDC { get; set; }

        public EventFee()
        {
            this.DiscountCodes = new List<CustomFieldCode>();
        }
    }

    public class EventLevelLimit
    {
        public int Spaces;
        public bool? EnableWaitList;

        public EventLevelLimit(int spaces)
        {
            this.Spaces = spaces;
        }
    }

    public enum GroupDiscount_GroupSizeOption
    {
        [CustomString("")]
        JustSize,

        [CustomString("or more")]
        SizeOrMore
    }

    public enum GroupDiscount_DiscountType
    {
        [CustomString("US Dollar")]
        USDollar,

        [CustomString("Percent")]
        Percent
    }

    public enum GroupDiscount_AdditionalRegOption
    {
        [CustomString("Additional")]
        Additional,

        [CustomString("All")]
        All,

        [CustomString("Any Additional")]
        AnyAdditional
    }

    public enum GroupDiscount_ApplyOption
    {
        ToAllEventFees,
        ToOnlySelectedFees
    }

    public class GroupDiscount
    {
        public int GroupSize;
        public GroupDiscount_GroupSizeOption GroupSizeOption;
        public double DiscountAmount;
        public GroupDiscount_DiscountType GroupDiscountType;
        public GroupDiscount_AdditionalRegOption AddtionalRegOption;
        public int? NumberOfAdditionalReg;
        public GroupDiscount_ApplyOption? ApplyOption;
    }

    public class PaymentMethod
    {
        public FormData.PaymentMethod PMethod;
        public bool? isPublic;
        public bool? isAdmin;
        public bool? isOnSite;

        public PaymentMethod(FormData.PaymentMethod pMethod)
        {
            this.PMethod = pMethod;
        }
    }

    public class PersonalInfoPageStandardField
    {
        public FormData.PersonalInfoField StandardField;
        public bool? Visible;
        public bool? Required;
    }

    public class EventAdvancedSettings
    {
        public bool? ThisIsAParentEvent;
        public bool? ThisIsAChildEvent;
        public Event ParentEvent;
    }

    public enum EventType
    {
        Running,
        Soccer
    }

    public class StartPage
    {
        public EventFee Event_Fee { get; set; }
        public EventType? EventType;
        public EventLevelLimit EventLimit;
        public GroupDiscount GroupDiscount;
        public bool? ForceSelectSameRegType;
        public bool? AllowGroupReg;
        public bool? AllowChangeRegType;
        public FormData.RegTypeDisplayOption? RegTypeDisplayOption;
        public DateTime? StartDate;
        public DateTime? EndDate;
        public DateTime? StartTime;
        public DateTime? EndTime;
        public string Location;
        public string Phone;
        public string Country;
        public string Address1;
        public string Address2;
        public string City;
        public string State;
        public string Zip;
        public string ContactInfo;
        public string EventHome;
        public string PageHeader;
        public string PageFooter;
        public EventAdvancedSettings AdvancedSettings;

        public List<RegType> RegTypes = new List<RegType>();
    }

    public class PersonalInfoPage
    {
        public List<PersonalInfoPageStandardField> StandardFields = new List<PersonalInfoPageStandardField>();
        public List<CustomField> CustomFields = new List<CustomField>();
        public string PageHeader;
        public string PageFooter;
    }

    public class AgendaPage
    {
        public List<AgendaItem> AgendaItems = new List<AgendaItem>();
        public bool? DoNotAllowOverlapping;
        public bool IsShoppingCart = false;
        public string PageHeader;
        public string PageFooter;
    }

    public class LodgingTravelPage
    {
        public Lodging Lodging;
        public string PageHeader;
        public string PageFooter;
    }

    public class MerchandisePage
    {
        public List<MerchandiseItem> Merchandises = new List<MerchandiseItem>();
        public string PageHeader;
        public string PageFooter;
    }

    public class CheckoutPage
    {
        public List<PaymentMethod> PaymentMethods = new List<PaymentMethod>();
        public string PageHeader;
        public string PageFooter;
    }

    public class EventWebsite
    {
        public bool? UseEventWebsiteAsTheStartingPageForEvent;
        public bool ShowNavigation;
    }
}
