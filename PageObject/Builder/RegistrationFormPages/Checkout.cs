namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;
    using System;

    public class Checkout : Window
    {
        public Clickable AddPaymentMethod = new Clickable("//*[@id='ctl00_cph_divAddPaymentMethod']/a", LocateBy.XPath);
        public PaymentMethodSelections PaymentMethodSelections = new PaymentMethodSelections("dialog");
        public Clickable CheckoutPageHeader = new Clickable("//*[text()='Add Checkout Page Header']", LocateBy.XPath);
        public HtmlEditor CheckoutPageHeaderEditor = new HtmlEditor("dialog");
        public Clickable CheckoutPageFooter = new Clickable("//*[text()='Add Checkout Page Footer']", LocateBy.XPath);
        public HtmlEditor CheckoutPageFooterEditor = new HtmlEditor("dialog");
        public RadioButton AddServiceFee = new RadioButton("ctl00_cph_radShare", LocateBy.Id);
        public CCOptions CC_Options_Popup_Frame = new CCOptions("dialog");
        public PaymentMethodRow Row_CreditCard = new PaymentMethodRow(DataCollection.FormData.PaymentMethodEnum.CreditCard);
        public PaymentMethodRow Row_Check = new PaymentMethodRow(DataCollection.FormData.PaymentMethodEnum.Check);
        private MultiChoiceDropdown EventCurrency = new MultiChoiceDropdown("ctl00_cph_roEventsCurrencyCode", LocateBy.Id);

        public void AddPaymentMethod_Click()
        {
            this.AddPaymentMethod.WaitForDisplay();
            this.AddPaymentMethod.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void CheckoutPageHeader_Click()
        {
            this.CheckoutPageHeader.WaitForDisplay();
            this.CheckoutPageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void CheckoutPageFooter_Click()
        {
            this.CheckoutPageFooter.WaitForDisplay();
            this.CheckoutPageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void EventCurrency_Select(Utilities.MoneyTool.CurrencyCode currency)
        {
            this.EventCurrency.WaitForDisplay();
            this.EventCurrency.SelectWithValue(currency.ToString());
            ConsumeAlert();
        }
    }

    public class PaymentMethodSelections : Frame
    {
        public PaymentMethodSelections(string name) : base(name) { }

        public MultiChoiceDropdown PaymentMethods = new MultiChoiceDropdown("ctl00_cphDialog_ddlPaymentMethod", LocateBy.Id);

        public void PaymentMethod_Select(DataCollection.PaymentMethod method)
        {
            this.PaymentMethods.WaitForDisplay();
            this.PaymentMethods.SelectWithText(CustomStringAttribute.GetCustomString(method.PMethod));
        }

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }
    }

    public class CCOptions : Frame
    {
        private MultiChoiceDropdown GatewaySelection = new MultiChoiceDropdown("ctl00_cphDialog_ddlEventsCustomerMerchantID", LocateBy.Id);
        private Input DynamicDescriptor = new Input("ctl00_cphDialog_txtEventsDynamicDescriptor", LocateBy.Id);

        public CCOptions(string name)
            : base(name)
        {
        }

        public void GatewaySelection_Select(DataCollection.FormData.Gateway gateway)
        {
            this.GatewaySelection.WaitForDisplay();
            this.GatewaySelection.SelectWithText(CustomStringAttribute.GetCustomString(gateway));
            WaitForAJAX();
            Utility.ThreadSleep(3);
        }

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }
    }

    public class PaymentMethodRow : Window
    {
        private const string Tr_Locator_Format_MethodRow = "//input[@value='{0}']/parent::td/parent::tr";

        public Clickable EditCCOptions_Link;
        public CheckBox Visible_Public;
        public CheckBox Visible_Admin;
        public CheckBox Visible_OnSite;
        public Clickable Delete;

        public PaymentMethodRow(DataCollection.FormData.PaymentMethodEnum method)
        {
            switch (method)
            {
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.CreditCard:
                    this.EditCCOptions_Link = new Clickable("//tr[@id='ctl00_cph_trCreditCard']/td/a[text()='Credit Cards']", LocateBy.XPath);
                    this.Visible_Public = new CheckBox("ctl00_cph_chkCreditCardsPublic", LocateBy.Id);
                    this.Visible_Admin = new CheckBox("ctl00_cph_chkCreditCardsAdmin", LocateBy.Id);
                    this.Visible_OnSite = new CheckBox("ctl00_cph_chkCreditCardsOnsite", LocateBy.Id);
                    this.Delete = new Clickable("//tr[@id='ctl00_cph_trCreditCard']/td/a/img[@alt='Delete']", LocateBy.XPath);
                    break;

                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.Check:
                    this.Visible_Public = new CheckBox(this.Compose_Locator_Visible_Public(method), LocateBy.XPath);
                    this.Visible_Admin = new CheckBox(this.Compose_Locator_Visible_Admin(method), LocateBy.XPath);
                    this.Visible_OnSite = new CheckBox(this.Compose_Locator_Visible_OnSite(method), LocateBy.XPath);
                    this.Delete = new Clickable(this.Compose_Locator_Delete(method), LocateBy.XPath);
                    break;

                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.PurchaseOrder:
                    throw new NotImplementedException();
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.Cash:
                    throw new NotImplementedException();
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.PayAtTheEvent:
                    throw new NotImplementedException();
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.CostCenter:
                    throw new NotImplementedException();
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.WireTransfer:
                    throw new NotImplementedException();
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.Custom:
                    throw new NotImplementedException();
                case RegOnline.RegressionTest.DataCollection.FormData.PaymentMethodEnum.PayPal:
                    throw new NotImplementedException();
                default:
                    break;
            }
        }

        private string Compose_Locator_Visible_Public(DataCollection.FormData.PaymentMethodEnum method)
        {
            return string.Format(Tr_Locator_Format_MethodRow, (int)method) + "/td[2]/input";
        }

        private string Compose_Locator_Visible_Admin(DataCollection.FormData.PaymentMethodEnum method)
        {
            return string.Format(Tr_Locator_Format_MethodRow, (int)method) + "/td[3]/input";
        }

        private string Compose_Locator_Visible_OnSite(DataCollection.FormData.PaymentMethodEnum method)
        {
            return string.Format(Tr_Locator_Format_MethodRow, (int)method) + "/td[4]/input";
        }

        private string Compose_Locator_Delete(DataCollection.FormData.PaymentMethodEnum method)
        {
            return string.Format(Tr_Locator_Format_MethodRow, (int)method) + "/td[5]/a/img[@alt='Delete']";
        }

        public void EditCCOptions_Link_ClickToOpen()
        {
            this.EditCCOptions_Link.WaitForDisplayAndClick();
            WaitForAJAX();
            WaitForLoad();
        }

        public void Visible_Public_Set(bool check)
        {
            this.Visible_Public.Set(check);
        }

        public void Visible_Admin_Set(bool check)
        {
            this.Visible_Admin.Set(check);
        }

        public void Visible_OnSite_Set(bool check)
        {
            this.Visible_OnSite.Set(check);
        }

        public void Delete_Click()
        {
            this.Delete.WaitForDisplayAndClick();
            ConsumeAlert();
            WaitForAJAX();
            Utility.ThreadSleep(2);
        }
    }
}
