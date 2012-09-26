namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HtmlButton
    {
        public void CreateButton(DataCollection.HtmlButton button)
        {
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Link_ButtonDesigner_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.SelectByName();

            if (button.Market_Selection != DataCollection.HtmlButton.MarketSelection.Events)
            {
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.ProgressBarStep_Click(PageObject.Manager.Dashboard.ButtonDesigner.ProgressBarStep.MarketSelection);
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.MarketSelection_Select(button.Market_Selection);
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.NextStep_Click();
            }

            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.BuildIt_Click(button);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.SetKeyPhrase(button);
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.GenerateCode_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.SetGeneratedCodeHtml(button);
        }

        public void OpenSavedCodeHtmlInBrowserAndVerify(DataCollection.HtmlButton button, string fileName)
        {
            string fileUrl = string.Format("file:///{0}", button.CodeHtmlFile_FullPath.Replace('\\', '/'));

        }
    }
}
