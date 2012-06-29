namespace RegOnline.RegressionTest.Managers.Emails
{
    using System.Text.RegularExpressions;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public partial class EmailManager : ManagerBase
    {
        private const string CreateNewContentLocator = "Create Content Template";
        private const string TitleLocator = "ctl00_ctl00_cphDialog_cpMgrMain_txtTitle";
        private const string HTMLViewLocator = "reMode_html";
        private const string HTMLContentLocator = "//textarea";
        private const string PreviewViewLocator = "reMode_preview";
        private const string PreviewContentLocator = "//html/head/body";
        private const string DesignViewLocator = "reMode_design";
        private const string DesignContentLocator = "ctl00_ctl00_cphDialog_cpMgrMain_reContentEditor_contentIframe";
        private const string SaveAsContentTemplateLocator = "Save as Content Template";
        private const string ContentTemplateNameLocator = "ctl00_cphDialog_wzEmailInvitation_txtSaveTemplate";
        private const string SaveContentTemplateLocator = "Save";
        private const string SaveCloseContentTemplateLocator = "//div/span[2][@class='BiggerButtonBase']/a";

        [Step]
        public void ClickCreateContentTemplate()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CreateNewContentLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void OpenContent(string contentName)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(contentName, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void TypeContentTitle(string title)
        {
            // Click to set focus, otherwise the focus will be set on the design frame
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(TitleLocator, LocateBy.Id);

            UIUtilityProvider.UIHelper.Type(TitleLocator, title, LocateBy.Id);
        }

        public void SwitchToHTMLView()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(HTMLViewLocator, LocateBy.ClassName);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SwitchToPreviewMode()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(PreviewViewLocator, LocateBy.ClassName);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void SwitchToDesignMode()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(DesignViewLocator, LocateBy.ClassName);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }

        public void TypeContentInHTMLView(string HTMLcontent)
        {
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(1);
            UIUtilityProvider.UIHelper.Type(HTMLContentLocator, HTMLcontent, LocateBy.XPath);
            UIUtilityProvider.UIHelper.SwitchToMainContent();
        }

        public void TypeContentInDesignView(string content)
        {
            UIUtilityProvider.UIHelper.TypeContentEditorOnWindowById(DesignContentLocator, content); 
        }

        [Step]
        public void TypeContentInWizardHTMLView(string content)
        {
            UIUtilityProvider.UIHelper.SelectIFrameOnCurrentIFrame(2);
            UIUtilityProvider.UIHelper.Type(HTMLContentLocator, content, LocateBy.XPath);
            this.SelectWizardFrame();
        }

        public string GetContentFromPreview()
        {
            Utilities.Utility.ThreadSleep(2);
            string content = string.Empty;
            UIUtilityProvider.UIHelper.SelectIFrame(0);
            content = UIUtilityProvider.UIHelper.GetPageSource();
            string[] body = Regex.Split(content, @"<body>|</body>");
            UIUtilityProvider.UIHelper.SwitchToMainContent();
            return body[1];
        }

        public void SaveCloseContentTemplate()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveCloseContentTemplateLocator, LocateBy.XPath);
            UIUtilityProvider.UIHelper.WaitForPageToLoad();
        }

        [Step]
        public void SaveContentAsTemplate(string name)
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveAsContentTemplateLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            UIUtilityProvider.UIHelper.Type(ContentTemplateNameLocator, name, LocateBy.Id);
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveContentTemplateLocator, LocateBy.LinkText);
            UIUtilityProvider.UIHelper.WaitForAJAXRequest();
        }
    }
}
