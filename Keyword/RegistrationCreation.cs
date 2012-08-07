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

            if (reg.Event.AgendaPage != null && !reg.Event.AgendaPage.IsShoppingCart)
            {
                Agenda(reg);
            }

            Checkout(reg);
        }

        public void GroupRegistration(List<Registrant> regs)
        {
            Checkin(regs[0]);

            for (int i = 0; i <= regs.Count - 2; i++)
            {
                PersonalInfo(regs[i]);
                Agenda(regs[i]);

                // Click add another person on checkout page
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

            if (reg.Event.AgendaPage != null && reg.Event.AgendaPage.IsShoppingCart)
            {
                this.ShoppingCart(reg);
            }

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

        private void ShoppingCart(DataCollection.Registrant reg)
        {
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.SelectView(FormData.EventCalendarView.Location);
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.AddToCart(reg);
            PageObject.PageObjectProvider.Register.RegistationSite.EventCalendar.ShoppingCart_RegisterButtonOne_Click();
        }

        public void Login(Registrant reg)
        {
            if (reg.Password != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type(reg.Password);
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Login.Password.Type(DataCollection.DefaultPersonalInfo.Password);
            }

            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
        }

        public void PersonalInfo(Registrant reg)
        {
            reg.SetCurrentRegistrantLastName();
            
            if (reg.FirstName != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.FirstName.Type(reg.FirstName);
            }

            if (reg.MiddleName != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.MiddleName.Type(reg.MiddleName);
            }

            PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.LastName.Type(reg.LastName);
            
            if (reg.JobTitle != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.JobTitle.Type(reg.JobTitle);
            }

            if (reg.Company != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Company.Type(reg.Company);
            }

            if (reg.AddressLineOne != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.AddressOne.Type(reg.AddressLineOne);
            }

            if (reg.City != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.City.Type(reg.City);
            }

            if (reg.Country.HasValue)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Country.SelectWithText(
                    Utilities.CustomStringAttribute.GetCustomString(reg.Country.Value));
            }

            if (reg.State != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.State.SelectWithText(reg.State);
            }

            if (reg.NonUSState != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.NonUSState.Type(reg.NonUSState);
            }

            if (reg.ZipCode != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Zip.Type(reg.ZipCode);
            }

            if (reg.WorkPhone != null)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.WorkPhone.Type(reg.WorkPhone);
            }

            if (reg.BirthDate.HasValue)
            {
                string date = string.Format("{0}/{1}/{2}", reg.BirthDate.Value.Month, reg.BirthDate.Value.Day, reg.BirthDate.Value.Year);
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.DateOfBirth.Type(date);
            }

            if (reg.Gender.HasValue)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Gender.SelectWithText(reg.Gender.Value.ToString());
            }

            if (reg.RegisterMethod != RegisterMethod.Admin)
            {
                if (reg.Password != null)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.Type(reg.Password);
                    PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PasswordReEnter.Type(reg.Password);
                }
                else
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Password.Type(DataCollection.DefaultPersonalInfo.Password);
                    PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.PasswordReEnter.Type(DataCollection.DefaultPersonalInfo.Password);
                }
            }

            if (reg.CustomFieldResponses.Count != 0)
            {
                foreach (CustomFieldResponse responses in reg.CustomFieldResponses)
                {
                    if (responses is CFResponse)
                    {
                        CFResponse response = responses as CFResponse;

                        switch (response.CustomField.Type)
                        {
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    CFCheckboxResponse resp = response as CFCheckboxResponse;
                                    PageObject.Register.CustomFieldRow row = new CustomFieldRow(resp.CustomField);
                                    ((CheckBox)row.CustomFieldType).Set(resp.Checked.Value);
                                }
                                break;
                            case FormData.CustomFieldType.RadioButton:
                            case FormData.CustomFieldType.Dropdown:
                            case FormData.CustomFieldType.Number:
                            case FormData.CustomFieldType.OneLineText:
                            case FormData.CustomFieldType.Paragraph:
                            case FormData.CustomFieldType.Contribution:
                            case FormData.CustomFieldType.Date:
                            case FormData.CustomFieldType.Time:
                            case FormData.CustomFieldType.FileUpload:
                                //To implement
                            default:
                                break;
                        }
                    }
                }
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
                foreach (CustomFieldResponse responses in reg.CustomFieldResponses)
                {
                    if ((responses is AgendaResponse) && responses.IsUpdate)
                    {
                        AgendaResponse response = responses as AgendaResponse;

                        switch (response.AgendaItem.Type)
                        {
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    AgendaCheckboxResponse resp = response as AgendaCheckboxResponse;
                                    ((CheckBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem).AgendaType).Set(resp.Checked.Value);
                                    if (resp.Code != null)
                                    {
                                        PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem).DiscountCodeInput.Type(resp.Code.Code);
                                    }
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
                            case FormData.CustomFieldType.FileUpload:
                                {
                                    AgendaFileUploadResponse resp = response as AgendaFileUploadResponse;
                                    ((ButtonOrLink)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                        response.AgendaItem).AgendaType).Click();
                                    AutoIt.UploadFile.UploadAFile("File Upload", resp.FileSource);
                                }
                                break;

                            case FormData.CustomFieldType.Duration:
                                {
                                    AgendaResponse_Duration resp = response as AgendaResponse_Duration;
                                    ((TextBox)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem).AgendaType).Type(resp.Duration.ToString("c"));
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    responses.IsUpdate = false;
                }
            }
            PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
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

            if (PageObject.PageObjectProvider.Register.RegistationSite.Checkout.AggreementToWaiver.IsDisplay)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkout.AggreementToWaiver.Set(true);
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
