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

            if (reg.EventFee_Response != null && reg.EventFee_Response.RegType != null && reg.EventFee_Response.RegType.IsSSO)
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

            if (reg.Event.LodgingTravelPage != null)
            {
                LodgingTravel(reg);
            }

            if (reg.Event.MerchandisePage != null)
            {
                Merchandise(reg);
            }

            CheckoutAndConfirmation(reg);
        }

        public void GroupRegistration(Group group)
        {
            Checkin(group.Primary);
            PersonalInfo(group.Primary);
            Agenda(group.Primary);
            LodgingTravel(group.Primary);
            Merchandise(group.Primary);

            for (int i = 0; i <= group.Secondaries.Count - 1; i++)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.AddAnotherPerson_Click();

                PageObject.PageObjectProvider.Register.RegistationSite.Checkin.EmailAddress.Type(group.Secondaries[i].Email);

                if (group.Secondaries[i].EventFee_Response != null)
                {
                    KeywordProvider.Register_Common.SelectRegType(group.Secondaries[i]);
                    KeywordProvider.Register_Common.TypeEventFeeDiscountCode(group.Secondaries[i]);
                }

                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
                PersonalInfo(group.Secondaries[i]);
                Agenda(group.Secondaries[i]);

                if (group.Secondaries[i].Lodging_Responses.Count != 0)
                {
                    LodgingTravel(group.Secondaries[i]);
                }
                else if (group.Primary.Event.LodgingTravelPage != null)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
                }

                if (group.Secondaries[i].Merchandise_Responses.Count != 0)
                {
                    Merchandise(group.Secondaries[i]);
                }
                else if (group.Primary.Event.MerchandisePage != null)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
                }
            }

            CheckoutAndConfirmation(group.Primary);
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

            if ((reg.EventFee_Response != null) && (reg.Register_Method != RegisterMethod.RegTypeDirectUrl))
            {
                KeywordProvider.Register_Common.SelectRegType(reg);
                KeywordProvider.Register_Common.TypeEventFeeDiscountCode(reg);
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

        public void PerformDefaultActions_PersonalInfo(Registrant reg)
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
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.DateOfBirth.SetValue(date);
            }

            if (reg.Gender.HasValue)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.PersonalInfo.Gender.SelectWithText(reg.Gender.Value.ToString());
            }

            if (reg.Register_Method != RegisterMethod.Admin)
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

            if (reg.CustomField_Responses.Count != 0)
            {
                foreach (CustomFieldResponse responses in reg.CustomField_Responses)
                {
                    if (responses is CFResponse)
                    {
                        CFResponse response = responses as CFResponse;

                        switch (response.CustomField.Type)
                        {
                            case FormData.CustomFieldType.CheckBox:
                                {
                                    CFResponse_Checkbox resp = response as CFResponse_Checkbox;
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
                                // To implement
                                break;
                            case FormData.CustomFieldType.FileUpload:
                                // Impossible
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void PersonalInfo(Registrant reg)
        {
            this.PerformDefaultActions_PersonalInfo(reg);

            if (PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish.IsPresent)
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkout.Finish_Click();
            }
            else
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
        }

        public void PerformDefaultActions_Agenda(Registrant reg)
        {
            foreach (CustomFieldResponse responses in reg.CustomField_Responses)
            {
                if (responses is AgendaResponse)
                {
                    AgendaResponse response = responses as AgendaResponse;

                    switch (response.AgendaItem.Type)
                    {
                        case FormData.CustomFieldType.AlwaysSelected:
                            {
                                AgendaResponse_AlwaysSelected resp = response as AgendaResponse_AlwaysSelected;
                                AgendaRow row = PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem);

                                if (resp.Code != null)
                                {
                                    row.DiscountCodeInput.Type(resp.Code.CodeString);
                                }
                            }
                            break;

                        case FormData.CustomFieldType.CheckBox:
                            {
                                AgendaResponse_Checkbox resp = response as AgendaResponse_Checkbox;
                                AgendaRow row = PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem);
                                ((CheckBox)row.AgendaType).Set(resp.Checked.Value);

                                if (resp.Code != null)
                                {
                                    row.DiscountCodeInput.Type(resp.Code.CodeString);
                                }
                            }
                            break;

                        case FormData.CustomFieldType.RadioButton:
                            {
                                AgendaResponse_MultipleChoice_RadioButton resp = response as AgendaResponse_MultipleChoice_RadioButton;
                                Label choiceItemLabel = new Label(string.Format("//*[contains(text(),'{0}')]", resp.ChoiceItem.Name), LocateBy.XPath);
                                resp.ChoiceItem.Id = Convert.ToInt32(choiceItemLabel.GetAttribute("for"));
                                RadioButton radio = new RadioButton(resp.ChoiceItem.Id.ToString(), LocateBy.Id);
                                radio.Click();

                                AgendaRow row = PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem);

                                if (resp.Code != null)
                                {
                                    row.DiscountCodeInput.Type(resp.Code.CodeString);
                                }
                            }
                            break;

                        case FormData.CustomFieldType.Dropdown:
                            {
                                AgendaResponse_MultipleChoice_DropDown resp = response as AgendaResponse_MultipleChoice_DropDown;

                                AgendaRow row = PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem);

                                ((MultiChoiceDropdown)row.AgendaType).SelectWithText(resp.ChoiceItem.Name);

                                if (resp.Code != null)
                                {
                                    row.DiscountCodeInput.Type(resp.Code.CodeString);
                                }
                            }
                            break;

                        case FormData.CustomFieldType.Number:
                        case FormData.CustomFieldType.OneLineText:
                        case FormData.CustomFieldType.Paragraph:
                            {
                                AgendaResponse_TextInput resp = response as AgendaResponse_TextInput;
                                ((Input)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(resp.CharToInput);
                            }
                            break;

                        case FormData.CustomFieldType.Contribution:
                            {
                                AgendaResponse_Contribution resp = response as AgendaResponse_Contribution;
                                ((Input)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(resp.ContributionAmount);
                            }
                            break;

                        case FormData.CustomFieldType.Date:
                            {
                                AgendaResponse_Date resp = response as AgendaResponse_Date;
                                string date = string.Format("{0}/{1}/{2}", resp.Date.Value.Month, resp.Date.Value.Day, resp.Date.Value.Year);
                                ((Input)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(date);
                            }
                            break;

                        case FormData.CustomFieldType.Time:
                            {
                                AgendaResponse_Time resp = response as AgendaResponse_Time;
                                string time = string.Format("{0}:{1}", resp.Time.Value.Hour, resp.Time.Value.Minute);
                                ((Input)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Type(time);
                            }
                            break;

                        case FormData.CustomFieldType.FileUpload:
                            {
                                AgendaResponse_FileUpload resp = response as AgendaResponse_FileUpload;
                                ((Clickable)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(
                                    response.AgendaItem).AgendaType).Click();
                                AutoIt.UploadFile.UploadAFile("File Upload", resp.FileSource);
                            }
                            break;

                        case FormData.CustomFieldType.Duration:
                            {
                                AgendaResponse_Duration resp = response as AgendaResponse_Duration;
                                ((Input)PageObject.PageObjectProvider.Register.RegistationSite.Agenda.GetAgendaItem(response.AgendaItem).AgendaType).Type(resp.Duration.ToString("c"));
                            }
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("No action defined for specified type of agenda item: {0}", response.AgendaItem.Type.ToString()));
                    }
                }
            }
        }

        public void Agenda(Registrant reg)
        {
            if (reg.CustomField_Responses.Count != 0 && reg.CustomField_Responses.HasAgendaResponse())
            {
                this.PerformDefaultActions_Agenda(reg);
                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
        }

        public void LodgingTravel(Registrant reg)
        {
            if (reg.Lodging_Responses.Count != 0)
            {
                foreach (LodgingResponse response in reg.Lodging_Responses)
                {
                    if (response.Hotel != null)
                    {
                        PageObject.PageObjectProvider.Register.RegistationSite.LodgingAndTravel.CollectLodging.WaitForDisplayAndClick();

                        if (response.CheckinDate.HasValue)
                        {
                            PageObject.PageObjectProvider.Register.RegistationSite.LodgingAndTravel.CheckinDate_Type(response.CheckinDate.Value);
                        }

                        if (response.CheckoutDate.HasValue)
                        {
                            PageObject.PageObjectProvider.Register.RegistationSite.LodgingAndTravel.CheckoutDate_Type(response.CheckoutDate.Value);
                        }

                        if (response.RoomType != null)
                        {
                            PageObject.PageObjectProvider.Register.RegistationSite.LodgingAndTravel.RoomPreference.SelectWithText(response.RoomType.RoomTypeName);
                        }
                    }
                }

                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
        }

        public void PerformDefaultActions_Merchandise(Registrant reg)
        {
            foreach (MerchandiseResponse response in reg.Merchandise_Responses)
            {
                if (response is MerchResponse_FixedPrice)
                {
                    MerchResponse_FixedPrice resp = response as MerchResponse_FixedPrice;
                    PageObject.PageObjectProvider.Register.RegistationSite.Merchandise.MerchInputField(resp.Merchandise_Item).Type(resp.Quantity);

                    if (resp.Discount_Code != null)
                    {
                        PageObject.PageObjectProvider.Register.RegistationSite.Merchandise.MerchDiscountCode(resp.Merchandise_Item).Type(resp.Discount_Code.CodeString);
                    }
                }

                if (response is MerchResponse_VariableAmount)
                {
                    MerchResponse_VariableAmount resp = response as MerchResponse_VariableAmount;
                    PageObject.PageObjectProvider.Register.RegistationSite.Merchandise.MerchInputField(resp.Merchandise_Item).Type(resp.Amount);
                }
            }

            PageObject.PageObjectProvider.Register.RegistationSite.Merchandise.ClickPageContentDivToRefresh();
        }

        public void Merchandise(Registrant reg)
        {
            if (reg.Merchandise_Responses.Count != 0)
            {
                this.PerformDefaultActions_Merchandise(reg);
                PageObject.PageObjectProvider.Register.RegistationSite.Continue_Click();
            }
        }

        public void Checkout(Registrant reg, bool continueWithErrors = false)
        {
            if (reg.WhetherToVerifyFeeOnCheckoutPage)
            {
                VerifyFeeSummary(reg);
            }

            if (reg.Payment_Method != null)
            {
                if (PageObject.PageObjectProvider.Register.RegistationSite.Checkout.PaymentMethodList.IsPresent)
                {
                    PageObject.PageObjectProvider.Register.RegistationSite.Checkout.PaymentMethodList.SelectWithText(
                        FormData.PaymentMethodCheckouLabelAttribute.GetPaymentMethodCheckouLabel(reg.Payment_Method.PMethod));
                }

                switch (reg.Payment_Method.PMethod)
                {
                    case FormData.PaymentMethodEnum.CreditCard:
                        PageObject.PageObjectProvider.Register.RegistationSite.Checkout.BillingInfo_Type(reg.Billing_Info);
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

            // Staying on checkout page after clicking Finish button means we got errors
            if (continueWithErrors && PageObject.PageObjectProvider.Register.RegistationSite.Checkout.DoesCurrentUrlContainsAbsolutePath("register/checkout.aspx"))
            {
                Keyword.KeywordProvider.Register_Common.FailTestWithErrorMessages();
            }
        }

        public void Confirmation(Registrant reg)
        {
            if (PageObject.PageObjectProvider.Register.RegistationSite.IsOnPage(FormData.RegisterPage.ConfirmationRedirect))
            {
                PageObject.PageObjectProvider.Register.RegistationSite.Checkout.AANoThanks_Click();
            }

            reg.Id = Convert.ToInt32(PageObject.PageObjectProvider.Register.RegistationSite.Confirmation.RegistrationId.Text);

            reg.Event.Registrants.Add(reg);
        }

        public void CheckoutAndConfirmation(Registrant reg)
        {
            this.Checkout(reg);
            this.Confirmation(reg);
        }

        public void SSOLogin(Registrant reg)
        {
            this.SSOLogin(reg.Email, reg.Password);
        }

        public void SSOLogin(string email, string password)
        {
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Email.SelectWithText(email);
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Password.SelectWithText(password);
            PageObject.PageObjectProvider.Register.RegistationSite.SSOLogin.Login_Click();
        }

        public void VerifyFeeSummary(Registrant reg)
        {
            string actual_Total = PageObject.PageObjectProvider.Register.RegistationSite.Checkout.FeeSummary_Total.Text;
            reg.ReCalculateFee();
            Utilities.VerifyTool.VerifyValue(Utilities.MoneyTool.FormatMoney(reg.Fee_Summary.Total), actual_Total, "Checkout total: {0}");
        }

        public void VerifyFeeSummary(Registrant reg, int rowIndex)
        {
        }
    }
}
