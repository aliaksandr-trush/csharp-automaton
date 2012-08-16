namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RegOnline.RegressionTest.WebElements;

    public class Merchandise : Window
    {
        public ButtonOrLink EmptyAddMerchandise = new ButtonOrLink("ctl00_cph_grdFees_lnkEmptyAdd", LocateBy.Id);
        public ButtonOrLink AddMerchandise = new ButtonOrLink("ctl00_cph_grdFees_hlAddNew", LocateBy.Id);
        public MerchandiseDefine MerchandiseDefine = new MerchandiseDefine("dialog");
        public ButtonOrLink MerchandisePageHeader = new ButtonOrLink("//*[text()='Add Merchandise Page Header']", LocateBy.XPath);
        public HtmlEditor MerchandisePageHeaderEditor = new HtmlEditor("dialog");
        public ButtonOrLink MerchandisePageFooter = new ButtonOrLink("//*[text()='Add Merchandise Page Footer']", LocateBy.XPath);
        public HtmlEditor MerchandisePageFooterEditor = new HtmlEditor("dialog");

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
        public ButtonOrLink Merchandise;

        public MerchandiseRow(DataCollection.Merchandise merch)
        {
            this.Merchandise = new ButtonOrLink(string.Format("//table[@id='ctl00_cph_grdFees_tblGrid']//a[text()='{0}']", merch.MerchandiseName), LocateBy.XPath);

            string merchHrefAttriString = this.Merchandise.GetAttribute("href");

            string tmp = merchHrefAttriString.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[3];
            tmp = tmp.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.MerchandiseId = Convert.ToInt32(tmp);
        }
    }
}
