namespace RegOnline.RegressionTest.Managers.Builder
{
    using System.Linq;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Linq;
    using RegOnline.RegressionTest.Managers;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class FormDetailManager : ManagerBase
    {
        [Step]
        public void SetEventConfirmationPage()
        {
            // enable schedule download
            UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_chkEventsEnableScheduleDownload", true, LocateBy.Id);
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
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cph_elConfirmationText_linkCheckmarkfrmEventTextsConfirm_Info", LocateBy.Id);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ucContent_radHtml", LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            UIUtilityProvider.UIHelper.Type("//textarea", message + "<br>", LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }
}