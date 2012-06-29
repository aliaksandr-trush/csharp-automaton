namespace RegOnline.RegressionTest.PageObject.Builder
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class PageObjectHelper : Window
    {
        public ButtonOrLink YesOnSplashPage = new ButtonOrLink("//div[@id='splashChoicePage']//span[text()='Yes']", LocateBy.XPath);
        public Label AgendaErrorMessage = new Label("//div[@id='ctl00_cph_valSummaryCF']/ul/li", LocateBy.XPath);

        private PopupFrameHelper popupFrameHelper = new PopupFrameHelper();

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

        public void SaveAndStay_Click()
        {
            popupFrameHelper.SaveAndStay_Click();
        }

        public void SaveAndClose_Click()
        {
            popupFrameHelper.SaveAndClose_Click();
        }
    }
}
