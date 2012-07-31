namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class EventAdvancedSettings : Frame
    {
        public EventAdvancedSettings(string name)
            : base(name)
        { }

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public CheckBox ThisIsAParentEvent = new CheckBox("ctl00_cphDialog_chkIsParent", UIUtility.LocateBy.Id);
        public CheckBox ThisIsAChildEvent = new CheckBox("ctl00_cphDialog_chkIsChild", UIUtility.LocateBy.Id);
        public MultiChoiceDropdown ParentEventList = new MultiChoiceDropdown("ctl00_cphDialog_ddlParentEventList", LocateBy.Id);

        public void SelectParentEvent(DataCollection.Event evt)
        {
            Utility.ThreadSleep(1);
            this.ParentEventList.WaitForDisplay();
            this.ParentEventList.SelectWithText(string.Format(
                "{0} ({1})", 
                evt.StartPage.AdvancedSettings.ParentEvent.Title, 
                evt.StartPage.AdvancedSettings.ParentEvent.Id));
        }

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            SwitchToMain();
        }
    }
}
