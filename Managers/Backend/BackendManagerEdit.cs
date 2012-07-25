namespace RegOnline.RegressionTest.Managers.Backend
{
    using System;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class BackendManager : ManagerBase
    {
        private const string CFPrefix = "CF";
        private const string CFTimeFieldFormat = "w{0}Id{1}";
        private const string CFDateFieldFormat = "wDate{0}_{1}";
        private const string DiscountCodeLocatorFormat = "//input[@id='cfPassword{0}')]";
        private const string PricingOptionLocatorFormat = "//input[@id='CFSchedule{0}{1}']";
        private const string CustomFieldStatusLocatorFormat = "//select[@id='RegStatusId{0}']";
        private const string AttendeeInfoWindowTitle = "Attendee Info";
        private const string MemberInfoWindowTitle = "Member Info";
        private const string DefaultCCExpirationMonth = "12";
        private const string DefaultCCExpirationMonthNew = "12 - Dec";

        public BackendPaymentMethodManager PaymentMethodMgr = new BackendPaymentMethodManager();

        public enum ConfirmWhenSaveEditPI
        {
            Correction,
            Substitution
        }

        public enum TimeField
        {
            Hour,
            Minute,
            ampm
        }

        public enum DateField
        {
            Year,
            Month,
            Day
        }

        public enum LodgingTravelBookingStatus
        {
            None,
            Initiated,
            Pending,
            Confirmed,
            Cancelled,
            WaitList,
            Unblocked,
            Reuse
        }

        [Step]
        public void OpenEditAttendeeSubPage(AttendeeSubPage attendeeSubPage)
        {
            string locator = string.Empty;
            string subPageEdit = string.Empty;
            string subPageWindow = string.Empty;
            string locatorFormat = "//div[@id='{0}']//a[text()='Edit']";

            switch (attendeeSubPage)
            {
                case AttendeeSubPage.PersonalInformation:
                    subPageEdit = "personal";
                    subPageWindow = "Personal";
                    break;
                case AttendeeSubPage.CustomFields:
                    subPageEdit = "cf";
                    subPageWindow = "cfs";
                    break;
                case AttendeeSubPage.Agenda:
                case AttendeeSubPage.MembershipFees:
                    subPageEdit = "agenda";
                    subPageWindow = "cfs";
                    break;
                case AttendeeSubPage.Merchandise:
                    subPageEdit = "fees";
                    subPageWindow = "cfs";
                    break;
                case AttendeeSubPage.LodgingAndTravel:
                    subPageEdit = "lodgingTravel";
                    subPageWindow = "prefs";
                    break;
                default:
                    throw new ArgumentException("Sub page " + attendeeSubPage.ToString() + " does not have an edit button.");
            }

            locator = string.Format(locatorFormat, subPageEdit);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectWindowByName(subPageWindow);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void SaveAndCloseEditPersonalInformation()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("save", LocateBy.Name);
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndCloseEditCF()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSaveAndClose", LocateBy.Id);
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndCloseEditMembershipFee()
        {
            this.SaveAndCloseEditCF();
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndCloseEditMerchandise()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("submit", LocateBy.Name);
            Utility.ThreadSleep(1);
        }

        [Step]
        public void SaveAndCloseEditLodingTravel()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Save & Close']", LocateBy.XPath);
            System.Threading.Thread.Sleep(1500);
        }

        [Step]
        public void ConfirmWhenSaveAndCloseEditPI(ConfirmWhenSaveEditPI confirmOption)
        {
            UIUtilityProvider.UIHelper.SelectWindowByName("Confirm");

            switch (confirmOption)
            {
                case ConfirmWhenSaveEditPI.Correction:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSpelling", LocateBy.Name);
                    break;
                case ConfirmWhenSaveEditPI.Substitution:
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSubstitute", LocateBy.Name);
                    break;
                default:
                    break;
            }

            Utility.ThreadSleep(2);
        }

        public void OpenPaymentMethod()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//tr[@id='trpaymnetmethodname']/td[1]/a", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectWindowByName("CreditCard");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        /// <summary>
        /// This covers the payments which do not require additional data: cash, wire transfer, event, paypal.
        /// </summary>
        public void EditPaymentMethod(PaymentMethodManager.PaymentMethod selection)
        {
            OpenPaymentMethod();
            this.PaymentMethodMgr.SelectPayBy(selection);

            this.PaymentMethodMgr.SaveAndClose();
            SelectAttendeeInfoWindow();
            UIUtilityProvider.UIHelper.RefreshPage();
        }

        public void EditPaymentMethodCreditCard()
        {
            OpenPaymentMethod();            
            this.PaymentMethodMgr.SelectPayBy(PaymentMethodManager.PaymentMethod.CreditCard);

            this.PaymentMethodMgr.SelectCreditCardType(PaymentManager.DefaultPaymentInfo.CCType);
            this.PaymentMethodMgr.TypeCreditCardNumber(PaymentManager.DefaultPaymentInfo.CCNumber);
            this.PaymentMethodMgr.SelectCreditCardExpirationMonth(DefaultCCExpirationMonth);
            this.PaymentMethodMgr.SelectCreditCardExpirationYear(PaymentManager.DefaultPaymentInfo.ExpirationYear);
            this.PaymentMethodMgr.TypeCreditCardName(PaymentManager.DefaultPaymentInfo.HolderName);
            this.PaymentMethodMgr.SelectCreditCardCountry(PaymentManager.DefaultPaymentInfo.Country);
            this.PaymentMethodMgr.TypeCreditCardAddress(PaymentManager.DefaultPaymentInfo.BillingAddressLineOne);
            this.PaymentMethodMgr.TypeCreditCardAddress2(PaymentManager.DefaultPaymentInfo.BillingAddressLineTwo);
            this.PaymentMethodMgr.TypeCreditCardCity(PaymentManager.DefaultPaymentInfo.BillingCity);
            this.PaymentMethodMgr.TypeCreditCardState(PaymentManager.DefaultPaymentInfo.BillingState);
            this.PaymentMethodMgr.TypeCreditCardZip(PaymentManager.DefaultPaymentInfo.ZipCode);

            this.PaymentMethodMgr.SaveAndClose();
            SelectAttendeeInfoWindow();
            UIUtilityProvider.UIHelper.RefreshPage();
        }

        [Step]
        public void EditAttendeePersonalInformation()
        {
            string expectedCountry = "United States";
            string expectedPhone = "909.900.9000";

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//a[contains(@href, 'LoadPersonalNewUI')]", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SelectWindowByName("Personal");
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectWithText("Country", expectedCountry, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type("Phone", expectedPhone, LocateBy.Name);
            this.SaveAndCloseEditPersonalInformation();
            SelectAttendeeInfoWindow();


            // Need a better waiting system here, waitforpagetoload is not working well... 
            Utility.ThreadSleep(3);

            string updatedCountry = UIUtilityProvider.UIHelper.GetText("//td[@id='tdThirtyOne']", LocateBy.XPath);

            Assert.AreEqual(updatedCountry, expectedCountry);

            string updatedPhone = UIUtilityProvider.UIHelper.GetText("//td[@id='tdThirtySeven']", LocateBy.XPath);
            Assert.AreEqual(updatedPhone, expectedPhone);
        }

        //Check/Uncheck event cost checkbox
        public void SetAndSaveOneEventCostItem(string feeName, bool isFeeSelected)
        {
            OpenAttendeeSubPage(AttendeeSubPage.PersonalInformation);
            OpenEditAttendeeEventCostWindow();

            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            SetCheckboxCFItem(feeName, isFeeSelected, null, null, null);
            SaveAndCloseEditCF();
            SaveAndBypassTransaction();
        }

        //Check/Uncheck agenda cost checkbox
        public void SetAndSaveOneAgendaCheckboxItem(string agendaName, bool isAgendaSelected)
        {
            //open agenda page
            OpenAttendeeSubPage(AttendeeSubPage.Agenda);
            OpenEditAttendeeSubPage(AttendeeSubPage.Agenda);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            SetCheckboxCFItem(agendaName, isAgendaSelected, null, null, null);
            SaveAndCloseEditCF();
            SaveAndBypassTransaction();
        }

        public void SetAndSaveOneMerchandiseItem(string merchName, int? feeQuantity)
        {
            //subtract merchandise cost
            OpenAttendeeSubPage(AttendeeSubPage.Merchandise);
            OpenEditAttendeeSubPage(AttendeeSubPage.Merchandise);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            SetMerchandiseItem(merchName, feeQuantity, null);
            SaveAndCloseEditMerchandise();
            SaveAndBypassTransaction();
        }

        [Step]
        public void OpenEditAttendeeEventCostWindow()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("lnkEventCost", LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectWindowByName("cfs");
        }

        [Step]
        public void SelectAttendeeInfoWindow()
        {
            Utility.ThreadSleep(.5); 
            try
            {
                UIUtilityProvider.UIHelper.SelectWindowByTitle(AttendeeInfoWindowTitle);
            }
            catch
            {
                UIUtilityProvider.UIHelper.SelectWindowByTitle(MemberInfoWindowTitle);
            }

            Utility.ThreadSleep(1.5);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void CancelUncancelRegistration(bool isCancel)
        {
            OpenAttendeeSubPage(AttendeeSubPage.Transactions);
            string attendeeId = UIUtilityProvider.UIHelper.ExtractElementInQueryString(UIUtilityProvider.UIHelper.GetAttribute("//tr[@class='AttendeeHeader']/td/a[3]", "href", LocateBy.XPath), "AttendeeId");
            //string idString = "Rolwinreguncancelasp";
            string wndID = string.Empty;
            string linkText = "Undo Cancellation";

            if (isCancel)
            {

                linkText = "Cancel Registration";
            }

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(linkText, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.SelectTopWindow();



            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Action", LocateBy.Name);
            SelectAttendeeInfoWindow();
        }

        [Step]
        public void SetGender(Gender gender)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(StringEnum.GetStringValue(gender), LocateBy.Id);
        }

        [Step]
        public void TypeLodgingArrivalDateField(DateField dateField, string value)
        {
            UIUtilityProvider.UIHelper.Type("LdgArrDate_" + dateField.ToString(), value, LocateBy.Id);
        }

        [Step]
        public void TypeLodgingDepartureDateField(DateField dateField, string value)
        {
            UIUtilityProvider.UIHelper.Type("LdgDptDate_" + dateField.ToString(), value, LocateBy.Id);
        }

        [Step]
        public void TypeTravelArrivalDateField(DateField dateField, string value)
        {
            UIUtilityProvider.UIHelper.Type("TrvArrDate_" + dateField.ToString(), value, LocateBy.Id);
        }

        [Step]
        public void TypeTravelDepartureDateField(DateField dateField, string value)
        {
            UIUtilityProvider.UIHelper.Type("TrvDptDate_" + dateField.ToString(), value, LocateBy.Id);
        }

        [Step]
        public void SelectTravelCCExpirationDate(DateField dateField, string value)
        {
            switch (dateField)
            {
                case DateField.Year:
                    UIUtilityProvider.UIHelper.SelectWithText("TrvccExpYear", value, LocateBy.Name);
                    break;

                case DateField.Month:
                    UIUtilityProvider.UIHelper.SelectWithText("TrvccExpMonth", value, LocateBy.Name);
                    break;

                case DateField.Day:
                    UIUtilityProvider.UIHelper.FailTest("There is NO 'Day' field for CC expiration date!");
                    break;

                default:
                    break;
            }
        }

        [Step]
        public int GetCFItemID(string name)
        {
            string id = UIUtilityProvider.UIHelper.GetAttribute(string.Format("//*[text()='{0}']/../../..//input[@name='hdnCFFieldName']", name), "value", LocateBy.XPath);

            // Use regular expression to split the number string out from the id string which may contains characters
            MatchCollection matches = Regex.Matches(id, @"\d+");

            return Convert.ToInt32(matches[0].Value);
        }

        [Step]
        public void SetCheckboxCFItem(int cfId, bool check)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(CFPrefix + cfId.ToString(), check, LocateBy.Id);
        }

        [Step]
        public void SetCheckboxCFItem(string cfName, bool check)
        {
            SetCheckboxCFItem(this.GetCFItemID(cfName), check);
        }

        [Step]
        public void SelectCFStatus(int cfId, CustomFieldStatus? customFieldStatus)
        {
            string customFieldStatusLocator = string.Format(CustomFieldStatusLocatorFormat, cfId.ToString());

            if (customFieldStatus.HasValue)
            {
                UIUtilityProvider.UIHelper.SelectWithText(customFieldStatusLocator, StringEnum.GetStringValue(customFieldStatus.Value), LocateBy.XPath);
            }
        }

        [Step]
        public void SelectCFStatus(string cfName, CustomFieldStatus? customFieldStatus)
        {
            this.SelectCFStatus(this.GetCFItemID(cfName), customFieldStatus);
        }

        public void TypeCFDiscountCode(int cfId, string code)
        {
            string discountCodeLocator = string.Format(DiscountCodeLocatorFormat, cfId.ToString());

            if (code != null)
            {
                UIUtilityProvider.UIHelper.Type(discountCodeLocator, code, LocateBy.XPath);
            }
        }

        public void SetCFPricingSchedule(int cfId, PricingOption? pricingOption)
        {
            if (pricingOption.HasValue)
            {
                string pricingOptionLocator = string.Format(PricingOptionLocatorFormat, (int)pricingOption.Value, cfId);
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(pricingOptionLocator, LocateBy.XPath);
            }
        }

        public void SetCheckboxCFItem(string cfName, bool check, string discountCode, PricingOption? pricingOption, CustomFieldStatus? customFieldStatus)
        {
            int checkboxId = this.GetCFItemID(cfName);
            this.SetCheckboxCFItem(checkboxId, check);
            this.TypeCFDiscountCode(checkboxId, discountCode);
            this.SetCFPricingSchedule(checkboxId, pricingOption);
            this.SelectCFStatus(checkboxId, customFieldStatus);
        }

        [Step]
        public void SelectCFMultiChoiceRadioButton(int cfId, string choice)
        {
            string choiceLocator = UIUtilityProvider.UIHelper.GetAttribute(
                string.Format("//input[@value='{0}']/../label[text()='{1}']", CFPrefix + cfId.ToString(), choice),
                "for", 
                LocateBy.XPath);

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(choiceLocator, LocateBy.Id);
        }

        public void SelectCFMultiChoiceRadioButton(string cfName, string choice)
        {
            this.SelectCFMultiChoiceRadioButton(this.GetCFItemID(cfName), choice);
        }

        [Step]
        public void ClearCFSelection(int cfId)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//a[@href=\"javascript:clearRadio('CF{0}');\"]", cfId.ToString()), LocateBy.XPath);
        }

        public void ClearCFSelection(string cfName)
        {
            this.ClearCFSelection(this.GetCFItemID(cfName));
        }

        [Step]
        public void SelectCFDropDownList(int cfId, string choice)
        {
            UIUtilityProvider.UIHelper.SelectWithText(CFPrefix + cfId.ToString(), choice, LocateBy.Id);
        }

        public void SelectCFDropDownList(string cfName, string choice)
        {
            SelectCFDropDownList(this.GetCFItemID(cfName), choice);
        }

        [Step]
        public void TypeCFText(int cfId, string text)
        {
            UIUtilityProvider.UIHelper.Type(CFPrefix + cfId.ToString(), text, LocateBy.Id);
        }

        public void TypeCFText(string cfName, string text)
        {
            TypeCFText(this.GetCFItemID(cfName), text);
        }

        [Step]
        public void SelectCFTimeField(int cfId, TimeField timeField, string value)
        {
            UIUtilityProvider.UIHelper.SelectWithText(
                string.Format(CFTimeFieldFormat, timeField.ToString(), cfId.ToString()),
                value, 
                LocateBy.Id);
        }

        public void SelectCFTimeField(string cfName, TimeField timeField, string value)
        {
            this.SelectCFTimeField(this.GetCFItemID(cfName), timeField, value);
        }

        [Step]
        public void TypeCFDateField(int cfId, DateField dateField, string value)
        {
            UIUtilityProvider.UIHelper.Type(
                string.Format(CFDateFieldFormat, cfId.ToString(), dateField.ToString()),
                value, 
                LocateBy.Id);
        }

        public void TypeCFDateField(string cfName, DateField dateField, string value)
        {
            this.TypeCFDateField(this.GetCFItemID(cfName).ToString(), dateField, value);
        }

        [Step]
        public void ClickShowCalendar(int cfId)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format("//input[@id='calwDate{0}']/following-sibling::img[@alt='Show calendar']", cfId.ToString()), LocateBy.XPath);
        }

        public void ClickShowCalendar(string cfName)
        {
            this.ClickShowCalendar(this.GetCFItemID(cfName));
        }

        public void SetMerchandiseItem(string itemName, int? itemQuantity, string discountCode)
        {
            string quantityLocator = "//td[contains(text(), '{0}')]/../td[4]/input[contains(@id, 'FeeQuantity')]";
            string discountCodeLocator = "//td[contains(text(), '{0}')]/../td[5]/input[contains(@name,'FeePassword')]";

            discountCodeLocator = string.Format(discountCodeLocator, itemName);
            quantityLocator = string.Format(quantityLocator, itemName);

            UIUtilityProvider.UIHelper.WaitForElementPresent(quantityLocator, LocateBy.XPath);

            if (!itemQuantity.HasValue)
            {
                UIUtilityProvider.UIHelper.Type(quantityLocator, string.Empty, LocateBy.XPath);
            }
            else
            {
                UIUtilityProvider.UIHelper.Type(quantityLocator, itemQuantity.Value.ToString(), LocateBy.XPath);
            }

            if (discountCode != null)
            {
                VerifyTool.VerifyValue(true, UIUtilityProvider.UIHelper.IsElementPresent(discountCodeLocator, LocateBy.XPath), "The discount code display is :{0}");
                UIUtilityProvider.UIHelper.Type(discountCodeLocator, discountCode, LocateBy.XPath);
            }
        }

        [Step]
        public void TypeMerchandiseItemAmount(int merchId, double? amount)
        {
            UIUtilityProvider.UIHelper.Type(string.Format("FeeAmount{0}", merchId), amount, LocateBy.Id);
        }

        [Step]
        public void TypeMerchandiseItemQuantity(int merchId, int? quantity)
        {
            UIUtilityProvider.UIHelper.Type(string.Format("FeeQuantity{0}", merchId), quantity, LocateBy.Id);
        }

        [Step]
        public void SelectMerchandiseItemMultiChoice(int merchId, string choice)
        {
            UIUtilityProvider.UIHelper.SelectWithText(string.Format("FeeDDValue{0}", merchId), choice, LocateBy.Name);
        }

        [Step]
        public void ChangeRenewalDate(DateTime date)
        {
            this.ClickChangeRenewalDate();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");

            this.ClickShowRenewalDateCalendar();
            this.TypeNextRenewalDate(date);
            this.ClickSaveRenewalDate();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void ClickChangeRenewalDate()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(NextRenewDateLinkLocator, LocateBy.XPath);
            Utility.ThreadSleep(1.5);
        }

        public void TypeNextRenewalDate(DateTime date)
        {
            UIUtilityProvider.UIHelper.SetDateTimeById("dtpRenewalDate", date);
        }

        public void ClickShowRenewalDateCalendar()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("dtpRenewalDate_CalendarPopupButton", LocateBy.Id);
        }

        public void ClickSaveRenewalDate()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnSave", LocateBy.Id);
            Utility.ThreadSleep(1.5);
        }

        public void ClickCancelRenewalDate()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("btnCancel", LocateBy.Id);
            Utility.ThreadSleep(1.5);
        }
    }
}

