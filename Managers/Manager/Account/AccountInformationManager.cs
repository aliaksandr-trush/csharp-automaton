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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void ClickSaveBillingInfo()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Step]
        public void ClickCancelBillingInfo()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(Locator_A_CancelBillingInfo_Id, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        [Verify]
        public void VerifyCanEditBillingInfo(bool canOrCannot)
        {
            UIUtilityProvider.UIHelper.VerifyElementEditable(
                "Billing first name",
                canOrCannot,
                UIUtilityProvider.UIHelper.IsEditable("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_textBillingFirstName", LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_SameAsPrimary(bool visible)
        {
            UIUtilityProvider.UIHelper.VerifyElementDisplay(
                "SameAsPrimary button", 
                visible,
                UIUtilityProvider.UIHelper.IsElementDisplay("ctl00_ctl00_cphDialog_cpMgrMain_uclMA_btnSame", LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_Save(bool visible)
        {
            UIUtilityProvider.UIHelper.VerifyElementDisplay(
                "Save button",
                visible,
                UIUtilityProvider.UIHelper.IsElementDisplay(Locator_A_EditOrSaveBillingInfo_Id, LocateBy.Id));
        }

        [Verify]
        public void VerifyButtonVisible_Cancel(bool visible)
        {
            UIUtilityProvider.UIHelper.VerifyElementDisplay(
                "Cancel button",
                visible,
                UIUtilityProvider.UIHelper.IsElementDisplay(Locator_A_CancelBillingInfo_Id, LocateBy.Id));
        }
    }
}