namespace RegOnline.RegressionTest.Fixtures.Emails.RegMail
{
	using System;
	using System.Linq;
	using NUnit.Framework;
	using RegOnline.RegressionTest.Configuration;
	using RegOnline.RegressionTest.Fixtures.Base;
	using RegOnline.RegressionTest.DataAccess;
	using RegOnline.RegressionTest.Managers.Emails;
	using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Attributes;

	[TestFixture]
	[Category(FixtureCategory.Regression)]
	public class EmailInvitationFixture : FixtureBase
	{
		#region emailContent
		private const string HTMLContentToEnter = @"<font face=""DejaVu Sans Mono""><strong>Dear /*Merge: FirstName*/,"+
			@"</strong><br><br>We are proud to announce the opening of registrations for /*Merge: EventTitle*/. We hope " + 
			@"you will be able to join us this year.</font><font face=""DejaVu Sans Mono""><br><br></font><div align=""right"">" +
			@"<font face=""DejaVu Sans Mono""><em>The Event</em></font><font face=""DejaVu Sans Mono""> is located at: /*Merge:" +
			@"LocationName*/, in /*Merge: LocationCity*/. </font><br></div><font face=""DejaVu Sans Mono""><br><br><br>If you " + 
			@"wish to join us, /*RegisterLink: Click Here to Register for this Event*/.</font><font face=""DejaVu Sans Mono""><br>" +
			@"<br>Thank you for your continued interest in our events. </font><font face=""DejaVu Sans Mono""><span style=""text" +
			@"-decoration: underline;"">We hope to see you there. <br></span><br>Sincerely, <br>RegOnline</font><br>" + 
			@"<a href=""https://beta.regonline.com/custImages/377977/betaregonline.txt""><br>A link to our flier.</a><br><br>" + 
			@"<img style="""" src=""https://beta.regonline.com/custImages/377977/book-spine_thumb.png""><br>";
		private const string ExpectedPreviewText = "<font face=\"DejaVu Sans Mono\"><b>Dear Jon,</b><br />\r\n<br />\r\nWe are " +
			"proud to announce the opening of registrations for Event Title. We hope you will be able to join us this year.</font>" +
			"<font face=\"DejaVu Sans Mono\"><br />\r\n<br />\r\n</font>\r\n<div align=\"right\"><font face=\"DejaVu Sans Mono\">" +
			"<i>The Event</i></font><font face=\"DejaVu Sans Mono\"> is located at: /*Merge:LocationName*/, in Boulder. </font>" +
			"<br />\r\n</div>\r\n<font face=\"DejaVu Sans Mono\"><br />\r\n<br />\r\n<br />\r\nIf you wish to join us, <a href=" +
			"\"https://beta.regonline.com/t/c.aspx?0=0&amp;2=0&amp;8=1&amp;9=M1N8MSnv9mI=&amp;10=7&amp;1=0&amp;3=\">Click Here to" +
			" Register for this Event</a>.</font><font face=\"DejaVu Sans Mono\"><br />\r\n<br />\r\nThank you for your continued " + 
			"interest in our events. </font><font face=\"DejaVu Sans Mono\"><u>We hope to see you there. <br />\r\n</u><br />\r" +
			"\nSincerely, <br />\r\nRegOnline</font><br />\r\n<a href=\"https://beta.regonline.com/custImages/377977/betaregonline.txt\"" + 
			" re_target=\"\" target=\"_blank\"><br />\r\nA link to our flier.</a><br />\r\n<br />\r\n<img " + 
			"src=\"https://beta.regonline.com/custImages/377977/book-spine_thumb.png\" style=\"\" /><br />";
		private const string UpdatedHTMLContentToEnter = @"<font face=""DejaVu Sans Mono""><strong>Dear /*Merge: FirstName*/," +
			@"</strong><br><br>We are proud to announce the opening of registrations for /*Merge: EventTitle*/. We hope " +
			@"you will be able to join us this year.</font><font face=""DejaVu Sans Mono""><br><br></font><div align=""right"">" +
			@"<font face=""DejaVu Sans Mono""><em>The Event</em></font><font face=""DejaVu Sans Mono""> is located at: /*Merge:" +
			@"LocationName*/, in /*Merge: LocationCity*/. </font><br></div><font face=""DejaVu Sans Mono""><br><br><br>If you " +
			@"wish to join us, /*RegisterLink: Click Here to Register for this Event*/.</font><font face=""DejaVu Sans Mono""><br>" +
			@"<br>Don't forget to mark the date! /*Merge: AddToCalendar*/<br>" +
			@"<br>Thank you for your continued interest in our events. </font><font face=""DejaVu Sans Mono""><span style=""text" +
			@"-decoration: underline;"">We hope to see you there. <br></span><br>Sincerely, <br>RegOnline</font><br>" +
			@"<a href=""https://beta.regonline.com/custImages/377977/betaregonline.txt""><br>A link to our flier.</a><br><br>" +
			@"<img style="""" src=""https://beta.regonline.com/custImages/377977/book-spine_thumb.png""><br>";
		private const string ExpectedUpdatedPreviewText = "<font face=\"DejaVu Sans Mono\"><b>Dear Jon,</b><br />\r\n<br />\r\nWe are " +
			"proud to announce the opening of registrations for Event Title. We hope you will be able to join us this year.</font>" +
			"<font face=\"DejaVu Sans Mono\"><br />\r\n<br />\r\n</font>\r\n<div align=\"right\"><font face=\"DejaVu Sans Mono\">" +
			"<i>The Event</i></font><font face=\"DejaVu Sans Mono\"> is located at: /*Merge:LocationName*/, in Boulder. </font>" +
			"<br />\r\n</div>\r\n<font face=\"DejaVu Sans Mono\"><br />\r\n<br />\r\n<br />\r\nIf you wish to join us, <a href=" +
			"\"https://beta.regonline.com/t/c.aspx?0=0&amp;2=0&amp;8=1&amp;9=M1N8MSnv9mI=&amp;10=7&amp;1=0&amp;3=\">Click Here to" +
			" Register for this Event</a>.</font><font face=\"DejaVu Sans Mono\"><br />\r\n<br />\r\n" +
			"Don't forget to mark the date! <br />\r\n<br />\r\nThank you for your continued " +
			"interest in our events. </font><font face=\"DejaVu Sans Mono\"><u>We hope to see you there. <br />\r\n</u><br />\r" +
			"\nSincerely, <br />\r\nRegOnline</font><br />\r\n<a href=\"https://beta.regonline.com/custImages/377977/betaregonline.txt\"" +
			" re_target=\"\" target=\"_blank\"><br />\r\nA link to our flier.</a><br />\r\n<br />\r\n<img " +
			"src=\"https://beta.regonline.com/custImages/377977/book-spine_thumb.png\" style=\"\" /><br />";
		private const string ExpectedEmailInvitationText = "Hello a,\r\n   We wanted to remind you that the " + EventName +
			" is taking place on at Location:\r\nAutomation Test Room One\r\n4750 Walnut Street\r\nBoulder, CO 99701.   \r\n   We " +
			"expect the available registrations to fill up quickly. So, we would like to offer you the opportunity to reserve your " +
			"spot at a reduced early bird rate! To RSVP, click here .  \r\n   We look forward to hearing from you!  \r\n   Thank you,   " +
			"\r\n Phone: 303.577.5100\r\n " + EventName;
		#endregion
		
		private const string EventName = EmailManager.DefaultEventName;

		[Test]
        [Category(Priority.Three)]
        [Description("747")]
		public void CreateEmailInvitation()
		{
			ManagerSiteMgr.OpenLogin();
			ManagerSiteMgr.Login();
			ManagerSiteMgr.GoToEventsTabIfNeeded();
			this.CreateNewEventForEmailInvitation();
			ManagerSiteMgr.GoToEmailTabIfNeeded();

			EmailMgr.ClickCreateNewEmailLink();

			EmailMgr.SelectWizardFrame();

			EmailMgr.EnterEmailBasics(false, true);

			EmailMgr.EmailWizardNextClick();

			// Skip the theme step for now
			EmailMgr.EmailWizardNextClick();

			EmailMgr.ClickAndInsertContentTemplate("Confirmation Email Template");

			// Go to review step
			EmailMgr.EmailWizardNextClick();

			// Go to delivery step
			EmailMgr.EmailWizardNextClick();

			// Verify send now and schedule are disabled since there is no event
			EmailMgr.EmailWizardVerifySendNowStatus(false);

			// Go back to basics
			EmailMgr.EmailWizardBreadCrumbClick(EmailManager.EmailWizardSteps.Basics);

			// Select an contact list
			EmailMgr.SelectContactList();

			// Go to theme and pick a theme
			EmailMgr.EmailWizardNextClick();

			EmailMgr.SelectTheme(EmailManager.Theme.SameAsRegistrationForm);

			// Go to content and change title and upload a logo
			EmailMgr.EmailWizardNextClick();
			EmailMgr.UploadLogo();
			EmailMgr.SetCustomTitle("Custom Title");

			// Go to review and verify title, logo, and theme
			EmailMgr.EmailWizardNextClick();
			EmailMgr.VerifyReviewTab("Same as Registration Form", "Custom Title");

			//Go back to deliver
			EmailMgr.EmailWizardBreadCrumbClick(EmailManager.EmailWizardSteps.Delivery);

			EmailMgr.ChooseDeliveryOption(EmailManager.Delivery.SendNow);

			// Pick send now
			EmailMgr.EmailWizardFinishClick();

			// Verify error message for terms
			EmailMgr.VerifyErrorMessage("You must accept that you've read the notice");

			// Check notice
			EmailMgr.CheckEmailTerms();

			// Press Send
			EmailMgr.EmailWizardFinishClick();

			Assert.IsFalse(WebDriverUtility.DefaultProvider.IsTextPresent("Job not yet scheduled"));
		}

		[Test]
        [Category(Priority.Two)]
        [Description("274")]
		public void CreateEmailContent()
		{
			//login go to email tab
			ManagerSiteMgr.OpenLogin();
			ManagerSiteMgr.Login();
			ManagerSiteMgr.GoToEmailTabIfNeeded();

			//create new content
			EmailMgr.ClickCreateContentTemplate();
			string contentTitle = "content" + DateTime.Now.Ticks;
			EmailMgr.TypeContentTitle(contentTitle);
			EnterContentInHTMLAndVerify(HTMLContentToEnter, ExpectedPreviewText);
			this.VerifyTextInPreview(ExpectedPreviewText);
			EmailMgr.SaveAndClose();

			//verify content saved correctly
			EmailMgr.OpenContent(contentTitle);
			VerifyTextInPreview(ExpectedPreviewText);

			//update content and verify
			EnterContentInHTMLAndVerify(UpdatedHTMLContentToEnter, ExpectedUpdatedPreviewText);
			this.VerifyTextInPreview(ExpectedUpdatedPreviewText);
			EmailMgr.SaveAndClose();

			//verify updated content
			EmailMgr.OpenContent(contentTitle);
			VerifyTextInPreview(ExpectedUpdatedPreviewText);
			EmailMgr.SaveAndClose();
			
			contentTitle = "content" + DateTime.Now.Ticks;
			EmailMgr.ClickCreateNewEmailLink();
			EmailMgr.SelectWizardFrame();
			EmailMgr.EnterEmailBasics(true, true);
			EmailMgr.EmailWizardNextClick();
			EmailMgr.EmailWizardNextClick();
			EmailMgr.SwitchToHTMLView();
			EmailMgr.TypeContentInWizardHTMLView(HTMLContentToEnter);
			EmailMgr.SaveContentAsTemplate(contentTitle);
			EmailMgr.Cancel();
			EmailMgr.OpenContent(contentTitle);
			VerifyTextInPreview(ExpectedPreviewText);
			EmailMgr.SaveAndClose();
		}

		[Test]
        [Category(Priority.Two)]
        [Description("275")]
		public void SendEmailInvitation()
		{
			ManagerSiteMgr.OpenLogin();
			ManagerSiteMgr.Login();
			ManagerSiteMgr.GoToEventsTabIfNeeded();
			this.CreateNewEventForEmailInvitation();
			int eventId = ManagerSiteMgr.GetFirstEventId(EventName);

			ManagerSiteMgr.GoToEmailTabIfNeeded();

			EmailMgr.ExpandContactLists();

			int listId = EmailMgr.GetContactListIdFromLink(EmailManager.DefaultContactListName);

			string attendeeID = GetAttendeeId(listId);

			EmailMgr.ClickCreateNewEmailLink();

			EmailMgr.SelectWizardFrame();

			string emailTitle = EmailMgr.EnterEmailBasics(true, true);

			EmailMgr.EmailWizardNextClick();

			//M3.Email.SelectTheme("Vail");
			EmailMgr.SelectTheme(EmailManager.Theme.SameAsRegistrationForm);

			// Go to content and change title and upload a logo
			EmailMgr.EmailWizardNextClick();

			EmailMgr.ClickAndInsertContentTemplate("Early Bird Registration Template");

			EmailMgr.UploadLogo();

			EmailMgr.SetCustomTitle("Custom Title");

			// Go to review and verify title, logo, and theme
			EmailMgr.EmailWizardNextClick();
			EmailMgr.VerifyReviewTab("Same as Registration Form", "Custom Title");

			//Go back to deliver
			EmailMgr.EmailWizardNextClick();

			EmailMgr.ChooseDeliveryOption(EmailManager.Delivery.SendNow);

			// Pick send now
			EmailMgr.EmailWizardFinishClick();

			// Verify error message for terms
			EmailMgr.VerifyErrorMessage("You must accept that you've read the notice");

			// Check notice
			EmailMgr.CheckEmailTerms();

			// Press Send
			EmailMgr.EmailWizardFinishClick();
			EmailMgr.SelectManagerWindow();

            ////EmailMgr.OpenEmailInvitationEmailUrl(eventId, Convert.ToInt32(attendeeID), emailTitle);

            ////Utilities.VerifyTool.VerifyValue(
            ////    string.Format("{0}CustImages/{1}/email{2}", 
            ////        ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.BaseUrlWithHttps, 
            ////        Convert.ToInt32(ConfigurationProvider.XmlConfig.PrivateLabelConfiguration.AccountId), 
            ////        EmailManager.LogoFileName),
            ////    EmailMgr.GetHeaderImgPathAttribute(),
            ////    "Image Path: {0}");

            ////Utilities.VerifyTool.VerifyValue("Custom Title", EmailMgr.GetEmailHeaderTitle(), "Email Title: {0}");
            ////Utilities.VerifyTool.VerifyValue(ExpectedEmailInvitationText, EmailMgr.GetBodyText(), "Body Text: {0}");
		}

		[Step]
		public string GetAttendeeId(int listId)
		{
			ClientDataContext db = new ClientDataContext();
			return Convert.ToString((from a in db.Attendees where a.ListId == listId orderby a.Id ascending select a).ToList().Last().Id);
		}

		[Verify]
		public void EnterContentInHTMLAndVerify(string contentToEnter, string expectedPreviewContent)
		{
			EmailMgr.SwitchToHTMLView();
			EmailMgr.TypeContentInHTMLView(contentToEnter);
			EmailMgr.SaveAndStay();
		}

		[Verify]
		public void VerifyTextInPreview(string expectedPreviewContent)
		{
			EmailMgr.SwitchToPreviewMode();
			string previewText = EmailMgr.GetContentFromPreview();
			Utilities.VerifyTool.VerifyValue(expectedPreviewContent, previewText, "Body: {0}");
			BuilderMgr.SelectBuilderWindow();
		}

		private void CreateNewEventForEmailInvitation()
		{
			ManagerSiteMgr.SelectFolder();

			if (!ManagerSiteMgr.EventExists(EventName))
			{
				ManagerSiteMgr.ClickAddEvent(Managers.Manager.ManagerSiteManager.EventType.ProEvent);
				BuilderMgr.SetStartPage(Managers.Manager.ManagerSiteManager.EventType.ProEvent, EventName);
				BuilderMgr.SaveAndClose();
			}
		}
	}
}
