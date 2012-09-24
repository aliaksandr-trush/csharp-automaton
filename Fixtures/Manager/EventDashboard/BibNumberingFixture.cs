namespace RegOnline.RegressionTest.Fixtures.Manager.EventDashboard
{
    using System;
    using System.Collections.Generic;
    using Managers.Manager;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.Managers.Builder;
    using RegOnline.RegressionTest.Managers.Emails;
    using RegOnline.RegressionTest.Managers.Manager.Dashboard;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class BibNumberingFixture : FixtureBase
    {
        private const string EventNameWithRegType = "TeamWithRegTypes";
        private const string EventNameWithoutRegType = "TeamWithoutRegTypes";
        private const int StartNumber = 50;
        private const string EmailContent = @"<table cellpadding=""15"" style=""font-size: 12px;"">" +
            @"<tbody><tr>" +
            @"<td valign=""top"">" +
            @"<p>Thank you for registering.</p>" +
            @"<p>Your bib is /*Merge: BibNumber*/.</p>" +
            @"<p>Your team is /*Merge: TeamName*/.</p>" +
            @"</td></tr></tbody></table>";

        private int eventId;
        private string eventSessionId;
        private int regs;
        private string email;
        private string grouperEmail;
        private int registrantId;
        private string teamName;
        private List<BibNumberingToolManager.TeamWithRegTypes> _teams;
        private BibNumberingToolManager.TeamWithRegTypes withTeamName;
        private BibNumberingToolManager.TeamWithRegTypes noTeamName;
        private BibNumberingToolManager.TeamWithRegTypes noGroupReg;

        ////[Test]
        ////[Category(Priority.Three)]
        ////[Description("567")]
        public void BibNumberingTool()
        {
            //step 1
            //bib numbers are ActiveEurope only.
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            //Avoid the known bug which causes overlap. (per QA: It's not in regression scope.)
            //when preparing these, ensure that StartingNumber is far enough away from each other and from the event starting number.
            this.CreateTeamWithRegType();

            ManagerSiteMgr.OpenEventDashboard(eventId);

            //steps 2-3
            ManagerSiteMgr.ChangeBibNumberingOption(BibNumberingToolManager.AssignNumberToMember.UniqueToEach, _teams);

            //step 4
            //Register for the event using single registrations and group registrations for the different reg types.
            //Enter Team Name where indicated. [Also set for you: teams with and without a name.]
            RegisterMgr.CurrentEventId = this.eventId;

            //popup separate window
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.TestRegister);
            UIUtil.DefaultProvider.SelectWindowByTitle(EventNameWithRegType);
            UIUtil.DefaultProvider.WaitForPageToLoad();

            this.regs = 0;

            for (int i = 1; i < 4; i++)
            {
                Register(this.withTeamName);
                this.regs += 1;
            }
            for (int i = 1; i < 4; i++)
            {
                Register(this.noTeamName);
                this.regs += 1;
            }
            for (int i = 1; i < 4; i++)
            {
                Register(this.noGroupReg);
                this.regs += 1;
            }

            RegisterGroup(this.noTeamName, 2);
            this.regs += 2;

            RegisterGroup(this.withTeamName, 2);
            this.regs += 2;

            //shut down the confirmation page / register project.
            //if we were in the same window, must navigate back. That means: relogin, reenter the Manager.
            //if Register had popped up, must close popup.
            UIUtil.DefaultProvider.ClosePopUpWindow();

            //step 5
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();
            ReportMgr.VerifyUniquenessOnAttendeeReport(eventId);
            ReportMgr.CloseReportPopupWindow();

            //step 6
            UIUtil.DefaultProvider.SelectOriginalWindow();
            ManagerSiteMgr.ChangeBibNumberingOption(BibNumberingToolManager.AssignNumberToMember.SameToEvery, _teams);

            //step 7
            ManagerSiteMgr.DashboardMgr.OpenAttendeeReportFromEventDashboard();
            ReportMgr.VerifySameOnAttendeeReport(eventId);
            ReportMgr.CloseReportPopupWindow();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("637")]
        public void TeamWithoutRegType()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            this.CreatTeamWithoutRegType();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("636")]
        public void TeamWithRegType()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            this.CreateTeamWithRegType();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("258")]
        public void BibNumberingToolWithoutRegType()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            this.CreatTeamWithoutRegType();
            this.BibSetting();
            this.RegisterWitouRegType();
            this.CheckBibInReport(1, this.regs, StartNumber, null);
            this.ChangeBibSetting(BibNumberingToolManager.AssignNumberToMember.SameToEvery);
            this.RegisterWitouRegType();
            this.CheckBibInReport(1, this.regs, StartNumber, null);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("259")]
        public void BibNumberingToolUpdates()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            this.CreateTeamWithRegType();
            this.BibSetting();
            this.ChangeBibSetting(BibNumberingToolManager.AssignNumberToMember.UniqueToEach);
            this.Register(withTeamName);
            this.regs = 1;
            this.CheckBibInReport(1, this.regs, StartNumber, null);
            this.UpdateReg(this.email);
            this.CheckBibInReport(1, this.regs, StartNumber, null);
            this.SubstituteReg(this.email);
            this.CheckBibInReport(1, this.regs, StartNumber, null);
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            ManagerSiteMgr.DashboardMgr.DeleteTestReg_ClickDelete();

            this.RegisterGroup(withTeamName, 2);
            this.regs = 2;
            this.CheckBibInReport(1, this.regs, StartNumber, null);
            this.UpdateReg(this.grouperEmail);
            this.CheckBibInReport(1, this.regs, StartNumber, null);
            this.SubstituteReg(this.grouperEmail);
            this.CheckBibInReport(1, this.regs, StartNumber, null);
        }

        [Test]
        [Category(Priority.Three)]
        [Description("260")]
        public void BibNumberingTool_EventCreationAndRegister_AndCheckInReport_AndCheckMergeCodeInEmail()
        {
            this.BibNumberingTool();
            ManagerSiteMgr.DashboardMgr.ClickAttendeeReportLinkOnFormDashboard();
            ManagerSiteMgr.SelectReportPopupWindow();
            this.CheckBibNumber(1, 3, 101, null);
            this.CheckBibNumber(4, 6, 104, null);
            this.CheckBibNumber(7, 9, 34, null);
            this.CheckBibNumber(10, 11, 107, null);
            this.CheckBibNumber(12, 13, 109, true);
            VerifyTool.VerifyValue(this.teamName, ReportMgr.GetTeamName(this.regs), "Team name for group regs in the same team: {0}");
            ReportMgr.CloseReportPopupWindow();
            this.VerifyBibInEmail(109);
            this.CheckTeamNameInEmail();
        }

        [Test]
        [Category(Priority.Three)]
        [Description("261")]
        public void TeamNameWithRegType()
        {
            ConfigReader.DefaultProvider.ReloadAccount(ConfigReader.AccountEnum.ActiveEurope);

            this.CreateNewTeamWithRegType();
            this.RegisterAndCheckTeamName();
        }

        ////[Test]
        ////[Category(Priority.Three)]
        ////[Description("263")]
        public void TeamNameReportsAndMergeCode()
        {
            this.BibNumberingTool();
            this.CheckTeamNameInReport();
            this.CheckTeamNameInEmail();
        }

        public void CheckTeamNameInReport()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickAttendeeReportLinkOnFormDashboard();
            ManagerSiteMgr.SelectReportPopupWindow();

            Assert.True(ReportMgr.GetTeamName(this.regs) == this.teamName);

            ReportMgr.CloseReportPopupWindow();
        }

        public void CheckTeamNameInEmail()
        {
            EmailMgr.OpenConfirmationEmailUrl(EmailManager.EmailCategory.Complete, this.eventId, this.registrantId);
            EmailMgr.VerifyCustomFieldPresent(this.teamName, true);
        }

        private void CreateNewTeamWithRegType()
        {
            this.withTeamName = new BibNumberingToolManager.TeamWithRegTypes() { CollectTeamName = true, DisableGroupReg = false, RegistantTypeName = "WithTeamName", StartingNumber = -1 };
            this.noTeamName = new BibNumberingToolManager.TeamWithRegTypes() { CollectTeamName = false, DisableGroupReg = false, RegistantTypeName = "NoTeamName", StartingNumber = -1 };
            this.noGroupReg = new BibNumberingToolManager.TeamWithRegTypes() { CollectTeamName = false, DisableGroupReg = true, RegistantTypeName = "NoGroupReg", StartingNumber = 34 };

            _teams = new List<BibNumberingToolManager.TeamWithRegTypes>() { this.withTeamName, this.noGroupReg, this.noTeamName };

            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            if (ManagerSiteMgr.EventExists(EventNameWithRegType))
            {
                ManagerSiteMgr.DeleteEventByName(EventNameWithRegType);
            }
            
            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ActiveEuropeEvent);
            this.eventId = BuilderMgr.GetEventId();
            BuilderMgr.SetEventNameAndShortcut(EventNameWithRegType);
            BuilderMgr.SelectEventType(FormDetailManager.ActiveEuropeEventType.Running);
            BuilderMgr.AddRegTypes<BibNumberingToolManager.TeamWithRegTypes>(_teams);
            BuilderMgr.SaveAndClose();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            UIUtil.DefaultProvider.SelectPopUpFrameByName("plain");
            ManagerSiteMgr.DashboardMgr.DeleteTestReg_ClickDelete();
            UIUtil.DefaultProvider.SwitchToMainContent();
            ManagerSiteMgr.DashboardMgr.ReturnToList();

            this.GoToEmailTabAddConfirmation(EventNameWithRegType);
        }

        private void RegisterAndCheckTeamName()
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.Checkin();
            RegisterMgr.SelectRegType(withTeamName.RegistantTypeName);
            RegisterMgr.Continue();

            RegisterMgr.VerifyCustomFieldPresent("Team Name", true);
            RegisterMgr.VerifyCustomFieldRequired("Team Name", true);
        }

        private void VerifyBibInEmail(int bib)
        {
            EmailMgr.OpenConfirmationEmailUrl(EmailManager.EmailCategory.Complete, this.eventId, this.registrantId);
            EmailMgr.VerifyCustomFieldPresent(bib.ToString(), true);
        }

        private void UpdateReg(string emailAddress)
        {
            RegisterMgr.OpenRegisterPage(this.eventId);
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.Continue();
            RegisterMgr.Continue();
            RegisterMgr.ClickCheckoutActiveWaiver();
            RegisterMgr.FinishRegistration();
        }

        private void SubstituteReg(string emailAddress)
        {
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.CheckinWithEmail(emailAddress);
            RegisterMgr.ClickCheckinAlreadyRegistered();
            RegisterMgr.EnterPassword(ConfigReader.DefaultProvider.AccountConfiguration.Password);
            RegisterMgr.Continue();

            if (RegisterMgr.HasSubstituteLink(1))
                RegisterMgr.ClickSubstituteLink(1);
            else
                RegisterMgr.ClickSubstituteLink(0);

            string substituteEmail = "this" + DateTime.Now.Ticks.ToString() + "@isatest.com";
            RegisterMgr.TypePersonalInfoEmail(substituteEmail);
            RegisterMgr.EnterProfileInfoEnduranceNew();
            RegisterMgr.FinishRegistration();
        }

        private void BibSetting()
        {
            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventAdditionalFunction.BibNumberingTool);
            ManagerSiteMgr.SelectBibNumberWindow();
            ManagerSiteMgr.BibNumberTool.SetStartingNumberDefault(StartNumber);
            ManagerSiteMgr.BibNumberTool.SaveAndClose(true);
        }

        private void RegisterWitouRegType()
        {
            this.regs = 0;

            for (int i = 0; i < 3; i++)
            {
                this.Register(null);
                this.regs += 1;
            }
            for (int i = 0; i < 2; i++)
            {
                this.RegisterGroup(null, 2);
                this.regs += 2;
            }
        }

        private int CheckBibInReport(int firstReg, int lastReg, int startNumber, bool? same)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, this.eventSessionId);
            ManagerSiteMgr.DashboardMgr.ClickAttendeeReportLinkOnFormDashboard();
            ManagerSiteMgr.SelectReportPopupWindow();

            int lastBib = this.CheckBibNumber(firstReg, lastReg, startNumber, same);

            ReportMgr.CloseReportPopupWindow();

            return lastBib;
        }

        private int CheckBibNumber(int firstReg, int lastReg, int startNumber, bool? same)
        {
            int lastBib = -1;

            if (same.HasValue && same.Value == true)
            {
                for (int i = firstReg; i <= lastReg; i++)
                {
                    ////Assert.True(startNumber == ReportMgr.GetBibId(i + 1));
                    VerifyTool.VerifyValue(startNumber, ReportMgr.GetBibId(i), "Entry number: {0}");
                }
            }
            else
            {
                int j = 0;

                for (int i = firstReg; i <= lastReg; i++)
                {
                    int actualBibId = ReportMgr.GetBibId(i);

                    if (i == lastReg)
                    {
                        lastBib = actualBibId;
                    }

                    ////Assert.True(startNumber + i == ReportMgr.GetBibId(i + 1));
                    VerifyTool.VerifyValue(startNumber + j, actualBibId, "Entry number: {0}");
                    
                    j++;
                }
            }

            return lastBib;
        }

        private void ChangeBibSetting(BibNumberingToolManager.AssignNumberToMember assignMethod)
        {
            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventAdditionalFunction.BibNumberingTool);
            ManagerSiteMgr.SelectBibNumberWindow();
            ManagerSiteMgr.BibNumberTool.SetTeamNumbers(assignMethod);
            ManagerSiteMgr.BibNumberTool.SaveAndClose(true);

            ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            ManagerSiteMgr.DashboardMgr.DeleteTestReg_ClickDelete();
        }

        private void GoToEmailTabAddConfirmation(string eventName)
        {
            ManagerSiteMgr.ClickEditRegistrationForm(eventName);
            BuilderMgr.GotoTab(FormDetailManager.Tab.Emails);
            BuilderMgr.OpenEditConfirmationEmail(EmailManager.EmailCategory.Complete);
            BuilderMgr.SelectEmailEditFrame();
            BuilderMgr.SwitchModeInEmail(FormDetailManager.Mode.Html);
            BuilderMgr.TypeContentInHTML(EmailContent);
            BuilderMgr.SelectEmailEditFrame();
            BuilderMgr.SaveAndClose();
            BuilderMgr.SelectBuilderWindow();
            BuilderMgr.SaveAndClose();
        }

        private void CreatTeamWithoutRegType()
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            if (ManagerSiteMgr.EventExists(EventNameWithoutRegType))
            {
                ManagerSiteMgr.DeleteEventByName(EventNameWithoutRegType);
            }

            ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ActiveEuropeEvent);
            BuilderMgr.SetEventNameAndShortcut(EventNameWithoutRegType);
            BuilderMgr.SelectEventType(FormDetailManager.ActiveEuropeEventType.Running);
            BuilderMgr.SaveAndClose();

            this.eventId = ManagerSiteMgr.GetFirstEventId(EventNameWithoutRegType);

            this.GoToEmailTabAddConfirmation(EventNameWithoutRegType);
        }

        private void CreateTeamWithRegType()
        {
            this.withTeamName = new BibNumberingToolManager.TeamWithRegTypes() { CollectTeamName = true, DisableGroupReg = false, RegistantTypeName = "WithTeamName", StartingNumber = -1 };
            this.noTeamName = new BibNumberingToolManager.TeamWithRegTypes() { CollectTeamName = false, DisableGroupReg = false, RegistantTypeName = "NoTeamName", StartingNumber = -1 };
            this.noGroupReg = new BibNumberingToolManager.TeamWithRegTypes() { CollectTeamName = false, DisableGroupReg = true, RegistantTypeName = "NoGroupReg", StartingNumber = 34 };

            _teams = new List<BibNumberingToolManager.TeamWithRegTypes>() { this.withTeamName, this.noGroupReg, this.noTeamName };

            this.PrepareActiveEuropeEvent(EventNameWithRegType);
            this.GoToEmailTabAddConfirmation(EventNameWithRegType);
        }

        //what follows here are manager-type helper functions.
        //TODO: move to manager and abstract this stuff some more.

        //adapted from RegOnlineCheckInRegServiceFixture
        //ActiveEurope has its own special features
        private void PrepareActiveEuropeEvent(string eventName)
        {
            ManagerSiteMgr.OpenLogin();
            ManagerSiteMgr.Login();
            this.eventSessionId = ManagerSiteMgr.GetEventSessionId();
            ManagerSiteMgr.SelectFolder();

            ManagerSiteMgr.DeleteEventByName(eventName);

            //if (!ManagerSiteMgr.EventExists(eventName))
            //{
                ManagerSiteMgr.ClickAddEvent(ManagerSiteManager.EventType.ActiveEuropeEvent);
                this.eventId = BuilderMgr.GetEventId();
                BuilderMgr.SetEventNameAndShortcut(eventName);
                BuilderMgr.SelectEventType(FormDetailManager.ActiveEuropeEventType.Running);
                BuilderMgr.AddRegTypes<BibNumberingToolManager.TeamWithRegTypes>(_teams);
                BuilderMgr.SaveAndClose();
            //}
            //else
            //{
            //    this.eventId = ManagerSiteMgr.GetFirstEventId(eventName);

            //    //duped from RegistrationLimitsFixture.DeleteTestRegistrations.
            //    //why purge test regs?
            //    //I found that when you start with a set of registrants with "Assign the same number to every member of a team";
            //    //then you renumber the event with the tool to "Assign a unique number to each member of a team.",
            //    //and then add more and do "Assign the same number to every member of a team" again...
            //    //it can break apart the bib numbers in the first set, if they're in a group without a team name.
            //    //I see this as a bug; I don't think it should ever separate bibs within a group.
            //    //That test case is beyond the scope of this project.
            //    //what IS in scope is re-runnability.
            //    ManagerSiteMgr.OpenEventDashboardUrl(this.eventId, ManagerSiteMgr.GetEventSessionId());
            //    ManagerSiteMgr.DashboardMgr.ChooseTabAndVerify(DashboardManager.DashboardTab.EventDetails);
            //    ManagerSiteMgr.DashboardMgr.ClickOption(DashboardManager.EventRegistrationFunction.DeleteTestRegistrations);
            //    UIUtilityProvider.UIHelper.SelectPopUpFrameByName("plain");
            //    ManagerSiteMgr.DashboardMgr.DeleteTestReg_ClickDelete();
            //    UIUtilityProvider.UIHelper.SwitchToMainContent();
            //    ManagerSiteMgr.DashboardMgr.ReturnToList();
            //}
        }

        //duped from BackendFixture
        public void EnterPersonalInfoDuringRegistration(string firstName, string middleName, string lastName)
        {
            RegisterMgr.EnterPersonalInfoNamePrefixSuffix(null, firstName, middleName, lastName, null);
            RegisterMgr.EnterPersonalInfoTitleBadgeCompanyCountry("Agent", null, "The Matrix", null);
            RegisterMgr.EnterPersonalInfoAddress("4750 Walnut st", null, "Boulder", "Colorado", null, "80301");
            RegisterMgr.EnterPersonalInfoPhoneNumbers(null, "3035775100", null, null, null);

            if (RegisterMgr.HasPasswordTextbox())
            {
                RegisterMgr.EnterPersonalInfoPassword("321321");
            }
        }

        private int Register(BibNumberingToolManager.TeamWithRegTypes rt)
        {
            RegisterMgr.OpenRegisterPage(eventId);
            this.email = "this" + DateTime.Now.Ticks.ToString() + "@isatest.com";
            RegisterMgr.CheckinWithEmail(this.email);

            if (rt != null)
            {
                RegisterMgr.SelectRegType(rt.RegistantTypeName);
            }

            // Go to PI page
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfoEnduranceNew();

            if (rt != null)
            {
                if (rt.CollectTeamName)
                    RegisterMgr.TypePersonalInfoEnduranceTeamName("team " + DateTime.Now.Ticks.ToString());//one per group
            }

            // Go to checkout page
            RegisterMgr.Continue();

            //no payment... just a waiver
            RegisterMgr.ClickCheckoutActiveWaiver();
            RegisterMgr.FinishRegistration();

            return Convert.ToInt32(
                RegisterMgr.GetConfirmationPageValueForPrimaryAttendee(RegisterManager.ConfirmationPageField.RegistrationId));
        }

        private void RegisterGroup(BibNumberingToolManager.TeamWithRegTypes rt, int followerCount)
        {
            RegisterMgr.OpenRegisterPage();
            RegisterMgr.Checkin();
            if (rt != null)
            {
                RegisterMgr.SelectRegType(rt.RegistantTypeName);
            }
            this.grouperEmail = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfoEnduranceNew();

            if (rt != null)
            {
                if (rt.CollectTeamName)
                {
                    this.teamName = "team " + DateTime.Now.Ticks.ToString();
                    RegisterMgr.TypePersonalInfoEnduranceTeamName(this.teamName);//one per group
                }
            }

            RegisterMgr.CurrentRegistrationId = RegisterMgr.GetRegIdFromSession();
            this.registrantId = RegisterMgr.GetRegIdFromSession();

            RegisterMgr.Continue();

            for (int i = 1; i < followerCount; i++)
            {
                string emailAddressTwo = string.Empty;
                int registerIDTwo = RegisterManager.InvalidId;

                if (rt != null)
                {
                    this.AddAnotherPerson(rt.RegistantTypeName, ref emailAddressTwo, ref registerIDTwo);
                }
                else
                {
                    this.AddAnotherPerson(null, ref emailAddressTwo, ref registerIDTwo);
                }
            }

            RegisterMgr.ClickCheckoutActiveWaiver();
            RegisterMgr.FinishRegistration();

            RegisterMgr.CurrentEmail = this.grouperEmail;
        }

        private void AddAnotherPerson(string regType, ref string email, ref int regID)
        {
            RegisterMgr.VerifyHasAddAnotherPersonButton(true);
            RegisterMgr.ClickAddAnotherPerson();

            RegisterMgr.Checkin();
            if (regType != null)
            {
                RegisterMgr.SelectRegType(regType);
            }
            email = RegisterMgr.CurrentEmail;
            RegisterMgr.Continue();
            RegisterMgr.EnterProfileInfoEnduranceNew();

            regID = RegisterMgr.GetRegIdFromSession();
            RegisterMgr.CurrentRegistrationId = regID;

            RegisterMgr.Continue();
        }
    }
}

