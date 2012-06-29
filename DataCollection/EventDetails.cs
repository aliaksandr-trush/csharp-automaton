﻿namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public int Id;
        public string Title;
        public string Shortcut = Guid.NewGuid().ToString();
        
        public StartPage StartPage = new StartPage();
        public PersonalInfoPage PersonalInfoPage = new PersonalInfoPage();
        public AgendaPage AgendaPage;
        public LodgingTravelPage LodgingTravelPage;
        public MerchandisePage MerchandisePage;
        public CheckoutPage CheckoutPage = new CheckoutPage();

        public Event(string title)
        {
            this.Title = title;
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

    public class GroupDiscount
    {
        public int GroupSize;
        public FormData.GroupSizeOption GroupSizeOption;
        public double DiscountAmount;
        public FormData.DiscountType GroupDiscountType;
        public FormData.AdditionalRegOption AddtionalRegOption;
        public int? AdditionalNumber;
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

    public class StartPage
    {
        public EventLevelLimit EventLimit;
        public GroupDiscount GroupDiscount;
        public bool? ForceSelectSameRegType;
        public bool? AllowGroupReg;
        public FormData.RegTypeDisplayOption? RegTypeDisplayOption;
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

        public List<RegType> RegTypes = new List<RegType>();
    }

    public class PersonalInfoPage
    {
        public List<PersonalInfoPageStandardField> StandardFields = new List<PersonalInfoPageStandardField>();
        public string PageHeader;
        public string PageFooter;
    }

    public class AgendaPage
    {
        public List<AgendaItem> AgendaItems = new List<AgendaItem>();
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
        public List<Merchandise> Merchandises = new List<Merchandise>();
        public string PageHeader;
        public string PageFooter;
    }

    public class CheckoutPage
    {
        public List<PaymentMethod> PaymentMethods = new List<PaymentMethod>();
        public string PageHeader;
        public string PageFooter;
    }
}
