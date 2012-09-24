namespace RegOnline.RegressionTest.Fixtures.New.FeeOptions
{
    using NUnit.Framework;
    using RegOnline.RegressionTest.Fixtures.Base;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.Keyword;

    [TestFixture]
    [Category(FixtureCategory.Regression)]
    public class GroupDiscountFixture : FixtureBase
    {
        Event evt1;
        PaymentMethod paymentMethod = new PaymentMethod(FormData.PaymentMethod.Check);
        RegType regType1 = new RegType("regType1");
        RegType regType2 = new RegType("regType2");
        AgendaItem_CheckBox agenda1 = new AgendaItem_CheckBox("agenda1");
        AgendaItem_CheckBox agenda2 = new AgendaItem_CheckBox("agenda2");
        MerchandiseItem merch1 = new MerchandiseItem("merch1");
        MerchandiseItem merch2 = new MerchandiseItem("merch2");
        EventFeeResponse resp1 = new EventFeeResponse();
        EventFeeResponse resp2 = new EventFeeResponse();
        AgendaResponse_Checkbox resp3 = new AgendaResponse_Checkbox();
        AgendaResponse_Checkbox resp4 = new AgendaResponse_Checkbox();
        MerchResponse_FixedPrice resp5 = new MerchResponse_FixedPrice();
        MerchResponse_VariableAmount resp6 = new MerchResponse_VariableAmount();

        [Test]
        [Category(Priority.Three)]
        [Description("1373")]
        public void OrMore()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 10;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-OrMore");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1374")]
        public void JustSizeAndAll()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.JustSize;
            groupDiscount.DiscountAmount = 10;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;
            groupDiscount.AddtionalRegOption = GroupDiscount_AdditionalRegOption.All;

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-All");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(2));
            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(4));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1378")]
        public void JustSizeAndAnyAdditional()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.JustSize;
            groupDiscount.DiscountAmount = 5;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.USDollar;
            groupDiscount.AddtionalRegOption = GroupDiscount_AdditionalRegOption.AnyAdditional;

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-AnyAdditional");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(4));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1375")]
        public void JustSizeAndAdditional()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.JustSize;
            groupDiscount.DiscountAmount = 10;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;
            groupDiscount.AddtionalRegOption = GroupDiscount_AdditionalRegOption.Additional;
            groupDiscount.NumberOfAdditionalReg = 1;

            this.CreateEventAndAddGroupDiscount(groupDiscount , "GroupDiscount-Additional");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(4));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1383")]
        public void ApplyToSelectedFee()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 10;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;
            groupDiscount.ApplyToAgendaItems.Add(agenda1);
            groupDiscount.ApplyToRegTypes.Add(regType1);

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-ApplyToSelected");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1379")]
        public void OneHundredPercent()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 100;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-100Percent");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(4));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1384")]
        public void ShowAndApply()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 50;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;
            groupDiscount.ShowAndApply = false;

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-ShowApply");

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(4));
        }

        [Test]
        [Category(Priority.Three)]
        [Description("1381")]
        public void ChangeDiscount()
        {
            GroupDiscount groupDiscount = new GroupDiscount();
            groupDiscount.GroupSize = 2;
            groupDiscount.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount.DiscountAmount = 50;
            groupDiscount.GroupDiscountType = GroupDiscount_DiscountType.Percent;

            this.CreateEventAndAddGroupDiscount(groupDiscount, "GroupDiscount-Change", true);
            Group group = this.GenerateGroup(2);
            this.GroupRegistrationAndVerifyTotal(group);

            KeywordProvider.SignIn.SignIn(EventFolders.Folders.RegistrationInventory);

            GroupDiscount groupDiscount2 = new GroupDiscount();
            groupDiscount2.GroupSize = 2;
            groupDiscount2.GroupSizeOption = GroupDiscount_GroupSizeOption.SizeOrMore;
            groupDiscount2.DiscountAmount = 10;
            groupDiscount2.GroupDiscountType = GroupDiscount_DiscountType.Percent;

            KeywordProvider.AddGroupDiscount.Add_GroupDiscount(evt1, groupDiscount2);

            Registrant reg = new Registrant(evt1);
            reg.Payment_Method = paymentMethod;
            reg.EventFee_Response = resp1;
            reg.CustomField_Responses.Add(resp4);
            group.Secondaries.Add(reg);

            KeywordProvider.RegistrationCreation.Checkin(group.Primary);
            KeywordProvider.RegistrationCreation.Login(group.Primary);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(reg.EventFee_Response.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(reg);
            KeywordProvider.RegistrationCreation.Agenda(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(reg);

            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(group));

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(group.Primary);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Cancel_Click(1);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.OK_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.Cancel_Click(2);
            PageObject.PageObjectProvider.Register.RegistationSite.AttendeeCheck.OK_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(group.Primary);

            Group group1 = this.GenerateGroup(2);
            group1.Primary.Email = group.Primary.Email;

            evt1.StartPage.GroupDiscount = groupDiscount2;

            PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.ChangeMyRegistration_Click();
            KeywordProvider.RegistrationCreation.Login(group1.Primary);
            PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(group1.Secondaries[0].Email);
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(group1.Secondaries[0].EventFee_Response.RegType);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.PersonalInfo(group1.Secondaries[0]);
            KeywordProvider.RegistrationCreation.Agenda(group1.Secondaries[0]);
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            KeywordProvider.RegistrationCreation.Checkout(group1.Primary);

            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation),
                KeywordProvider.CalculateFee.CalculateTotalFee(group1));

            this.GroupRegistrationAndVerifyTotal(this.GenerateGroup(3));
        }

        private void CreateEventAndAddGroupDiscount(GroupDiscount groupDiscount, string eventName, bool recreate = false)
        {
            this.evt1 = new Event(eventName);
            this.evt1.CheckoutPage.PaymentMethods.Add(this.paymentMethod);
            this.regType1.Price = 20;
            this.regType2.Price = 30;
            this.evt1.StartPage.RegTypes.Add(this.regType1);
            this.evt1.StartPage.RegTypes.Add(this.regType2);
            this.evt1.StartPage.GroupDiscount = groupDiscount;
            this.evt1.AgendaPage = new AgendaPage();
            this.agenda1.Price = 40;
            this.agenda2.Price = 50;
            this.evt1.AgendaPage.AgendaItems.Add(this.agenda1);
            this.evt1.AgendaPage.AgendaItems.Add(this.agenda2);
            this.evt1.MerchandisePage = new MerchandisePage();
            merch1.Type = FormData.MerchandiseType.Fixed;
            merch1.Price = 60;
            merch2.Type = FormData.MerchandiseType.Variable;
            merch2.MinPrice = 65;
            merch2.MaxPrice = 100;
            this.evt1.MerchandisePage.Merchandises.Add(this.merch1);
            this.evt1.MerchandisePage.Merchandises.Add(this.merch2);

            KeywordProvider.SignIn.SignInAndRecreateEventAndGetEventId(EventFolders.Folders.RegistrationInventory, evt1, recreate);

            this.resp1.RegType = this.regType1;
            this.resp1.Fee = 20;
            this.resp2.RegType = this.regType2;
            this.resp2.Fee = 30;
            this.resp3.AgendaItem = this.agenda1;
            this.resp3.Checked = true;
            this.resp3.Fee = 40;
            this.resp4.AgendaItem = this.agenda2;
            this.resp4.Checked = true;
            this.resp4.Fee = 50;
            this.resp5.Merchandise_Item = this.merch1;
            this.resp5.Quantity = 1;
            this.resp6.Merchandise_Item = this.merch2;
            this.resp6.Amount = 70;
        }

        private Group GenerateGroup(int groupSize)
        {
            Registrant reg1 = new Registrant(evt1);
            reg1.Payment_Method = paymentMethod;
            reg1.EventFee_Response = resp1;
            reg1.CustomField_Responses.Add(resp3);
            reg1.CustomField_Responses.Add(resp4);
            reg1.Merchandise_Responses.Add(resp5);
            reg1.Merchandise_Responses.Add(resp6);
            System.Threading.Thread.Sleep(10);

            Registrant reg2 = new Registrant(evt1);
            reg2.Payment_Method = paymentMethod;
            reg2.EventFee_Response = resp2;
            reg2.CustomField_Responses.Add(resp3);
            System.Threading.Thread.Sleep(10);

            Registrant reg3 = new Registrant(evt1);
            reg3.Payment_Method = paymentMethod;
            reg3.EventFee_Response = resp1;
            reg3.CustomField_Responses.Add(resp4);
            System.Threading.Thread.Sleep(10);

            Registrant reg4 = new Registrant(evt1);
            reg4.Payment_Method = paymentMethod;
            reg4.EventFee_Response = resp2;
            reg4.CustomField_Responses.Add(resp3);
            reg4.CustomField_Responses.Add(resp4);

            Group group = new Group();
            group.Primary = reg1;
            group.Secondaries.Add(reg2);

            if (groupSize >= 3)
            {
                group.Secondaries.Add(reg3);
            }

            if (groupSize >= 4)
            {
                group.Secondaries.Add(reg4);
            }

            return group;
        }

        private void GroupRegistrationAndVerifyTotal(Group group)
        {
            KeywordProvider.RegistrationCreation.GroupRegistration(group);
            Assert.AreEqual(KeywordProvider.RegisterDefault.GetTotal(FormData.RegisterPage.Confirmation), 
                KeywordProvider.CalculateFee.CalculateTotalFee(group));
        }
    }
}
