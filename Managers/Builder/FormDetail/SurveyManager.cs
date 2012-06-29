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
            if (UIUtilityProvider.UIHelper.IsElementPresent(AddCustomQuestionLink, LocateBy.LinkText))
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(AddCustomQuestionLink, LocateBy.LinkText);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
            else
            {
                UIUtilityProvider.UIHelper.WaitForDisplayAndClick(CreateCustomQuestion, LocateBy.LinkText);
                UIUtilityProvider.UIHelper.WaitForAJAXRequest();
            }
        }

        public void SetCustomQuestionName(string name)
        {
            UIUtilityProvider.UIHelper.Type("ctl00_cph_ucCF_mipNam_elDesc_TextArea", name, LocateBy.Id);
        }

        public void SetCustomQuestionVisibilities(bool? visible, bool? required, bool? admin)
        {
            if (visible.HasValue)
            {
                UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_ucCF_chkActive", visible.Value, LocateBy.Id);
            }
            if (required.HasValue)
            {
                UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_ucCF_chkRequired", required.Value, LocateBy.Id);
            }
            if (admin.HasValue)
            {
                UIUtilityProvider.UIHelper.SetCheckbox("ctl00_cph_ucCF_chkAdminOnly", admin.Value, LocateBy.Id);
            }
        }

        public void SetIntroductoryMessage(string message)
        {
            UIUtilityProvider.UIHelper.TypeContentEditorOnWindowById(ContentEditorLocator, message);
        }

        [Verify]
        public void VerifyIntroductoryMessageWhenPreview(string expectedMessage)
        {
            SelectPreviewFrame();

            VerifyTool.VerifyValue(
                expectedMessage,
                UIUtilityProvider.UIHelper.GetText("//div[@id='pageContent']/p", LocateBy.XPath), 
                "Introductory message: {0}");

            SelectBuilderWindow();
        }

        public void SetConfirmationMessage(string message)
        {
            UIUtilityProvider.UIHelper.TypeContentEditorOnWindowById(ContentEditorLocator, message);
        }

        [Verify]
        public void VerifyConfirmationMessageWhenPreview(string expectedMessage)
        {
            SelectPreviewFrame();

            VerifyTool.VerifyValue(
                expectedMessage,
                UIUtilityProvider.UIHelper.GetText("//div[@id='pageContent']/p", LocateBy.XPath),
                "Confirmation message: {0}");

            SelectBuilderWindow();
        }

        public void SaveQuestion()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Save Item']/..", LocateBy.XPath);
            Thread.Sleep(1500);
        }

        public void SaveAndNewQuestion()
        {
            UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//span[text()='Save & New']/..", LocateBy.XPath);
            Thread.Sleep(1500);
        }
    }
}