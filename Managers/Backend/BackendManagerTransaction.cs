namespace RegOnline.RegressionTest.Managers.Backend
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using NUnit.Framework;
	using RegOnline.RegressionTest.Attributes;
	using RegOnline.RegressionTest.DataAccess;
	using RegOnline.RegressionTest.Managers.Register;
	using RegOnline.RegressionTest.UIUtility;
	using RegOnline.RegressionTest.Utilities;

	public partial class BackendManager : ManagerBase
	{
		private const string NextButton = "//span[text()='Next']";
		private const string NewTransactionPayMethodDropDownLocator = "ctl00_cphDialog_TypeId";
		private const string CCAmountLocatorNew = "ctl00_cph_txtPartialPayment";
		private const string CCAmountLocator = "ctl00_cphDialog_total";
		private const string CCNumberListLocator = "ctl00_cph_rblExistingCCs";
		private const string CCNumberRadioButtonLocatorFormat = "ctl00_cph_rblExistingCCs_{0}";
		private const string CCNumberListLocatorOld = "ctl00_cphDialog_rblCCs";
		private const string CCNumberRadioButtonLocatorFormatOld = "ctl00_cphDialog_rblCCs_{0}";
		private const string AmountToBeChargedLocator = "//div[@id='wrpPartialPayment']//div[text()='Amount:']/following-sibling::div";
		private const string TotalToBeChargedLocator = "//div[@id='wrpPartialPayment']//div[text()='Total:']/following-sibling::div";
		private const string SaveAndClose = @"//button[text()='Save & Close']";

		public enum TransactionCellInRow
		{
			Type = 3,
			Amount = 6
		}

		public enum TransactionType
		{
			[StringValue("Online Credit Card Payment")]
			OnlineCCPayment,

			[StringValue("Online Credit Card Refund")]
			OnlineCCRefund,

			[StringValue("Enter a manual (offline) payment")]
			ManualOfflinePayment,

			[StringValue("Enter a manual (offline) refund")]
			ManualOfflineRefund,

			[StringValue("Adjust this registrant's balance due")]
			RevenueAdjustments
		}

		public enum TransactionTypeString
		{
			[StringValue("Online Credit Card Payment")]
			OnlineCCPayment,

			[StringValue("Online Credit Card Refund")]
			OnlineCCRefund,

			[StringValue("Offline Credit Card Payment")]
			OfflineCCPayment,

			[StringValue("Offline Credit Card Refund")]
			OfflineCCRefund,

			[StringValue("Other Charges")]
			OtherCharges,

			[StringValue("Other Payments")]
			OtherPayments
		}

		public enum NewTransactionPayMethod
		{
			[StringValue("Other Charges")]
			OtherCharges,

			[StringValue("Other Credits")]
			OtherCredits,

			[StringValue("Offline Credit Card Payment")]
			OfflineCCPayment,

			[StringValue("Offline Credit Card Refund")]
			OfflineCCRefund,

			[StringValue("Offline Credit Card Chargeback")]
			OfflineCCChargeback,

			[StringValue("Check Payment")]
			CheckPayment,

			[StringValue("Check Refund")]
			CheckRefund,

			[StringValue("Other Payments")]
			OtherPayments,

			[StringValue("Other Refunds")]
			OtherRefunds,

			[StringValue("Cash Payment")]
			CashPayment,

			[StringValue("Wire Transfer Payment")]
			WireTransferPayment,

			[StringValue("Wire Transfer Refund")]
			WireTransferRefund,

			[StringValue("Online credit card payment")]
			OnlineCCPayment,

			[StringValue("Online credit card refund")]
			OnlineCCRefund
		}

		public struct CCInfoLocatorOld
		{
			public const string ExpirationMonth = "ctl00_cphDialog_ExpMonth";
			public const string ExpirationYear = "ctl00_cphDialog_ExpYear";
			public const string HolderName = "ctl00_cphDialog_ccName";
			public const string BillingAddressLineOne = "ctl00_cphDialog_ccAddress1";
			public const string BillingCity = "ctl00_cphDialog_ccCity";
			public const string BillingState = "ctl00_cphDialog_ccState";
			public const string ZipCode = "ctl00_cphDialog_ccZip";
			public const string Country = "ctl00_cphDialog_ddlCountry";
		}

		public struct CCInfoLocatorNew
		{
			public const string ExpirationMonth = "ctl00_cph_ddlCCExpMonth";
			public const string ExpirationYear = "ctl00_cph_ddlCCExpYear";
			public const string HolderName = "ctl00_cph_txtCCName";
			public const string BillingAddressLineOne = "ctl00_cph_txtCCAddress";
			public const string BillingCity = "ctl00_cph_txtCCCity";
			public const string BillingState = "ctl00_cph_txtCCState";
			public const string ZipCode = "ctl00_cph_txtCCZip";
			public const string Country = "ctl00_cph_ddlCCCountry";
		}

		[Step]
		public void SaveAndBypassTransaction()
		{
			UIUtilityProvider.UIHelper.WaitForDisplayAndClick("Cancel", LocateBy.LinkText);
			UIUtilityProvider.UIHelper.SelectOriginalWindow();
			UIUtilityProvider.UIHelper.WaitForPageToLoad();
		}

		[Step]
		public void OpenNewTransaction()
		{
			string transactionWindowName = "Transaction";

			this.OpenAttendeeSubPage(AttendeeSubPage.Transactions);
			UIUtilityProvider.UIHelper.WaitForDisplayAndClick("New Transaction", LocateBy.LinkText);
			UIUtilityProvider.UIHelper.SelectWindowByName(transactionWindowName);
		}

		[Step]
		public void SelectTransactionTypeAndNext(TransactionType type)
		{
			switch (type)
			{
				case TransactionType.OnlineCCPayment:
					UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ROnlineCCPayment", LocateBy.Id);
					break;

				case TransactionType.OnlineCCRefund:
					UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ROnlineCCRefund", LocateBy.Id);
					break;

				case TransactionType.ManualOfflinePayment:
					UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ROfflinePayment", LocateBy.Id);
					break;

				case TransactionType.ManualOfflineRefund:
					UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_ROfflineRefund", LocateBy.Id);
					break;

				case TransactionType.RevenueAdjustments:
					UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_cphDialog_RRevenueBalanceDue", LocateBy.Id);
					break;

				default:
					throw new ArgumentException(string.Format("No such transaction type: {0}", type.ToString()));
			}

			UIUtilityProvider.UIHelper.WaitForDisplayAndClick(NextButton, LocateBy.XPath);
			Utility.ThreadSleep(1);
			UIUtilityProvider.UIHelper.WaitForPageToLoad();
		}

		/// <summary>
		/// If you don't want to use a new CC number, specify 'newCCNumber' as null
		/// </summary>
		/// <param name="newCCNumber"></param>
		/// <param name="amount"></param>
		[Step]
		public void EnterOnlineCCPaymentInfo(string newCCNumber, double amount)
		{
			if (!string.IsNullOrEmpty(newCCNumber))
			{
				this.EnterNewCCNumber(newCCNumber);
			}

			this.EnterCCAmount(amount);
		}

		// Specify 'newCCNumber' as null is to use the original CCNumber
		public void EnterNewCCNumber(string newCCNumber)
		{
			if (UIUtilityProvider.UIHelper.UrlContainsPath("/reports/transactions/New_CC.aspx?"))
			{

				string ccNumberRadioButtonLocator = "ctl00_cphDialog_rCCNumber";
				string ccNumberTextboxLocator = "ctl00_cphDialog_ccNumber";
				UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ccNumberRadioButtonLocator, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(ccNumberTextboxLocator, newCCNumber, LocateBy.Id);
			}
			if (UIUtilityProvider.UIHelper.UrlContainsPath("/register/checkout.aspx"))
			{
				string ccNumberRadioButtonLocator = "ctl00_cph_rbNewCC";
				string ccNumberTextboxLocator = "ctl00_cph_txtCC";
				UIUtilityProvider.UIHelper.WaitForDisplayAndClick(ccNumberRadioButtonLocator, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(ccNumberTextboxLocator, newCCNumber, LocateBy.Id);
			}            
		}

		[Step]
		public void EnterCCAmount(double amount)
		{
			if (UIUtilityProvider.UIHelper.UrlContainsPath("/reports/transactions/New_CC.aspx?"))
			{
				UIUtilityProvider.UIHelper.Type(CCAmountLocator + "_text", amount, LocateBy.Id);
			}
			if (UIUtilityProvider.UIHelper.UrlContainsPath("/register/checkout.aspx"))
			{
				UIUtilityProvider.UIHelper.Type(CCAmountLocatorNew, amount, LocateBy.Id);
			}            
		}

		/// <summary>
		/// Index starts from zero
		/// </summary>
		/// <param name="index"></param>
		[Step]
		public void SelectCCNumber(int index)
		{
			UIUtilityProvider.UIHelper.WaitForDisplayAndClick(string.Format(CCNumberRadioButtonLocatorFormat, index), LocateBy.Id);
		}

		/// <summary>
		/// Index starts from zero
		/// </summary>
		/// <param name="index"></param>
		/// <param name="check"></param>
		[Verify]
		public void VerifyCCNumberSelection(int index, bool check)
		{
			VerifyTool.VerifyValue(
				check,
				UIUtilityProvider.UIHelper.IsChecked(string.Format(CCNumberRadioButtonLocatorFormat, index), LocateBy.Id),
				"CC number " + index.ToString() + " is selected: {0}");
		}

		public void ConfirmCCRefund()
		{
			string confirmYesButtonLocator = "actionYes";
			UIUtilityProvider.UIHelper.WaitForDisplayAndClick(confirmYesButtonLocator, LocateBy.Id);
		}

		[Step]
		public void EnterRevenueAdjustmentsInfo(NewTransactionPayMethod payMethod, double amount)
		{
			UIUtilityProvider.UIHelper.SelectWithText(NewTransactionPayMethodDropDownLocator, StringEnum.GetStringValue(payMethod), LocateBy.Id);
			Utility.ThreadSleep(0.75);
			UIUtilityProvider.UIHelper.Type("ctl00_cphDialog_TransAmount_text", amount, LocateBy.Id);
			Utility.ThreadSleep(0.75);
		}

		/// <summary>
		/// Enter the attendee page transaction default credit card info
		/// </summary>
		public void EnterTransactionDefaultCCInfo()
		{
			if (UIUtilityProvider.UIHelper.UrlContainsPath("/reports/transactions/New_CC.aspx?"))
			{
				this.EnterNewCCNumber(PaymentManager.DefaultPaymentInfo.CCNumber);
				UIUtilityProvider.UIHelper.SelectWithText(CCInfoLocatorOld.ExpirationMonth, DefaultCCExpirationMonth, LocateBy.Id);
				UIUtilityProvider.UIHelper.SelectWithText(CCInfoLocatorOld.ExpirationYear, PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);
				this.EnterDefaultHolderName();
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorOld.BillingAddressLineOne, PaymentManager.DefaultPaymentInfo.BillingAddressLineOne, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorOld.BillingCity, PaymentManager.DefaultPaymentInfo.BillingCity, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorOld.BillingState, PaymentManager.DefaultPaymentInfo.BillingState, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorOld.ZipCode, PaymentManager.DefaultPaymentInfo.ZipCode, LocateBy.Id);
				UIUtilityProvider.UIHelper.SelectWithText(CCInfoLocatorOld.Country, PaymentManager.DefaultPaymentInfo.Country, LocateBy.Id);
			}
			if (UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx"))
			{
				this.EnterNewCCNumber(PaymentManager.DefaultPaymentInfo.CCNumber);
				UIUtilityProvider.UIHelper.SelectWithText(CCInfoLocatorNew.ExpirationMonth, DefaultCCExpirationMonthNew, LocateBy.Id);
				UIUtilityProvider.UIHelper.SelectWithText(CCInfoLocatorNew.ExpirationYear, PaymentManager.DefaultPaymentInfo.ExpirationYear, LocateBy.Id);
				this.EnterDefaultHolderName();
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorNew.BillingAddressLineOne, PaymentManager.DefaultPaymentInfo.BillingAddressLineOne, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorNew.BillingCity, PaymentManager.DefaultPaymentInfo.BillingCity, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorNew.BillingState, PaymentManager.DefaultPaymentInfo.BillingState, LocateBy.Id);
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorNew.ZipCode, PaymentManager.DefaultPaymentInfo.ZipCode, LocateBy.Id);
				UIUtilityProvider.UIHelper.SelectWithText(CCInfoLocatorNew.Country, PaymentManager.DefaultPaymentInfo.Country, LocateBy.Id);
			}
		}

		public void EnterDefaultHolderName()
		{
			if (UIUtilityProvider.UIHelper.UrlContainsPath("/reports/transactions/New_CC.aspx?"))
			{
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorOld.HolderName, PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);
			}
			if (UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx"))
			{
				UIUtilityProvider.UIHelper.Type(CCInfoLocatorNew.HolderName, PaymentManager.DefaultPaymentInfo.HolderName, LocateBy.Id);
			}
		}
		
		[Step]
		public void SaveAndCloseTransaction()
		{
			UIUtilityProvider.UIHelper.WaitForPageToLoad();

			if (UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx"))
			{
				UIUtilityProvider.UIHelper.WaitForDisplayAndClick("//button[@name='ctl00$cph$btnFinalize']", LocateBy.XPath);
			}
			else
			{
				UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
			}

			Utility.ThreadSleep(1);
			UIUtilityProvider.UIHelper.SelectTopWindow();
			UIUtilityProvider.UIHelper.WaitForPageToLoad();
		}

		/// <summary>
		/// This method will charge remaining balance and bypass the confirmation pop-up. 
		/// </summary>
		/// <param name="amount"></param>
		[Step]
		public void ChargeRemainingBalance(double amount)
		{
			decimal charge = Convert.ToDecimal(amount);
			this.ChargeRemainingBalance(charge);
		}

		private void ChargeRemainingBalance(decimal charge)
		{
			string format = StringTool.FormatAmount(Convert.ToDecimal(charge));

			string confirmationPattern = "Are you sure want to CHARGE test test for ${0}?";
				//"^Are you sure want to CHARGE "
				//+ RegOnline.RegressionTest.Managers.Registration.PaymentManager.DefaultPaymentInfo.HolderName
				//+ " for \\${0}\\{1}[\\s\\S]*\\?$";

			confirmationPattern = string.Format(
				confirmationPattern,
				/*decimal.Truncate(charge),
				(charge - decimal.Truncate(charge)).ToString("F2", CultureInfo.InvariantCulture).Remove(0, 1)*/ format);

			UIUtilityProvider.UIHelper.WaitForDisplayAndClick("inButCharge", LocateBy.Id);
			string confirmationMessage = UIUtilityProvider.UIHelper.GetConfirmationText();
			UIUtilityProvider.UIHelper.GetConfirmation();
			VerifyTool.VerifyValue(confirmationPattern, confirmationMessage, "Confirmation: {0}");

			Utility.ThreadSleep(1.5);
			UIUtilityProvider.UIHelper.SelectTopWindow();

			if (UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx"))
			{
				UIUtilityProvider.UIHelper.SelectWindowByName("Charge");
				string totalCharges;

				if (UIUtilityProvider.UIHelper.IsElementPresent(TotalToBeChargedLocator, LocateBy.XPath))
				{
					totalCharges = UIUtilityProvider.UIHelper.GetText(TotalToBeChargedLocator, LocateBy.XPath);
				}
				else
				{
					totalCharges = UIUtilityProvider.UIHelper.GetText(AmountToBeChargedLocator, LocateBy.XPath);
				}

				VerifyTool.VerifyValue("$" + format, totalCharges, "Total to be charged: {0}");
				UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveAndClose, LocateBy.XPath);
				Utility.ThreadSleep(2);
				UIUtilityProvider.UIHelper.SelectOriginalWindow();
			}

			Utility.ThreadSleep(2);
		}

		[Step]
		public void RefundCreditDue(double amount)
		{
			decimal charge = Convert.ToDecimal(amount);
			this.RefundCreditDue(charge);
		}

		private void RefundCreditDue(decimal charge)
		{
			if (UIUtilityProvider.UIHelper.UrlContainsPath("register/checkout.aspx"))
			{
				string confirmationPattern = "Are you sure want to REFUND test test for -${0}{1}?";
				//"^Are you sure want to REFUND "
				//+ RegOnline.RegressionTest.Managers.Registration.PaymentManager.DefaultPaymentInfo.HolderName
				//+ " for -\\${0}\\{1}[\\s\\S]*\\?$";

				confirmationPattern = string.Format(
					confirmationPattern,
					decimal.Truncate(charge),
					(charge - decimal.Truncate(charge)).ToString("F2", CultureInfo.InvariantCulture).Remove(0, 1));

				UIUtilityProvider.UIHelper.WaitForDisplayAndClick("inButRefund", LocateBy.Id);
				string confirmationMessage = UIUtilityProvider.UIHelper.GetConfirmationText();

				VerifyTool.VerifyValue(confirmationPattern, confirmationMessage, "Confirmation: {0}");

				// After clicking 'OK' in the js confirmation dialog, a 'Online credit card refund' window will popup
				UIUtilityProvider.UIHelper.GetConfirmation();
				string refundPopupWindowName = "Charge";
				UIUtilityProvider.UIHelper.SelectWindowByName(refundPopupWindowName);
				string totalCharges;

				if (UIUtilityProvider.UIHelper.IsElementPresent(TotalToBeChargedLocator, LocateBy.XPath))
				{
					totalCharges = UIUtilityProvider.UIHelper.GetText(TotalToBeChargedLocator, LocateBy.XPath);
				}
				else
				{
					totalCharges = UIUtilityProvider.UIHelper.GetText(AmountToBeChargedLocator, LocateBy.XPath);
				}

				VerifyTool.VerifyValue("$" + charge, totalCharges, "Total to be refunded: {0}");

				UIUtilityProvider.UIHelper.WaitForDisplayAndClick(SaveAndClose, LocateBy.XPath);
				UIUtilityProvider.UIHelper.SelectOriginalWindow();

				Utility.ThreadSleep(2);
			}
			else
			{
				string confirmationPattern = "Are you sure want to REFUND test test for -${0}{1}?";
					//"^Are you sure want to REFUND "
					//+ RegOnline.RegressionTest.Managers.Registration.PaymentManager.DefaultPaymentInfo.HolderName
					//+ " for -\\${0}\\{1}[\\s\\S]*\\?$";

				confirmationPattern = string.Format(
					confirmationPattern,
					decimal.Truncate(charge),
					(charge - decimal.Truncate(charge)).ToString("F2", CultureInfo.InvariantCulture).Remove(0, 1));

				UIUtilityProvider.UIHelper.WaitForDisplayAndClick("inButRefund", LocateBy.Id);
				string confirmationMessage = UIUtilityProvider.UIHelper.GetConfirmationText();

				VerifyTool.VerifyValue(confirmationPattern, confirmationMessage, "Confirmation: {0}");

				UIUtilityProvider.UIHelper.GetConfirmation();
				//if (!Regex.IsMatch(confirmationMessage, confirmationPattern))
				//{
				//    Assert.Fail(string.Format("Confirmation msg:{0}; Regex pattern:{1}", confirmationMessage, confirmationPattern));
				//}

				// After clicking 'OK' in the js confirmation dialog, a 'Online credit card refund' window will popup
				string refundPopupWindowName = "Charge";
				//WaitForPopUp(refundPopupWindowName);
				UIUtilityProvider.UIHelper.SelectWindowByName(refundPopupWindowName);
				//string totalCharges;
				//if(IsElementPresent(TotalToBeChargedLocator))
				//{
				//     totalCharges = GetText(TotalToBeChargedLocator);
				//}
				//else
				//{
				//     totalCharges = GetText(AmountToBeChargedLocator); 
				//}
				//VerifyValue("$" + charge, totalCharges, "Total to be refunded: {0}");

				UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00$cph$btnFinalize", LocateBy.Name);
				UIUtilityProvider.UIHelper.SelectOriginalWindow();

				Utility.ThreadSleep(2);
			}
		}

		/// <summary>
		/// Change the transactions of specified registrant to one day ago to settle them
		/// </summary>
		/// <param name="registerId"></param>
		public void ChangeTransactionDateToSettle(int registerId)
		{
			ClientDataContext db = new ClientDataContext();

			List<Transaction> transactions = (from t in db.Transactions where t.RegisterId == registerId select t).ToList();

			foreach (Transaction t in transactions)
			{
				if (t.TransDate.HasValue)
				{
					t.TransDate = t.TransDate.Value.AddDays(-1);
				}
			}

			db.SubmitChanges();
		}

		public void ClickAlterTransaction(int order)
		{
			OpenAttendeeSubPage(AttendeeSubPage.Transactions);
			TransactionResponse[] transactions = GetTransactions();
			string locator;

			if ((order > 0) && (order <= transactions.Length))
			{
				locator = string.Format("//div[@id='transactions']/div[2]/table/tbody/tr[{0}]/td", order + 1) + "[4]/input";
				UIUtilityProvider.UIHelper.WaitForDisplayAndClick(locator, LocateBy.XPath);
				UIUtilityProvider.UIHelper.SelectWindowByName("Transaction");

			}
			else
			{
				Assert.Fail("This is a wrong order.");
				//will go nae further
			}
		}

		public void VoidTransaction(int order)
		{
			ClickAlterTransaction(order);

			//this.EnterTransactionDefaultCCInfo();
			UIUtilityProvider.UIHelper.WaitForDisplayAndClick("actionYes", LocateBy.Id);
			UIUtilityProvider.UIHelper.WaitForDisplayAndClick("ctl00_btnSaveClose", LocateBy.Id);
		}
		
		// Specify the 'rowIndex' to null to verify the last row
		// For history, if you don't want to verify something, specify the parameter to null
		[Verify]
		public void VerifyTransactionHistory(
			int? rowIndex,
			TransactionTypeString typeString, 
			string ccNumber, 
			double amount, 
			int eventId)
		{
			string transactionRowLocator = string.Empty;

			if (rowIndex.HasValue)
			{
				transactionRowLocator = string.Format("{0}[{1}]", TransactionRows, rowIndex);
			}
			else
			{
				// The last row shows the total balance, so locate to the last but one
				transactionRowLocator = string.Format("{0}[{1}]", TransactionRows, "last()-1");
			}

			string transactionCellInRowLocatorFormat = "{0}/td[{1}]";
			
			string transactionTypeCellInRowLocator = string.Format(
				transactionCellInRowLocatorFormat, 
				transactionRowLocator, 
				(int)(TransactionCellInRow.Type));

			string transactionAmountCellInRowLocator = string.Format(
				transactionCellInRowLocatorFormat, 
				transactionRowLocator,
				(int)(TransactionCellInRow.Amount));

			string expectedTransactionTypeCellString = StringEnum.GetStringValue(typeString);

			// If the CC number is specified as null, it will not be verified.
			if (!string.IsNullOrEmpty(ccNumber))
			{
				expectedTransactionTypeCellString += 
					"\r\n" +
					Utility.GetEncryptedCCNumber(ccNumber) +
					" (Details)";
			}

			string actualTransactionTypeCellString = UIUtilityProvider.UIHelper.GetText(transactionTypeCellInRowLocator, LocateBy.XPath);

			VerifyTool.VerifyValue(expectedTransactionTypeCellString, actualTransactionTypeCellString, "Transaction type: {0}");

			VerifyTool.VerifyValue(
				MoneyTool.FormatMoney(amount),
				UIUtilityProvider.UIHelper.GetText(transactionAmountCellInRowLocator, LocateBy.XPath),
				"Transaction amount: {0}");
		}

		/// <summary>
		/// Open transaction details by clicking the 'Details' link in the transaction table.
		/// </summary>
		/// <param name="rowIndex">If the row index is specified as 'null', open details for the last row.</param>
		[Step]
		public void OpenTransactionDetails(int? rowIndex)
		{
			string detailsWindowName = "TransDetails";
			string detailLinkLocator = string.Empty;

			if (rowIndex.HasValue)
			{
				detailLinkLocator = TransactionRows + string.Format("[{0}]", rowIndex) + "/td[3]/a";
			}
			else
			{
				// The last row shows the total balance, so locate to the last but one
				detailLinkLocator = TransactionRows + string.Format("[{0}]", "last()-1") + "/td[3]/a";
			}

			UIUtilityProvider.UIHelper.WaitForDisplayAndClick(detailLinkLocator, LocateBy.XPath);
			//WaitForPopUp(detailsWindowName);
			UIUtilityProvider.UIHelper.SelectWindowByName(detailsWindowName);
			UIUtilityProvider.UIHelper.WaitForPageToLoad();
		}

		[Verify]
		public void VerifyTransactionDetails(
			TransactionTypeString typeString,
			double amount,
			PaymentManager.CCType creditCardType,
			string ccNumber,
			string holderName,
			int eventId)
		{
			string detailDIVLocator = "//div[@id='pageContent']";
			string detailNameLocatorFormat = detailDIVLocator + "//th[text()='{0}']";
			string detailResponseLocatorFormat = detailNameLocatorFormat + "/following-sibling::td";

			VerifyTool.VerifyValue(
				StringEnum.GetStringValue(typeString),
				UIUtilityProvider.UIHelper.GetText(string.Format(detailResponseLocatorFormat, "Type:"), LocateBy.XPath),
				"Transaction type: {0}");

			VerifyTool.VerifyValue(
				MoneyTool.FormatMoney(amount),
				UIUtilityProvider.UIHelper.GetText(string.Format(detailResponseLocatorFormat, "Amount:"), LocateBy.XPath),
				"Transaction amount: {0}");

			VerifyTool.VerifyValue(
				creditCardType.ToString(),
				UIUtilityProvider.UIHelper.GetText(string.Format(detailResponseLocatorFormat, "Credit Card Type:"), LocateBy.XPath),
				"Transaction credit card type: {0}");

			VerifyTool.VerifyValue(
				Utility.GetEncryptedCCNumber(ccNumber),
				UIUtilityProvider.UIHelper.GetText(string.Format(detailResponseLocatorFormat, "Credit Card Number:"), LocateBy.XPath),
				"Transaction credit card number: {0}");

			VerifyTool.VerifyValue(
				holderName,
				UIUtilityProvider.UIHelper.GetText(string.Format(detailResponseLocatorFormat, "Cardholder Name:"), LocateBy.XPath),
				"Transaction cardholder name: {0}");
		}

		[Step]
		public void CloseTransactionDetails()
		{
			UIUtilityProvider.UIHelper.ClosePopUpWindow();
		}

		// Specify the 'maxRefundable' to null so that max refundable will not be verified
		[Verify]
		public void VerifyCCNumberAndMaxRefundable(string ccNumber, double? maxRefundable, int eventId)
		{
			// Sample: ************4448(Max Refundable: $100.00)
			StringBuilder stringToVerify = new StringBuilder(Utility.GetEncryptedCCNumber(ccNumber));

			if (maxRefundable.HasValue)
			{
				stringToVerify.Append(" (Max Refundable: ");
				stringToVerify.Append(MoneyTool.FormatMoney(maxRefundable.Value));
				stringToVerify.Append(")");
			}

			string expectedString = stringToVerify.ToString();
			int ccNumberCount = UIUtilityProvider.UIHelper.GetXPathCountByXPath("//*[@id='" + CCNumberListLocator + "']/input");
			string actualString = string.Empty;

			for (int cnt = 1; cnt <= ccNumberCount; cnt++)
			{
				actualString = UIUtilityProvider.UIHelper.GetText(string.Format("//*[@id='{0}']/label[{1}]", CCNumberListLocator, cnt), LocateBy.XPath);
				
				// If max refundable amount is not to be verified, remove them from the actual string
				if (!maxRefundable.HasValue)
				{
					actualString = actualString.Remove(ccNumber.Length);
				}

				if (actualString.Equals(expectedString))
				{
					return;
				}
			}

			UIUtilityProvider.UIHelper.FailTest(string.Format("Expected: {0}; Actual: {1}", expectedString, actualString));
		}

		[Verify]
		public void VerifyCCAmount(double amount)
		{
			string actual = StringTool.FormatAmount(Convert.ToDecimal(UIUtilityProvider.UIHelper.GetValue(CCAmountLocatorNew, LocateBy.Id)));
			VerifyTool.VerifyValue(amount + ".00", actual, "CC amount: {0}");
		}

		[Verify]
		public void VerifyTransactionDefaultCCInfo(int eventId)
		{
			this.VerifyCCNumberAndMaxRefundable(PaymentManager.DefaultPaymentInfo.CCNumber, null, eventId);

			//VerifyTool.VerifyValue(
			//    DefaultCCExpirationMonth,
			//    UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdown("//select[@id='" + CCInfoLocatorOld.ExpirationMonth + "']"),
			//    "CC expiration month: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.ExpirationYear,
			//    UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdown("//select[@id='" + CCInfoLocatorOld.ExpirationYear + "']"),
			//    "CC expiration year: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.HolderName,
			//    UIUtilityProvider.UIHelper.GetValue(UIUtilityProvider.UIHelper.GetLocatorPrefix(RegOnline.RegressionTest.UIBase.UIManager.FindLocatorBy.Id) + CCInfoLocatorOld.HolderName),
			//    "CC holder name: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.BillingAddressLineOne,
			//    UIUtilityProvider.UIHelper.GetValue(UIUtilityProvider.UIHelper.GetLocatorPrefix(RegOnline.RegressionTest.UIBase.UIManager.FindLocatorBy.Id) + CCInfoLocatorOld.BillingAddressLineOne),
			//    "CC billing address one: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.BillingCity,
			//    UIUtilityProvider.UIHelper.GetValue(UIUtilityProvider.UIHelper.GetLocatorPrefix(RegOnline.RegressionTest.UIBase.UIManager.FindLocatorBy.Id) + CCInfoLocatorOld.BillingCity),
			//    "CC billing city: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.BillingState,
			//    UIUtilityProvider.UIHelper.GetValue(UIUtilityProvider.UIHelper.GetLocatorPrefix(RegOnline.RegressionTest.UIBase.UIManager.FindLocatorBy.Id) + CCInfoLocatorOld.BillingState),
			//    "CC billing state: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.ZipCode,
			//    UIUtilityProvider.UIHelper.GetValue(UIUtilityProvider.UIHelper.GetLocatorPrefix(RegOnline.RegressionTest.UIBase.UIManager.FindLocatorBy.Id) + CCInfoLocatorOld.ZipCode),
			//    "CC zip code: {0}");

			//VerifyTool.VerifyValue(
			//    PaymentManager.DefaultPaymentInfo.Country,
			//    UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdown("//select[@id='" + CCInfoLocatorOld.Country + "']"),
			//    "CC country: {0}");

			VerifyTool.VerifyValue(
				DefaultCCExpirationMonthNew,
				UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdownByXPath("//select[@id='" + CCInfoLocatorNew.ExpirationMonth + "']"),
				"CC expiration month: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.ExpirationYear,
				UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdownByXPath("//select[@id='" + CCInfoLocatorNew.ExpirationYear + "']"),
				"CC expiration year: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.HolderName,
				UIUtilityProvider.UIHelper.GetValue(CCInfoLocatorNew.HolderName, LocateBy.Id),
				"CC holder name: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.BillingAddressLineOne,
				UIUtilityProvider.UIHelper.GetValue(CCInfoLocatorNew.BillingAddressLineOne, LocateBy.Id),
				"CC billing address one: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.BillingCity,
				UIUtilityProvider.UIHelper.GetValue(CCInfoLocatorNew.BillingCity, LocateBy.Id),
				"CC billing city: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.BillingState,
				UIUtilityProvider.UIHelper.GetValue(CCInfoLocatorNew.BillingState, LocateBy.Id),
				"CC billing state: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.ZipCode,
				UIUtilityProvider.UIHelper.GetValue(CCInfoLocatorNew.ZipCode, LocateBy.Id),
				"CC zip code: {0}");

			VerifyTool.VerifyValue(
				PaymentManager.DefaultPaymentInfo.Country,
				UIUtilityProvider.UIHelper.GetSelectedOptionFromDropdownByXPath("//select[@id='" + CCInfoLocatorNew.Country + "']"),
				"CC country: {0}");
		}
	}
}
