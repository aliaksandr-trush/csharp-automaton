namespace RegOnline.RegressionTest.Managers.Manager.Dashboard
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class ActivateEventManager : ManagerBase
    {
        private const string PricingOptionId = "rblEventsPricingOptionId_{0}";
        private const string ActivateButton = "//span[text()='Activate']/parent::a";
        private const string RemoveTestRegistrationsLocator = "chkDeleteTestRegs";
        private const string AutomaticallyChangeStatusLocator = "chkChangeStatus";
        private const string ChangeStatusDateLocator = "dtpStatusChangeDate_dateInput_text";
        private const string ChangeStatusTimeLocator = "dtpStatusChangeTime_dateInput_text";
        private const string AutomaticallyChangeStatusMessageLocator = "elClosingStatusMessage_linkCheckmarkfrmEventTextsClosingStatusMessage";
        private const string StatusLocator = "ddlStatuses";
        private const string InitialingLocator = "txtInitials";
        private const string AgreeTOSLocator = "chkAuthorizedSigner";
        private const string CommentUnderBankCountryLocator = "ucBankingInfo_lblEnsureThatYourBankCanAccept";
        private const string ErrorDIVLocator = "//div[@id='valSummary']";
        private const string ErrorLocator = ErrorDIVLocator + "/ul";

        private const string Error_UnableToActiveEventWithXAuthNotApproved = "Note: Your request for external authentication has not yet been approved. You will not be able to activate an event with external authentication until your account has been approved. To expedite your request, contact RegOnline Customer Support.";

        /// <summary>
        /// Choose pricing option based on its index, top is 0, next is 1, and so on
        /// </summary>
        /// <param name="pricingOptionIndex">0 base index, so the first one in the list is 0, next is 1, etc</param>
        [Step]
        public void SelectPricingOption(int pricingOptionIndex)
        {
            string locator = string.Format(PricingOptionId, pricingOptionIndex);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.Id);
        }

        [Step]
        public void ActivateEvent()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ActivateButton, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }
        
        /// <summary>
        /// If you need to choose whether or not to remove the test registrations.
        /// </summary>
        /// <param name="removeTestRegistrations">true to delete, false to keep</param>
        [Step]
        public void RemoveTestRegs(bool removeTestRegistrations)
        {
            UIUtilityProvider.UIHelper.SetCheckbox(RemoveTestRegistrationsLocator, removeTestRegistrations, LocateBy.Id);
        }

        /// <summary>
        /// check the status of checkbox 'remove test registrations'.
        /// </summary>
        /// <param name="isCheck">true to ischeck, false to not check</param>
        [Verify]
        public void IsCheckRemoveTestRegs(bool isCheck)
        {
            Assert.AreEqual(UIUtilityProvider.UIHelper.IsChecked(RemoveTestRegistrationsLocator, LocateBy.Id), isCheck);
        }

        /// <summary>
        /// Check if the comment warning against currency mismatch, itself, matches the currency in question
        /// </summary>
        /// <param name="currency">in format, "US Dollar (USD)"</param>
        [Verify]
        public void IsCommentUnderBankCountryFor(string currency)
        {
            Assert.AreEqual(
                UIUtilityProvider.UIHelper.GetText(CommentUnderBankCountryLocator, LocateBy.Id), 
                string.Format("Important Note: To prevent delays in payment, ensure that your bank can accept payments in {0}.", currency));
        }

        /// <summary>
        /// Fills out automatically change status at a later date/time
        /// </summary>
        /// <param name="status"></param>
        /// <param name="date">mm/dd/yyyy</param>
        /// <param name="time">hh:mm AM/PM</param>
        /// <param name="statusMessage">Start page message to be displayed</param>
        public void AutomaticallyChangeStatus(DashboardManager.EventStatus status, string date, string time, string statusMessage)
        {
            string StatusToChangeTo = string.Empty;
            string ExceptionMessage = "Could not find status {0}, is this your first time activating the event? Then {1} will not appear";
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AutomaticallyChangeStatusLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();

            switch (status)
            {
                case DashboardManager.EventStatus.Active:
                    throw new Exception("Cannot automatically change status to Active");
                case DashboardManager.EventStatus.Cancelled:
                    try
                    {
                        StatusToChangeTo = "Cancelled";
                        UIUtilityProvider.UIHelper.SelectWithText(StatusLocator, StatusToChangeTo, LocateBy.Id);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ExceptionMessage, StatusToChangeTo));
                    }
                    break;
                case DashboardManager.EventStatus.OnSite:
                    try
                    {
                        StatusToChangeTo = "On-Site";
                        UIUtilityProvider.UIHelper.SelectWithText(StatusLocator, StatusToChangeTo, LocateBy.Id);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ExceptionMessage, StatusToChangeTo));
                    }
                    break;
                case DashboardManager.EventStatus.SoldOut:
                    try
                    {
                        StatusToChangeTo = "Sold Out";
                        UIUtilityProvider.UIHelper.SelectWithText(StatusLocator, StatusToChangeTo, LocateBy.Id);
                    }
                    catch
                    {
                        throw new Exception(string.Format(ExceptionMessage, StatusToChangeTo));
                    }
                    break;
                case DashboardManager.EventStatus.Inactive:
                    StatusToChangeTo = "Inactive";
                    UIUtilityProvider.UIHelper.SelectWithText(StatusLocator, StatusToChangeTo, LocateBy.Id);
                    break;
                case DashboardManager.EventStatus.Archived:
                    StatusToChangeTo = "Archived";
                    UIUtilityProvider.UIHelper.SelectWithText(StatusLocator, StatusToChangeTo, LocateBy.Id);
                    break;
                case DashboardManager.EventStatus.Testing:
                    StatusToChangeTo = "Testing";
                    UIUtilityProvider.UIHelper.SelectWithText(StatusLocator, StatusToChangeTo, LocateBy.Id);
                    break;
            }

            UIUtilityProvider.UIHelper.Type(ChangeStatusDateLocator, date, LocateBy.Id);
            UIUtilityProvider.UIHelper.Type(ChangeStatusTimeLocator, time, LocateBy.Id);

            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AutomaticallyChangeStatusMessageLocator, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
            UIUtilityProvider.UIHelper.TypeContentEditorOnFrame(statusMessage);
            UIUtilityProvider.UIHelper.SaveAndCloseContentEditorFrame();
        }

        [Step]
        public void CheckAgreementForTermsOfService(bool check)
        {
            if (UIUtilityProvider.UIHelper.IsElementPresent(AgreeTOSLocator, LocateBy.Id))
            {
                UIUtilityProvider.UIHelper.SetCheckbox(AgreeTOSLocator, check, LocateBy.Id);
            }
        }

        [Step]
        public void TypeInitialing(string txt)
        {
            if (UIUtilityProvider.UIHelper.IsElementPresent(InitialingLocator, LocateBy.Id))
            {
                UIUtilityProvider.UIHelper.Type(InitialingLocator, txt, LocateBy.Id);
            }
        }

        #region XAuth

        [Verify]
        public void VerifyUnableToActivateEventMessageShown(bool shown)
        {
            List<string> errorMessages = new List<string>();
            errorMessages.Add(Error_UnableToActiveEventWithXAuthNotApproved);
            List<string> actualErrors = GetErrorMessages();

            if (shown)
            {
                Assert.AreEqual(1, actualErrors.Count);
                Assert.AreEqual(errorMessages[0], actualErrors[0]);
            }
            else
            {
                Assert.AreEqual(0, actualErrors.Count);
            }
        }

        #endregion

        private List<string> GetErrorMessages()
        {
            List<string> errorList = new List<string>();

            if (UIUtilityProvider.UIHelper.GetAttribute(ErrorDIVLocator, "@style", LocateBy.XPath) != "display: none;")
            {
                int count = Convert.ToInt32(UIUtilityProvider.UIHelper.GetXPathCountByXPath(ErrorLocator + "/li"));
                string errorFormat = ErrorLocator + "/li[{0}]";

                for (int i = 1; i <= count; i++)
                {
                    errorList.Add(UIUtilityProvider.UIHelper.GetText(string.Format(errorFormat, i), LocateBy.XPath));
                }
            }

            return errorList;
        }
    }
}
