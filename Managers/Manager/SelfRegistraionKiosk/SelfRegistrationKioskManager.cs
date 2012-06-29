namespace RegOnline.RegressionTest.Managers.Manager.SelfRegistrationKiosk
{
    using System;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.UIUtility;

    public class SelfRegistrationKioskManager : ManagerBase
    {
        private const string Locator_Name_Frame = "plain";
        private const string AddWelcomeMessage = "ctl00_cphDialog_elHeader_linkCheckmarkOnsiteConfigHeaderText";
        private const string AddConfirmationMessage = "ctl00_cphDialog_elConfirmation_linkCheckmarkOnsiteConfigConfirmationText";
        private const string KioskTimeOutPeriod = "ctl00_cphDialog_txtTimeout_text";
        private const string LaunchKiosk = "ctl00_cphDialog_btnContinue";
        private const string SaveAndClose = "ctl00_cphDialog_btnSaveClose";
        private const string CancelButton = "ctl00_cphDialog_btnCancel";
        private const string BadgeDropDown = "ctl00_cphDialog_ddBadges";
        private const string KioskWindow = "_kiosk";
        private const string EditPaidInFullMessageLocator = "ctl00_cphDialog_elPaymentMessage_linkCheckmarkOnsiteConfigPaymentPageMsgText";

        private struct Locator_SelfRegKioskConfig
        {
            public const string BarcodeScanningSearchByRegId = "ctl00_cphDialog_chkSearchByRegId";
            public const string RequireRegistrantsToBePaidInFull = "ctl00_cphDialog_chkRequirePayment";
            public const string AllowCCPayments = "ctl00_cphDialog_rbAllowCCPayment";
            public const string DisplayMessage = "ctl00_cphDialog_rbAllowPaymentMessage";
            public const string RequirePassword = "ctl00_cphDialog_chkRequirePassword";
            public const string AllowOnSiteRegistrations = "ctl00_cphDialog_chkAllowOnsiteRegistrations";
            public const string AllowUpdates = "ctl00_cphDialog_chkUpdateProfile";
            public const string AllowGroupCheckIn = "ctl00_cphDialog_chkAllowGroupCheckin";
            public const string AllowPrintBadge = "ctl00_cphDialog_chkPrintBadge";
        }

        public void SelectThisFrame()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(Locator_Name_Frame);
        }

        public void ConfigureSelfRegistrationKioskOptions(
            bool enableBarcodeSearchByRegId, 
            bool paidInFull, 
            bool allowCC,
            string paidInFullMessage,
            bool requirePassword, 
            bool allowOnSiteRegs, 
            bool allowUpdates, 
            bool allowGroupCheckIn, 
            bool allowBadgePrinting)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.BarcodeScanningSearchByRegId, enableBarcodeSearchByRegId, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.RequireRegistrantsToBePaidInFull, paidInFull, LocateBy.Id);

            if (paidInFull)
            {
                if (allowCC)
                {
                    UIUtilityProvider.UIHelper.WaitForDisplayAndClick(Locator_SelfRegKioskConfig.AllowCCPayments, LocateBy.Id);
                }
                else
                {
                    this.EditPaidInFullMessage(paidInFullMessage);
                }
            }

            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.RequirePassword, requirePassword, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.AllowOnSiteRegistrations, allowOnSiteRegs, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.AllowUpdates, allowUpdates, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.AllowGroupCheckIn, allowGroupCheckIn, LocateBy.Id);
            UIUtilityProvider.UIHelper.SetCheckbox(Locator_SelfRegKioskConfig.AllowPrintBadge, allowBadgePrinting, LocateBy.Id);

            if (allowBadgePrinting)
            {
                this.SelectFirstBadgeOption();
            }
        }

        private void EditPaidInFullMessage(string message)
        {
            RADContentEditorHelper radContentEditorHelper = new RADContentEditorHelper();
            string locator_Name_ContentEditorFrame = "dialog2";

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(Locator_SelfRegKioskConfig.DisplayMessage, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(EditPaidInFullMessageLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(locator_Name_ContentEditorFrame);
            radContentEditorHelper.SwitchMode(RADContentEditorHelper.Mode.HTML);
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            radContentEditorHelper.TypeHTMLTextarea(message);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(locator_Name_ContentEditorFrame);
            UIUtilityProvider.UIHelper.SaveAndCloseContentEditorFrame();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            this.SelectThisFrame();
        }

        public void AddKioskWelcomeMessage(string message)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddWelcomeMessage, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.TypeContentEditorOnFrame(message);
            UIUtilityProvider.UIHelper.SaveAndCloseContentEditorFrame();
        }

        public void AddKioskConfirmationMessage(string message)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddConfirmationMessage, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.TypeContentEditorOnFrame(message);
            UIUtilityProvider.UIHelper.SaveAndCloseContentEditorFrame();
        }

        public void EditKioskTimeOut(int timeout)
        {
            UIUtilityProvider.UIHelper.Type(KioskTimeOutPeriod, timeout, LocateBy.Id);
        }

        public void ClickLaunchSelfRegistrationKiosk()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(LaunchKiosk, LocateBy.Id);
            Utility.ThreadSleep(5);
            UIUtilityProvider.UIHelper.SelectTopWindow();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            AllowCookies();
        }

        public void SaveAndCloseKioskConfig()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveAndClose, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void CancelKioskConfig()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CancelButton, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SelectFirstBadgeOption()
        {
            UIUtilityProvider.UIHelper.SelectWithText(BadgeDropDown, UIUtilityProvider.UIHelper.GetText("//option[2]", LocateBy.XPath), LocateBy.Id);
        }
    }
}
