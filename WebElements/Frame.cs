namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class Frame : ElementBase
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
            UIUtil.DefaultProvider.SelectPopUpFrameByName(Name);
        }

        public void SelectById()
        {
            UIUtil.DefaultProvider.SelectPopUpFrameById(Id);
        }

        public void SelectByIndex()
        {
            UIUtil.DefaultProvider.SelectIFrameOnCurrentIFrame(Index);
        }

        public void SwitchToMain()
        {
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        public void WaitForAJAX()
        {
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void WaitForLoad()
        {
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }
    }
}
