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
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(CreateNewContentLocator, LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void OpenContent(string contentName)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(contentName, LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void TypeContentTitle(string title)
        {
            // Click to set focus, otherwise the focus will be set on the design frame
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(TitleLocator, LocateBy.Id);

            WebDriverUtility.DefaultProvider.Type(TitleLocator, title, LocateBy.Id);
        }

        public void SwitchToHTMLView()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(HTMLViewLocator, LocateBy.ClassName);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SwitchToPreviewMode()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(PreviewViewLocator, LocateBy.ClassName);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void SwitchToDesignMode()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(DesignViewLocator, LocateBy.ClassName);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }

        public void TypeContentInHTMLView(string HTMLcontent)
        {
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(1);
            WebDriverUtility.DefaultProvider.Type(HTMLContentLocator, HTMLcontent, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
        }

        public void TypeContentInDesignView(string content)
        {
            WebDriverUtility.DefaultProvider.TypeContentEditorOnWindowById(DesignContentLocator, content); 
        }

        [Step]
        public void TypeContentInWizardHTMLView(string content)
        {
            WebDriverUtility.DefaultProvider.SelectIFrameOnCurrentIFrame(2);
            WebDriverUtility.DefaultProvider.Type(HTMLContentLocator, content, LocateBy.XPath);
            this.SelectWizardFrame();
        }

        public string GetContentFromPreview()
        {
            Utilities.Utility.ThreadSleep(2);
            string content = string.Empty;
            WebDriverUtility.DefaultProvider.SelectIFrame(0);
            content = WebDriverUtility.DefaultProvider.GetPageSource();
            string[] body = Regex.Split(content, @"<body>|</body>");
            WebDriverUtility.DefaultProvider.SwitchToMainContent();
            return body[1];
        }

        public void SaveCloseContentTemplate()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(SaveCloseContentTemplateLocator, LocateBy.XPath);
            WebDriverUtility.DefaultProvider.WaitForPageToLoad();
        }

        [Step]
        public void SaveContentAsTemplate(string name)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(SaveAsContentTemplateLocator, LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            WebDriverUtility.DefaultProvider.Type(ContentTemplateNameLocator, name, LocateBy.Id);
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(SaveContentTemplateLocator, LocateBy.LinkText);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
        }
    }
}
