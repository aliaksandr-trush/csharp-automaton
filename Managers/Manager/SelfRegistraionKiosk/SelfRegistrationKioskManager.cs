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
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(Locator_Name_Frame);
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
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.BarcodeScanningSearchByRegId, enableBarcodeSearchByRegId, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.RequireRegistrantsToBePaidInFull, paidInFull, LocateBy.Id);

            if (paidInFull)
            {
                if (allowCC)
                {
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator_SelfRegKioskConfig.AllowCCPayments, LocateBy.Id);
                }
                else
                {
                    this.EditPaidInFullMessage(paidInFullMessage);
                }
            }

            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.RequirePassword, requirePassword, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.AllowOnSiteRegistrations, allowOnSiteRegs, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.AllowUpdates, allowUpdates, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.AllowGroupCheckIn, allowGroupCheckIn, LocateBy.Id);
            WebDriverUtility.DefaultProvider.SetCheckbox(Locator_SelfRegKioskConfig.AllowPrintBadge, allowBadgePrinting, LocateBy.Id);

            if (allowBadgePrinting)
            {
                this.SelectFirstBadgeOption();
            }
        }

        private void EditPaidInFullMessage(string message)
        {
            RADContentEditorHelper radContentEditorHelper = new RADContentEditorHelper();
            string locator_Name_ContentEditorFrame = "dialog2";

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(Locator_SelfRegKioskConfig.DisplayMessage, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(EditPaidInFullMessageLocator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(locator_Name_ContentEditorFrame);
            radContentEditorHelper.SwitchMode(RADContentEditorHelper.Mode.HTML);
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            radContentEditorHelper.TypeHTMLTextarea(message);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(locator_Name_ContentEditorFrame);
            WebDriverUtility.DefaultProvider.SaveAndCloseContentEditorFrame();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            this.SelectThisFrame();
        }

        public void AddKioskWelcomeMessage(string message)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddWelcomeMessage, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.TypeContentEditorOnFrame(message);
            WebDriverUtility.DefaultProvider.SaveAndCloseContentEditorFrame();
        }

        public void AddKioskConfirmationMessage(string message)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddConfirmationMessage, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            WebDriverUtility.DefaultProvider.TypeContentEditorOnFrame(message);
            WebDriverUtility.DefaultProvider.SaveAndCloseContentEditorFrame();
        }

        public void EditKioskTimeOut(int timeout)
        {
            WebDriverUtility.DefaultProvider.Type(KioskTimeOutPeriod, timeout, LocateBy.Id);
        }

        public void ClickLaunchSelfRegistrationKiosk()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(LaunchKiosk, LocateBy.Id);
            Utility.ThreadSleep(5);
            WebDriverUtility.DefaultProvider.SelectTopWindow();
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            AllowCookies();
        }

        public void SaveAndCloseKioskConfig()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(SaveAndClose, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void CancelKioskConfig()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(CancelButton, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SelectFirstBadgeOption()
        {
            WebDriverUtility.DefaultProvider.SelectWithText(BadgeDropDown, WebDriverUtility.DefaultProvider.GetText("//option[2]", LocateBy.XPath), LocateBy.Id);
        }
    }
}
