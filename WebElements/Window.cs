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

        public string CurrentURL
        {
            get
            {
                return WebDriverUtility.DefaultProvider.GetLocation();
            }
        }

        public bool URLContains(string url)
        {
            return WebDriverUtility.DefaultProvider.UrlContainsPath(url);
        }

        public string GetQueryStringValue(string queryString)
        {
            return WebDriverUtility.DefaultProvider.GetQueryStringValue(queryString);
        }

        public void SelectByName()
        {
            WebDriverUtility.DefaultProvider.SelectWindowByName(Name);
        }

        public void Close()
        {
            WebDriverUtility.DefaultProvider.CloseWindow();
        }

        public void GoBackToPreviousPage()
        {
            WebDriverUtility.DefaultProvider.GoBackToPreviousPage();
        }

        public void CloseAndBackToPrevious()
        {
            this.Close();
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
            WebDriverUtility.DefaultProvider.SelectTopWindow();
        }

        public void WaitForLoad()
        {
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        public void WaitForLoad(TimeSpan timeOut)
        {
            WebDriverUtility.DefaultProvider.WaitForPageToLoad(timeOut);
        }

        public void WaitForAJAX()
        {
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void Refresh()
        {
            WebDriverUtility.DefaultProvider.RefreshPage();
        }
    }
}
