namespace RegOnline.RegressionTest.Fixtures.FormTypes.ProEvent
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Manager;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

    public class ButtonDesignerFixture : FixtureBase
    {
        private const string BaseFormName = "ButtonDesigner- {0}";
        private const string BaseFileText = "<header>{0}</header>";

        private string formName = string.Empty;
        private StreamWriter htmlButtons;
        private int eventId = 0;
        private string SessionId = string.Empty;

        [Test]
        [Category(Priority.Four)]
        [Description("680")]
        public void ChecklistTest_ButtonDesigner()
        {
            formName = BuildEventFixture.EventName;
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            ManagerSiteMgr.GoToEventsTabIfNeeded();
            ManagerSiteMgr.SelectFolder();
            htmlButtons = NewFile(string.Format(@"\\ws0034qaauto01\C$\QA\ButtonDesigner\ChecklistTest_{0}.html", DateTime.Now.ToString("yyMMdd_HHmm")));
            ManagerSiteMgr.OpenEventDashboard(formName);

            //Text right here- register
            GenerateButton(ButtonDesignerManager.ButtonType.Text,
                ButtonDesignerManager.ButtonDestination.Form, false,
                null, "Right here!");

            //Button new window- website
            GenerateButton(ButtonDesignerManager.ButtonType.Button,
                ButtonDesignerManager.ButtonDestination.Website, true,
                ButtonDesignerManager.ButtonStyle.Style1, "In a new window!");

            //Flash widget new window- calendar
            GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                ButtonDesignerManager.ButtonDestination.Calendar, true,
                null, null);

            //IFrame- register
            GenerateButton(ButtonDesignerManager.ButtonType.InlineForm,
                ButtonDesignerManager.ButtonDestination.Form, null,
                null, null);

            htmlButtons.Close();
        }

        public void GenerateMembershipButtons()
        {
            formName = string.Format(BaseFormName, "Membership");

            // open login page on BETA
            ManagerSiteMgr.OpenLogin();

            try
            {
                // login to regression testing account
                ManagerSiteMgr.Login();
                ManagerSiteMgr.GoToEventsTabIfNeeded();
                ManagerSiteMgr.SelectFolder();
                ManagerSiteMgr.DeleteEventByName(formName);
                BuildBasicMembership(formName);

                htmlButtons = NewFile(string.Format(@"\\ws0034qaauto01\C$\QA\ButtonDesigner\Membership_{0}.html", DateTime.Now.ToString("yyMMdd_HHmm")));
                ManagerSiteMgr.OpenEventDashboard(formName);

                // All possible for a text box, same window
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Form, false,
                    null, "Right here!");
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Website, false,
                    null, "Right here!");
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Calendar, false,
                    null, "Right here!");
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Renewal, false,
                    null, "Right here!");

                // All possible for a text box, new window
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Form, true,
                    null, "In a new window!");
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Website, true,
                    null, "In a new window!");
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Calendar, true,
                    null, "In a new window!");
                GenerateButton(ButtonDesignerManager.ButtonType.Text,
                    ButtonDesignerManager.ButtonDestination.Renewal, true,
                    null, "In a new window!");

                // All styles for a button, mix of new/same window, mix of destinations
                GenerateButton(ButtonDesignerManager.ButtonType.Button,
                    ButtonDesignerManager.ButtonDestination.Form, false,
                    ButtonDesignerManager.ButtonStyle.Style0, "Right here!");
                GenerateButton(ButtonDesignerManager.ButtonType.Button,
                    ButtonDesignerManager.ButtonDestination.Website, true,
                    ButtonDesignerManager.ButtonStyle.Style1, "In a new window!");
                GenerateButton(ButtonDesignerManager.ButtonType.Button,
                    ButtonDesignerManager.ButtonDestination.Calendar, false,
                    ButtonDesignerManager.ButtonStyle.Style2, "Right here!");
                GenerateButton(ButtonDesignerManager.ButtonType.Button,
                    ButtonDesignerManager.ButtonDestination.Renewal, true,
                    ButtonDesignerManager.ButtonStyle.Style3, "In a new window!");
                GenerateButton(ButtonDesignerManager.ButtonType.Button,
                    ButtonDesignerManager.ButtonDestination.Website, false,
                    ButtonDesignerManager.ButtonStyle.Style4,
                    "I've got a lot of text, a lot of text, a lot of text, I've got a lot of text and you just have to deal, hah!");

                // All destinations for flash widget, same window
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Form, false,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Website, false,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Calendar, false,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Renewal, false,
                    null, null);

                // All destinations for flash widget, new window
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Form, true,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Website, true,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Calendar, true,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.FlashWidget,
                    ButtonDesignerManager.ButtonDestination.Renewal, true,
                    null, null);

                // All destinations for iframe
                GenerateButton(ButtonDesignerManager.ButtonType.InlineForm,
                    ButtonDesignerManager.ButtonDestination.Form, null,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.InlineForm,
                    ButtonDesignerManager.ButtonDestination.Website, null,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.InlineForm,
                    ButtonDesignerManager.ButtonDestination.Calendar, null,
                    null, null);
                GenerateButton(ButtonDesignerManager.ButtonType.InlineForm,
                    ButtonDesignerManager.ButtonDestination.Renewal, null,
                    null, null);

                htmlButtons.Close();
            }
            catch
            {
                // doesn't seem to be working on integrator...
                UIUtilityProvider.UIHelper.CaptureFailureScreenshot();
                throw;
            }
            finally
            {
                htmlButtons.Close();
            }
        }

        [Step]
        private void GenerateButton(ButtonDesignerManager.ButtonType buttonType,
            ButtonDesignerManager.ButtonDestination buttonDest,
            bool? newWindow,
            ButtonDesignerManager.ButtonStyle? buttonStyle,
            string buttonText)
        {
            htmlButtons.WriteLine(string.Format(BaseFileText, formName));
            string typeAndDest = "Type: " + buttonType.ToString() + "  Destination: " + buttonDest.ToString();
            string extraInfo = string.Empty;

            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventAdditionalFunction.ButtonDesigner);
            UIUtilityProvider.UIHelper.SelectPopUpFrameByName("dialog");
            ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickField(buttonType);
            ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickButtonDesignerNext();
            ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickField(buttonDest);
            if (buttonType != ButtonDesignerManager.ButtonType.InlineForm)
            {
                if (newWindow == true)
                {
                    ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickField(ButtonDesignerManager.ButtonWindowOption.New);
                    typeAndDest = string.Concat(typeAndDest, " (new window)");
                }
                else
                {
                    typeAndDest = string.Concat(typeAndDest, " (same window)");
                }
                ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickButtonDesignerNext();

                if (buttonType != ButtonDesignerManager.ButtonType.FlashWidget)
                {
                    ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.SetFieldValue(ButtonDesignerManager.ButtonLinkText.Text, buttonText);
                    if (buttonType == ButtonDesignerManager.ButtonType.Button)
                    {
                        ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickField(buttonStyle);
                        extraInfo = "Style: " + buttonStyle.ToString() + "  Text: " + buttonText;
                    }
                    else
                    {
                        extraInfo = "Text: " + buttonText;
                    }
                }
            }
            ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickButtonDesignerNext();
            string buttonCode = ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.GetButtonCode();
            ManagerSiteMgr.DashboardMgr.ButtonDesignerMgr.ClickButtonDesignerClose();
            UIUtilityProvider.UIHelper.SwitchToMainContent();

            // Write to file
            htmlButtons.WriteLine(string.Format(BaseFileText, typeAndDest));
            if (!string.IsNullOrEmpty(extraInfo)) htmlButtons.WriteLine(string.Format(BaseFileText, extraInfo));
            htmlButtons.WriteLine(buttonCode);
        }

        private void BuildBasicMembership(string formName)
        {
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.Membership);

            // set up Start page
            BuilderMgr.VerifyStartPageInitialDefaults(ManagerSiteManager.EventType.Membership);
            BuilderMgr.SetEventNameAndShortcut(formName);
            BuilderMgr.SaveAndStay();

            // get event id
            eventId = BuilderMgr.GetEventId();

            //get sessionId
            SessionId = BuilderMgr.GetEventSessionId();

            // save and close
            BuilderMgr.SaveAndClose();
        }

        [Step]
        private StreamWriter NewFile(string nameAndPath)
        {
            FileStream fileStream = null;

            if (File.Exists(nameAndPath))
            {
                File.Delete(nameAndPath);
            }

            fileStream = new FileStream(nameAndPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            return new StreamWriter(fileStream);
        }
    }
}
