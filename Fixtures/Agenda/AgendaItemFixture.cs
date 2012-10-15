namespace RegOnline.RegressionTest.Fixtures.Agenda
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Backend;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.Managers.Report;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class AgendaItemFixture : FixtureBase
    {
        #region Locators/Helpers
        int[] regId = new int[6];


        private const string EventName = "AgendaItemConditionalLogic";
        private const string EventNameWaitlist = "AgendaItemWaitlisting";
        private const string MiddleName = "T.";
        private const string LastName = "McTester";
        private const string OneLineText = "This is a triumph";
        private const string Checkbox = "Checkbox Waitlist";
        private const string AlwaysSelected = "Always Selected Waitlist";

        private readonly string AgendaReportValueLocator;
        private readonly string AgendaReportCountLocator;
        private readonly string AgendaWaitlistReportCountLocator;
        private readonly string AgendaWaitListDragAndDropLocator;

        private enum CustomField
        {
            [StringValue("Personal Info Checkbox")]
            PICheckbox,

            [StringValue("Personal Info Drop Down")]
            PIDropDown,

            [StringValue("Section Header")]
            AgendaSectionHeader,

            [StringValue("Parent Check Box")]
            AgendaParentCheckBox,

            [StringValue("Checkbox Conditional")]
            AgendaCheckboxConditional,

            [StringValue("1 line text conditional")]
            AgendaOneLineTextConditional,

            [StringValue("Always Selected")]
            AgendaAlwaysSelected,

            [StringValue("Radio Buttons")]
            AgendaRadioButtons,

            [StringValue("More Radio Buttons")]
            AgendaMoreRadioButtons,

            [StringValue("Disagree")]
            AgendaPredefinedDisagree,

            [StringValue("Yes")]
            AgendaPredefinedYes,

            [StringValue("No")]
            AgendaPredefinedNo
        }

        private enum ConfirmedRegs
        {
            oneThroughTwo,
            oneThroughThree,
            oneThroughTwoAndSix,
            twoThroughFour,
            twoAndFiveThroughSix,
            threeThroughFive,
            fourThroughSix
        }
         
        private string registrationID = string.Empty;
        private string sessionID;
        private int eventID;
        private Dictionary<CustomField, double?> agendaFee;
        private Dictionary<CustomField, string> agendaFormatedFee;
        private Dictionary<CustomField, int> cfIDs;

        #endregion

        public AgendaItemFixture()
        {
            this.AgendaReportValueLocator = "//div[contains(text(), '{0}')]/../../following-sibling::tr[1]//input[1]";

            this.AgendaReportCountLocator = "//tr[contains(@id, '_trItem')]//input[contains(@value, '~{0}')]/../..//a/b";

            this.AgendaWaitlistReportCountLocator = "//tr[contains(@id, '{0}')]//input[contains(@value, '~{0}')]/../..//a/b";
            this.AgendaWaitListDragAndDropLocator = "//tr[contains(@id, '{0}_{1}')]"; 

            this.agendaFee = new Dictionary<CustomField, double?>();
            this.agendaFee.Add(CustomField.AgendaParentCheckBox, null);
            this.agendaFee.Add(CustomField.AgendaCheckboxConditional, 10);
            this.agendaFee.Add(CustomField.AgendaOneLineTextConditional, null);
            this.agendaFee.Add(CustomField.AgendaAlwaysSelected, 15);
            this.agendaFee.Add(CustomField.AgendaRadioButtons, null);
            this.agendaFee.Add(CustomField.AgendaMoreRadioButtons, 10);

            this.cfIDs = new Dictionary<CustomField,int>();
        }

        ////[Test]
        ////[Category(Priority.Two)]
        ////[Description("419")]
        ////public void AgendaItemConditionalLogic()
        ////{
        ////    this.CreateEvent();
        ////    this.Register();
        ////}

        ////[Test]
        ////[Category(Priority.Two)]
        ////[Description("420")]
        ////public void AgendaItemWaitlisting()
        ////{
        ////    this.CreateWaitlistEvent();
        ////    this.RegisterWaitlistEvent(2);
        ////    this.ChangeWaitlistedRegistrations();
        ////}

        [Test]
        [Category(Priority.Two)]
        [Description("278")]
        public void AgendaItemWaitlistReportReorder()
        {
            this.DeleteTestRegistrations();
            this.UpdateWaitlistLimits(2);
            this.RegisterWaitlistEvent(2);
            this.ChangeWaitlistedRegistrationsReorder();
        }

        #region Event creation
        [Step]
        private void CreateEvent()
        {
            Login();

            // This event must be created on each test
            ManagerSiteMgr.DeleteEventByName(EventName);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetStartPage(EventName);
            BuilderMgr.Next();
            this.SetPersonalInfoPage();
            BuilderMgr.Next();
            this.SetAgendaPage();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();
            BuilderMgr.SaveAndClose();
            //UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        private void CreateWaitlistEvent()
        {
            Login();

            ManagerSiteMgr.DeleteEventByName(EventNameWaitlist);
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ProEvent);
            this.eventID = BuilderMgr.GetEventId();
            this.SetStartPage(EventNameWaitlist);
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            this.SetAgendaPageWaitlist();
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            this.SetCheckoutPage();
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private void DeleteTestRegistrations()
        {
            Login();

            this.eventID = ManagerSiteMgr.GetFirstEventId(EventNameWaitlist);
            ManagerSiteMgr.OpenEventDashboard(eventID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("plain");
            ManagerSiteMgr.DashboardMgr.DeleteTestReg_ClickDelete();
            UIUtil.DefaultProvider.SwitchToMainContent();
            ManagerSiteMgr.DashboardMgr.ReturnToManagerScreenEventList();
        }

        private void SetStartPage(string EventName)
        {
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);
            BuilderMgr.SetEventNameAndShortcut(EventName, Guid.NewGuid().ToString());
            BuilderMgr.SaveAndStay();
        }

        private void SetPersonalInfoPage()
        {
            BuilderMgr.AddPICustomField(
                CustomFieldManager.CustomFieldType.CheckBox, 
                StringEnum.GetStringValue(CustomField.PICheckbox));

            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(CustomFieldManager.CustomFieldType.CheckBox, StringEnum.GetStringValue(CustomField.PICheckbox));

            this.cfIDs.Add(CustomField.PICheckbox, BuilderMgr.GetCustomFieldID(StringEnum.GetStringValue(CustomField.PICheckbox)));
            
            BuilderMgr.AddPICustomField(
                CustomFieldManager.CustomFieldType.Dropdown, 
                StringEnum.GetStringValue(CustomField.PIDropDown));

            BuilderMgr.VerifyPersonalInfoCustomFieldInDatabase(
                CustomFieldManager.CustomFieldType.Dropdown, 
                StringEnum.GetStringValue(CustomField.PIDropDown));

            this.cfIDs.Add(CustomField.PIDropDown, BuilderMgr.GetCustomFieldID(StringEnum.GetStringValue(CustomField.PIDropDown)));

            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPage()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();
            
            BuilderMgr.AddAgendaItem(
                AgendaItemManager.AgendaItemType.SectionHeader, 
                StringEnum.GetStringValue(CustomField.AgendaSectionHeader));

            BuilderMgr.VerifyAgendaItemInDatabase(
                AgendaItemManager.AgendaItemType.SectionHeader, 
                StringEnum.GetStringValue(CustomField.AgendaSectionHeader));
            
            this.AddAgendaItemWithConditionalLogic(
                CustomField.AgendaParentCheckBox, 
                CustomField.PICheckbox,
                AgendaItemManager.AgendaItemType.CheckBox, 
                null);

            this.AddAgendaItemWithConditionalLogic(
                CustomField.AgendaCheckboxConditional, 
                CustomField.AgendaParentCheckBox,
                AgendaItemManager.AgendaItemType.CheckBox, 
                10.00);

            this.AddAgendaItemWithConditionalLogic(
                CustomField.AgendaOneLineTextConditional, 
                CustomField.AgendaCheckboxConditional,
                AgendaItemManager.AgendaItemType.OneLineText, 
                null);

            this.AddAgendaItemWithConditionalLogic(
                CustomField.AgendaAlwaysSelected, 
                CustomField.AgendaPredefinedDisagree,
                AgendaItemManager.AgendaItemType.AlwaysSelected, 
                15.00);

            this.AddAgendaItemWithConditionalLogic(
                CustomField.AgendaRadioButtons, 
                CustomField.PIDropDown,
                AgendaItemManager.AgendaItemType.RadioButton, 
                null);

            this.AddAgendaItemWithConditionalLogic(
                CustomField.AgendaMoreRadioButtons, 
                CustomField.AgendaPredefinedYes,
                AgendaItemManager.AgendaItemType.RadioButton, 
                10.00);

            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPageWaitlist()
        {
            BuilderMgr.VerifySplashPage();
            BuilderMgr.ClickYesOnSplashPage();
            BuilderMgr.VerifyEventAgendaPage();

            this.AddAgendaItemWithWaitlist(
                AgendaItemManager.AgendaItemType.CheckBox, 
                Checkbox, 
                2, 
                10.00);

            this.AddAgendaItemWithWaitlist(AgendaItemManager.AgendaItemType.AlwaysSelected, AlwaysSelected, 2, 10.00);
        }

        private void AddAgendaItemWithConditionalLogic(
            CustomField name, 
            CustomField conditionalParent,
            AgendaItemManager.AgendaItemType type, 
            double? standardPrice)
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(StringEnum.GetStringValue(name));
            BuilderMgr.AGMgr.SetTypeWithDefaults(type);
            BuilderMgr.AGMgr.SetConditionalLogic(true, StringEnum.GetStringValue(conditionalParent));

            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.RadioButton ||
                type == AgendaItemManager.AgendaItemType.Dropdown ||
                type == AgendaItemManager.AgendaItemType.FileUpload ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected)
            {
                BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(standardPrice);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
            this.cfIDs.Add(name, BuilderMgr.AGMgr.GetAgendaItemID(StringEnum.GetStringValue(name)));
        }

        public void AddAgendaItemWithWaitlist(
            AgendaItemManager.AgendaItemType type, 
            string name, 
            int limit, 
            double price)
        {
            BuilderMgr.ClickAddAgendaItem();
            BuilderMgr.AGMgr.SetName(name);
            BuilderMgr.AGMgr.SetTypeWithDefaults(type);
            BuilderMgr.AGMgr.FeeMgr.SetStandardPrice(price);

            // sets waitlist if one of the types below(waitlist only available on these)
            if (type == AgendaItemManager.AgendaItemType.CheckBox ||
                type == AgendaItemManager.AgendaItemType.AlwaysSelected ||
                type == AgendaItemManager.AgendaItemType.FileUpload)
            {
                BuilderMgr.AGMgr.SetWaitlist(limit);
            }

            BuilderMgr.AGMgr.ClickSaveItem();
        }

        private void SetCheckoutPage()
        {
            BuilderMgr.EnterEventCheckoutPage();
            BuilderMgr.SaveAndStay();
        }
        #endregion

        #region Registration
        [Step]
        private void Register()
        {
            RegisterMgr.CurrentEventId = this.eventID;

            this.InitializeFormatedFee();

            // Register first registration
            this.FirstRegistration();
            int regID1 = RegisterMgr.GetRegID();

            // Register second registration
            this.SecondRegistration();
            int regID2 = RegisterMgr.GetRegID();

            // Register third registration
            this.ThirdRegistration();
            int regID3 = RegisterMgr.GetRegID();
        }

        private void FirstRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            this.EnterPersonalInfo("Test", MiddleName, LastName);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CustomField.PICheckbox), true);

            RegisterMgr.SelectCustomFieldDropDown(
                StringEnum.GetStringValue(CustomField.PIDropDown), 
                StringEnum.GetStringValue(CustomField.AgendaPredefinedDisagree));

            RegisterMgr.Continue();
            Assert.True(UIUtil.DefaultProvider.IsTextPresent(StringEnum.GetStringValue(CustomField.AgendaSectionHeader))); 
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaParentCheckBox), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaAlwaysSelected), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaRadioButtons), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaPredefinedYes), true);
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaPredefinedNo), true);

            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CustomField.AgendaParentCheckBox), true);

            RegisterMgr.WaitForConditionalLogic();
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaCheckboxConditional), true);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CustomField.AgendaCheckboxConditional), true);

            RegisterMgr.WaitForConditionalLogic();
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaOneLineTextConditional), true);

            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(
                StringEnum.GetStringValue(CustomField.AgendaOneLineTextConditional),
                OneLineText);
            
            RegisterMgr.SelectCustomFieldRadioButtons(
                StringEnum.GetStringValue(CustomField.AgendaRadioButtons),
                StringEnum.GetStringValue(CustomField.AgendaPredefinedYes));

            RegisterMgr.WaitForConditionalLogic();
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaMoreRadioButtons), true);

            RegisterMgr.SelectCustomFieldRadioButtons(
                StringEnum.GetStringValue(CustomField.AgendaMoreRadioButtons),
                StringEnum.GetStringValue(CustomField.AgendaPredefinedNo));

            RegisterMgr.Continue();
            
            double totalFee = 0;

            foreach (KeyValuePair<CustomField, double?> fee in this.agendaFee)
            {
                if (fee.Value.HasValue)
                {
                    totalFee += fee.Value.Value;
                }
            }

            RegisterMgr.PayMoneyAndVerify(totalFee, totalFee, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void SecondRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            this.EnterPersonalInfo("SecondTest", MiddleName, LastName);
            RegisterMgr.Continue();
            Assert.True(UIUtil.DefaultProvider.IsTextPresent(StringEnum.GetStringValue(CustomField.AgendaSectionHeader)));
            RegisterMgr.Continue();
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void ThirdRegistration()
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            this.EnterPersonalInfo("ThirdTest", MiddleName, LastName);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CustomField.PICheckbox), true);
            RegisterMgr.Continue();
            Assert.True(UIUtil.DefaultProvider.IsTextPresent(StringEnum.GetStringValue(CustomField.AgendaSectionHeader)));
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaParentCheckBox), true);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CustomField.AgendaParentCheckBox), true);

            RegisterMgr.WaitForConditionalLogic();
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaCheckboxConditional), true);
            RegisterMgr.SetCustomFieldCheckBox(StringEnum.GetStringValue(CustomField.AgendaCheckboxConditional), true);

            RegisterMgr.WaitForConditionalLogic();
            RegisterMgr.VerifyAgendaItem(StringEnum.GetStringValue(CustomField.AgendaOneLineTextConditional), true);
            
            RegisterMgr.FillOutCustomOneLineTextOrNumberOrContribution(
                StringEnum.GetStringValue(CustomField.AgendaOneLineTextConditional),
                OneLineText);

            RegisterMgr.Continue();
            double totalFee = this.agendaFee[CustomField.AgendaCheckboxConditional].Value;
            RegisterMgr.PayMoneyAndVerify(totalFee, totalFee, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        [Step]
        private void RegisterWaitlistEvent(int limit)
        {
            RegisterNotWaitlisted("First");
            regId[0] = RegisterMgr.GetRegID();
            RegisterNotWaitlisted("Second");
            regId[1] = RegisterMgr.GetRegID();
            if (limit == 2)
            {
                RegisterWaitlisted("Third");
                regId[2] = RegisterMgr.GetRegID();
            }
            if (limit == 3)
            {
                RegisterNotWaitlisted("Third");
                regId[2] = RegisterMgr.GetRegID();
            }
            RegisterWaitlisted("Fourth");
            regId[3] = RegisterMgr.GetRegID();
            RegisterWaitlisted("Fifth");
            regId[4] = RegisterMgr.GetRegID();
            RegisterWaitlisted("Sixth");
            regId[5] = RegisterMgr.GetRegID();
        }

        private void RegisterNotWaitlisted(string firstName)
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            EnterPersonalInfo(firstName, MiddleName, LastName);
            RegisterMgr.Continue();
            RegisterMgr.SetCustomFieldCheckBox(Checkbox, true);
            RegisterMgr.Continue();
            RegisterMgr.PayMoneyAndVerify(20.00, RegOnline.RegressionTest.Managers.Register.RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void RegisterWaitlisted(string firstName)
        {
            RegisterMgr.OpenRegisterPage(this.eventID);
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            EnterPersonalInfo(firstName, MiddleName, LastName);
            RegisterMgr.Continue();
            RegisterMgr.SetCustomFieldCheckBox(Checkbox, true);
            List<string> CustomFieldNames = RegisterMgr.GetCustomFieldNames();

            for (int i = 0; i < CustomFieldNames.Count; i++)
            {
                string id = string.Format("//label[text()='{0}']", CustomFieldNames[i]);

                VerifyTool.VerifyValue(
                    "This item is full. To add yourself to the waitlist, select the item.",
                    UIUtil.DefaultProvider.GetText(id + "/../..//span[@class='wlist']", LocateBy.XPath), 
                    "Error Message: {0}");

            }

            RegisterMgr.ClickRecalculateTotal();
            RegisterMgr.VerifyAgendaPageTotalAmount(0.00);
            RegisterMgr.Continue();

            for (int i = 0; i < CustomFieldNames.Count; i++)
            {
                RegisterMgr.VerifyAgendaItemWaitlistMessage(CustomFieldNames[0]);
            }

            RegisterMgr.VerifyPaymentWaitlistMessage();
            RegisterMgr.VerifyCheckoutTotal(0.00);
            RegisterMgr.PayMoney(RegOnline.RegressionTest.Managers.Register.RegisterManager.PaymentMethod.CreditCard);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private void EnterPersonalInfo(string firstName, string middleName, string lastName)
        {
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, firstName, middleName, lastName, null);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry("QA Tester", null, "RegOnline", null);
            RegisterMgr.EnterPersonalInfoAddress("4750 Walnut st", null, "Boulder", "Colorado", null, "80301");
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);
            RegisterMgr.EnterPersonalInfoPassword();
        }

        private void InitializeFormatedFee()
        {
            this.agendaFormatedFee = new Dictionary<CustomField, string>();

            foreach (KeyValuePair<CustomField, double?> fee in this.agendaFee)
            {
                string formatedFee = string.Empty;

                if (fee.Value.HasValue)
                {
                    formatedFee = MoneyTool.FormatMoney(fee.Value.Value);
                }

                this.agendaFormatedFee.Add(fee.Key, formatedFee);
            }
        }
        #endregion

        #region Waitlist Changes
        [Step]
        private void ChangeWaitlistedRegistrations()
        {
            Login();
            ManagerSiteMgr.OpenEventDashboard(this.eventID);
            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.oneThroughTwo);
            CloseAgendaReport();

            UpdateWaitlistLimits(3);

            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.oneThroughThree);
            CloseAgendaReport();
            VerifyAttendeeHasFeesApplied(regId[2]);

            ClearConfirmedRegistrantsAgendaSelections();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.twoThroughFour);
            CloseAgendaReport();
            VerifyAttendeeHasFeesApplied(regId[3]);

            CancelAttendee();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.threeThroughFive);
            CloseAgendaReport();
            VerifyAttendeeHasFeesApplied(regId[4]);
        }

        [Step]
        private void ChangeWaitlistedRegistrationsReorder()
        {
            Login();
            ManagerSiteMgr.OpenEventDashboard(this.eventID);
            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.oneThroughTwo);
            MoveAttendeeToTopOfAgendaWaitlist(regId[5].ToString(), regId[2].ToString(), Checkbox + " Waitlist", 2);
            MoveAttendeeToTopOfAgendaWaitlist(regId[5].ToString(), regId[2].ToString(), AlwaysSelected + " Waitlist", 2);
            CloseAgendaReport();


            UpdateWaitlistLimits(3);

            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.oneThroughTwoAndSix);
            MoveAttendeeToTopOfAgendaWaitlist(regId[4].ToString(), regId[2].ToString(), Checkbox + " Waitlist", 2);
            MoveAttendeeToTopOfAgendaWaitlist(regId[4].ToString(), regId[2].ToString(), AlwaysSelected + " Waitlist", 2);
            VerifyAttendeeHasFeesApplied(regId[5]);

            ClearConfirmedRegistrantsAgendaSelections();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.twoAndFiveThroughSix);
            MoveAttendeeToTopOfAgendaWaitlist(regId[3].ToString(), regId[2].ToString(), Checkbox + " Waitlist", 2);
            MoveAttendeeToTopOfAgendaWaitlist(regId[3].ToString(), regId[2].ToString(), AlwaysSelected + " Waitlist", 2);
            VerifyAttendeeHasFeesApplied(regId[4]);

            CancelAttendee();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            OpenReportsTabAndAgendaReport();
            VerifyAgendaReport(ConfirmedRegs.fourThroughSix);
            VerifyAttendeeHasFeesApplied(regId[3]);
        }

        private void VerifyRegIdsOnAgendaReportByItem(string agendaItemName, bool regId1, bool regId2, bool regId3, bool regId4, bool regId5, bool regId6)
        {
            if (regId1 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItem(regId[0].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItem(regId[0].ToString(), agendaItemName));
            }
            if (regId2 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItem(regId[1].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItem(regId[1].ToString(), agendaItemName));
            }
            if (regId3 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItem(regId[2].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItem(regId[2].ToString(), agendaItemName));
            }
            if (regId4 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItem(regId[3].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItem(regId[3].ToString(), agendaItemName));
            }
            if (regId5 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItem(regId[4].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItem(regId[4].ToString(), agendaItemName));
            }
            if (regId6 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItem(regId[5].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItem(regId[5].ToString(), agendaItemName));
            }
        }

        private void VerifyRegIdsOnAgendaReportByItemWaitlist(string agendaItemName, bool regId1, bool regId2, bool regId3, bool regId4, bool regId5, bool regId6)
        {
            if (regId1 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[0].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[0].ToString(), agendaItemName));
            }
            if (regId2 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[1].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[1].ToString(), agendaItemName));
            }
            if (regId3 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[2].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[2].ToString(), agendaItemName));
            }
            if (regId4 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[3].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[3].ToString(), agendaItemName));
            }
            if (regId5 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[4].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[4].ToString(), agendaItemName));
            }
            if (regId6 == true)
            {
                Assert.True(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[5].ToString(), agendaItemName));
            }
            else
            {
                Assert.False(VerifyRegIdPresentUnderAgendaItemWaitlist(regId[5].ToString(), agendaItemName));
            }
        }

        private bool VerifyRegIdPresentUnderAgendaItem(string regId, string agendaItemName)
        {
            string locator_XPath = string.Format(AgendaReportValueLocator, agendaItemName);

            locator_XPath = UIUtil.DefaultProvider.GetAttribute(locator_XPath, "value", LocateBy.XPath);

            string[] valueLocator = locator_XPath.Split(new char[] { '~' });
            locator_XPath = string.Format(AgendaReportCountLocator, valueLocator[1]);
            int xPathCount = UIUtil.DefaultProvider.GetXPathCountByXPath(locator_XPath);
            string[] results = new string[xPathCount];

            for (int i = 0; i < xPathCount; i++)
            {
                results[i] = UIUtil.DefaultProvider.GetText(string.Format("//*[contains(text(), '{0} (')]/../../following-sibling::tr[" + (i + 1) + "]//a/b", agendaItemName), LocateBy.XPath);
                if (results[i] == regId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool VerifyRegIdPresentUnderAgendaItemWaitlist(string regId, string agendaItemName)
        {
            string locator_XPath = string.Format(AgendaReportValueLocator, agendaItemName);
            locator_XPath = UIUtil.DefaultProvider.GetAttribute(locator_XPath, "value", LocateBy.XPath);
            string[] valueLocator = locator_XPath.Split(new char[] { '~' });
            locator_XPath = string.Format(AgendaWaitlistReportCountLocator, valueLocator[1]);
            int xPathCount = UIUtil.DefaultProvider.GetXPathCountByXPath(locator_XPath);
            string[] results = new string[xPathCount];

            for (int i = 0; i < xPathCount; i++)
            {
                results[i] = UIUtil.DefaultProvider.GetText(string.Format("//*[contains(text(), '{0}')]/../../following-sibling::tr[" + (i + 1) + "]//a/b", agendaItemName), LocateBy.XPath);
                
                if (results[i] == regId)
                {
                    return true;
                }
            }
            return false;
        }

        private void MoveAttendeeToTopOfAgendaWaitlist(string regId, string regId2, string agendaItemName, int offsetMove)
        {
            string locator_XPath = string.Format(AgendaReportValueLocator, agendaItemName);
            locator_XPath = UIUtil.DefaultProvider.GetAttribute(locator_XPath, "value", LocateBy.XPath);
            string[] valueLocator = locator_XPath.Split(new char[] { '~' });
            locator_XPath = string.Format(AgendaWaitlistReportCountLocator, valueLocator[1]);
            int xPathCount = UIUtil.DefaultProvider.GetXPathCountByXPath(locator_XPath);
            string startPoint = string.Format(AgendaWaitListDragAndDropLocator, valueLocator[1], regId/*, offsetMove*/);
            string endPoint = string.Format(AgendaWaitListDragAndDropLocator, valueLocator[1], regId2/*, offsetMove*/);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(startPoint, LocateBy.XPath);
            UIUtil.DefaultProvider.DragAndDrop(startPoint, endPoint);
        }

        private void VerifyAgendaReport(ConfirmedRegs regs)
        {
            switch (regs)
            {
                case ConfirmedRegs.oneThroughTwo:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, true, true, false, false, false, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, true, true, true, true);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, true, true, false, false, false, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, true, true, true, true);
                    break;
                case ConfirmedRegs.oneThroughThree:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, true, true, true, false, false, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, false, true, true, true);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, true, true, true, false, false, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, false, true, true, true);
                    break;
                case ConfirmedRegs.oneThroughTwoAndSix:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, true, true, false, false, false, true);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, true, true, true, false);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, true, true, false, false, false, true);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, true, true, true, false);
                    break;
                case ConfirmedRegs.twoThroughFour:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, false, true, true, true, false, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, false, false, true, true);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, false, true, true, true, false, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, false, false, true, true);
                    break;
                case ConfirmedRegs.twoAndFiveThroughSix:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, false, true, false, false, true, true);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, true, true, false, false);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, false, true, false, false, true, true);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, true, true, false, false);
                    break;
                case ConfirmedRegs.threeThroughFive:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, false, false, true, true, true, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, false, false, false, true);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, false, false, true, true, true, false);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, false, false, false, true);
                    break;
                case ConfirmedRegs.fourThroughSix:
                    VerifyRegIdsOnAgendaReportByItem(Checkbox, false, false, false, true, true, true);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(Checkbox + " Waitlist", false, false, true, false, false, false);
                    VerifyRegIdsOnAgendaReportByItem(AlwaysSelected, false, false, false, true, true, true);
                    VerifyRegIdsOnAgendaReportByItemWaitlist(AlwaysSelected + " Waitlist", false, false, true, false, false, false);
                    break;
            }
        }

        private void VerifyAttendeeHasFeesApplied(int regId)
        {
            BackendMgr.OpenAttendeeInfoURL(this.sessionID, regId);
            BackendMgr.VerifyTotalCharges("$20.00");
        }

        [Step]
        private void UpdateWaitlistLimits(int newLimit)
        {
            ManagerSiteMgr.OpenEventBuilderStartPage(eventID, sessionID);
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            BuilderMgr.AGMgr.OpenAgendaByName(Checkbox);
            BuilderMgr.AGMgr.UpdateLimit(newLimit);
            BuilderMgr.AGMgr.ClickSaveItem();
            BuilderMgr.AGMgr.OpenAgendaByName(AlwaysSelected);
            BuilderMgr.AGMgr.UpdateLimit(newLimit);
            BuilderMgr.AGMgr.ClickSaveItem();
            BuilderMgr.SaveAndClose();
        }

        private void ClearConfirmedRegistrantsAgendaSelections()
        {
            BackendMgr.OpenAttendeeInfoURL(this.sessionID, regId[0]);
            BackendMgr.OpenEditAttendeeSubPage(BackendManager.AttendeeSubPage.Agenda);
            BackendMgr.SetCheckboxCFItem(Checkbox, false);
            BackendMgr.SetCheckboxCFItem(AlwaysSelected, false);
            BackendMgr.SaveAndCloseEditCF();
            BackendMgr.SaveAndBypassTransaction();
        }

        private void CancelAttendee()
        {
            BackendMgr.OpenAttendeeInfoURL(this.sessionID, regId[1]);
            BackendMgr.CancelRegistrationAndVerify();
        }

        private void OpenReportsTabAndAgendaReport()
        {
            ManagerSiteMgr.DashboardMgr.ChooseTab(DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.OpenCommonReport(ReportManager.CommonReportType.AgendaSelections);
        }

        private void CloseAgendaReport()
        {
            UIUtil.DefaultProvider.CloseWindow();
            UIUtil.DefaultProvider.SelectOriginalWindow();
        }
        #endregion

        public void Login()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder_DefaultForCurrentAccount();
            this.sessionID = ManagerSiteMgr.GetEventSessionId();
        }
    }
}

