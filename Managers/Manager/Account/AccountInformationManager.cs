namespace RegOnline.RegressionTest.Managers.Manager.Account
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class AccountInformationManager
    {
        private const string Locator_A_EditOrSaveBillingInfo_Id = "ctl00_ctl00_cphDialog_cpMgrMain_uclMA_btnChangeBilling";
        private const string Locator_A_CancelBillingInfo_Id = "ctl00_ctl00_cphDialog_cpMgrMain_uclMA_btnCancelBilling";

        [Step]
        public void ClickEditBillingInfo()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void ClickSaveBillingInfo()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void ClickCancelBillingInfo()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator_A_CancelBillingInfo_Id, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        [Verify]
        public void VerifyCanEditBillingInfo(bool canOrCannot)
        {
            WebDriverUtility.DefaultProvider.VerifyElementEditable(
                "Billing first name",
                canOrCannot,
                WebDriverUtility.DefaultProvider.IsEditable("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_textBillingFirstName", LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_SameAsPrimary(bool visible)
        {
            WebDriverUtility.DefaultProvider.VerifyElementDisplay(
                "SameAsPrimary button", 
                visible,
                WebDriverUtility.DefaultProvider.IsElementDisplay("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_btnSame", LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_Save(bool visible)
        {
            WebDriverUtility.DefaultProvider.VerifyElementDisplay(
                "Save button",
                visible,
                WebDriverUtility.DefaultProvider.IsElementDisplay(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_Cancel(bool visible)
        {
            WebDriverUtility.DefaultProvider.VerifyElementDisplay(
                "Cancel button",
                visible,
                WebDriverUtility.DefaultProvider.IsElementDisplay(Locator_A_CancelBillingInfo_Id, LocateBy.Id));
        }
    }
}