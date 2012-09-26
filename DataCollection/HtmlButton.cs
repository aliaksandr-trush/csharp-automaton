namespace RegOnline.RegressionTest.DataCollection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Utilities;
    using System.IO;

    public class HtmlButton
    {
        public enum MarketSelection
        {
            Camps,
            Events
        }

        public enum GraphicType
        {
            Button = 0,
            Secure,
            TextLink,
            CountdownWidget
        }

        public enum ButtonTheme
        {
            OrangeWithArrow,
            Grey,
            GreyWithLock,
            Blue,
            BlueWithLock,
            Purple,
            Green
        }

        public enum KeyPhrase
        {
            [CustomStringAttribute("event management software")]
            [CustomStringAttribute("http://www.regonline.com/__articles/products/event-management-software?utm_source=RegOnline&utm_medium=Button&utm_campaign=Event%2BManagement")]
            EventManagementSoftware,

            [CustomStringAttribute("event registration software")]
            [CustomStringAttribute("http://www.regonline.com/__articles/products/online-event-registration-software?utm_source=RegOnline&utm_medium=Button&utm_campaign=Event%2BRegistration")]
            EventRegistrationSoftware,

            [CustomStringAttribute("online registration form")]
            [CustomStringAttribute("http://www.regonline.com/__features/details/online-registration-form?utm_source=RegOnline&utm_medium=Button&utm_campaign=Registration%2BForm")]
            OnlineRegistrationForm,

            [CustomStringAttribute("event marketing")]
            [CustomStringAttribute("http://www.regonline.com/__features/details/event-marketing-tools?utm_source=RegOnline&utm_medium=Button&utm_campaign=Marketing%2BTools")]
            EventMarketing,

            [CustomStringAttribute("event websites")]
            [CustomStringAttribute("http://www.regonline.com/__features/details/event-website?utm_source=RegOnline&utm_medium=Button&utm_campaign=Event%2BWebsite")]
            EventWebsites,

            [CustomStringAttribute("event badges")]
            [CustomStringAttribute("http://www.regonline.com/__features/details/badge-maker?utm_source=RegOnline&utm_medium=Button&utm_campaign=Badge%2BMaker")]
            EventBadges,

            [CustomStringAttribute("event surveys")]
            [CustomStringAttribute("http://www.regonline.com/__features/details/online-event-surveys?utm_source=RegOnline&utm_medium=Button&utm_campaign=Event%2BSurveys")]
            EventSurveys
        }

        public MarketSelection Market_Selection { get; set; }
        public GraphicType Graphic_Type { get; set; }
        public bool WithPreview { get; set; }
        public ButtonTheme Button_Theme { get; set; }
        public string ButtonText { get; set; }
        public KeyPhrase Button_KeyPhrase { get; set; }
        public string CodeHtml { get; set; }
        public string CodeHtmlFile_FullPath { get; set; }
        
        public string Button_KeyPhrase_Text
        {
            get
            {
                return CustomStringAttribute.GetCustomString(this.Button_KeyPhrase);
            }
        }

        public string Button_KeyPhrase_Link
        {
            get
            {
                return CustomStringAttribute.GetCustomStrings(this.Button_KeyPhrase)[1];
            }
        }

        public HtmlButton()
        {
            this.Market_Selection = MarketSelection.Events;
            this.Graphic_Type = GraphicType.Button;
            this.WithPreview = true;
            this.Button_Theme = ButtonTheme.OrangeWithArrow;
        }

        public void SaveCodeHtmlToFile()
        {
            string fileRelativePath = string.Format("ButtonDesigner/{0}", this.Graphic_Type.ToString());

            FileStream fileStream = new FileStream(
                fileRelativePath, 
                FileMode.Create, 
                FileAccess.ReadWrite, 
                FileShare.None);

            StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(this.CodeHtml);
            writer.Close();
            this.CodeHtmlFile_FullPath = Path.GetFullPath(fileRelativePath);
        }
    }
}
