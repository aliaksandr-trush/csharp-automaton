﻿namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Utilities;

    public class Event
    {
        /// <summary>
        /// Used for event-linked-list, to indicate whether this event object is root node or not
        /// </summary>
        public bool Root { get; set; }

        /// <summary>
        /// Used for event-linked-list, to indicate the preceding node of this event object
        /// </summary>
        public Event Preceding { get; set; }

        /// <summary>
        /// Used for event-linked-list, to indicate the following node of this event object
        /// </summary>
        public Event Following { get; set; }

        public int Id;
        public string Title;
        public string Shortcut;
        public bool IsActive;
        public EventData_Common.FormType FormType;
        public TaxRate TaxRateOne;
        public TaxRate TaxRateTwo;
        public List<Registrant> Registrants = new List<Registrant>();
        
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
            FormType = EventData_Common.FormType.ProEvent;
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
        public List<CustomFieldCode> AllCustomCodes { get; set; }
        public bool RequireDC { get; set; }

        public EventFee()
        {
            this.AllCustomCodes = new List<CustomFieldCode>();
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
        public GroupDiscount_AdditionalRegOption? AddtionalRegOption;
        public int? NumberOfAdditionalReg;
        public GroupDiscount_ApplyOption? ApplyOption;
        public List<AgendaItem> ApplyToAgendaItems = new List<AgendaItem>();
        public List<RegType> ApplyToRegTypes = new List<RegType>();
        public bool ShowAndApply = true;
    }

    public class PaymentMethod
    {
        public EventData_Common.PaymentMethodEnum PMethod;
        public bool? isPublic;
        public bool? isAdmin;
        public bool? isOnSite;

        public PaymentMethod(EventData_Common.PaymentMethodEnum pMethod)
        {
            this.PMethod = pMethod;
        }
    }

    public class PersonalInfoPageStandardField
    {
        public EventData_Common.PersonalInfoField StandardField;
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
        public CustomizeRegTypeDisplayOptions Customize_RegType_DisplayOptions { get; set; }
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

        public StartPage()
        {
            this.Customize_RegType_DisplayOptions = new CustomizeRegTypeDisplayOptions();
        }
    }

    public class CustomizeRegTypeDisplayOptions
    {
        public bool IsCustomized { get; set; }
        public EventData_Common.RegTypeDisplayOption DisplayOption { get; set; }

        public CustomizeRegTypeDisplayOptions()
        {
            this.IsCustomized = false;
            this.DisplayOption = EventData_Common.RegTypeDisplayOption.RadioButton;
        }
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
        public double? ShippingFee;
        public string PageHeader;
        public string PageFooter;
    }

    public enum CCType
    {
        [CustomString("Visa Debit Card")]
        VisaDebitCard,

        [CustomString("Mastercard Debit Card")]
        MastercardDebitCard,

        [CustomString("Visa")]
        Visa,

        [CustomString("Mastercard")]
        Mastercard,

        [CustomString("American Express")]
        AmericanExpress
    }

    public class CCOptions
    {
        public EventData_Common.Gateway Payment_Gateway { get; set; }
        public bool? ChargeInitialReg { get; set; }
    }

    public class CheckoutPage
    {
        public Utilities.MoneyTool.CurrencyCode? Event_Currency;
        public CCOptions CC_Options { get; set; }
        public List<PaymentMethod> PaymentMethods = new List<PaymentMethod>();
        public bool? AddServiceFee;

        public string PageHeader;
        public string PageFooter;

        public void ApplySettings_AMS_USD()
        {
            Event_Currency = MoneyTool.CurrencyCode.USD;
            CC_Options = new CCOptions();
            CC_Options.Payment_Gateway = EventData_Common.Gateway.AMS_USD;
        }
    }

    public class EventWebsite
    {
        public bool? UseEventWebsiteAsTheStartingPageForEvent;
        public bool ShowNavigation;
    }
}
