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
                return UIUtil.DefaultProvider.GetLocation();
            }
        }

        public bool URLContains(string url)
        {
            return UIUtil.DefaultProvider.UrlContainsPath(url);
        }

        public string GetQueryStringValue(string queryString)
        {
            return UIUtil.DefaultProvider.GetQueryStringValue(queryString);
        }

        public void SelectByName()
        {
            UIUtil.DefaultProvider.SelectWindowByName(Name);
        }

        public void Close()
        {
            UIUtil.DefaultProvider.CloseWindow();
        }

        public void GoBackToPreviousPage()
        {
            UIUtil.DefaultProvider.GoBackToPreviousPage();
        }

        public void CloseAndBackToPrevious()
        {
            this.Close();
            UIUtil.DefaultProvider.SelectOriginalWindow();
            UIUtil.DefaultProvider.SelectTopWindow();
        }

        public void WaitForLoad()
        {
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        public void WaitForLoad(TimeSpan timeOut)
        {
            UIUtil.DefaultProvider.WaitForPageToLoad(timeOut);
        }

        public void WaitForAJAX()
        {
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void Refresh()
        {
            UIUtil.DefaultProvider.RefreshPage();
        }
    }
}
