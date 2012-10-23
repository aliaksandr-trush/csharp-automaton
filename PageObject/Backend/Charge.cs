namespace RegOnline.RegressionTest.PageObject.Backend
{
    using RegOnline.RegressionTest.WebElements;

    public class Charge : Window
    {
        public Charge(string name) : base(name) { }

        public RadioButton UseNewCC = new RadioButton("ctl00_cph_rbNewCC", UIUtility.LocateBy.Id);
        public Input NewCC = new Input("ctl00_cph_txtCC", UIUtility.LocateBy.Id);
        private Clickable SaveAndClose = new Clickable("//button[text()='Save & Close']", UIUtility.LocateBy.XPath);

        public Label ExistingCC(int index)
        {
            return new Label(string.Format("//label[@for='ctl00_cph_rblExistingCCs_{0}']", index), UIUtility.LocateBy.XPath);
        }

        public void SaveAndClose_Click()
        {
            this.SaveAndClose.WaitForDisplay();
            this.SaveAndClose.Click();
            Utilities.Utility.ThreadSleep(2);
        }
    }
}
