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
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(Name);
        }

        public void SelectById()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameById(Id);
        }

        public void SelectByIndex()
        {
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(Index);
        }

        public void SwitchToMain()
        {
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void WaitForAJAX()
        {
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void WaitForLoad()
        {
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }
    }
}
