namespace RegOnline.RegressionTest.Fixtures.FeeOptions
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class TaxesFixture : FixtureBase
    {
        private const string AgendaItemName = "TaxesTest-AI";
        private const double AgendaPrice = 100;
        private const string MerchandiseItemName = "TaxesTestMerchandise";
        private const double MerchandisePrice = 100;
        private const string TaxTitle = "TaxTest";
        private const string TaxOneCaption = "Tax1";
        private const string TaxTwoCaption = "Tax2";
        private const double TaxOnePercentage = 10;
        private const double TaxTwoPercentage = 20;
        private const int MerchQuantity = 1;
        private const double SubTotal = AgendaPrice + MerchandisePrice * MerchQuantity;

        private int eventID = ManagerBase.InvalidId;
        private string sessionID = ManagerBase.InvalidSessionId;
        private bool withTax;
        private bool? applyTaxOne;
        private bool? applyTaxTwo;
        private string applyTaxCountry;
        
        #region Test Methods
        [Test]
        [Category(Priority.Two)]
        [Description("527")]
        public void TaxVerifyEUCountry()
        {
            this.withTax = false;
            this.applyTaxOne = null;
            this.applyTaxTwo = null;
            this.applyTaxCountry = string.Empty;
            this.CreateNewEvent("TaxOnlyAppliedToEUCountry");
            BuilderMgr.TaxMgr.SetAndVerifyEUCountry(true, this.sessionID, this.eventID);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("522")]
        public void RegWithNoTax()
        {
            this.withTax = false;
            this.applyTaxOne = null;
            this.applyTaxTwo = null;
            this.applyTaxCountry = string.Empty;
            this.CreateNewEvent("RegWithNoTax");
            this.TestRegistration(string.Empty);
            this.VerifyTestResult(" ", " ", " ", " ", false);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("523")]
        public void RegWithTaxOne()
        {
            this.withTax = true;
            this.applyTaxOne = true;
            this.applyTaxTwo = false;
            this.applyTaxCountry = string.Empty;
            this.CreateNewEvent("RegWithTaxOne");
            this.TestRegistration(string.Empty);
            this.VerifyTestResult("$10.00", "$0.00", "$10.00", "$0.00", false);           
        }

        [Test]
        [Category(Priority.Two)]
        [Description("524")]
        public void RegWithTaxTwo()
        {
            this.withTax = true;
            this.applyTaxOne = false;
            this.applyTaxTwo = true;
            this.applyTaxCountry = string.Empty;
            this.CreateNewEvent("RegWithTaxTwo");
            this.TestRegistration(string.Empty);
            this.VerifyTestResult("$0.00", "$20.00", "$0.00", "$20.00", false);
        }

        [Test]
        [Category(Priority.Two)]
        [Description("525")]
        public void RegWithTaxOneandTaxTwo()
        {
            this.withTax = true;
            this.applyTaxOne = true;
            this.applyTaxTwo = true;
            this.applyTaxCountry = string.Empty;
            this.CreateNewEvent("RegWithTaxOneAndTaxTwo");
            this.TestRegistration(string.Empty);
            this.VerifyTestResult("$10.00", "$20.00", "$10.00", "$20.00", false);           
        }

        [Test]
        [Category(Priority.Two)]
        [Description("526")]
        public void TaxOnlyInOneCountry()
        {
            this.withTax = true;
            this.applyTaxOne = true;
            this.applyTaxTwo = true;
            this.applyTaxCountry = "Australia";
            this.CreateNewEvent("TaxOnlyAppliedToOneCountry");
            ManagerSiteMgr.OpenEventBuilderStartPage(this.eventID, this.sessionID);
            BuilderMgr.GotoPage(FormDetailManager.Page.Checkout);
            BuilderMgr.TaxMgr.SetApplyTaxToCountriesCheckbox(true);
            BuilderMgr.TaxMgr.SelectCountry(true, "Australia");
            BuilderMgr.SaveAndClose();

            this.TestRegistration("United States");
            this.TestRegistration("Australia");
            this.VerifyTestResult("$10.00", "$20.00", "$10.00", "$20.00", true);
        }
        #endregion

        #region Event builder
        [Step]
        private void CreateNewEvent(string eventName)
        {
            ManagerSiteMgr.OpenLogin();

            // login to regression testing account
            ManagerSiteMgr.Login();

            // make sure we're on the Events tab
            ManagerSiteMgr.GoToEventsTabIfNeeded();

            // click "Regression" folder
            ManagerSiteMgr.SelectFolder();

            // always create new event for now
            ManagerSiteMgr.DeleteEventByName(eventName);

            // select "Pro Event" from drop down
            ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);

            //get sessionId
            this.sessionID = ManagerSiteMgr.GetEventSessionId();

            // set start page
            this.SetEventStartPage(eventName);

            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);

            // set agenda page
            this.SetAgendaPage();

            BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);

            // set merchandise page
            this.SetMerchandisePage();

            // go to next page
            BuilderMgr.Next();

            // set checkout page
            this.SetCheckoutPage();

            // go to next page
            BuilderMgr.Next();

            // set confirmation page
            this.SetConfirmationPage();

            // get event id
            this.eventID = BuilderMgr.GetEventId();

            if (this.withTax)
            {
                this.ApplyTax();
            }

            // save and close
            BuilderMgr.SaveAndClose();
        }

        private void SetEventStartPage(string eventname)
        {
            // verify initial defaults
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.ProEvent);

            // enter event start page info
            BuilderMgr.SetStartPage(ManagerSiteManager.EventType.ProEvent, eventname);

            // save and stay
            BuilderMgr.SaveAndStay();
        }

        private void SetAgendaPage()
        {
            // verify splash page
            BuilderMgr.VerifySplashPage();

            // continue to agenda page
            BuilderMgr.ClickYesOnSplashPage();

            // verify agenda page
            BuilderMgr.VerifyEventAgendaPage();

            // add/modify agenda items of each type
            BuilderMgr.AddAgendaItemWithPriceAndNoDate(
                AgendaItemManager.AgendaItemType.CheckBox, 
                AgendaItemName, 
                AgendaPrice);
        }

        private void SetMerchandisePage()
        {
            BuilderMgr.AddMerchandiseItemWithFeeAmount(MerchandiseManager.MerchandiseType.Fixed, MerchandiseItemName, MerchandisePrice, null, null);
        }

        private void SetCheckoutPage()
        {
            // enter event checkout page info
            BuilderMgr.EnterEventCheckoutPage();

            // save and stay
            BuilderMgr.SaveAndStay();

            // add taxes
            BuilderMgr.TaxMgr.SetTaxRateOptions(TaxTitle, TaxOneCaption, TaxOnePercentage, TaxTwoCaption, TaxTwoPercentage);

            // save and stay
            BuilderMgr.SaveAndStay();
        }

        private void SetConfirmationPage()
        {
            // enter event checkout page info
            BuilderMgr.SetEventConfirmationPage();

            // save and stay
            BuilderMgr.SaveAndStay();

            // verify event checkout page info
            BuilderMgr.VerifyEventConfirmationPage();
        }
         
        private void ApplyTax()
        {
            // Apply tax rates to agenda
            BuilderMgr.GotoPage(FormDetailManager.Page.Agenda);
            BuilderMgr.AGMgr.OpenAgendaByName(AgendaItemName);
            BuilderMgr.AGMgr.FeeMgr.ExpandOption();
            BuilderMgr.AGMgr.FeeMgr.Tax.ApplyTaxRatesToFee(this.applyTaxOne.Value, this.applyTaxTwo.Value);
            BuilderMgr.AGMgr.ClickSaveItem();

            // Apply tax rates to merchandise
            BuilderMgr.GotoPage(FormDetailManager.Page.Merchandise);
            BuilderMgr.OpenMerchandiseItem(MerchandiseItemName);
            BuilderMgr.MerchMgr.ExpandAdvanced();
            BuilderMgr.MerchMgr.TaxMgr.ApplyTaxRatesToFee(this.applyTaxOne.Value, this.applyTaxTwo.Value);
            BuilderMgr.MerchMgr.SaveAndClose();
            BuilderMgr.SaveAndStay();
        }
        #endregion

        #region Registration and verify
        [Step]
        private void TestRegistration(string countrySelection)
        {
            RegisterMgr.CurrentEventId = this.eventID;
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfo();
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry(null, null, null, countrySelection);
            RegisterMgr.Continue();
            RegisterMgr.SelectAgendaItems();
            RegisterMgr.Continue();
            RegisterMgr.SelectMerchandise(MerchQuantity);
            RegisterMgr.Continue();
            RegisterMgr.PayMoneyAndVerify(this.GetTotalFee(countrySelection), SubTotal, RegisterManager.PaymentMethod.Check);
            RegisterMgr.FinishRegistration();
            RegisterMgr.ConfirmRegistration();
        }

        private double GetTotalFee(string countrySelection)
        {
            double total = SubTotal;

            if (this.withTax && this.applyTaxCountry == countrySelection)
            {
                if (this.applyTaxOne.Value)
                {
                    total += SubTotal * TaxOnePercentage / 100;
                }

                if (this.applyTaxTwo.Value)
                {
                    total += SubTotal * TaxTwoPercentage / 100;
                }
            }

            return total;
        }

        [Verify]
        private void VerifyTestResult(string expectedVal1, string expectedVal2, string expectedVal3, string expectedVal4, bool isTaxonlycountry)
        {
            string reportName = "taxtest";
            this.LoginAndGetSessionID();
            
            // Create custom report
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventID, this.sessionID);
            ManagerSiteMgr.DashboardMgr.ChooseTab(Managers.Manager.Dashboard.DashboardManager.DashboardTab.Reports);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.OpenCustomReportCreator();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetName(reportName);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseTab(CustomReportCreator.Tab.Fields);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseFieldsCategory(CustomReportCreator.FieldsCategory.EventFeeAndAgenda);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.MoveAllItemsToCurrentChoices();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SetShownColumn(CustomReportCreator.EventFeeAndAgendaFieldsColumn.Taxes, true);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.ChooseFieldsCategory(CustomReportCreator.FieldsCategory.Merchandise);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.MoveAllItemsToCurrentChoices();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Apply();
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.Cancel();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_ctl00_cphDialog_cpMgrMain_grdCustomReports_ctl02_hlFormatLink", LocateBy.Id);
            ManagerSiteMgr.DashboardMgr.CustomReportCreationMgr.SelectReportPopupWindow();

            if (isTaxonlycountry)
            {
                ReportMgr.VerifyReportValue(1, 4, " ");
                ReportMgr.VerifyReportValue(1, 5, " ");
                ReportMgr.VerifyReportValue(1, 8, " ");
                ReportMgr.VerifyReportValue(1, 9, " ");

                ReportMgr.VerifyReportValue(2, 4, expectedVal1);
                ReportMgr.VerifyReportValue(2, 5, expectedVal2);
                ReportMgr.VerifyReportValue(2, 8, expectedVal3);
                ReportMgr.VerifyReportValue(2, 9, expectedVal4);
            }
            else
            {
                ReportMgr.VerifyReportValue(1, 4, expectedVal1);
                ReportMgr.VerifyReportValue(1, 5, expectedVal2);
                ReportMgr.VerifyReportValue(1, 8, expectedVal3);
                ReportMgr.VerifyReportValue(1, 9, expectedVal4);
            }
            ReportMgr.CloseReportPopupWindow();
        }
        #endregion

        #region Helper
        private void LoginAndGetSessionID()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.sessionID = ManagerSiteMgr.GetEventSessionId();
        }
        #endregion
    }
}
