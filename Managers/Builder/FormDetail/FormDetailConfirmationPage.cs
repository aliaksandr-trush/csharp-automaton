namespace RegOnline.RegressionTest.Managers.Builder
{
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.DataAccess;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        [Step]
        public void SetEventConfirmationPage()
        {
            // enable schedule download
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkEventsEnableScheduleDownload", true, LocateBy.Id);
        }

        public void SetEnableHotelSearchFeature(bool check)
        {
            WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_chkEventsHotelSearch", check, LocateBy.Id);
        }

        [Verify]
        public void VerifyEventConfirmationPage()
        {
            ////ReloadEvent();
            ClientDataContext db = new ClientDataContext();

            Event = (from e in db.Events where e.Id == EventId select e).Single();

            // verify confirmation page
            Assert.That(Event.TellaFriend ?? false);
            Assert.That(Event.SelfReminder ?? false);
            Assert.That(Event.EventCollectionField.Feedback);
            Assert.That(Event.EventCollectionField.EnableAddToMyCalendar);
            Assert.That(Event.MapInConfirmPage ?? false);
            Assert.That(Event.IncludeDirectoryConfirmPage);
            Assert.That(Event.EnableScheduleDownload);
            Assert.IsFalse(Event.BypassConfirmationPage);
        }

        public void AddConfirmationMessage(string message)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cph_elConfirmationText_linkCheckmarkfrmEventTextsConfirm_Info", LocateBy.Id);
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            WebDriverUtility.DefaultProvider.Type("//textarea", message + "<br>", LocateBy.XPath);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName("dialog");
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
        }

    }
}