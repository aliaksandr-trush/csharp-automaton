namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.UIUtility;

    public class HtmlButton
    {
        public void CreateButton(DataCollection.HtmlButton button)
        {
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Link_ButtonDesigner_Click();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.SelectByName();
            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.OpenInnerFrameUrl();

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
            this.OpenSavedCodeHtmlInBrowserAndVerify(button);
        }

        public void OpenSavedCodeHtmlInBrowserAndVerify(DataCollection.HtmlButton button)
        {
            string fileUrl = string.Format("file:///{0}", button.CodeHtmlFile_FullPath.Replace('\\', '/'));
            PageObject.PageObjectHelper.NavigateTo(fileUrl);

            UIUtil.DefaultProvider.VerifyValue(
                button.Button_KeyPhrase_Text, 
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.Link_ButtonKeyword.Text,
                "Verify button key-phrase text.");

            UIUtil.DefaultProvider.VerifyValue(
                button.Button_KeyPhrase_Link,
                PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.Link_ButtonKeyword.GetAttribute("href"),
                "Verify button key-phrase href.");

            PageObject.PageObjectProvider.Manager.Dashboard.EventDetails.Frame_ButtonDesigner.RegisterNow_ClickToOpen();

            UIUtil.DefaultProvider.VerifyValue(
                button.Evt.Id.ToString(), 
                UIUtil.DefaultProvider.GetQueryStringValue("EventID"), 
                "Verify event id in url after clicking 'Register now'");
        }
    }
}
