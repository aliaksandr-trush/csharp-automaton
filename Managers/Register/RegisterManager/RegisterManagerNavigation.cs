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
            UIUtil.DefaultProvider.WaitForDisplayAndClick(ContinueButton, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();

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
            UIUtil.DefaultProvider.WaitForDisplayAndClick(ContinueButton, LocateBy.XPath);
            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void VerifyHasContinueOrContinueNextStepButton(bool hasButton)
        {
            VerifyTool.VerifyValue(hasButton, HasContinueOrContinueNextStepButton(), "HasContinueButton = {0}");
        }

        public bool HasContinueOrContinueNextStepButton()
        {
            return (UIUtil.DefaultProvider.IsElementPresent(ContinueButton, LocateBy.XPath) ||
                UIUtil.DefaultProvider.IsElementPresent(ContinueButton_Old, LocateBy.XPath) ||
                UIUtil.DefaultProvider.IsElementPresent(ContinueToNextStepButton, LocateBy.XPath));
        }

        [Step]
        public void ClickAddAnotherPerson()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(AddAnotherPersonLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Verify]
        public void VerifyHasAddAnotherPersonButton(bool hasButton)
        {
            VerifyTool.VerifyValue(hasButton, HasAddAnotherPersonButton(), "HasAddAnotherPersonButton : {0}");
        }

        public bool HasAddAnotherPersonButton()
        {
            return UIUtil.DefaultProvider.IsElementPresent(AddAnotherPersonLocator, LocateBy.XPath);
        }

        public bool HasAddPersonToWaitlistButton()
        {
            return UIUtil.DefaultProvider.IsElementPresent(AddPersonToWaitlistLocator, LocateBy.XPath);
        }

        public void ClickAddPersonToWaitlist()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(AddPersonToWaitlistLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void FinishRegistration()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(FinishButton, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();

            if (OnConfirmationRedirectPage())
            {
                UIUtil.DefaultProvider.VerifyOnPage(OnConfirmationRedirectPage(), "Active Advantage");
                ClickAdvantageNo();
            }
        }

        [Step]
        public void GoBackToPreviousPage()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick("Go back", LocateBy.LinkText);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void VerifyRegTypeMinimumMessage(string expMessage)
        {
            string actMessage = GetRegTypeMinimumMessage();
            VerifyTool.VerifyValue(expMessage, actMessage, "The registration type minimum message is:{0}");
        }

        public string GetRegTypeMinimumMessage()
        {
            return UIUtil.DefaultProvider.GetText(AddAnotherPersonLocator + "/preceding-sibling::p", LocateBy.XPath);
        }

        public void VerifyEventLimitReachedAndContinueMessage(string expMessage)
        {
            string actMessage = GetEventLimitReachedAndContinueMessage();
            VerifyTool.VerifyValue(expMessage, actMessage, "The event limit reached message is:{0}");
        }

        public string GetEventLimitReachedAndContinueMessage()
        {
            string xPath = ContinueButton + "/preceding-sibling::p";
            string messages = UIUtil.DefaultProvider.GetText(xPath, LocateBy.XPath);

            string[] separator = { "\n" };
            string message = messages.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
            message += messages.Split(separator, StringSplitOptions.RemoveEmptyEntries)[1];

            return message;
        }
        #endregion

    }
}
