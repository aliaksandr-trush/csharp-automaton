namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;
    using System;

    public class Window
    {
        public string Name { get; set; }

        public Window() { }

        public Window(string name)
        {
            this.Name = name;
        }

        public string URL
        {
            get
            {
                return UIUtilityProvider.UIHelper.GetLocation();
            }
        }

        public bool URLContains(string url)
        {
            return UIUtilityProvider.UIHelper.UrlContainsPath(url);
        }

        public string GetQueryStringValue(string queryString)
        {
            return UIUtilityProvider.UIHelper.GetQueryStringValue(queryString);
        }

        public void SelectByName()
        {
            UIUtilityProvider.UIHelper.SelectWindowByName(Name);
        }

        public void Close()
        {
            UIUtilityProvider.UIHelper.CloseWindow();
        }

        public void GoBackToPreviousPage()
        {
            UIUtilityProvider.UIHelper.GoBackToPreviousPage();
        }

        public void CloseAndBackToPrevious()
        {
            this.Close();
            UIUtilityProvider.UIHelper.SelectOriginalWindow();
            UIUtilityProvider.UIHelper.SelectTopWindow();
        }

        public void WaitForLoad()
        {
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        public void WaitForLoad(TimeSpan timeOut)
        {
            UIUtilityProvider.UIHelper.WaitForPageToLoad(timeOut);
        }

        public void WaitForAJAX()
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void Refresh()
        {
            UIUtilityProvider.UIHelper.RefreshPage();
        }
    }
}
