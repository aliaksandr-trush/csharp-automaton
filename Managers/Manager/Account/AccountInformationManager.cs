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
            UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void ClickSaveBillingInfo()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void ClickCancelBillingInfo()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator_A_CancelBillingInfo_Id, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        [Verify]
        public void VerifyCanEditBillingInfo(bool canOrCannot)
        {
            UIUtil.DefaultProvider.VerifyElementEditable(
                "Billing first name",
                canOrCannot,
                UIUtil.DefaultProvider.IsEditable("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_textBillingFirstName", LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_SameAsPrimary(bool visible)
        {
            UIUtil.DefaultProvider.VerifyElementDisplay(
                "SameAsPrimary button", 
                visible,
                UIUtil.DefaultProvider.IsElementDisplay("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_btnSame", LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_Save(bool visible)
        {
            UIUtil.DefaultProvider.VerifyElementDisplay(
                "Save button",
                visible,
                UIUtil.DefaultProvider.IsElementDisplay(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_Cancel(bool visible)
        {
            UIUtil.DefaultProvider.VerifyElementDisplay(
                "Cancel button",
                visible,
                UIUtil.DefaultProvider.IsElementDisplay(Locator_A_CancelBillingInfo_Id, LocateBy.Id));
        }
    }
}