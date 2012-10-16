namespace RegOnline.RegressionTest.Fixtures.Manager.EventDashboard.OnSite
{
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Attributes;

    internal class OnSiteFixtureHelper : FixtureBase
    {
        public class RegistrationInfo
        {
            public int regId;
            public string emailAddress;
            public string fullName;
        }

        [Step]
        public string Login()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            return ManagerSiteMgr.GetEventSessionId();
        }

        public void SetupStartPage(string eventName)
        {
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventName);
            BuilderMgr.AddRegTypeWithEventFee("One", 10.00);
            BuilderMgr.VerifyHasRegTypeInDatabase("One");
            BuilderMgr.AddRegTypeWithEventFee("Two", 20.00);
            BuilderMgr.VerifyHasRegTypeInDatabase("Two");
        }

        public void SetupPersonalInfoPage()
        {
            BuilderMgr.VerifyPersonalInfoPageDefaults();
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.CheckBox, "CF-Checkbox");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.RadioButton, "CF-Radio");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Dropdown, "CF-DropDown");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Number, "CF-Numeric");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.OneLineText, "CF-Text");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Time, "CF-Time");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.AlwaysSelected, "CF-Always");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Paragraph, "CF-Paragraph");
            this.AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType.Date, "CF-Date");
        }

        private void AddAndVerifyPersonalInfoCustomField(CustomFieldManager.CustomFieldType type, string name)
        {
            BuilderMgr.AddPICustomField(type, name);
            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(type, name);
        }

        public void SetupAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            // Add/verify agenda items of each type
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.CheckBox, "AI-Checkbox", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.RadioButton, "AI-Radio", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Dropdown, "AI-DropDown", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Number, "AI-Numeric", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.OneLineText, "AI-Text", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Time, "AI-Time", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.AlwaysSelected, "AI-Always", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Paragraph, "AI-Paragraph", 10.00);
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(AgendaItemManager.AgendaItemType.Date, "AI-Date", 10.00);

            BuilderMgr.AGMgr.DoNotAllowSelectionOverlappingAgendaItems(false);
        }

        public void SetupMerchandisePage()
        {
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, "FEE-Fixed", 10.00, null, null);
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Fixed, "FEE-Fixed");
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Variable, "FEE-Variable", null, 0.01, 10000.00);
            BuilderMgr.VerifyMerchandiseItem(MerchandiseManager.MerchandiseType.Variable, "FEE-Variable");
        }

        public void SetupCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventCheckoutPage();
        }

        public void SetupConfirmationPage()
        {
            BuilderMgr.SetEventConfirmationPage();
            BuilderMgr.SaveAndStay();
            BuilderMgr.VerifyEventConfirmationPage();
        }

        public void CheckoutPage(RegisterManager.PaymentMethod paymentMethod, double totalAmount)
        {
            RegisterMgr.VerifyCheckoutTotal(totalAmount);
            RegisterMgr.VerifyCheckoutSubTotal(totalAmount);
            RegisterMgr.PayMoney(paymentMethod);
            RegisterMgr.FinishRegistration();
        }
    }
}
