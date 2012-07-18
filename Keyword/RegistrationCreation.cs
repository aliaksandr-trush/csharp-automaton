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
        public void CreateRegistration(Registrant reg)
        {
            Checkin(reg);

            if (reg.RegType != null && reg.RegType.IsSSO)
            {
                SSOLogin(reg);
                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
            else
            {
                PersonalInfo(reg);
            }

            Agenda(reg);
            Checkout(reg);
        }

        public void GroupRegistration(List<Registrant> regs)
        {
            Checkin(regs[0]);

            for (int i = 0; i <= regs.Count - 2; i++)
            {
                PersonalInfo(regs[i]);
                Agenda(regs[i]);
                //Click add another person on checkout page
                PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();
                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(regs[i + 1].Email);
                if (regs[i + 1].RegType != null)
                {
                    if (regs[i + 1].Event.StartPage.RegTypeDisplayOption.HasValue)
                    {
                        if (regs[i + 1].Event.StartPage.RegTypeDisplayOption.Value == FormData.RegTypeDisplayOption.DropDownList)
                        {
                            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.RegTypeDropDown.SelectWithText(regs[i + 1].RegType.RegTypeName);
                        }
                        else
                        {
                            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(regs[i + 1].RegType);
                        }
                    }
                    else
                    {
                        PageObject.PageObjectProvider.Register.RegistationSite.Checkin.SelectRegTypeRadioButton(regs[i + 1].RegType);
                    }

                    if (regs[i + 1].RegType.DiscountCode.Count != 0)
                    {
                        PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.Type(regs[i + 1].RegType.DiscountCode[0].Code);
                    }
                }
                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }

            PersonalInfo(regs[regs.Count - 1]);
            Agenda(regs[regs.Count - 1]);
            Checkout(regs[0]);
        }

        public void Checkin(Registrant reg)
        {
            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.OpenUrl(reg);

            if (PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(FormData.RegisterPage.Login))
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.StartNewRegistration_Click();
            }

            PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(reg.Email);

            if (PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.IsPresent)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.WaitForDisplay();
                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.VerifyEmailAddress.Type(reg.Email);
            }

            if ((reg.RegType != null) && (reg.RegisterMethod != RegisterMethod.RegTypeDirectUrl))
            {
                KeywordProvider.RegisterDefault.SelectRegType(reg.RegType);

                if (reg.RegType.DiscountCode.Count != 0)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EventFeeDiscountCode.Type(reg.RegType.DiscountCode[0].Code);
                }
            }

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
        }

        public void Login(Registrant reg)
        {
            if (reg.Password != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type(reg.Password);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type(Registrant.Default.Password);
            }

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
        }

        public void PersonalInfo(Registrant reg)
        {
            string lastName = reg.LastName = KeywordProvider.RegisterDefault.GenerateCurrentRegistrantLastName();
            
            if (reg.FirstName != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.FirstName.Type(reg.FirstName);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.FirstName.Type(Registrant.Default.FirstName);
            }
            if (reg.MiddleName != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.MiddleName.Type(reg.MiddleName);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.MiddleName.Type(Registrant.Default.MiddleName);
            }

            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.LastName.Type(lastName);
            
            if (reg.JobTitle != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Type(reg.JobTitle);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Type(Registrant.Default.JobTitle);
            }
            if (reg.Company != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Company.Type(reg.Company);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Company.Type(Registrant.Default.Company);
            }
            if (reg.AddressLineOne != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.AddressOne.Type(reg.AddressLineOne);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.AddressOne.Type(Registrant.Default.AddressLineOne);
            }
            if (reg.City != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.City.Type(reg.City);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.City.Type(Registrant.Default.City);
            }
            if (reg.State != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.State.SelectWithText(reg.State);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.State.SelectWithText(Registrant.Default.State);
            }
            if (reg.ZipCode != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Zip.Type(reg.ZipCode);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Zip.Type(Registrant.Default.ZipCode);
            }
            if (reg.WorkPhone != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.WorkPhone.Type(reg.WorkPhone);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.WorkPhone.Type(Registrant.Default.WorkPhone);
            }
            if (reg.Password != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.Type(reg.Password);
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PasswordReEnter.Type(reg.Password);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.Type(Registrant.Default.Password);
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PasswordReEnter.Type(Registrant.Default.Password);
            }
            if (PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish.IsPresent)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish_Click();
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
        }

        public void Agenda(Registrant reg)
        {
            if (reg.CustomFieldResponses.Count != 0)
            {
                foreach (AgendaResponse response in reg.CustomFieldResponses)
                {
                    switch (response.AgendaItem.Type)
                    {
                        case FormData.CustomFieldType.CheckBox:
                            {
                                AgendaCheckboxResponse resp = response as AgendaCheckboxResponse;
                                ((CheckBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem).AgendaType).Set(resp.Checked.Value);
                            }
                            break;
                        case FormData.CustomFieldType.RadioButton:
                            {
                                AgendaRadioButtonResponse resp = response as AgendaRadioButtonResponse;
                                RadioButton radio = new RadioButton(resp.ChoiceItem.Id.ToString(), LocateBy.Id);
                                radio.Click();
                            }
                            break;
                        case FormData.CustomFieldType.Dropdown:
                            {
                                AgendaDropDownResponse resp = response as AgendaDropDownResponse;
                                ((MultiChoiceDropdown)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).SelectWithValue(resp.ChoiceItem.Id.ToString());
                            }
                            break;
                        case FormData.CustomFieldType.Number:
                        case FormData.CustomFieldType.OneLineText:
                        case FormData.CustomFieldType.Paragraph:
                            {
                                AgendaCharInputResponse resp = response as AgendaCharInputResponse;
                                ((TextBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(resp.CharToInput);
                            }
                            break;
                        case FormData.CustomFieldType.Contribution:
                            {
                                AgendaContributionResponse resp = response as AgendaContributionResponse;
                                ((TextBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(resp.Contribution.Value);
                            }
                            break;
                        case FormData.CustomFieldType.Date:
                            {
                                AgendaDateResponse resp = response as AgendaDateResponse;
                                string date = string.Format("{0}/{1}/{2}", resp.Date.Value.Month, resp.Date.Value.Day, resp.Date.Value.Year);
                                ((TextBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(date);
                            }
                            break;
                        case FormData.CustomFieldType.Time:
                            {
                                AgendaTimeResponse resp = response as AgendaTimeResponse;
                                string time = string.Format("{0}:{1}", resp.Time.Value.Hour, resp.Time.Value.Minute);
                                ((TextBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(time);
                            }
                            break;
                        default:
                            break;
                    }
                }

                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
        }

        public void Checkout(Registrant reg)
        {
            if (reg.PaymentMethod != null)
            {
                if (PageObject.PageObjectProvider.Register.RegistationSite.Checkout.PaymentMethodList.IsPresent)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Checkout.PaymentMethodList.SelectWithText(
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

            PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish_Click();

            if (PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(FormData.RegisterPage.ConfirmationRedirect))
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkout.AANoThanks_Click();
            }

            reg.Id = Convert.ToInt32(PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.RegistrationId.Text);
        }

        public void SSOLogin(Registrant reg)
        {
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Email.SelectWithText(reg.Email);
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Password.SelectWithText(reg.Password);
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Login_Click();
        }

        public void SSOLogin(string email, string password)
        {
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Email.SelectWithText(email);
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Password.SelectWithText(password);
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Login_Click();
        }
    }
}
