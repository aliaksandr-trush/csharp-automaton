namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class GroupDiscountDefine : Frame
    {
        public GroupDiscountDefine(string name)
            : base(name)
        { }

        public readonly string Locator_Id_RADWindowDiv = "RadWindowWrapper_ctl00_dialog";
        public Input GroupSize = new Input("ctl00_cphDialog_GroupSizeTextBox_text", LocateBy.Id);
        public MultiChoiceDropdown GroupSizeOption = new MultiChoiceDropdown("ctl00_cphDialog_ddlIsGroupSizeOrMore", LocateBy.Id);
        public Input DiscountAmount = new Input("ctl00_cphDialog_AmountTextBox_text", LocateBy.Id);
        public MultiChoiceDropdown DiscountType = new MultiChoiceDropdown("ctl00_cphDialog_ddlGroupRuleType", LocateBy.Id);
        public MultiChoiceDropdown AddtionalRegOption = new MultiChoiceDropdown("ctl00_cphDialog_AdditionalAllDropDownList", LocateBy.Id);
        public Input AdditionalNumber = new Input("ctl00_cphDialog_EffectedSizeTextBox_text", LocateBy.Id);
        public RadioButton ApplyToAllEventFees = new RadioButton("ctl00_cphDialog_rbApplyAll", LocateBy.Id);
        public RadioButton ApplyToSelectedFees = new RadioButton("ctl00_cphDialog_rbApplySelected", LocateBy.Id);
        public CheckBox All = new CheckBox("//li[@class='rtLI rtFirst rtLast']/div/input", LocateBy.XPath);
        public CheckBox ShowAndApply = new CheckBox("ctl00_cphDialog_groupDiscountEnabledCheckBox", LocateBy.Id);

        public CheckBox ApplyToAgendaItem(DataCollection.AgendaItem agenda)
        {
            return new CheckBox(string.Format("//input[following-sibling::span[text()='{0}']]", agenda.NameOnForm), LocateBy.XPath);
        }

        public CheckBox ApplyToRegType(DataCollection.RegType regType)
        {
            return new CheckBox(string.Format("//input[following-sibling::span[text()='{0}_Event_Fee']]", regType.Name), LocateBy.XPath);
        }

        public void ApplyToSelectedFees_Click()
        {
            this.ApplyToSelectedFees.WaitForDisplay();
            this.ApplyToSelectedFees.Click();
            Utilities.Utility.ThreadSleep(3);
            WaitForAJAX();
        }

        public void SaveAndStay_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            Utilities.Utility.ThreadSleep(2);
            SwitchToMain();
        }

        public void Cancel_Click()
        {
            PageObjectHelper.PopupFrame_Helper.Cancel_Click();
            SwitchToMain();
        }

        public void AdjustRADWindowPositionAndResize()
        {
            PageObject.PageObjectHelper.AdjustRADWindowPosition(this.Locator_Id_RADWindowDiv, 20, 20);
            PageObject.PageObjectHelper.ResizeRADWindow(this.Name, 800, 1000);
        }
    }
}
