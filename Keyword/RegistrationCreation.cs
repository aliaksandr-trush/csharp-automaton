namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.WebElements;

    public class RegistrationCreation
    {
        private PageObjectHelper RegisterHelper = new PageObjectHelper();
        private Checkin CheckinPage = new Checkin();
        private Login LoginPage = new Login();
        private PersonalInfo PersonalInfoPage = new PersonalInfo();
        private Agenda AgendaItem = new Agenda();
        private Checkout CheckoutPage = new Checkout();
        private Confirmation Confirmation = new Confirmation();

        public void CreateRegistration(Registrant reg)
        {
            Checkin(reg);
            PersonalInfo(reg);
            Agenda(reg);
            Checkout(reg);
        }

        public void GroupRegistration(List<Registrant> regs)
        {
            Checkin(regs[0]);

            for (int i = 0; i <= regs.Count - 2; i++)
            {
                PersonalInfo(regs[i]);
                //Click add another person on checkout page
                RegisterHelper.AddAnotherPerson_Click();
                CheckinPage.EmailAddress.Type(regs[i + 1].Email);
                if (regs[i + 1].RegType != null)
                {
                    if (regs[i + 1].Event.StartPage.RegTypeDisplayOption.HasValue)
                    {
                        if (regs[i + 1].Event.StartPage.RegTypeDisplayOption.Value == FormData.RegTypeDisplayOption.DropDownList)
                        {
                            CheckinPage.RegTypeDropDown.SelectWithText(regs[i + 1].RegType.RegTypeName);
                        }
                        else
                        {
                            CheckinPage.SelectRegTypeRadioButton(regs[i + 1].RegType.RegTypeName);
                        }
                    }
                    else
                    {
                        CheckinPage.SelectRegTypeRadioButton(regs[i + 1].RegType.RegTypeName);
                    }

                    if (regs[i + 1].RegType.DiscountCode.Count != 0)
                    {
                        CheckinPage.EventFeeDiscountCode.Type(regs[i + 1].RegType.DiscountCode[0].Code);
                    }
                }
                RegisterHelper.Continue_Click();
            }

            PersonalInfo(regs[regs.Count - 1]);
            Checkout(regs[0]);
        }

        public void Checkin(Registrant reg)
        {
            KeywordProvider.RegisterDefault.OpenRegisterPage(reg.Event.Id);

            if (KeywordProvider.RegisterDefault.IsOnLoginPage())
            {
                LoginPage.StartNewRegistration_Click();
            }

            CheckinPage.EmailAddress.Type(reg.Email);

            if (CheckinPage.VerifyEmailAddress.IsPresent)
            {
                CheckinPage.VerifyEmailAddress.WaitForDisplay();
                CheckinPage.VerifyEmailAddress.Type(reg.Email);
            }

            if (reg.RegType != null)
            {
                KeywordProvider.RegisterDefault.SelectRegType(reg.RegType.RegTypeName);

                if (reg.RegType.DiscountCode.Count != 0)
                {
                    CheckinPage.EventFeeDiscountCode.Type(reg.RegType.DiscountCode[0].Code);
                }
            }

            RegisterHelper.Continue_Click();
        }

        public void Login(Registrant reg)
        {
            if (reg.Password != null)
            {
                LoginPage.Password.Type(reg.Password);
            }
            else
            {
                LoginPage.Password.Type(Registrant.Default.Password);
            }

            RegisterHelper.Continue_Click();
        }

        public void PersonalInfo(Registrant reg)
        {
            string lastName = reg.LastName = KeywordProvider.RegisterDefault.GenerateCurrentRegistrantLastName();
            
            if (reg.FirstName != null)
            {
                PersonalInfoPage.FirstName.Type(reg.FirstName);
            }
            else
            {
                PersonalInfoPage.FirstName.Type(Registrant.Default.FirstName);
            }
            if (reg.MiddleName != null)
            {
                PersonalInfoPage.MiddleName.Type(reg.MiddleName);
            }
            else
            {
                PersonalInfoPage.MiddleName.Type(Registrant.Default.MiddleName);
            }
            
            PersonalInfoPage.LastName.Type(lastName);
            
            if (reg.JobTitle != null)
            {
                PersonalInfoPage.JobTitle.Type(reg.JobTitle);
            }
            else
            {
                PersonalInfoPage.JobTitle.Type(Registrant.Default.JobTitle);
            }
            if (reg.Company != null)
            {
                PersonalInfoPage.Company.Type(reg.Company);
            }
            else
            {
                PersonalInfoPage.Company.Type(Registrant.Default.Company);
            }
            if (reg.AddressLineOne != null)
            {
                PersonalInfoPage.AddressOne.Type(reg.AddressLineOne);
            }
            else
            {
                PersonalInfoPage.AddressOne.Type(Registrant.Default.AddressLineOne);
            }
            if (reg.City != null)
            {
                PersonalInfoPage.City.Type(reg.City);
            }
            else
            {
                PersonalInfoPage.City.Type(Registrant.Default.City);
            }
            if (reg.State != null)
            {
                PersonalInfoPage.State.SelectWithText(reg.State);
            }
            else
            {
                PersonalInfoPage.State.SelectWithText(Registrant.Default.State);
            }
            if (reg.ZipCode != null)
            {
                PersonalInfoPage.Zip.Type(reg.ZipCode);
            }
            else
            {
                PersonalInfoPage.Zip.Type(Registrant.Default.ZipCode);
            }
            if (reg.WorkPhone != null)
            {
                PersonalInfoPage.WorkPhone.Type(reg.WorkPhone);
            }
            else
            {
                PersonalInfoPage.WorkPhone.Type(Registrant.Default.WorkPhone);
            }
            if (reg.Password != null)
            {
                PersonalInfoPage.Password.Type(reg.Password);
                PersonalInfoPage.PasswordReEnter.Type(reg.Password);
            }
            else
            {
                PersonalInfoPage.Password.Type(Registrant.Default.Password);
                PersonalInfoPage.PasswordReEnter.Type(Registrant.Default.Password);
            }

            if (CheckoutPage.Finish.IsPresent)
            {
                CheckoutPage.Finish_Click();
            }
            else
            {
                RegisterHelper.Continue_Click();
            }
        }

        public void Agenda(Registrant reg)
        {
            if (reg.AgendaItems.Count != 0)
            {
                foreach (AgendaItem agenda in reg.AgendaItems)
                {
                    switch (agenda.Type)
                    {
                        case FormData.CustomFieldType.CheckBox:
                            ((CheckBox)AgendaItem.GetAgendaItem(agenda).AgendaType).Set(true);
                            break;
                        case FormData.CustomFieldType.RadioButton:
                            {
                                AgendaItemRadioButton ag = agenda as AgendaItemRadioButton;
                                foreach (ChoiceItem choice in ag.ChoiceItems)
                                {
                                    RadioButton radio = new RadioButton(choice.Id.ToString(), LocateBy.Id);
                                    if (choice.Select)
                                    {
                                        radio.Click();
                                    }
                                }
                            }
                            break;
                        case FormData.CustomFieldType.Dropdown:
                            {
                                AgendaItemDropDown ag = agenda as AgendaItemDropDown;
                                foreach (ChoiceItem choice in ag.ChoiceItems)
                                {
                                    if (choice.Select)
                                    {
                                        ((MultiChoiceDropdown)AgendaItem.GetAgendaItem(agenda).AgendaType).SelectWithValue(choice.Id.ToString());
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                RegisterHelper.Continue_Click();
            }
        }

        public void Checkout(Registrant reg)
        {
            if (reg.PaymentMethod != null)
            {
                if (CheckoutPage.PaymentMethodList.IsPresent)
                {
                    CheckoutPage.PaymentMethodList.SelectWithText(
                        FormData.PaymentMethodCheckouLabelAttribute.GetPaymentMethodCheckouLabel(reg.PaymentMethod.PMethod));
                }

                switch (reg.PaymentMethod.PMethod)
                { 
                    /*****
                     To implement
                     *****/
                    case FormData.PaymentMethod.CreditCard:
                        break;
                    default:
                        break;
                }
            }

            CheckoutPage.Finish_Click();

            if (KeywordProvider.RegisterDefault.IsOnConfirmationRedirectPage())
            {
                CheckoutPage.AANoThanks_Click();
            }

            reg.Id = Convert.ToInt32(Confirmation.RegistrationId.Text);
        }
    }
}
