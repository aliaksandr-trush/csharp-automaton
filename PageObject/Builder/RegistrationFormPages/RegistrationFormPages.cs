namespace RegOnline.RegressionTest.PageObject.Builder.RegistrationFormPages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.DataCollection;

    public class RegistrationFormPages : Window
    {
        public EventStart StartPage = new EventStart();
        public Agenda AgendaPage = new Agenda();
        public Checkout CheckoutPage = new Checkout();
        public LodgingTravel LodgingTravelPage = new LodgingTravel();
        public Merchandise MerchandisePage = new Merchandise();
        public PersonalInfo PersonalInfoPage = new PersonalInfo();
        public XAuthOld XAuthOld = new XAuthOld();

        public ButtonOrLink YesOnSplashPage = new ButtonOrLink("//div[@id='splashChoicePage']//span[text()='Yes']", LocateBy.XPath);
        public Label AgendaErrorMessage = new Label("//div[@id='ctl00_cph_valSummaryCF']/ul/li", LocateBy.XPath);

        public void YesOnSplashPage_Click()
        {
            this.YesOnSplashPage.WaitForDisplay();
            this.YesOnSplashPage.Click();
            WaitForLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        public void GotoPage(FormData.Page page)
        {
            string accesskey = string.Empty;

            switch (page)
            {
                case FormData.Page.Start:
                    accesskey = "S";
                    break;

                case FormData.Page.PI:
                    accesskey = "I";
                    break;

                case FormData.Page.Agenda:
                    accesskey = "H";
                    break;

                case FormData.Page.LodgingTravel:
                    accesskey = "L";
                    break;

                case FormData.Page.Merchandise:
                    accesskey = "M";
                    break;

                case FormData.Page.Checkout:
                    accesskey = "K";
                    break;

                case FormData.Page.Confirmation:
                    accesskey = "C";
                    break;

                default:
                    break;
            }

            ButtonOrLink Page = new ButtonOrLink(string.Format("//a[@accesskey='{0}']", accesskey), LocateBy.XPath);
            Page.Click();
            WaitForLoad();
            UIUtilityProvider.UIHelper.HideActiveSpecificFooter(true);
        }

        public void Advanced_Click()
        {
            UIUtilityProvider.UIHelper.ExpandAdvanced();
        }
    }
}
