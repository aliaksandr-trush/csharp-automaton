namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Merchandise : Window
    {
        public Clickable EmptyAddMerchandise = new Clickable("ctl00_cph_grdFees_lnkEmptyAdd", LocateBy.Id);
        public Clickable AddMerchandise = new Clickable("ctl00_cph_grdFees_hlAddNew", LocateBy.Id);
        public MerchandiseDefine MerchandiseDefine = new MerchandiseDefine("dialog");
        public Clickable MerchandisePageHeader = new Clickable("//*[text()='Add Merchandise Page Header']", LocateBy.XPath);
        public HtmlEditor MerchandisePageHeaderEditor = new HtmlEditor("dialog");
        public Clickable MerchandisePageFooter = new Clickable("//*[text()='Add Merchandise Page Footer']", LocateBy.XPath);
        public HtmlEditor MerchandisePageFooterEditor = new HtmlEditor("dialog");
        public CheckBox AddShippingFee = new CheckBox("ctl00_cph_chkEnableShippingFee", LocateBy.Id);
        public Input ShippingFee = new Input("ctl00_cph_txtShippingFee_text", LocateBy.Id);

        public void EmptyAddMerchandise_Click()
        {
            this.EmptyAddMerchandise.WaitForDisplay();
            this.EmptyAddMerchandise.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void AddMerchandise_Click()
        {
            this.AddMerchandise.WaitForDisplay();
            this.AddMerchandise.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void MerchandisePageHeader_Click()
        {
            this.MerchandisePageHeader.WaitForDisplay();
            this.MerchandisePageHeader.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void MerchandisePageFooter_Click()
        {
            this.MerchandisePageFooter.WaitForDisplay();
            this.MerchandisePageFooter.Click();
            Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }
    }

    public class MerchandiseRow
    {
        public int MerchandiseId;
        public Clickable Merchandise;

        public MerchandiseRow(DataCollection.MerchandiseItem merch)
        {
            this.Merchandise = new Clickable(string.Format("//table[@id='ctl00_cph_grdFees_tblGrid']//a[text()='{0}']", merch.Name), LocateBy.XPath);

            string merchHrefAttriString = this.Merchandise.GetAttribute("href");

            string tmp = merchHrefAttriString.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[3];
            tmp = tmp.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.MerchandiseId = Convert.ToInt32(tmp);
        }
    }
}
