namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class CodeDefine : Frame
    {
        public CodeDefine(string name, string parentFrame) : base(name, parentFrame) { }
        public CodeDefine(string name) : base(name) { }

        #region WebElements
        public RadioButton Discount = new RadioButton("ctl00_cphDialog_rblCodeTypes_0", LocateBy.Id);
        public RadioButton Access = new RadioButton("ctl00_cphDialog_rblCodeTypes_1", LocateBy.Id);
        public TextBox Code = new TextBox("ctl00_cphDialog_txtDiscountCodeTitle", LocateBy.Id);
        public MultiChoiceDropdown CodeDirection = new MultiChoiceDropdown("ctl00_cphDialog_ddlChangePriceByDirection", LocateBy.Id);
        public TextBox Amount = new TextBox("ctl00_cphDialog_radNChangePriceBy_text", LocateBy.Id);
        public RadioButton Percentage = new RadioButton("ctl00_cphDialog_rblChangeTypePercent", LocateBy.Id);
        public RadioButton FixAmount = new RadioButton("ctl00_cphDialog_rblChangeTypeFixed", LocateBy.Id);
        public TextBox UseLimit = new TextBox("ctl00_cphDialog_radNCodeUseLimit_text", LocateBy.Id);
        #endregion

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        #region Basic Actions
        public void Discount_Click()
        {
            this.Discount.WaitForDisplay();
            this.Discount.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void Access_Click()
        {
            this.Access.WaitForDisplay();
            this.Access.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
        }

        public void Percentage_Click()
        {
            this.Percentage.WaitForDisplay();
            this.Percentage.Click();
            WaitForAJAX();
        }

        public void FixAmount_Click()
        {
            this.FixAmount.WaitForDisplay();
            this.FixAmount.Click();
            WaitForAJAX();
        }

        public void SaveAndNew_Click()
        {
            popupFrameHelper.SaveAndNew_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }

        public void Cancel_Click()
        {
            popupFrameHelper.Cancel_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            SelectParentFrame();
        }
        #endregion
    }
}
