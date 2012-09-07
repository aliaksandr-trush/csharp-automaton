namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class Frame : WebElement
    {
        public string Name;
        public int Index;
        public string Id;

        public Frame(string name)
        {
            this.Name = name;
        }

        public Frame(int index)
        {
            this.Index = index;
        }

        public Frame()
        { }

        public void SelectByName()
        {
            WebDriverUtility.DefaultProvider.SelectPopUpFrameByName(Name);
        }

        public void SelectById()
        {
            WebDriverUtility.DefaultProvider.SelectPopUpFrameById(Id);
        }

        public void SelectByIndex()
        {
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(Index);
        }

        public void SwitchToMain()
        {
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
        }

        public void WaitForAJAX()
        {
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void WaitForLoad()
        {
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }
    }
}
