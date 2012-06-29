namespace RegOnline.RegressionTest.Managers.Backend
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Managers.Register;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;
    using RX = System.Text.RegularExpressions;

    public partial class BackendManager : ManagerBase
    {
        public void VerifyHeaderIsCorrect(string header)
        {
            string attendeeInfoHeader = UIUtilityProvider.UIHelper.GetText("span.panelHeadTallAttendeeName", LocateBy.CssSelector);
            VerifyTool.VerifyValue(attendeeInfoHeader, header, "AttendeeInfo header: {0}");
        }

        [Verify]
        public void VerifyAttendeeInfoInformation(RegisterManager registration)
        {
            this.VerifyAttendeeInfoInformation(registration, registration.CurrentRegistrationId);
        }

        public void VerifyAttendeeInfoInformation(RegisterManager registration, int registrationID)
        {
            this.VerifyCustomFields(registration.customFieldResponses, registrationID);
            this.VerifyAgendaItems(registration.customFieldResponses, registrationID);
            this.VerifyMerchandise(registration.merchandiseResponses, registrationID);
            this.VerifyLodgingInformation(registration);
            this.VerifyTravelInformation(registration);
            this.VerifyTotalCharges(registration.CurrentTotal);
            this.VerifyTotalTransactions(registration.CurrentTotal);
            this.VerifyTotalBalanceDue(registration.CurrentTotal);
        }

        public void VerifyCustomFields(List<RegisterManager.CustomFieldResponses> cfResponses, int registrationID)
        {
            this.VerifyCustomFields(cfResponses.Find(responses => responses.RegistrationID == registrationID).customFields);
        }

        public void VerifyCustomFields(Dictionary<int, RegisterManager.CustomFieldResponse> customFieldResponses)
        {
            List<RegisterManager.CustomFieldResponse> responses = new List<RegisterManager.CustomFieldResponse>();

            foreach (KeyValuePair<int, RegisterManager.CustomFieldResponse> response in customFieldResponses)
            {
                if (response.Value.category == 1)
                {
                    responses.Add(response.Value);
                }
            }

            foreach (RegisterManager.CustomFieldResponse response in responses)
            {
                string foundCustomFieldResponse = UIUtilityProvider.UIHelper.GetText(String.Format(CustomFieldResponseText, response.customFieldId), LocateBy.XPath);

                if (foundCustomFieldResponse.Contains("\n"))
                {
                    foundCustomFieldResponse = foundCustomFieldResponse.Split('\n')[0];
                }

                VerifyTool.VerifyValue(
                    response.customFieldDescription,
                    foundCustomFieldResponse,
                    "Custom field response " + response.customFieldId + ": {0}");
            }
        }

        public void VerifyAgendaItems(List<RegisterManager.CustomFieldResponses> cfResponses, int registrationID)
        {
            this.VerifyAgendaItems(cfResponses.Find(responses => responses.RegistrationID == registrationID).customFields);
        }

        public void VerifyAgendaItems(Dictionary<int, RegisterManager.CustomFieldResponse> customFieldResponses)
        {
            List<RegisterManager.CustomFieldResponse> responses = new List<RegisterManager.CustomFieldResponse>();

            foreach (KeyValuePair<int, RegisterManager.CustomFieldResponse> response in customFieldResponses)
            {
                if (response.Value.category == 2)
                {
                    responses.Add(response.Value);
                }
            }

            foreach (RegisterManager.CustomFieldResponse response in responses)
            {
                string foundCustomFieldResponse = UIUtilityProvider.UIHelper.GetText(String.Format(CustomFieldResponseText, response.customFieldId), LocateBy.XPath);

                if (foundCustomFieldResponse.Contains("\n"))
                {
                    foundCustomFieldResponse = foundCustomFieldResponse.Split('\n')[0];
                }
                if (foundCustomFieldResponse.Contains("\r"))
                {
                    foundCustomFieldResponse = foundCustomFieldResponse.Split('\r')[0];
                }

                VerifyTool.VerifyValue(
                    response.customFieldDescription,
                    foundCustomFieldResponse,
                    "CustomField description " + response.customFieldId + ": {0}");

                if (!String.IsNullOrEmpty(response.code))
                {
                    string foundCustomFieldResponseCode = UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldResponseCodeText, response.customFieldId), LocateBy.XPath);
                    VerifyTool.VerifyValue(response.code, foundCustomFieldResponseCode, "Agenda code " + response.customFieldId + ": {0}");
                }

                if (!String.IsNullOrEmpty(response.amount))
                {
                    string foundCustomFieldResponseAmount = UIUtilityProvider.UIHelper.GetText(String.Format(CustomFieldResponseAmountText, response.customFieldId), LocateBy.XPath);

                    foundCustomFieldResponseAmount = RX.Regex.Replace(foundCustomFieldResponseAmount, NumericRegex, String.Empty);

                    VerifyTool.VerifyValue(
                        Convert.ToDouble(response.amount),
                        Convert.ToDouble(foundCustomFieldResponseAmount),
                        "Agenda amount " + response.customFieldId + ": {0}");
                }
            }
        }

        public void VerifyMerchandise(List<RegisterManager.MerchandiseResponses> merchResponses, int registrationID)
        {
            this.VerifyMerchandise(merchResponses.Find(responses => responses.RegistrationID == registrationID).merchandises);
        }

        public void VerifyMerchandise(Dictionary<int, RegisterManager.MerchandiseResponse> merchandiseResponses)
        {
            double merchandiseTotal = 0;
            List<RegisterManager.MerchandiseResponse> responses = new List<RegisterManager.MerchandiseResponse>();

            foreach (KeyValuePair<int, RegisterManager.MerchandiseResponse> response in merchandiseResponses)
            {
                responses.Add(response.Value);
            }

            foreach (RegisterManager.MerchandiseResponse response in responses)
            {
                //check the fee description
                string foundMerchandiseResponse = UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseResponseText, response.merchandiseId), LocateBy.XPath);
                VerifyTool.VerifyValue(response.merchandiseDescription, foundMerchandiseResponse, "Merchandise description: {0}");

                //check the quantity for the merchandise
                string foundMerchandiseResponseQuantity = UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseQuantityText, response.merchandiseId), LocateBy.XPath);
                int quantity = Convert.ToInt32(foundMerchandiseResponseQuantity);
                int trackedQuantity = Convert.ToInt32(response.response);
                VerifyTool.VerifyValue(trackedQuantity, quantity, "Merchandise quantity: {0}");

                //check the amount for the merchandise
                string foundMerchandiseResponseAmount = UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseAmountText, response.merchandiseId), LocateBy.XPath);
                foundMerchandiseResponseAmount = RX.Regex.Replace(foundMerchandiseResponseAmount, NumericRegex, String.Empty);
                double trackedAmount = Convert.ToDouble(response.amount);
                VerifyTool.VerifyValue(trackedAmount, Convert.ToDouble(foundMerchandiseResponseAmount), "Merchandise amount: {0}");

                //check running sub total
                string foundMerchandiseResponseSubTotal = UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseSubTotalText, response.merchandiseId), LocateBy.XPath);
                foundMerchandiseResponseSubTotal = RX.Regex.Replace(foundMerchandiseResponseSubTotal, NumericRegex, String.Empty);
                double trackedSubTotal = ((double)trackedQuantity * trackedAmount);
                VerifyTool.VerifyValue(trackedSubTotal, Convert.ToDouble(foundMerchandiseResponseSubTotal), "Merchandise subtotal: {0}");

                merchandiseTotal += Convert.ToDouble(trackedSubTotal);
            }

            string foundSubTotalFromOptions = UIUtilityProvider.UIHelper.GetText(SubTotalLocator, LocateBy.XPath);
            foundSubTotalFromOptions = RX.Regex.Replace(foundSubTotalFromOptions, NumericRegex, String.Empty);
            merchandiseTotal += Convert.ToDouble(foundSubTotalFromOptions);

            string foundMerchandiseTotal = UIUtilityProvider.UIHelper.GetText(MerchandiseTotalText, LocateBy.XPath);
            foundMerchandiseTotal = RX.Regex.Replace(foundMerchandiseTotal, NumericRegex, String.Empty);
            VerifyTool.VerifyValue(merchandiseTotal, Convert.ToDouble(foundMerchandiseTotal), "Merchandise total: {0}");
        }

        public void VerifyLodgingInformation(RegisterManager registration)
        {
            //TODO: need to verify ALL lodging information
            //DateTime foundArrivalDate = DateTime.Parse(GetText(LodgingArrivalDate));
            //Assert.AreEqual(registration.lodgingResponses.ArrivalDate, foundArrivalDate);

            //DateTime foundDepartureDate = DateTime.Parse(GetText(LodgingDepartureDate));
            //Assert.AreEqual(registration.lodgingResponses.DepartureDate, foundDepartureDate);

            string foundBedPreference = UIUtilityProvider.UIHelper.GetText(LodgingBedPreference, LocateBy.XPath);
            Assert.AreEqual(registration.lodgingResponses.BedType, foundBedPreference);

            string foundRoomPreference = UIUtilityProvider.UIHelper.GetText(LodgingRoomPreference, LocateBy.XPath);
            Assert.AreEqual(registration.lodgingResponses.RoomType, foundRoomPreference);

            string foundSmokingPreference = UIUtilityProvider.UIHelper.GetText(LodgingSmokingPreference, LocateBy.XPath);
            Assert.AreEqual(registration.lodgingResponses.SmokingPreference, foundSmokingPreference);
        }

        public void VerifyTravelInformation(RegisterManager registration)
        {
            //TODO: need to verify all travel information

            string foundArrivalAirline = UIUtilityProvider.UIHelper.GetText(TravelArrivalAirline, LocateBy.XPath);
            Assert.AreEqual(registration.travelResponses.ArrivalAirline, foundArrivalAirline);

            string foundArrivalFlightNumber = UIUtilityProvider.UIHelper.GetText(TravelArrivalFlightNumber, LocateBy.XPath);
            Assert.AreEqual(registration.travelResponses.ArrivalFlightNumber, foundArrivalFlightNumber);
        }

        public void VerifyTransactions(TransactionResponse[] expectedResponse)
        {
            TransactionResponse[] actualResponses = GetTransactions();

            if (actualResponses.Length != expectedResponse.Length)
            {
                VerifyTool.VerifyValue(expectedResponse.Length, actualResponses.Length, "There are {0} transactions");
            }
            else
            {
                for (int i = 0; i < actualResponses.Length; i++)
                {
                    if (!actualResponses.Equals(expectedResponse))
                    {
                        VerifyTool.VerifyValue(expectedResponse[i].Id, actualResponses[i].Id, "The transaction id is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].Date, actualResponses[i].Date, "The transaction date is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].Type, actualResponses[i].Type, "The transaction type is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].Notes, actualResponses[i].Notes, "The transaction notes is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].Amount, actualResponses[i].Amount, "The transaction amount is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].SubTotal, actualResponses[i].SubTotal, "The transaction sub-total is {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].AddBy, actualResponses[i].AddBy, "The transaction added by {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].ModBy, actualResponses[i].ModBy, "The transaction moded by {0}");
                        VerifyTool.VerifyValue(expectedResponse[i].Delete, actualResponses[i].Delete, "The transaction deleted by {0}");
                    }
                }
            }
        }

        public void VerifyTransactionsByOrder(TransactionResponse expectedResponse, int order)
        {
            TransactionResponse[] actualResponses = GetTransactions();
            if ((0 < order) && (order <= actualResponses.Length))
            {
                TransactionResponse actualResponse = actualResponses[order - 1];

                if (!actualResponse.Equals(expectedResponse))
                {
                    VerifyTool.VerifyValue(expectedResponse.Id, actualResponse.Id, "The transaction id is {0}");
                    VerifyTool.VerifyValue(expectedResponse.Date, actualResponse.Date, "The transaction date is {0}");
                    VerifyTool.VerifyValue(expectedResponse.Type, actualResponse.Type, "The transaction type is {0}");
                    VerifyTool.VerifyValue(expectedResponse.Notes, actualResponse.Notes, "The transaction notes is {0}");
                    VerifyTool.VerifyValue(expectedResponse.Amount, actualResponse.Amount, "The transaction amount is {0}");
                    VerifyTool.VerifyValue(expectedResponse.SubTotal, actualResponse.SubTotal, "The transaction sub-total is {0}");
                    VerifyTool.VerifyValue(expectedResponse.AddBy, actualResponse.AddBy, "The transaction added by {0}");
                    VerifyTool.VerifyValue(expectedResponse.ModBy, actualResponse.ModBy, "The transaction moded by {0}");
                    VerifyTool.VerifyValue(expectedResponse.Delete, actualResponse.Delete, "The transaction deleted by {0}");
                }
            }
            else
            {
                Assert.Fail("This is a wrong order.");
            }
        }

        [Step]
        public TransactionResponse[] GetTransactions()
        {
            List<TransactionResponse> actResponse = new List<TransactionResponse>();

            int count = Convert.ToInt32(UIUtilityProvider.UIHelper.GetXPathCountByXPath(TransactionRows)) - 1;

            for (int i = 0; i < count; i++)
            {
                TransactionResponse response = new TransactionResponse();
                response.Id = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[1]", i + 1), LocateBy.XPath);
                response.Date = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[2]", i + 1), LocateBy.XPath);
                response.Type = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[3]", i + 1), LocateBy.XPath);
                response.Notes = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[5]", i + 1), LocateBy.XPath);
                response.Amount = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[6]", i + 1), LocateBy.XPath);
                response.SubTotal = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[7]", i + 1), LocateBy.XPath);
                response.AddBy = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[8]", i + 1), LocateBy.XPath);
                response.ModBy = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[9]", i + 1), LocateBy.XPath);
                response.Delete = UIUtilityProvider.UIHelper.GetText(string.Format(TransactionRows + "[{0}]/td[10]", i + 1), LocateBy.XPath);
                actResponse.Add(response);
            }

            return actResponse.ToArray();
        }

        public void VerifyRefundOptionExists(bool isRefund)
        {
            UIUtilityProvider.UIHelper.VerifyElementPresent("//input[@name='Refund']", isRefund, LocateBy.XPath);
        }

        [Verify]
        public void VerifyAttendeeFees(
            double expectedTotalCharges,
            double expectedBalanceDue,
            double expectedMerchandiseTotal,
            double expectedTransactionsTotal)
        {
            this.VerifyTotalCharges(expectedTotalCharges);
            this.VerifyTotalBalanceDue(expectedBalanceDue);
            this.VerifyTotalFees(expectedMerchandiseTotal);
            this.VerifyTotalTransactions(expectedTransactionsTotal);
        }

        [Verify]
        public void VerifyTotalCharges(string totalCharges)
        {
            VerifyTool.VerifyValue(totalCharges, UIUtilityProvider.UIHelper.GetText(TotalChargesLocator, LocateBy.XPath), "Total Charges: {0}");
        }

        [Verify]
        public void VerifyTotalTransactions(string totalTransactions)
        {
            VerifyTool.VerifyValue(totalTransactions, UIUtilityProvider.UIHelper.GetText(TotalTransactionsLocator, LocateBy.XPath), "Total Transactions: {0}");
        }

        [Verify]
        public void VerifyTotalBalanceDue(string totalBalanceDue)
        {
            VerifyTool.VerifyValue(totalBalanceDue, UIUtilityProvider.UIHelper.GetText(TotalBalanceDueLocator, LocateBy.XPath), "Total Balance Due: {0}");
        }

        [Verify]
        public void VerifyTotalRecurringFees(double totalFees)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(totalFees),
                UIUtilityProvider.UIHelper.GetText(TotalRecurringChargesLocator, LocateBy.XPath), 
                "Total recurring fees: {0}");
        }

        public void VerifyTotalCharges(double totalCharges)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(totalCharges),
                UIUtilityProvider.UIHelper.GetText(TotalChargesLocator, LocateBy.XPath), 
                "Total Charges: {0}");
        }

        public string GetTotalCharges()
        {
            string totalCharges = UIUtilityProvider.UIHelper.GetText(TotalChargesLocator, LocateBy.XPath);
            return totalCharges; 
        }

        public void VerifyTotalTransactions(double totalTransactions)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(totalTransactions),
                UIUtilityProvider.UIHelper.GetText(TotalTransactionsLocator, LocateBy.XPath), 
                "Total Transactions: {0}");
        }

        [Verify]
        public void VerifyTotalBalanceDue(double totalBalanceDue)
        {
            OpenAttendeeSubPage(AttendeeSubPage.ViewAll);

            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(totalBalanceDue),
                UIUtilityProvider.UIHelper.GetText(TotalBalanceDueLocator, LocateBy.XPath), 
                "Total Balance Due: {0}");
        }

        public void VerifyTotalFees(double totalFees)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(totalFees),
                UIUtilityProvider.UIHelper.GetText(MerchandiseTotalText, LocateBy.XPath),
                "Total Fees: {0}");
        }

        [Verify]
        public void VerifyEventCost(double eventCost, CustomFieldStatus? status)
        {
            string eventCostTRLocator = "//tr[@id='trEventCost']";
            string eventCostStatusLocator = eventCostTRLocator + "//font";
            string eventCostLocator = eventCostStatusLocator + "/following-sibling::table";

            if (status.HasValue)
            {
                VerifyTool.VerifyValue(
                    "(" + StringEnum.GetStringValue(status.Value) + ")", 
                    UIUtilityProvider.UIHelper.GetText(eventCostStatusLocator, LocateBy.XPath), 
                    "Event Cost Status: {0}");
            }

            VerifyTool.VerifyValue(
                "Cost: " + MoneyTool.FormatMoney(eventCost),
                UIUtilityProvider.UIHelper.GetText(eventCostLocator, LocateBy.XPath),
                "Event Cost: {0}");
        }

        public void VerifyCFStatus(int cfId, string cfName, CustomFieldStatus? status)
        {
            if (status.HasValue)
            {
                VerifyTool.VerifyValue(
                    "(" + StringEnum.GetStringValue(status) + ")",
                    UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldResponseText, cfId.ToString()) + "/font", LocateBy.XPath),
                    "CF item '" + cfName + "'status: {0}");
            }
        }

        public void VerifyCFFee(int cfId, string cfName, double expectedFee)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(expectedFee),
                UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldResponseAmountText, cfId.ToString()), LocateBy.XPath),
                "CF item '" + cfName + "' fee: {0}");
        }

        [Verify]
        public void VerifyCFCheckboxItem(int cfId, string cfName, bool check, CustomFieldStatus? status)
        {
            VerifyTool.VerifyValue(
                check,
                UIUtilityProvider.UIHelper.IsElementPresent(string.Format(CustomFieldResponseText, cfId.ToString()), LocateBy.XPath), 
                "CF checkbox item '" + cfName + "' is present: {0}");

            this.VerifyCFStatus(cfId, cfName, status);
        }

        [Verify]
        public void VerifyCFCheckboxItem(
            int cfId, 
            string cfName, 
            bool check, 
            CustomFieldStatus? status, 
            double expectedFee)
        {
            this.VerifyCFCheckboxItem(cfId, cfName, check, status);
            this.VerifyCFFee(cfId, cfName, expectedFee);
        }

        [Verify]
        public void VerifyCFMultiChoice(int cfId, string cfName, string expectedChoice, CustomFieldStatus? status)
        {
            VerifyTool.VerifyValue(
                expectedChoice,
                UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldResponseText, cfId.ToString()) + "/../following-sibling::tr//b", LocateBy.XPath),
                "CF multi-choice '" + cfName + "' choice: {0}");

            this.VerifyCFStatus(cfId, cfName, status);
        }

        [Verify]
        public void VerifyCFMultiChoice(
            int cfId,
            string cfName,
            string expectedChoice,
            CustomFieldStatus? status,
            double expectedFee)
        {
            this.VerifyCFMultiChoice(cfId, cfName, expectedChoice, status);
            this.VerifyCFFee(cfId, cfName, expectedFee);
        }

        [Verify]
        public void VerifyCFParagraph(int cfId, string cfName, string expectedParagraph, CustomFieldStatus? status)
        {
            VerifyTool.VerifyValue(
                expectedParagraph,
                UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldResponseText, cfId.ToString()) + "/../following-sibling::tr//b", LocateBy.XPath),
                "CF paragraph item '" + cfName + "' text: {0}");

            this.VerifyCFStatus(cfId, cfName, status);
        }

        [Verify]
        public void VerifyCFNumberTextDateTime(int cfId, string cfName, string expectedText, CustomFieldStatus? status)
        {
            VerifyTool.VerifyValue(
                expectedText,
                UIUtilityProvider.UIHelper.GetText(string.Format(CustomFieldResponseText, cfId.ToString()) + "/../td/b", LocateBy.XPath),
                "CF number/text/date/time item '" + cfName + "' text: {0}");

            this.VerifyCFStatus(cfId, cfName, status);
        }

        [Verify]
        public void VerifyCFContribution(
            int cfId, 
            string cfName, 
            CustomFieldStatus? status, 
            double expectedFee)
        {
            this.VerifyCFStatus(cfId, cfName, status);
            this.VerifyCFFee(cfId, cfName, expectedFee);
        }

        [Verify]
        public void VerifyConfirmationOnHotelChange()
        {
            UIUtilityProvider.UIHelper.GetConfirmation();
        }

        [Verify]
        public void VerifyLodgingField(LodgingViewField field, object expectedValue)
        {
            string expectedValueString = Convert.ToString(expectedValue);
            string actualValue = UIUtilityProvider.UIHelper.GetText(this.GetLocator(field), LocateBy.XPath);

            VerifyTool.VerifyValue(
                expectedValueString, 
                actualValue, 
                "Lodging field '" + field.ToString() +  "' : {0}");
        }

        [Verify]
        public void VerifyTravelField(TravelViewField field, object expectedValue)
        {
            string expectedValueString = Convert.ToString(expectedValue);
            string actualValue = UIUtilityProvider.UIHelper.GetText(this.GetLocator(field), LocateBy.XPath);

            VerifyTool.VerifyValue(
                expectedValueString,
                actualValue,
                "Travel field '" + field.ToString() + "' : {0}");
        }

        [Verify]
        public void VerifyMerchandiseItemResponse(int merchId, string name)
        {
            VerifyTool.VerifyValue(name, UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseResponseText, merchId), LocateBy.XPath), "Merchandise item response: {0}");
        }

        [Verify]
        public void VerifyMerchandiseItemQuantity(int merchId, int quantity)
        {
            VerifyTool.VerifyValue(
                quantity.ToString(),
                UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseQuantityText, merchId), LocateBy.XPath), 
                "Merchandise item quantity: {0}");
        }

        [Verify]
        public void VerifyMerchandiseItemAmount(int merchId, double amount)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(amount),
                UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseAmountText, merchId), LocateBy.XPath), 
                "Merchandise item amount: {0}");
        }

        [Verify]
        public void VerifyMerchandiseItemSubtotal(int merchId, double subTotal)
        {
            VerifyTool.VerifyValue(
                MoneyTool.FormatMoney(subTotal),
                UIUtilityProvider.UIHelper.GetText(string.Format(MerchandiseSubTotalText, merchId), LocateBy.XPath), 
                "Merchandise item subtotal: {0}");
        }

        [Verify]
        public void VerifyNextRenewDate(DateTime expectedDate)
        {
            VerifyTool.VerifyValue(expectedDate.ToString("dd-MMM-yyyy"), UIUtilityProvider.UIHelper.GetText(NextRenewDateLinkLocator, LocateBy.XPath), "Next renew date: {0}");
        }

        [Verify]
        public void VerifyNextPayDate(DateTime expectedDate)
        {
            VerifyTool.VerifyValue(expectedDate.ToString("dd-MMM-yyyy"), UIUtilityProvider.UIHelper.GetText("lblNextPayDate", LocateBy.Id), "Next pay date: {0}");
        }

        public void VerifyUpdateHistory(string expectedNotes)
        {
            if (!UIUtilityProvider.UIHelper.IsTextPresent(expectedNotes))
            {
                UIUtilityProvider.UIHelper.FailTest("Expected updating history notes '" + expectedNotes + "' not present!");
            }
        }

        [Verify]
        public void VerifyErrorMessage(string message)
        {
            Assert.IsTrue(UIUtilityProvider.UIHelper.IsElementDisplay(string.Format("//b[text()='{0}']", message), LocateBy.XPath));
        }

        public void VerifyPaymentMethod(RegOnline.RegressionTest.Managers.Builder.PaymentMethodManager.PaymentMethod paymentMethod)
        {
            VerifyTool.VerifyValue(StringEnum.GetStringValue(paymentMethod), UIUtilityProvider.UIHelper.GetText("tdPaymentMethod", LocateBy.Id), "Payment method: {0}");
        }

        [Verify]
        public void VerifyRegistrationTypeOptions(List<string> regTypes)
        {
            VerifyTool.VerifyList(regTypes, GetRegTypeOptions());
        }

        private List<string> GetRegTypeOptions()
        {
            List<string> types = new List<string>();
            string regTypeLocator = "//select[@id='RegTypeId']";
            int count = Convert.ToInt32(UIUtilityProvider.UIHelper.GetXPathCountByXPath(regTypeLocator + "/option"));
            string regTypeFormat = regTypeLocator + "/option[{0}]";
            for (int i = 2; i <= count; i++)
            {
                types.Add(UIUtilityProvider.UIHelper.GetText(string.Format(regTypeFormat, i), LocateBy.XPath).Trim());
            }
            return types;
        }

        public void VerifyPaymentInfoCCNumber(string ccNumber)
        {
            VerifyTool.VerifyValue(ccNumber, UIUtilityProvider.UIHelper.GetText("tdMaskedCCNumber", LocateBy.Id), "Payment Info CC number: {0}");
        }
    }
}
