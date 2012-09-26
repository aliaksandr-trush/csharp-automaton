namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class ButtonDesigner : Frame
    {
        public enum ProgressBarStep
        {
            MarketSelection,
            GraphicType,
            Style,
            GetCode
        }

        private MultiChoiceDropdown MarketSelection = new MultiChoiceDropdown("response_market", UIUtility.LocateBy.Id);
        private Clickable NextStep = new Clickable("market_button", UIUtility.LocateBy.Id);
        private Clickable BuildIt_Button_Preview = new Clickable("buildit_button_preview", UIUtility.LocateBy.Id);
        private Clickable BuildIt_Button = new Clickable("buildit_button", UIUtility.LocateBy.Id);
        private Clickable BuildIt_Secure_Preview = new Clickable("buildit_secure_preview", UIUtility.LocateBy.Id);
        private Clickable BuildIt_Secure = new Clickable("buildit_secure", UIUtility.LocateBy.Id);
        private Clickable BuildIt_TextLink_Preview = new Clickable("buildit_link_preview", UIUtility.LocateBy.Id);
        private Clickable BuildIt_TextLink = new Clickable("buildit_link", UIUtility.LocateBy.Id);
        private Clickable BuildIt_CountdownWidget_Preview = new Clickable("buildit_widget_preview", UIUtility.LocateBy.Id);
        private Clickable BuildIt_CountdownWidget = new Clickable("buildit_widget", UIUtility.LocateBy.Id);
        private Clickable ProgressBarStep_Button_MarketSelection = new Clickable("progress_step1", UIUtility.LocateBy.Id);
        private Clickable ProgressBarStep_Button_GraphicType = new Clickable("progress_step2", UIUtility.LocateBy.Id);
        private Clickable ProgressBarStep_Button_Style = new Clickable("progress_step3", UIUtility.LocateBy.Id);
        private Clickable ProgressBarStep_Button_GetCode = new Clickable("progress_step4", UIUtility.LocateBy.Id);
        private Clickable Link_ButtonKeyword = new Clickable("//div[@id='btn_foot']/a", UIUtility.LocateBy.XPath);
        private Clickable Button_GenerateCode = new Clickable("generate_code", UIUtility.LocateBy.Id);

        public ButtonDesigner(string name)
            : base(name)
        { }

        public void MarketSelection_Select(DataCollection.HtmlButton.MarketSelection market)
        {
            this.MarketSelection.WaitForDisplay();
            this.MarketSelection.SelectWithText(market.ToString());
        }

        public void NextStep_Click()
        {
            this.NextStep.WaitForDisplayAndClick();
            Utilities.Utility.ThreadSleep(2);
            WaitForAJAX();
            WaitForLoad();
        }

        public void BuildIt_Click(DataCollection.HtmlButton button)
        {
            if (button.WithPreview)
            {
                switch (button.Graphic_Type)
                {
                    case DataCollection.HtmlButton.GraphicType.Button:
                        this.BuildIt_Button_Preview.WaitForDisplay();
                        this.BuildIt_Button_Preview.Click();
                        break;
                    case DataCollection.HtmlButton.GraphicType.Secure:
                        this.BuildIt_Secure_Preview.WaitForDisplay();
                        this.BuildIt_Secure_Preview.Click();
                        break;
                    case DataCollection.HtmlButton.GraphicType.TextLink:
                        this.BuildIt_TextLink_Preview.WaitForDisplay();
                        this.BuildIt_TextLink_Preview.Click();
                        break;
                    case DataCollection.HtmlButton.GraphicType.CountdownWidget:
                        this.BuildIt_CountdownWidget_Preview.WaitForDisplay();
                        this.BuildIt_CountdownWidget_Preview.Click();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (button.Graphic_Type)
                {
                    case DataCollection.HtmlButton.GraphicType.Button:
                        this.BuildIt_Button.WaitForDisplay();
                        this.BuildIt_Button.Click();
                        break;
                    case DataCollection.HtmlButton.GraphicType.Secure:
                        this.BuildIt_Secure.WaitForDisplay();
                        this.BuildIt_Secure.Click();
                        break;
                    case DataCollection.HtmlButton.GraphicType.TextLink:
                        this.BuildIt_TextLink.WaitForDisplay();
                        this.BuildIt_TextLink.Click();
                        break;
                    case DataCollection.HtmlButton.GraphicType.CountdownWidget:
                        this.BuildIt_CountdownWidget.WaitForDisplay();
                        this.BuildIt_CountdownWidget.Click();
                        break;
                    default:
                        break;
                }
            }

            Utilities.Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void ProgressBarStep_Click(ProgressBarStep step)
        {
            switch (step)
            {
                case ProgressBarStep.MarketSelection:
                    this.ProgressBarStep_Button_MarketSelection.WaitForDisplayAndClick();
                    break;
                case ProgressBarStep.GraphicType:
                    this.ProgressBarStep_Button_GraphicType.WaitForDisplayAndClick();
                    break;
                case ProgressBarStep.Style:
                    this.ProgressBarStep_Button_Style.WaitForDisplayAndClick();
                    break;
                case ProgressBarStep.GetCode:
                    this.ProgressBarStep_Button_GetCode.WaitForDisplayAndClick();
                    break;
                default:
                    break;
            }

            Utilities.Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void SetKeyPhrase(DataCollection.HtmlButton button)
        {
            string text = this.Link_ButtonKeyword.Text;

            foreach (DataCollection.HtmlButton.KeyPhrase phrase in Enum.GetValues(typeof(DataCollection.HtmlButton.KeyPhrase)))
            {
                if (text.Equals(Utilities.CustomStringAttribute.GetCustomString(button.Button_KeyPhrase)))
                {
                    button.Button_KeyPhrase = phrase;
                }
            }

            UIUtility.UIUtil.DefaultProvider.FailTest(string.Format("No matching key phrase for '{0}'", text));
        }

        public void GenerateCode_Click()
        {
            this.Button_GenerateCode.WaitForDisplayAndClick();
            Utilities.Utility.ThreadSleep(2);
            WaitForAJAX();
        }

        public void SetGeneratedCodeHtml(DataCollection.HtmlButton button)
        {
            ElementBase TextArea_GeneratedCode = new ElementBase(string.Format("code_{0}", (int)button.Graphic_Type), UIUtility.LocateBy.Id);

            StringBuilder script = new StringBuilder();
            script.Append(string.Format("var textArea_GeneratedCode = document.getElementById('{0}');", TextArea_GeneratedCode.Locator));
            script.Append(string.Format("textArea_GeneratedCode.setAttribute('value', textArea_GeneratedCode.value);", TextArea_GeneratedCode.Locator));

            UIUtility.UIUtil.DefaultProvider.ExecuteJavaScript(script.ToString());
            button.CodeHtml = TextArea_GeneratedCode.GetAttribute("value");
        }
    }
}
