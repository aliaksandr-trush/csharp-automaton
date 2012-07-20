namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class CustomFieldDefine : Frame
    {
        public CustomFieldDefine(string name) : base(name) { }

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

        public TextBox NameOnForm = new TextBox("ctl00_cphDialog_cfCF_mipNam_elDesc_TextArea", LocateBy.Id);
        public ButtonOrLink FieldType = new ButtonOrLink("ctl00_cphDialog_cfCF_selectedFieldTypeToggleImageSpan", LocateBy.Id);

        public void FieldType_Click()
        {
            this.FieldType.WaitForDisplay();
            this.FieldType.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void CFType_Select(DataCollection.FormData.CustomFieldType type)
        {
            ButtonOrLink Type = new ButtonOrLink(
                string.Format("//div[@id='divMoreFormats']//span[text()='{0}']", CustomStringAttribute.GetCustomString(type)),
                LocateBy.XPath);

            Type.WaitForDisplay();
            Type.Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }
    }
}
