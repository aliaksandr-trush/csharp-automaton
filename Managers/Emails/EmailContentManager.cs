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
            UIUtil.DefaultProvider.WaitForDisplayAndClick(CreateNewContentLocator, LocateBy.LinkText);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void OpenContent(string contentName)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(contentName, LocateBy.LinkText);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void TypeContentTitle(string title)
        {
            // Click to set focus, otherwise the focus will be set on the design frame
            UIUtil.DefaultProvider.WaitForDisplayAndClick(TitleLocator, LocateBy.Id);

            UIUtil.DefaultProvider.Type(TitleLocator, title, LocateBy.Id);
        }

        public void SwitchToHTMLView()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(HTMLViewLocator, LocateBy.ClassName);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SwitchToPreviewMode()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(PreviewViewLocator, LocateBy.ClassName);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void SwitchToDesignMode()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(DesignViewLocator, LocateBy.ClassName);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }

        public void TypeContentInHTMLView(string HTMLcontent)
        {
            UIUtil.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            UIUtil.DefaultProvider.Type(HTMLContentLocator, HTMLcontent, LocateBy.XPath);
            UIUtil.DefaultProvider.SwitchToMainContent();
        }

        public void TypeContentInDesignView(string content)
        {
            UIUtil.DefaultProvider.TypeContentEditorOnWindowById(DesignContentLocator, content); 
        }

        [Step]
        public void TypeContentInWizardHTMLView(string content)
        {
            UIUtil.DefaultProvider.SelectIFrameOnCurrentIFrame(2);
            UIUtil.DefaultProvider.Type(HTMLContentLocator, content, LocateBy.XPath);
            this.SelectWizardFrame();
        }

        public string GetContentFromPreview()
        {
            Utilities.Utility.ThreadSleep(2);
            string content = string.Empty;
            UIUtil.DefaultProvider.SelectIFrame(0);
            content = UIUtil.DefaultProvider.GetPageSource();
            string[] body = Regex.Split(content, @"<body>|</body>");
            UIUtil.DefaultProvider.SwitchToMainContent();
            return body[1];
        }

        public void SaveCloseContentTemplate()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(SaveCloseContentTemplateLocator, LocateBy.XPath);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void SaveContentAsTemplate(string name)
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(SaveAsContentTemplateLocator, LocateBy.LinkText);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
            UIUtil.DefaultProvider.Type(ContentTemplateNameLocator, name, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForDisplayAndClick(SaveContentTemplateLocator, LocateBy.LinkText);
            UIUtil.DefaultProvider.WaitForAJAXRequest();
        }
    }
}
