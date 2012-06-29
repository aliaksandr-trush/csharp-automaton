namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;
    using RegOnline.RegressionTest.PageObject.Manager;
    using RegOnline.RegressionTest.Utilities;

    public class EventCreation
    {
        private Events Events = new Events();
        private EventDetails EventDetails = new EventDetails();
        private PersonalInfo PersonalInfo = new PersonalInfo();
        private Agenda Agenda = new Agenda();
        private LodgingTravel LodgingTravel = new LodgingTravel();
        private PageObject.Builder.Merchandise Merchandise = new PageObject.Builder.Merchandise();
        private Checkout Checkout = new Checkout();
        private PageObject.Builder.PageObjectHelper BuilderHelper = new PageObject.Builder.PageObjectHelper();

        public void CreateEvent(Event details)
        {
            Events.AddEvent_Click();
            Events.EventType_Select(CustomStringAttribute.GetCustomString(FormData.EventType.ProEvent));
            EventDetails.EventId.WaitForPresent();
            details.Id = Convert.ToInt32(EventDetails.EventId.GetAttribute("value"));

            #region Start Page
            if (details.StartPage.Location != null)
            {
                EventDetails.LocationName.Type(details.StartPage.Location);
            }
            if (details.StartPage.Phone != null)
            {
                EventDetails.LocationPhone.Type(details.StartPage.Phone);
            }
            if (details.StartPage.Country != null)
            {
                EventDetails.Country_Select(details.StartPage.Country);
            }
            if (details.StartPage.Address1 != null)
            {
                EventDetails.AddressLineOne.Type(details.StartPage.Address1);
            }
            if (details.StartPage.Address2 != null)
            {
                EventDetails.AddressLineTwo.Type(details.StartPage.Address2);
            }
            if (details.StartPage.City != null)
            {
                EventDetails.City.Type(details.StartPage.City);
            }
            if (details.StartPage.State != null)
            {
                EventDetails.State.SelectWithText(details.StartPage.State);
            }
            if (details.StartPage.Zip != null)
            {
                EventDetails.PostalCode.Type(details.StartPage.Zip);
            }
            if (details.StartPage.ContactInfo != null)
            {
                EventDetails.EditContactInfo_Click();
                EventDetails.ContactInfo.SelectByName();
                EventDetails.ContactInfo.HtmlMode_Click();
                EventDetails.ContactInfo.Content_Type(details.StartPage.ContactInfo);
                EventDetails.ContactInfo.SaveAndClose_Click();
            }
            if (details.StartPage.EventHome != null)
            {
                EventDetails.EventHome.Type(details.StartPage.EventHome);
            }
            if (details.StartPage.EventLimit != null)
            {
                EventDetails.LimitRegs_Set(true);
                EventDetails.RegSpaces.Type(details.StartPage.EventLimit.Spaces);

                if (details.StartPage.EventLimit.EnableWaitList.HasValue)
                {
                    EventDetails.EnableWaitlist.Set(details.StartPage.EventLimit.EnableWaitList.Value);
                }
            }
            if (details.StartPage.RegTypes.Count != 0)
            {
                foreach (RegType regType in details.StartPage.RegTypes)
                {
                    KeywordProvider.AddRegType.AddRegTypes(regType);
                    PageObject.Builder.RegTypeRow row = new PageObject.Builder.RegTypeRow(regType.RegTypeName);
                    regType.RegTypeId = row.RegTypeId;
                }
            }
            if (details.StartPage.RegTypeDisplayOption.HasValue)
            {
                EventDetails.RegTypeDisplayOption_Set(true);
                EventDetails.RegTypeDisplayFormat.SelectWithValue(CustomStringAttribute.GetCustomString(details.StartPage.RegTypeDisplayOption.Value));
            }

            //Set event name and shortcut after regType created to avoid bug 24560
            EventDetails.Title.Type(details.Title);
            EventDetails.Shortcut.Type(details.Shortcut);
            
            if (details.StartPage.AllowGroupReg.HasValue)
            {
                EventDetails.AllGroupReg_Set(details.StartPage.AllowGroupReg.Value);
                BuilderHelper.SaveAndStay_Click();
            }
            if (details.StartPage.ForceSelectSameRegType.HasValue)
            {
                EventDetails.ForceSameGroupType.Set(details.StartPage.ForceSelectSameRegType.Value);
            }
            if (details.StartPage.GroupDiscount != null)
            {
                EventDetails.AddGroupDiscount_Click();
                EventDetails.GroupDiscountDefine.SelectByName();
                EventDetails.GroupDiscountDefine.GroupSize.Type(details.StartPage.GroupDiscount.GroupSize);
                EventDetails.GroupDiscountDefine.GroupSizeOption.SelectWithText(CustomStringAttribute.GetCustomString(details.StartPage.GroupDiscount.GroupSizeOption));
                EventDetails.GroupDiscountDefine.DiscountAmount.Type(details.StartPage.GroupDiscount.DiscountAmount);
                EventDetails.GroupDiscountDefine.DiscountType.SelectWithText(CustomStringAttribute.GetCustomString(details.StartPage.GroupDiscount.GroupDiscountType));
                EventDetails.GroupDiscountDefine.AddtionalRegOption.SelectWithText(CustomStringAttribute.GetCustomString(details.StartPage.GroupDiscount.AddtionalRegOption));

                if (details.StartPage.GroupDiscount.AdditionalNumber.HasValue)
                {
                    EventDetails.GroupDiscountDefine.AdditionalNumber.Type(details.StartPage.GroupDiscount.AdditionalNumber.Value);
                }

                EventDetails.GroupDiscountDefine.SaveAndClose_Click();
            }

            if ((details.StartPage.PageHeader != null) || (details.StartPage.PageFooter != null))
            {
                BuilderHelper.GotoPage(FormData.Page.Start);

                if (details.StartPage.PageHeader != null)
                {
                    EventDetails.StartPageHeader_Click();
                    EventDetails.StartPageHeaderEditor.SelectByName();
                    EventDetails.StartPageHeaderEditor.HtmlMode_Click();
                    EventDetails.StartPageHeaderEditor.Content_Type(details.StartPage.PageHeader);
                    EventDetails.StartPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.StartPage.PageFooter != null)
                {
                    EventDetails.StartPageFooter_Click();
                    EventDetails.StartPageFooterEditor.SelectByName();
                    EventDetails.StartPageFooterEditor.HtmlMode_Click();
                    EventDetails.StartPageFooterEditor.Content_Type(details.StartPage.PageFooter);
                    EventDetails.StartPageFooterEditor.SaveAndClose_Click();
                }
            }
            #endregion

            #region PI Page
            if ((details.PersonalInfoPage.PageHeader != null) || (details.PersonalInfoPage.PageFooter != null))
            {
                BuilderHelper.GotoPage(FormData.Page.PI);

                if (details.PersonalInfoPage.PageHeader != null)
                {
                    PersonalInfo.PersonalInfoPageHeader_Click();
                    PersonalInfo.PersonalInfoPageHeaderEditor.SelectByName();
                    PersonalInfo.PersonalInfoPageHeaderEditor.HtmlMode_Click();
                    PersonalInfo.PersonalInfoPageHeaderEditor.Content_Type(details.PersonalInfoPage.PageHeader);
                    PersonalInfo.PersonalInfoPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.PersonalInfoPage.PageFooter != null)
                {
                    PersonalInfo.PersonalInfoPageFooter_Click();
                    PersonalInfo.PersonalInfoPageHeaderEditor.SelectByName();
                    PersonalInfo.PersonalInfoPageFooterEditor.HtmlMode_Click();
                    PersonalInfo.PersonalInfoPageHeaderEditor.Content_Type(details.PersonalInfoPage.PageFooter);
                    PersonalInfo.PersonalInfoPageHeaderEditor.SaveAndClose_Click();
                }
            }
            if (details.PersonalInfoPage.StandardFields.Count != 0)
            {
                BuilderHelper.GotoPage(FormData.Page.PI);

                foreach (PersonalInfoPageStandardField field in details.PersonalInfoPage.StandardFields)
                {
                    if (field.Visible.HasValue)
                    {
                        PersonalInfo.SetPersonalInfoFieldVisible(field.StandardField, field.Visible.Value);
                    }
                    if (field.Required.HasValue)
                    {
                        PersonalInfo.SetPersonalInfoFieldRequired(field.StandardField, field.Required.Value);
                    }
                }
            }
            #endregion

            #region Agenda Page
            if (details.AgendaPage != null)
            {
                BuilderHelper.GotoPage(FormData.Page.Agenda);
                BuilderHelper.YesOnSplashPage_Click();

                foreach(AgendaItem agendaItem in details.AgendaPage.AgendaItems)
                {
                    KeywordProvider.AddAgendaItem.AddAgendaItems(agendaItem);
                    agendaItem.Id = Convert.ToInt32(Agenda.AgendaItemId.Value);
                }

                if (details.AgendaPage.PageHeader != null)
                {
                    Agenda.AgendaPageHeader_Click();
                    Agenda.AgendaPageHeaderEditor.SelectByName();
                    Agenda.AgendaPageHeaderEditor.Content_Type(details.AgendaPage.PageHeader);
                    Agenda.AgendaPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.AgendaPage.PageFooter != null)
                {
                    Agenda.AgendaPageFooter_Click();
                    Agenda.AgendaPageFooterEditor.SelectByName();
                    Agenda.AgendaPageFooterEditor.HtmlMode_Click();
                    Agenda.AgendaPageFooterEditor.Content_Type(details.AgendaPage.PageFooter);
                    Agenda.AgendaPageFooterEditor.SaveAndClose_Click();
                }
            }
            #endregion

            #region L&T Page
            if (details.LodgingTravelPage != null)
            {
                BuilderHelper.GotoPage(FormData.Page.LodgingTravel);
                BuilderHelper.YesOnSplashPage_Click();

                foreach(LodgingStandardFields field in details.LodgingTravelPage.Lodging.StandardFields)
                {
                    if (field.Visible.HasValue)
                    {
                        LodgingTravel.SetLodgingStandardFieldVisible(field.Field, field.Visible.Value);
                    }
                    if (field.Required.HasValue)
                    {
                        LodgingTravel.SetLodgingStandardFieldRequired(field.Field, field.Required.Value);
                    }
                }

                if (details.LodgingTravelPage.PageHeader != null)
                {
                    LodgingTravel.LTPageHeader_Click();
                    LodgingTravel.LTPageHeaderEditor.SelectByName();
                    LodgingTravel.LTPageHeaderEditor.HtmlMode_Click();
                    LodgingTravel.LTPageHeaderEditor.Content_Type(details.LodgingTravelPage.PageHeader);
                    LodgingTravel.LTPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.LodgingTravelPage.PageFooter != null)
                {
                    LodgingTravel.LTPageFooter_Click();
                    LodgingTravel.LTPageFooterEditor.SelectByName();
                    LodgingTravel.LTPageFooterEditor.HtmlMode_Click();
                    LodgingTravel.LTPageFooterEditor.Content_Type(details.LodgingTravelPage.PageFooter);
                    LodgingTravel.LTPageFooterEditor.SaveAndClose_Click();
                }
            }
            #endregion

            #region Merchandise Page
            if (details.MerchandisePage != null)
            {
                BuilderHelper.GotoPage(FormData.Page.Merchandise);

                foreach (DataCollection.Merchandise merch in details.MerchandisePage.Merchandises)
                {
                    KeywordProvider.AddMerchandise.AddMerchandises(merch);
                }

                if (details.MerchandisePage.PageHeader != null)
                {
                    Merchandise.MerchandisePageHeader_Click();
                    Merchandise.MerchandisePageHeaderEditor.SelectByName();
                    Merchandise.MerchandisePageHeaderEditor.HtmlMode_Click();
                    Merchandise.MerchandisePageHeaderEditor.Content_Type(details.MerchandisePage.PageHeader);
                    Merchandise.MerchandisePageHeaderEditor.SaveAndClose_Click();
                }

                if (details.MerchandisePage.PageFooter != null)
                {
                    Merchandise.MerchandisePageFooter_Click();
                    Merchandise.MerchandisePageFooterEditor.SelectByName();
                    Merchandise.MerchandisePageFooterEditor.HtmlMode_Click();
                    Merchandise.MerchandisePageFooterEditor.Content_Type(details.MerchandisePage.PageFooter);
                    Merchandise.MerchandisePageFooterEditor.SaveAndClose_Click();
                }
            }
            #endregion

            #region Checkout Page
            if ((details.CheckoutPage.PageHeader != null) || (details.CheckoutPage.PageFooter != null))
            {
                BuilderHelper.GotoPage(FormData.Page.Checkout);

                if (details.CheckoutPage.PageHeader != null)
                {
                    Checkout.CheckoutPageHeader_Click();
                    Checkout.CheckoutPageHeaderEditor.SelectByName();
                    Checkout.CheckoutPageHeaderEditor.HtmlMode_Click();
                    Checkout.CheckoutPageHeaderEditor.Content_Type(details.CheckoutPage.PageHeader);
                    Checkout.CheckoutPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.CheckoutPage.PageFooter != null)
                {
                    Checkout.CheckoutPageFooter_Click();
                    Checkout.CheckoutPageFooterEditor.SelectByName();
                    Checkout.CheckoutPageFooterEditor.HtmlMode_Click();
                    Checkout.CheckoutPageFooterEditor.Content_Type(details.CheckoutPage.PageFooter);
                    Checkout.CheckoutPageFooterEditor.SaveAndClose_Click();
                }
            }
            if (details.CheckoutPage.PaymentMethods.Count != 0)
            {
                BuilderHelper.GotoPage(FormData.Page.Checkout);

                foreach (PaymentMethod method in details.CheckoutPage.PaymentMethods)
                {
                    KeywordProvider.AddPaymentMethod.AddPaymentMethods(method);
                }
            }
            #endregion

            BuilderHelper.SaveAndClose_Click();
        }
    }
}
