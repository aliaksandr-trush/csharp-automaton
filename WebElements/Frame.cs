namespace RegOnline.RegressionTest.WebElements
{
    using RegOnline.RegressionTest.UIUtility;

    public class Frame : WebElement
    {
        public string Name;
        public string ParentFrame;
        public int Index;

        public Frame(string name)
        {
            this.Name = name;
        }

        public Frame(int index)
        {
            this.Index = index;
        }

        public Frame(string name, string parentFrame)
        {
            this.Name = name;
            this.ParentFrame = parentFrame;
        }

        public void SelectByName()
        {
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName(Name);
        }

        public void SelectParentFrame()
        {
            if (this.ParentFrame != null)
            {
                UIUtilityProvider.UIHelper.SelectPopUpFrameByName(ParentFrame);
            }
        }

        public void SelectByIndex()
        {
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(Index);
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
