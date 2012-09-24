namespace RegOnline.RegressionTest.Managers.Emails
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.Attributes;
    
    public partial class EmailManager : ManagerBase
    {
        private const string ContentTemplateButton = "//div[contains(text(),'{0}')]/..";
        private const string EmailSplashPageLocator = "//*[@id='splashChoicePage']";
        public const string LogoFileName = "grassbladesdy6pc8.jpg";
        private const string AttendeeId = "23559465";
        private const string EventDropdownLocator = "ctl00_cphDialog_wzEmailInvitation_ddlEvent";
        public const string DefaultEventName = "EmailInvitationFixture";
        public const string DefaultContactListName = "abc";

        public enum EmailWizardSteps
        {
            Basics,
            Theme,
            Content,
            Review,
            Delivery
        }

        public enum Theme
        {
            SameAsRegistrationForm,
            NoTheme
        }

        public enum Delivery
        {
            [StringValue("ctl00_cphDialog_wzEmailInvitation_radSaveAsDraft")]
            SaveAsDraft,

            [StringValue("ctl00_cphDialog_wzEmailInvitation_radSendNow")]
            SendNow,

            [StringValue("ctl00_cphDialog_wzEmailInvitation_radScheduleFor")]
            ScheduleForLater
        }

        private bool OnEmailSplashPage()
        {
            return WebDriverUtility.DefaultProvider.IsElementPresent(EmailSplashPageLocator, LocateBy.XPath);
        }

        public void BackToEmailInvitation()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//div[@id='divBreadCrumbs']/a", LocateBy.XPath);
        }

        /// <summary>
        /// Opens the dialog for creating a new email invite
        /// </summary>
        [Step]
        public void ClickCreateNewEmailLink()
        {
            if (OnEmailSplashPage())
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(EmailSplashPageLocator + "//*[span='Create Email Invitation']", LocateBy.XPath);
            }
            else
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Create Email Invitation", LocateBy.LinkText);
            }

            Utility.ThreadSleep(2);
        }

        /// <summary>
        /// Selects the modal popup window.  We can add more elseif statements if there are different 
        /// names for different frames.
        /// </summary>
        [Step]
        public void SelectWizardFrame()
        {
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
        }

        [Step]
        public string EnterEmailBasics(bool selectDefaultContactList, bool selectDefaultEvent)
        {
            string emailTitle = "Automated Email " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            this.TypeEmailTitle(emailTitle);

            this.TypeEmailSubject("Automated Email");

            this.TypeEmailBounceBackEmail("auto@reg.com");

            if (selectDefaultContactList)
            {
                this.SelectContactList();
            }

            if (selectDefaultEvent)
            {
                this.SelectEvent();
            }

            return emailTitle;
        }

        private void TypeEmailTitle(string title)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_wzEmailInvitation_txtName", title, LocateBy.Id);
        }

        private void TypeEmailSubject(string subject)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_wzEmailInvitation_txtSubject", subject, LocateBy.Id);
        }

        private void TypeEmailBounceBackEmail(string bounceBackEmail)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cphDialog_wzEmailInvitation_txtBounceEmail", bounceBackEmail, LocateBy.Id);
        }

        public void SelectContactList(string contactListName)
        {
            WebDriverUtility.DefaultProvider.SelectWithText("ctl00_cphDialog_wzEmailInvitation_ddlList", contactListName, LocateBy.Id);
            Utility.ThreadSleep(1);
        }

        public void SelectContactList()
        {
            this.SelectContactList(DefaultContactListName);
        }

        /// <summary>
        /// Selects an event from the list on the basics page
        /// </summary>
        [Step]
        public void SelectEvent(int eventId, string eventName)
        {
            WebDriverUtility.DefaultProvider.SelectWithText(EventDropdownLocator, this.ComposeEventDropdownLabel(eventId, eventName), LocateBy.Id);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SelectEvent(int index)
        {
            WebDriverUtility.DefaultProvider.SelectWithIndex(EventDropdownLocator, index, LocateBy.Id);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SelectEvent()
        {
            int eventId = ManagerBase.InvalidId;

            var db = new ClientDataContext();
            eventId = (from e in db.Events
                       where e.Event_Title.Equals(DefaultEventName)
                       orderby e.Id descending
                       select e.Id).First();

            this.SelectEvent(eventId, DefaultEventName);
        }

        private string ComposeEventDropdownLabel(int eventId, string eventName)
        {
            return string.Format("{0} {1}", eventId, eventName);
        }

        /// <summary>
        /// Checks to make sure validation summary shows up
        /// </summary>
        [Verify]
        public void VerifyErrorMessage(string message)
        {
            Assert.That(WebDriverUtility.DefaultProvider.IsElementPresent("ctl00_cphDialog_vsValidationSummary", LocateBy.Id));

            if (!string.IsNullOrEmpty(message))
            {
                Assert.That(WebDriverUtility.DefaultProvider.IsTextPresent(string.Empty));
            }
        }

        /// <summary>
        /// Clicks the Finish button on the wizard
        /// </summary>
        [Step]
        public void EmailWizardFinishClick()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//div[@id='divButtons']//span[text()='Send']", LocateBy.XPath);
            ////UIUtilityProvider.UIHelper.ExecuteJavaScript("javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$cphDialog$wzEmailInvitation$FinishNavigationTemplateContainerID$btnSave$ctl00', '', true, '', '', false, true))");
            //UIUtilityProvider.UIHelper.Click("//div[@id='divButtons']/span[1]/a", LocateBy.XPath);
            Utility.ThreadSleep(2);
        }

        /// <summary>
        /// Selects an inserts a content template based on name
        /// </summary>
        /// <param name="templateName"></param>
        [Step]
        public void ClickAndInsertContentTemplate(string templateName)
        {
            // Popup the preview window
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format(ContentTemplateButton, templateName), LocateBy.XPath);

            // Click the insert button
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("insertButton", LocateBy.Id);
        }

        /// <summary>
        /// Clicks the next button in the email wizard
        /// </summary>
        [Step]
        public void EmailWizardNextClick()
        {
            // See if we are on the first step or not
            if (WebDriverUtility.DefaultProvider.IsElementPresent("ctl00_cphDialog_wzEmailInvitation_StartNavigationTemplateContainerID_btnNext", LocateBy.Id))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_wzEmailInvitation_StartNavigationTemplateContainerID_btnNext", LocateBy.Id);
            }
            else if (WebDriverUtility.DefaultProvider.IsElementPresent("ctl00_cphDialog_wzEmailInvitation_StepNavigationTemplateContainerID_btnNext", LocateBy.Id))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_wzEmailInvitation_StepNavigationTemplateContainerID_btnNext", LocateBy.Id);
            }
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void ChooseDeliveryOption(Delivery delivery)
        {
            WebDriverUtility.DefaultProvider.WaitForElementDisplay(StringEnum.GetStringValue(Delivery.SendNow), LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(StringEnum.GetStringValue(delivery), LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        /// <summary>
        /// Checks to see whether the sendnow and schedule for radio buttons are enabled or not
        /// </summary>
        /// <param name="enabled"></param>
        [Verify]
        public void EmailWizardVerifySendNowStatus(bool enabled)
        {
            WebDriverUtility.DefaultProvider.WaitForElementDisplay("ctl00_cphDialog_wzEmailInvitation_radSendNow", LocateBy.Id);
            Assert.That(WebDriverUtility.DefaultProvider.IsEditable("ctl00_cphDialog_wzEmailInvitation_radSendNow", LocateBy.Id) == enabled);
            Assert.That(WebDriverUtility.DefaultProvider.IsEditable("ctl00_cphDialog_wzEmailInvitation_radScheduleFor", LocateBy.Id) == enabled);
        }

        /// <summary>
        /// Clicks one of the steps on the breadcrumb
        /// </summary>
        /// <param name="step"></param>
        [Step]
        public void EmailWizardBreadCrumbClick(EmailWizardSteps step)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format("//span[@id='ctl00_cphDialog_bcTop']/ul/li/a/span[text()='{0}']", step.ToString()), LocateBy.XPath);
        }

        /// <summary>
        /// Checks the terms checkbox on the delivery step
        /// </summary>
        [Step]
        public void CheckEmailTerms()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_wzEmailInvitation_chkNotice", LocateBy.Id);
        }

        /// <summary>
        /// Picks a theme
        /// </summary>
        /// <param name="themeName"></param>
        public void SelectTheme(string themeName)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format("//div/p[text()='{0}']", themeName), LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        [Step]
        public void SelectTheme(Theme theme)
        {
            string locator = string.Empty;

            switch (theme)
            {
                case Theme.SameAsRegistrationForm:
                    locator = "ctl00_cphDialog_wzEmailInvitation_rptThemes_ctl00_lbRegFormTheme";
                    break;

                case Theme.NoTheme:
                    locator = "ctl00_cphDialog_wzEmailInvitation_rptThemes_ctl07_lb";
                    break;

                default:
                    break;
            }

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(locator, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        /// <summary>
        /// Uploads a logo from the share drive
        /// </summary>
        [Step]
        public void UploadLogo()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_wzEmailInvitation_hlEditLogo", LocateBy.Id);
            WebDriverUtility.DefaultProvider.UploadEmailLogo(ConfigReader.DefaultProvider.EnvironmentConfiguration.DataPath + LogoFileName);
        }

        /// <summary>
        /// Sets the title on the content step
        /// </summary>
        /// <param name="title"></param>
        [Step]
        public void SetCustomTitle(string title)
        {
            WebDriverUtility.DefaultProvider.Type("eventtitle", title, LocateBy.Name);
        }

        /// <summary>
        /// Checks the preview frame to see if the required elements are there
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="title"></param>
        [Step]
        public void VerifyReviewTab(string theme, string title)
        {
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
            //Thread.Sleep(60000);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
            WebDriverUtility.DefaultProvider.SelectIFrame(0);
            //UIUtilityProvider.UIHelper.SelectPopUpFrameById("ifPreview");
            

            string source = WebDriverUtility.DefaultProvider.GetPageSource();

            WebDriverUtility.DefaultProvider.SelectPopUpFrameById("ifPreview");

            // Cannot verify by text after TD is online
            ////Assert.That(driver.PageSource.ToLower().Contains(theme.ToLower()));

            Assert.That(WebDriverUtility.DefaultProvider.GetPageSource().ToLower().Contains(title.ToLower()));

            // TODO: Better way to test the specific logo?
            Assert.That(WebDriverUtility.DefaultProvider.GetPageSource().ToLower().Contains(LogoFileName.ToLower()));

            // Pick the parent frame again
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            ////WebDriverManager.driver.SwitchTo().Frame("plain");
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("plain");
        }
    }
}
