namespace RegOnline.RegressionTest.PageObject.Manager.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;

    public class ButtonDesigner : Frame
    {
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

        

        public ButtonDesigner(string name)
            : base(name)
        { }

        public void BuildIt_Click(GraphicType type, bool isPreview)
        {
            if (isPreview)
            {
                switch (type)
                {
                    case GraphicType.Button:
                        this.BuildIt_Button_Preview.WaitForDisplay();
                        this.BuildIt_Button_Preview.Click();
                        break;
                    case GraphicType.Secure:
                        this.BuildIt_Secure_Preview.WaitForDisplay();
                        this.BuildIt_Secure_Preview.Click();
                        break;
                    case GraphicType.TextLink:
                        this.BuildIt_TextLink_Preview.WaitForDisplay();
                        this.BuildIt_TextLink_Preview.Click();
                        break;
                    case GraphicType.CountdownWidget:
                        this.BuildIt_CountdownWidget_Preview.WaitForDisplay();
                        this.BuildIt_CountdownWidget_Preview.Click();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case GraphicType.Button:
                        this.BuildIt_Button.WaitForDisplay();
                        this.BuildIt_Button.Click();
                        break;
                    case GraphicType.Secure:
                        this.BuildIt_Secure.WaitForDisplay();
                        this.BuildIt_Secure.Click();
                        break;
                    case GraphicType.TextLink:
                        this.BuildIt_TextLink.WaitForDisplay();
                        this.BuildIt_TextLink.Click();
                        break;
                    case GraphicType.CountdownWidget:
                        this.BuildIt_CountdownWidget.WaitForDisplay();
                        this.BuildIt_CountdownWidget.Click();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
