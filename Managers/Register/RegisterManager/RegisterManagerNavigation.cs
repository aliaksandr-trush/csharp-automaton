namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class RegisterManager : ManagerBase
    {
        #region Navigation

        [Step]
        public void Continue()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContinueButton, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();

            ////if (this.HasErrors())
            ////{
            ////    List<string> errorMessagesList = this.GetErrorMessagesList();

            ////    if (errorMessagesList.Count > 0)
            ////    {
            ////        UIUtilityProvider.UIHelper.FailTest(string.Format("Errors: {0}", this.GetErrorMessageString(errorMessagesList)));
            ////    }
            ////}
        }

        [Step]
        public void ContinueWithErrors()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ContinueButton, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void VerifyHasContinueOrContinueNextStepButton(bool hasButton)
        {
            VerifyTool.VerifyValue(hasButton, HasContinueOrContinueNextStepButton(), "HasContinueButton = {0}");
        }

        public bool HasContinueOrContinueNextStepButton()
        {
            return (UIUtilityProvider.UIHelper.IsElementPresent(ContinueButton, LocateBy.XPath) ||
                UIUtilityProvider.UIHelper.IsElementPresent(ContinueButton_Old, LocateBy.XPath) ||
                UIUtilityProvider.UIHelper.IsElementPresent(ContinueToNextStepButton, LocateBy.XPath));
        }

        [Step]
        public void ClickAddAnotherPerson()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddAnotherPersonLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Verify]
        public void VerifyHasAddAnotherPersonButton(bool hasButton)
        {
            VerifyTool.VerifyValue(hasButton, HasAddAnotherPersonButton(), "HasAddAnotherPersonButton : {0}");
        }

        public bool HasAddAnotherPersonButton()
        {
            return UIUtilityProvider.UIHelper.IsElementPresent(AddAnotherPersonLocator, LocateBy.XPath);
        }

        public bool HasAddPersonToWaitlistButton()
        {
            return UIUtilityProvider.UIHelper.IsElementPresent(AddPersonToWaitlistLocator, LocateBy.XPath);
        }

        public void ClickAddPersonToWaitlist()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddPersonToWaitlistLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void FinishRegistration()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(FinishButton, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();

            if (OnConfirmationRedirectPage())
            {
                UIUtilityProvider.UIHelper.VerifyOnPage(OnConfirmationRedirectPage(), "Active Advantage");
                ClickAdvantageNo();
            }
        }

        [Step]
        public void GoBackToPreviousPage()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Go back", LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void VerifyRegTypeMinimumMessage(string expMessage)
        {
            string actMessage = GetRegTypeMinimumMessage();
            VerifyTool.VerifyValue(expMessage, actMessage, "The registration type minimum message is:{0}");
        }

        public string GetRegTypeMinimumMessage()
        {
            return UIUtilityProvider.UIHelper.GetText(AddAnotherPersonLocator + "/preceding-sibling::p", LocateBy.XPath);
        }

        public void VerifyEventLimitReachedAndContinueMessage(string expMessage)
        {
            string actMessage = GetEventLimitReachedAndContinueMessage();
            VerifyTool.VerifyValue(expMessage, actMessage, "The event limit reached message is:{0}");
        }

        public string GetEventLimitReachedAndContinueMessage()
        {
            string xPath = ContinueButton + "/preceding-sibling::p";
            string messages = UIUtilityProvider.UIHelper.GetText(xPath, LocateBy.XPath);

            string[] separator = { "\n" };
            string message = messages.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
            message += messages.Split(separator, StringSplitOptions.RemoveEmptyEntries)[1];

            return message;
        }
        #endregion

    }
}
