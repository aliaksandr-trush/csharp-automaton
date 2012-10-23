namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class CustomFieldDefine : Frame
    {
        public CustomFieldDefine(string name) : base(name) { }

        public Input NameOnForm = new Input("ctl00_cphDialog_cfCF_mipNam_elDesc_TextArea", LocateBy.Id);
        public Clickable FieldType = new Clickable("ctl00_cphDialog_cfCF_selectedFieldTypeToggleImageSpan", LocateBy.Id);

        public void FieldType_Click()
        {
            this.FieldType.WaitForDisplay();
            this.FieldType.Click();
            Utility.ThreadSleep(1);
            WaitForAJAX();
            WaitForLoad();
        }

        public void CFType_Select(DataCollection.EventData_Common.CustomFieldType type)
        {
            Clickable Type = new Clickable(
                string.Format("//div[@id='divMoreFormats']//span[text()='{0}']", CustomStringAttribute.GetCustomString(type)),
                LocateBy.XPath);

            Type.WaitForDisplay();
            Type.Click();
        }

        public void SaveAndClose_Click()
        {
            PageObjectHelper.PopupFrame_Helper.SaveAndClose_Click();
            SwitchToMain();
        }
    }
}
