namespace RegOnline.RegressionTest.Managers.Builder
{
    using RegOnline.RegressionTest.Utilities;
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using System.Threading;
    using RegOnline.RegressionTest.Attributes;

    public class SurveyManager : ManagerBase
    {
        private const string CreateCustomQuestion = "Create Custom Question";
        private const string AddCustomQuestionLink = "Add Custom Question";
        private const string ContentEditorLocator = "ctl00_cph_reContentEditor";
        
        public void ClickAddCustomQuestion()
        {
            if (WebDriverUtility.DefaultProvider.IsElementPresent(AddCustomQuestionLink, LocateBy.LinkText))
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(AddCustomQuestionLink, LocateBy.LinkText);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            }
            else
            {
                WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(CreateCustomQuestion, LocateBy.LinkText);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            }
        }

        public void SetCustomQuestionName(string name)
        {
            WebDriverUtility.DefaultProvider.Type("ctl00_cph_ucCF_mipNam_elDesc_TextArea", name, LocateBy.Id);
        }

        public void SetCustomQuestionVisibilities(bool? visible, bool? required, bool? admin)
        {
            if (visible.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_ucCF_chkActive", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_ucCF_chkRequired", required.Value, LocateBy.Id);
            }
            if (admin.HasValue)
            {
                WebDriverUtility.DefaultProvider.SetCheckbox("ctl00_cph_ucCF_chkAdminOnly", admin.Value, LocateBy.Id);
            }
        }

        public void SetIntroductoryMessage(string message)
        {
            WebDriverUtility.DefaultProvider.TypeContentEditorOnWindowById(ContentEditorLocator, message);
        }

        [Verify]
        public void VerifyIntroductoryMessageWhenPreview(string expectedMessage)
        {
            SelectPreviewFrame();

            VerifyTool.VerifyValue(
                expectedMessage,
                WebDriverUtility.DefaultProvider.GetText("//div[@id='pageContent']/p", LocateBy.XPath), 
                "Introductory message: {0}");

            SelectBuilderWindow();
        }

        public void SetConfirmationMessage(string message)
        {
            WebDriverUtility.DefaultProvider.TypeContentEditorOnWindowById(ContentEditorLocator, message);
        }

        [Verify]
        public void VerifyConfirmationMessageWhenPreview(string expectedMessage)
        {
            SelectPreviewFrame();

            VerifyTool.VerifyValue(
                expectedMessage,
                WebDriverUtility.DefaultProvider.GetText("//div[@id='pageContent']/p", LocateBy.XPath),
                "Confirmation message: {0}");

            SelectBuilderWindow();
        }

        public void SaveQuestion()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Save Item']/..", LocateBy.XPath);
            Thread.Sleep(1500);
        }

        public void SaveAndNewQuestion()
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("//span[text()='Save & New']/..", LocateBy.XPath);
            Thread.Sleep(1500);
        }
    }
}