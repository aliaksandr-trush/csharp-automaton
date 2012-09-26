﻿namespace RegOnline.RegressionTest.Fixtures.SSO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.Fixtures.Base;
    using NUnit.Framework;

    [TestFixture]
    [Category(FixtureCategory.SSO)]
    public class AdminRegistrationFixture : SSOFixtureBase
    {
        [Test]
        public void AdminRegister()
        {
            DataCollection.Event evt = new DataCollection.Event("SSO_AdminRegister");
            DataCollection.RegType regType = new DataCollection.RegType("SSORegType");
            regType.IsSSO = true;
            evt.StartPage.RegTypes.Add(regType);

            DataCollection.Registrant reg = new DataCollection.Registrant(evt, DataCollection.SSOData.SSOTestEmail);
            reg.Password = DataCollection.SSOData.SSOPassword;
            reg.EventFee_Response = new DataCollection.EventFeeResponse(regType);
            reg.Register_Method = DataCollection.RegisterMethod.Admin;

            Keyword.KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(DataCollection.EventFolders.Folders.SSO, evt);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);
            Keyword.KeywordProvider.RegistrationCreation.SSOLogin(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.EventFee_Response.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            AssertHelper.VerifyOnPage(DataCollection.FormData.RegisterPage.AttendeeCheck, true);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            Keyword.KeywordProvider.RegistrationCreation.Checkout(reg);
        }
    }
}
