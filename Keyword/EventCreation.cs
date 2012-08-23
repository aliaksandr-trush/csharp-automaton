namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;
    using RegOnline.RegressionTest.PageObject.Manager;
    using RegOnline.RegressionTest.Utilities;

    public class EventCreation
    {
        public void CreateEvent(Event details)
        {
            this.ClickAddEventAndGetEventId(details);

            this.StartPage(details);
            this.PersonalInfo(details);
            this.Agenda(details);
            this.LodgingTravel(details);
            this.Merchandise(details);
            this.Checkout(details);
            this.EventWebsite(details);

            PageObject.PageObjectProvider.Builder.EventDetails.SaveAndClose_Click();

            this.ActivateEvent(details);
        }

        public void ClickAddEventAndGetEventId(Event details)
        {
            PageObject.PageObjectProvider.Manager.Events.AddEvent_Click();
            PageObject.PageObjectProvider.Manager.Events.EventType_Select(details.FormType);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventId.WaitForPresent();
            details.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventId.GetAttribute("value"));
        }

        public void StartPage(Event details)
        {
            if (details.StartPage.EventType.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventType.SelectWithText(details.StartPage.EventType.ToString());
            }

            if (details.StartPage.StartDate.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartDate_Type(details.StartPage.StartDate.Value);
            }

            if (details.StartPage.EndDate.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EndDate_Type(details.StartPage.EndDate.Value);
            }

            if (details.StartPage.StartTime.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartTime_Type(details.StartPage.StartTime.Value);
            }

            if (details.StartPage.EndTime.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EndTime_Type(details.StartPage.EndTime.Value);
            }

            if (details.StartPage.Location != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.LocationName.Type(details.StartPage.Location);
            }
            if (details.StartPage.Phone != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.LocationPhone.Type(details.StartPage.Phone);
            }
            if (details.StartPage.Country != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.Country_Select(details.StartPage.Country);
            }
            if (details.StartPage.Address1 != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AddressLineOne.Type(details.StartPage.Address1);
            }
            if (details.StartPage.Address2 != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AddressLineTwo.Type(details.StartPage.Address2);
            }
            if (details.StartPage.City != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.City.Type(details.StartPage.City);
            }
            if (details.StartPage.State != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.State.SelectWithText(details.StartPage.State);
            }
            if (details.StartPage.Zip != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.PostalCode.Type(details.StartPage.Zip);
            }
            if (details.StartPage.ContactInfo != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EditContactInfo_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.ContactInfo.SelectByName();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.ContactInfo.HtmlMode_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.ContactInfo.Content_Type(details.StartPage.ContactInfo);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.ContactInfo.SaveAndClose_Click();
            }
            if (details.StartPage.EventHome != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventHome.Type(details.StartPage.EventHome);
            }
            if (details.StartPage.EventLimit != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.LimitRegs_Set(true);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegSpaces.Type(details.StartPage.EventLimit.Spaces);

                if (details.StartPage.EventLimit.EnableWaitList.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EnableWaitlist.Set(details.StartPage.EventLimit.EnableWaitList.Value);
                }
            }
            if (details.StartPage.RegTypes.Count != 0)
            {
                foreach (RegType regType in details.StartPage.RegTypes)
                {
                    KeywordProvider.AddRegType.Add_RegType(regType, details);
                    PageObject.Builder.RegistrationFormPages.RegTypeRow row = new PageObject.Builder.RegistrationFormPages.RegTypeRow(regType.RegTypeName);
                    regType.RegTypeId = row.RegTypeId;
                }
            }

            if (details.StartPage.RegTypeDisplayOption.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDisplayOption_Set(true);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.RegTypeDisplayFormat.SelectWithValue(CustomStringAttribute.GetCustomString(details.StartPage.RegTypeDisplayOption.Value));
            }

            // Set event name and shortcut after regType created to avoid bug 24560
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.Title.Type(details.Title);
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.Shortcut.Type(details.Shortcut);

            // Set event fee
            if (details.StartPage.Event_Fee != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFee_Type(details.StartPage.Event_Fee.StandardPrice);

                if (details.StartPage.Event_Fee.Early_Price != null ||
                    details.StartPage.Event_Fee.Late_Price != null ||
                    (details.StartPage.Event_Fee.DiscountCodes != null && details.StartPage.Event_Fee.DiscountCodes.Count > 0))
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeAdvanced_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.AdjustRADWindowPositionAndResize();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.Options_Expand();

                    if (details.StartPage.Event_Fee.Name != null)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.NameOnReceipt.Type(details.StartPage.Event_Fee.Name);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.NameOnReports.Type(details.StartPage.Event_Fee.Name);
                    }
                    else
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.NameOnReceipt.Type(details.StartPage.Event_Fee.Name + "_" + RegType.Default.FeeName);
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.NameOnReports.Type(details.StartPage.Event_Fee.Name + "_" + RegType.Default.FeeName);
                    }

                    if (details.StartPage.Event_Fee.DiscountCodes.Count != 0)
                    {
                        foreach (DataCollection.DiscountCode code in details.StartPage.Event_Fee.DiscountCodes)
                        {
                            KeywordProvider.AddDiscountCode.AddDiscountCodes(code, FormData.Location.EventFee);
                        }

                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.RequireCode.Set(details.StartPage.Event_Fee.RequireDC);
                    }

                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventFeeDefine.SaveAndClose_Click();
                }
            }

            if (details.StartPage.AllowGroupReg.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AllGroupReg_Set(details.StartPage.AllowGroupReg.Value);
                PageObject.PageObjectProvider.Builder.EventDetails.SaveAndStay_Click();
            }

            if (details.StartPage.ForceSelectSameRegType.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.ForceSameGroupType.Set(details.StartPage.ForceSelectSameRegType.Value);
            }

            if (details.StartPage.AllowChangeRegType.HasValue)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AllowChangeRegType.Set(details.StartPage.AllowChangeRegType.Value);
            }

            if (details.StartPage.GroupDiscount != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AddGroupDiscount_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.AdjustRADWindowPositionAndResize();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.SelectByName();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.GroupSize.Type(details.StartPage.GroupDiscount.GroupSize);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.GroupSizeOption.SelectWithText(CustomStringAttribute.GetCustomString(details.StartPage.GroupDiscount.GroupSizeOption));
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.DiscountAmount.Type(details.StartPage.GroupDiscount.DiscountAmount);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.DiscountType.SelectWithText(CustomStringAttribute.GetCustomString(details.StartPage.GroupDiscount.GroupDiscountType));
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.AddtionalRegOption.SelectWithText(CustomStringAttribute.GetCustomString(details.StartPage.GroupDiscount.AddtionalRegOption));
                
                if (details.StartPage.GroupDiscount.NumberOfAdditionalReg.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.AdditionalNumber.Type(details.StartPage.GroupDiscount.NumberOfAdditionalReg.Value);
                }

                switch (details.StartPage.GroupDiscount.ApplyOption)
                {
                    case GroupDiscount_ApplyOption.ToAllEventFees:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.ApplyToAllEventFees.Click();
                        break;

                    case GroupDiscount_ApplyOption.ToOnlySelectedFees:
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.ApplyToSelectedFees.Click();
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.All.Set(true);
                        break;

                    default:
                        break;
                }

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.GroupDiscountDefine.SaveAndClose_Click();
            }

            if (details.StartPage.AdvancedSettings != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.AdvancedSettings_Click();
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventAdvancedSettings.SelectByName();

                if (details.StartPage.AdvancedSettings.ThisIsAParentEvent.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventAdvancedSettings.ThisIsAParentEvent.Set(details.StartPage.AdvancedSettings.ThisIsAParentEvent.Value);
                }

                if (details.StartPage.AdvancedSettings.ThisIsAChildEvent.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventAdvancedSettings.ThisIsAChildEvent.Set(details.StartPage.AdvancedSettings.ThisIsAChildEvent.Value);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventAdvancedSettings.SelectParentEvent(details);
                }

                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.EventAdvancedSettings.SaveAndClose_Click();
            }

            if ((details.StartPage.PageHeader != null) || (details.StartPage.PageFooter != null))
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Start);

                if (details.StartPage.PageHeader != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageHeader_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageHeaderEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageHeaderEditor.Content_Type(details.StartPage.PageHeader);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.StartPage.PageFooter != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageFooter_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageFooterEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageFooterEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageFooterEditor.Content_Type(details.StartPage.PageFooter);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.StartPage.StartPageFooterEditor.SaveAndClose_Click();
                }
            }
        }

        public void PersonalInfo(Event details)
        {
            if ((details.PersonalInfoPage.PageHeader != null) || (details.PersonalInfoPage.PageFooter != null))
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.PI);

                if (details.PersonalInfoPage.PageHeader != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeader_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.Content_Type(details.PersonalInfoPage.PageHeader);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.PersonalInfoPage.PageFooter != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageFooter_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageFooterEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.Content_Type(details.PersonalInfoPage.PageFooter);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.PersonalInfoPageHeaderEditor.SaveAndClose_Click();
                }
            }
            if (details.PersonalInfoPage.StandardFields.Count != 0)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.PI);

                foreach (PersonalInfoPageStandardField field in details.PersonalInfoPage.StandardFields)
                {
                    if (field.Visible.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.SetPersonalInfoFieldVisible(field.StandardField, field.Visible.Value);
                    }
                    if (field.Required.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.SetPersonalInfoFieldRequired(field.StandardField, field.Required.Value);
                    }
                }
            }
            if (details.PersonalInfoPage.CustomFields.Count != 0)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.PI);

                if (PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.EmptyAddCustomField.IsPresent)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.EmptyAddCustomField_Click();
                }
                else
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.AddCustomField_Click();
                }

                for (int i = 0; i < details.PersonalInfoPage.CustomFields.Count; i++)
                {
                    KeywordProvider.CustomFieldCreation.AddCustomField(details.PersonalInfoPage.CustomFields[i]);
                    PageObject.Builder.RegistrationFormPages.PICustomFieldRow row = new PageObject.Builder.RegistrationFormPages.PICustomFieldRow(details.PersonalInfoPage.CustomFields[i].NameOnForm);
                    details.PersonalInfoPage.CustomFields[i].Id = row.CustomFieldId;

                    if (i < details.PersonalInfoPage.CustomFields.Count - 1)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.PersonalInfoPage.AddCustomField_Click();
                    }
                }
            }
        }

        public void Agenda(Event details)
        {
            if (details.AgendaPage != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Agenda);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.YesOnSplashPage_Click();

                foreach (AgendaItem agendaItem in details.AgendaPage.AgendaItems)
                {
                    KeywordProvider.AddAgendaItem.AddAgendaItems(agendaItem, details);
                    agendaItem.Id = Convert.ToInt32(PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaItemId.Value);
                }

                if (details.AgendaPage.DoNotAllowOverlapping.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DoNotAllowOverlapping.Set(details.AgendaPage.DoNotAllowOverlapping.Value);
                }

                if (details.AgendaPage.IsShoppingCart)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.DoNotAllowOverlapping.Set(false);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.IsShoppingCart.Set(true);
                }

                if (details.AgendaPage.PageHeader != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageHeader_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageHeaderEditor.Content_Type(details.AgendaPage.PageHeader);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.AgendaPage.PageFooter != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageFooter_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageFooterEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageFooterEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageFooterEditor.Content_Type(details.AgendaPage.PageFooter);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.AgendaPage.AgendaPageFooterEditor.SaveAndClose_Click();
                }
            }
        }

        public void LodgingTravel(Event details)
        {
            if (details.LodgingTravelPage != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.LodgingTravel);
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.YesOnSplashPage_Click();

                foreach (LodgingStandardFields field in details.LodgingTravelPage.Lodging.StandardFields)
                {
                    if (field.Visible.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.SetLodgingStandardFieldVisible(field.Field, field.Visible.Value);
                    }
                    if (field.Required.HasValue)
                    {
                        PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.SetLodgingStandardFieldRequired(field.Field, field.Required.Value);
                    }
                }

                if (details.LodgingTravelPage.PageHeader != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageHeader_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageHeaderEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageHeaderEditor.Content_Type(details.LodgingTravelPage.PageHeader);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.LodgingTravelPage.PageFooter != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageFooter_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageFooterEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageFooterEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageFooterEditor.Content_Type(details.LodgingTravelPage.PageFooter);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.LodgingTravelPage.LTPageFooterEditor.SaveAndClose_Click();
                }
            }
        }

        public void Merchandise(Event details)
        {
            if (details.MerchandisePage != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Merchandise);

                foreach (DataCollection.MerchandiseItem merch in details.MerchandisePage.Merchandises)
                {
                    KeywordProvider.AddMerchandise.AddMerchandises(merch, details);
                    PageObject.Builder.RegistrationFormPages.MerchandiseRow row = new PageObject.Builder.RegistrationFormPages.MerchandiseRow(merch);
                    merch.Id = row.MerchandiseId;
                }

                if (details.MerchandisePage.PageHeader != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageHeader_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageHeaderEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageHeaderEditor.Content_Type(details.MerchandisePage.PageHeader);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageHeaderEditor.SaveAndClose_Click();
                }

                if (details.MerchandisePage.PageFooter != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageFooter_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageFooterEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageFooterEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageFooterEditor.Content_Type(details.MerchandisePage.PageFooter);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.MerchandisePage.MerchandisePageFooterEditor.SaveAndClose_Click();
                }
            }
        }

        public void Checkout(Event details)
        {
            if ((details.CheckoutPage.PageHeader != null) || (details.CheckoutPage.PageFooter != null))
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Checkout);

                if (details.CheckoutPage.PageHeader != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageHeader_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageHeaderEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageHeaderEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageHeaderEditor.Content_Type(details.CheckoutPage.PageHeader);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageHeaderEditor.SaveAndClose_Click();
                }

                if (details.CheckoutPage.PageFooter != null)
                {
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageFooter_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageFooterEditor.SelectByName();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageFooterEditor.HtmlMode_Click();
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageFooterEditor.Content_Type(details.CheckoutPage.PageFooter);
                    PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.CheckoutPageFooterEditor.SaveAndClose_Click();
                }
            }
            if (details.CheckoutPage.PaymentMethods.Count != 0)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.FormPages.GotoPage(FormData.Page.Checkout);

                foreach (PaymentMethod method in details.CheckoutPage.PaymentMethods)
                {
                    KeywordProvider.AddPaymentMethod.AddPaymentMethods(method);
                }
            }
        }

        public void EventWebsite(Event details)
        {
            if (details.EventWebsite != null)
            {
                PageObject.PageObjectProvider.Builder.EventDetails.EventWebsite_Click();

                if (details.EventWebsite.UseEventWebsiteAsTheStartingPageForEvent.HasValue)
                {
                    PageObject.PageObjectProvider.Builder.EventWebsite.UseEventWebsiteAsTheStartingPageForEvent.WaitForDisplay();

                    PageObject.PageObjectProvider.Builder.EventWebsite.UseEventWebsiteAsTheStartingPageForEvent.Set(
                        details.EventWebsite.UseEventWebsiteAsTheStartingPageForEvent.Value);
                }
                if (details.EventWebsite.ShowNavigation)
                {
                    PageObject.PageObjectProvider.Builder.EventWebsite.EventWebsiteFrame.SelectById();
                    PageObject.PageObjectProvider.Builder.EventWebsite.EventWebsiteFrame.ShowNavigation_Click();
                    PageObject.PageObjectProvider.Builder.EventWebsite.EventWebsiteFrame.SwitchToMain();
                }
                
                PageObject.PageObjectProvider.Builder.EventDetails.RegistrationFormPages_Click();
            }
        }

        public void ActivateEvent(Event details)
        {
            if (details.IsActive)
            {
                KeywordProvider.ManagerDefault.OpenFormDashboard(details.Title);
                PageObject.PageObjectProvider.Manager.Dashboard.Activate_Click();
                PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.SelectByName();
                PageObject.PageObjectProvider.Manager.Dashboard.ActivateEvent.Activate_Click();
                PageObject.PageObjectProvider.Manager.Dashboard.DashboardTab_Click(FormData.DashboardTab.EventDetails);
                PageObject.PageObjectProvider.Manager.Dashboard.ReturnToList_Click();
            }
        }
    }
}
