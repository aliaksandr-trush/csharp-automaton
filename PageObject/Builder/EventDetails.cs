namespace RegOnline.RegressionTest.PageObject.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class EventDetails : Window
    {
        public RegistrationFormPages.RegistrationFormPages FormPages = new RegistrationFormPages.RegistrationFormPages();

        public Clickable Button_RegistrationFormPages = new Clickable("//a[@accesskey='P']", UIUtility.LocateBy.XPath);
        public Clickable EventWebsite = new Clickable("//a[@accesskey='W']", UIUtility.LocateBy.XPath);
        public Clickable Preview = new Clickable("ctl00_btnPreview", UIUtility.LocateBy.Id);
        public Clickable SaveAndStay = new Clickable("ctl00_btnSaveStay", UIUtility.LocateBy.Id);
        public Clickable SaveAndClose = new Clickable("ctl00_btnSaveClose", UIUtility.LocateBy.Id);
        public Clickable CloseButton = new Clickable("ctl00_hplReturnToManger", UIUtility.LocateBy.Id);

        public void RegistrationFormPages_Click()
        {
            this.Button_RegistrationFormPages.WaitForDisplay();
            this.Button_RegistrationFormPages.Click();
            WaitForLoad();
        }

        public void EventWebsite_Click()
        {
            this.EventWebsite.WaitForDisplay();
            this.EventWebsite.Click();
            WaitForLoad();
        }

        public void SaveAndStay_Click()
        {
            this.SaveAndStay.WaitForDisplay();
            this.SaveAndStay.Click();
            WaitForLoad();
        }

        public void SaveAndClose_Click()
        {
            this.SaveAndClose.WaitForDisplay();
            this.SaveAndClose.Click();
            WaitForLoad();
        }

        public void Close_Click()
        {
            this.CloseButton.WaitForDisplay();
            this.CloseButton.Click();
            Utilities.Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }
}
