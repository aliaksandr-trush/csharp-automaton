namespace RegOnline.RegressionTest.Managers.Builder
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public partial class FormDetailManager : ManagerBase
    {
        public enum Mode
        {
            Edit,
            Html,
            Preview
        }

        private partial class Locator
        {
            public const string EmailRADWindowId = "dialog";
            public const string CompleteConfirmationToRegistrant = "//span[@id='ctl00_cph_mdNew']/a";
            public const string IncompleteNotificationToCustomer = "//span[@id='ctl00_cph_mdIncomplete']/a";
            public const string UpdateConfirmationToRegistrant = "//span[@id='ctl00_cph_mdUpdate']/a";
            public const string SubstituteConfirmationToRegistrant = "//span[@id='ctl00_cph_mdSub']/a";
            public const string CancelConfirmationToRegistrant = "//span[@id='ctl00_cph_mdCancel']/a";
        }

        public void SelectEmailEditFrame()
        {
            UIUtil.DefaultProvider.SelectPopUpFrameByName(Locator.EmailRADWindowId);
        }

        public void OpenEditConfirmationEmail(RegOnline.RegressionTest.Managers.Emails.EmailManager.EmailCategory category)
        {
            switch (category)
            {
                case RegOnline.RegressionTest.Managers.Emails.EmailManager.EmailCategory.Complete:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator.CompleteConfirmationToRegistrant, LocateBy.XPath);
                    break;
                case RegOnline.RegressionTest.Managers.Emails.EmailManager.EmailCategory.Incomplete:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator.IncompleteNotificationToCustomer, LocateBy.XPath);
                    break;
                case RegOnline.RegressionTest.Managers.Emails.EmailManager.EmailCategory.Update:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator.UpdateConfirmationToRegistrant, LocateBy.XPath);
                    break;
                case RegOnline.RegressionTest.Managers.Emails.EmailManager.EmailCategory.Substitute:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator.SubstituteConfirmationToRegistrant, LocateBy.XPath);
                    break;
                case RegOnline.RegressionTest.Managers.Emails.EmailManager.EmailCategory.Cancel:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick(Locator.CancelConfirmationToRegistrant, LocateBy.XPath);
                    break;
                default:
                    break;
            }

            Utility.ThreadSleep(1);
            UIUtil.DefaultProvider.SelectPopUpFrameByName(Locator.EmailRADWindowId);
        }

        public void SaveAndStayEditEmail()
        {
            SaveAndStay();
        }

        public void SaveAndCloseEditEmail()
        {
            SaveAndClose();
            SelectBuilderWindow();
        }

        public void CancelEditEmail()
        {
            UIUtil.DefaultProvider.ClickCancel();
            SelectBuilderWindow();
        }

        public void SwitchModeInEmail(Mode mode)
        {
            switch (mode)
            {
                case Mode.Edit:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucEmail_ucContent_radEdit", LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case Mode.Html:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucEmail_ucContent_radHtml", LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                case Mode.Preview:
                    UIUtil.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucEmail_ucContent_radPreview", LocateBy.Id);
                    UIUtil.DefaultProvider.WaitForAJAXRequest();
                    break;
                default:
                    throw new ArgumentException(string.Format("No such mode: {0}", mode.ToString()));
            }
        }

        public void TypeContentInHTML(string content)
        {
            UIUtil.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            UIUtil.DefaultProvider.Type("//textarea", content, LocateBy.XPath);
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        public void SetWhetherToSendIncompleteNotificationEmail(bool check)
        {
            UIUtil.DefaultProvider.SetCheckbox("ctl00_cph_chkIncomplete", check, LocateBy.Id);
        }
    }
}
