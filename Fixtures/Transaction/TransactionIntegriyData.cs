namespace RegOnline.RegressionTest.Fixtures.Transaction.TransactionIntegrity
{
    using RegOnline.RegressionTest.Utilities;
    using System;

    internal class ProEvent
    {
        public const string EventName = "TransactionIntegrityFixture_ProEvent";
        public const double EventFee = 10;
        public const string DiscountCode = "half";
        public const double DiscountPercentage = 0.5;
        public const double GroupDiscountPercentage = 0.5;
        public const double ServiceFeePercentage = 1;
        public const int GroupCount = 3;

        // See this fee on admin site -> Open customer db -> Billing -> Edit billing fees and percentages
        public const double PerRegFee = 6;

        public const double CCChargePercentage = 0.0395;

        public class Tax
        {
            public class Name
            {
                public const string TaxOne = "Tax 1";
                public const string TaxTwo = "Tax 2";
            }

            public class Percentage
            {
                public const double TaxOne = 0.1;
                public const double TaxTwo = 0.2;
            }
        }

        public class RegType
        {
            public class Name
            {
                public const string One = "Registrant type 1";
                public const string Two = "Registrant type 2";
            }

            public class Fee
            {
                public const double One = 50;
                public const double Two = 25;
            }
        }

        public class AgendaItem
        {
            public class Fee
            {
                public const double Checkbox = 10;
                public const double RadioButtonOnParent = 10;
                public const double RadioButtonOnItems_Ex = 20;
                public const double RadioButtonOnItems_Less = 10;
                public const double AlwaysSelected = 10;
                public const double DropdownOnParent = 10;
                public const double DropdownOnItems_Fair = 20;
                public const double DropdownOnItems_Ade = 10;
                public const double Contribution = 10;
            }

            public class Name
            {
                public const string CheckboxAgendaPage = "Checkbox Agenda Page";
                public const string RadioButtonsParentPrice = "Radio Buttons Price On Parent";
                public const string RadioButtonsParentPriceSelection = "Medium";
                public const string RadionButtonsPriceOnItem = "Radio Buttons Price on Items";
                public const string RadioButtonsOnItemSelection = "Less Expensive: $10.00";
                public const string RadioButtonsOnItemSelectionPound = "Less Expensive: £10.00";
                public const string ContributionAgendaPage = "Contribution Agenda Page";
                public const string DropDownParentPrice = "Drop Down Price on Parent Item";
                public const string DropDownParentSelection = "Thursday";
                public const string DropDownPriceOnItem = "Drop Down Price on Item";
                public const string DropDownOnItemSelection = "Adequately Priced: $10.00";
                public const string DropDownOnItemSelectionPound = "Adequately Priced: £10.00";
                public const string AlwaysSelectedAgenda = "Always Selected Agenda With Fee";
            }
        }

        public class LT
        {
            public const string CheckinDate = "7/13/2015";
            public const string CheckoutDate = "7/17/2015";
            public const string RoomPreference = "Boulderado King Room";
            public const double HotelFeePerNight = 25;
            public const int Nights = 4;
            public const double BookingFee = 10;
        }

        public class Merchandise
        {
            public class Fee
            {
                public const double FixedPrice = 10;
                public const double FixedPriceWithMultiChoice = 10;
                public const double ShippingFeePerItem = 10;
                public const int Quantity = 2;
                public const double VariableAmount = 20;
            }

            public class Name
            {
                public const string MerchFixedPrice = "Merchandise Fixed Price";
                public const string MerchFixedPriceMC = "Merchandise Fixed Price W/MC";
                public const string MerchVariableAmount = "Merchandise Variable Amount";
                public const string MerchVariableAmountMC = "Merchandise Variable Amount W/MC";
                public const string FixedPriceMCSelection = "item 2";
                public const string VariableAmountMCSelection = "Second Item";
            }
        }

        public class FeeCalculation_SingleReg : IFeeCalculation
        {
            private const double FeeItemsWithDiscount =
                (RegType.Fee.One +
                AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade +
                Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity);

            private const double DiscountedFee = FeeItemsWithDiscount * (1 - DiscountPercentage);

            private const double NonDiscountedFee = 
                AgendaItem.Fee.Contribution + 
                Merchandise.Fee.VariableAmount + 
                Merchandise.Fee.VariableAmount;

            private const double _AgendaFeeTotal =
                (AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade) * (1 - DiscountPercentage) +
                AgendaItem.Fee.Contribution;

            private const double ShippingFee =
                Merchandise.Fee.ShippingFeePerItem *
                (Merchandise.Fee.Quantity + Merchandise.Fee.Quantity + 1 + 1);

            private const double _MerchandiseFeeTotal =
                (Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity) * (1 - DiscountPercentage) +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount +
                ShippingFee;

            private const double SubTotal = DiscountedFee + NonDiscountedFee;

            // Agenda item with the type of 'Contribution' cannot be applied with any tax
            private const double TaxOne = Tax.Percentage.TaxOne * (SubTotal - AgendaItem.Fee.Contribution);

            private const double TaxTwo = Tax.Percentage.TaxTwo * (SubTotal - AgendaItem.Fee.Contribution);

            private const double LodgingSubTotal = LT.Nights * LT.HotelFeePerNight;

            private const double LodgingFeeTotal = LodgingSubTotal + LT.BookingFee;

            private readonly static double TransactionFees = Math.Round(
                CCChargePercentage * (SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal), 
                2);

            private readonly static double ServiceFee = PerRegFee + TransactionFees;

            private readonly static double TotalFee = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal + ServiceFee;

            private const double DiscountSaving = FeeItemsWithDiscount * DiscountPercentage;

            /// <summary>
            /// These arrays are for verifying data in our Transaction and Transaction fees reports.
            /// For the transaction fees report the data it is looking at in order is this:
            /// 
            /// 1.Total Amount Collected, 
            /// 2.TransactionFees, 
            /// 3.Registrants in group, 
            /// 4.Total per Reg Fee,
            /// 5.Net Amount Due/Owed, 
            /// 6.Transaction %, 
            /// 7.Transaction Fee, 
            /// 8.Per Reg Fee, 
            /// 9.Flat Fee, 
            /// 10.Per Reg Fee,
            /// 11.Group Count, 
            /// 12.Total Fees, 
            /// 13.% Passed To Participant,
            /// 14.Total Paid By Participant,
            /// 15.Fees Due.
            /// 
            /// Note: if the Transactions Report is failing it might be due to an outstanding balance due. 
            /// This test assumes everyone in the report is paid in full.
            /// </summary>
            private readonly static string[] _ExpectedTransactionFeesReportData = new string[15]
            { 
                MoneyTool.FormatMoney(-TotalFee), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //5.Net Amount Due/Owed
                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //6.Transaction %
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //7.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees), //8.Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(-ServiceFee), //14.Total Paid By Participant
                MoneyTool.FormatMoney(0) //15.Fees Due
            };

            /// <summary>
            /// These arrays are for the expect data in the Transactions report. They check the: 
            /// 1.Type, 
            /// 2.Amount, 
            /// 3.Running balance, 
            /// 4.Transaction Notes. 
            /// </summary>
            private readonly static string[] _ExpectedTransactionReportData = new string[13]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee), MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee), 
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee), MoneyTool.FormatMoney(TotalFee - ServiceFee), 
                "Service Fee", MoneyTool.FormatMoney(ServiceFee), MoneyTool.FormatMoney(TotalFee), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalFee), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_SingleReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get
                {
                    return TotalFee;
                }
            }

            public double? AgendaFeeTotal
            {
                get
                {
                    return _AgendaFeeTotal;
                }
            }

            public double? MerchandiseFeeTotal
            {
                get
                {
                    return _MerchandiseFeeTotal;
                }
            }

            public double? ShippingFeeTotal
            {
                get
                {
                    return ShippingFee;
                }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return ServiceFee;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get
                {
                    return TaxOne;
                }
            }

            public double? Tax2Amount
            {
                get
                {
                    return TaxTwo;
                }
            }

            public double? LodgingBookingFee
            {
                get
                {
                    return LT.BookingFee;
                }
            }

            public double? LodgingSubtotal
            {
                get
                {
                    return LodgingSubTotal;
                }
            }

            public double? Subtotal
            {
                get
                {
                    return SubTotal;
                }
            }

            public double? DiscountCodeSavings
            {
                get
                {
                    return DiscountSaving;
                }
            }

            public double? GroupDiscountSavings
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringSubtotal
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringTax1Amount
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringTax2Amount
            {
                get
                {
                    return null;
                }
            }

            public double? YearlyFees
            {
                get
                {
                    return null;
                }
            }

            public double? YearlyFeesDiscount
            {
                get
                {
                    return null;
                }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }

        public class FeeCalculation_GroupReg : IFeeCalculation
        {
            private const double NonMerchFeeItemsWithDiscount =
                (RegType.Fee.One +
                AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade);

            private const double PrimaryDiscountedFee = NonMerchFeeItemsWithDiscount * (1 - DiscountPercentage);

            private const double GroupMemberGroupDiscountedFee = NonMerchFeeItemsWithDiscount * (1 - GroupDiscountPercentage);

            private const double GroupMemberDiscountedFee = GroupMemberGroupDiscountedFee * (1 - DiscountPercentage);

            private const double GroupAgendaContributionFee = AgendaItem.Fee.Contribution * GroupCount;

            private const double ShippingFee =
                Merchandise.Fee.ShippingFeePerItem *
                (Merchandise.Fee.Quantity + Merchandise.Fee.Quantity + 1 + 1);

            private const double MerchandiseFeeDiscountSaving =
                (Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity) * DiscountPercentage;

            private const double MerchandiseFeeTotalWithoutShippingFee =
                (Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity) * (1 - DiscountPercentage) +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double Fee_Primary = PrimaryDiscountedFee;

            private const double Fee_GroupMember = GroupMemberDiscountedFee * (GroupCount - 1);

            private const double SubTotal = 
                Fee_Primary + 
                Fee_GroupMember + 
                GroupAgendaContributionFee + 
                MerchandiseFeeTotalWithoutShippingFee;

            private const double TaxOne = Tax.Percentage.TaxOne * (SubTotal - GroupAgendaContributionFee);

            private const double TaxTwo = Tax.Percentage.TaxTwo * (SubTotal - GroupAgendaContributionFee);

            // The second registrant is sharing the room with the primary registrant
            private const double LodgingSubTotal = LT.Nights * LT.HotelFeePerNight * (GroupCount - 1);

            private const double _LodgingBookingFee = LT.BookingFee * (GroupCount - 1);

            private const double LodgingFeeTotal = LodgingSubTotal + _LodgingBookingFee;

            private const double TransactionFeeItems = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal;

            private readonly static double TransactionFees = Math.Round(
                CCChargePercentage * TransactionFeeItems,
                2);

            private const double GroupPerRegFee = PerRegFee * GroupCount;

            private readonly static double ServiceFee = GroupPerRegFee + TransactionFees;

            private readonly static double TotalFee = TransactionFeeItems + ServiceFee;

            private const double GroupMemberGroupDiscountSaving = NonMerchFeeItemsWithDiscount * GroupDiscountPercentage;

            // Primary is not included according to group discount rule's settings
            private const double GroupDiscountSaving = GroupMemberGroupDiscountSaving * (GroupCount - 1);

            private const double PrimaryDiscountSaving = NonMerchFeeItemsWithDiscount * DiscountPercentage;

            private const double GroupMemberDiscountSaving = GroupMemberGroupDiscountSaving * DiscountPercentage;

            private const double DiscountSaving = PrimaryDiscountSaving + GroupMemberDiscountSaving * (GroupCount - 1) + MerchandiseFeeDiscountSaving;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[15]
            { 
                MoneyTool.FormatMoney(-TotalFee), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                GroupCount.ToString(), //3.Registrants in group
                MoneyTool.FormatMoney(GroupPerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //5.Net Amount Due/Owed
                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //6.Transaction %
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //7.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees), //8.Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                GroupCount.ToString(), //11.Group Count
                MoneyTool.FormatMoney(ServiceFee), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(-ServiceFee), //14.Total Paid By Participant
                MoneyTool.FormatMoney(0) //15.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[13]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee), MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee), 
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee), MoneyTool.FormatMoney(TotalFee - ServiceFee), 
                "Service Fee", MoneyTool.FormatMoney(ServiceFee), MoneyTool.FormatMoney(TotalFee), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalFee), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_GroupReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get { return TotalFee; }
            }

            public double? AgendaFeeTotal
            {
                get { return null; }
            }

            public double? MerchandiseFeeTotal
            {
                get { return null; }
            }

            public double? ShippingFeeTotal
            {
                get { return null; }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return ServiceFee;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get { return TaxOne; }
            }

            public double? Tax2Amount
            {
                get { return TaxTwo; }
            }

            public double? LodgingBookingFee
            {
                get { return _LodgingBookingFee; }
            }

            public double? LodgingSubtotal
            {
                get { return LodgingSubTotal; }
            }

            public double? Subtotal
            {
                get { return SubTotal; }
            }

            public double? DiscountCodeSavings
            {
                get { return DiscountSaving; }
            }

            public double? GroupDiscountSavings
            {
                get { return GroupDiscountSaving; }
            }

            public double? RecurringSubtotal
            {
                get { return null; }
            }

            public double? RecurringTax1Amount
            {
                get { return null; }
            }

            public double? RecurringTax2Amount
            {
                get { return null; }
            }

            public double? YearlyFees
            {
                get { return null; }
            }

            public double? YearlyFeesDiscount
            {
                get { return null; }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }

        public class FeeCalculation_SimpleReg : IFeeCalculation
        {
            private const double SubTotal = RegType.Fee.One + AgendaItem.Fee.AlwaysSelected;
            private const double TaxOne = SubTotal * Tax.Percentage.TaxOne;
            private const double TaxTwo = SubTotal * Tax.Percentage.TaxTwo;
            private const double TransactionFeeItems = SubTotal + TaxOne + TaxTwo;

            private readonly static double TransactionFees = Math.Round(
                CCChargePercentage * TransactionFeeItems,
                2);

            private readonly static double ServiceFee = PerRegFee + TransactionFees;

            private readonly static double TotalFee = TransactionFeeItems + ServiceFee;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[15]
            { 
                MoneyTool.FormatMoney(-TotalFee), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //5.Net Amount Due/Owed
                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //6.Transaction %
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //7.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees), //8.Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(-ServiceFee), //14.Total Paid By Participant
                MoneyTool.FormatMoney(0) //15.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[10]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalFee - ServiceFee), MoneyTool.FormatMoney(TotalFee - ServiceFee), 
                "Service Fee", MoneyTool.FormatMoney(ServiceFee), MoneyTool.FormatMoney(TotalFee), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalFee), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_SimpleReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get { return TotalFee; }
            }

            public double? AgendaFeeTotal
            {
                get { return null; }
            }

            public double? MerchandiseFeeTotal
            {
                get { return null; }
            }

            public double? ShippingFeeTotal
            {
                get { return null; }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return ServiceFee;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get { return TaxOne; }
            }

            public double? Tax2Amount
            {
                get { return TaxTwo; }
            }

            public double? LodgingBookingFee
            {
                get { return null; }
            }

            public double? LodgingSubtotal
            {
                get { return null; }
            }

            public double? Subtotal
            {
                get { return SubTotal; }
            }

            public double? DiscountCodeSavings
            {
                get { return null; }
            }

            public double? GroupDiscountSavings
            {
                get { return null; }
            }

            public double? RecurringSubtotal
            {
                get { return null; }
            }

            public double? RecurringTax1Amount
            {
                get { return null; }
            }

            public double? RecurringTax2Amount
            {
                get { return null; }
            }

            public double? YearlyFees
            {
                get { return null; }
            }

            public double? YearlyFeesDiscount
            {
                get { return null; }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }

        public class FeeCalculation_UpdateReg : IFeeCalculation
        {
            private const double SubTotal_InitialReg = RegType.Fee.One + AgendaItem.Fee.AlwaysSelected;
            private const double TaxOne_InitialReg = SubTotal_InitialReg * Tax.Percentage.TaxOne;
            private const double TaxTwo_InitialReg = SubTotal_InitialReg * Tax.Percentage.TaxTwo;
            private const double TransactionFeeItems_InitialReg = SubTotal_InitialReg + TaxOne_InitialReg + TaxTwo_InitialReg;

            private readonly static double TransactionFees_InitialReg = Math.Round(
                CCChargePercentage * TransactionFeeItems_InitialReg,
                2);

            private readonly static double ServiceFee_InitialReg = PerRegFee + TransactionFees_InitialReg;

            private readonly static double TotalFee_InitialReg = TransactionFeeItems_InitialReg + ServiceFee_InitialReg;

            private const double TransactionAmount_InitialReg = SubTotal_InitialReg + TaxOne_InitialReg + TaxTwo_InitialReg;

            private const double FeeItemsWithDiscount =
                (AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade +
                Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity);

            private const double DiscountedFee = FeeItemsWithDiscount * (1 - DiscountPercentage);

            private const double NonDiscountedFee =
                RegType.Fee.One +
                AgendaItem.Fee.Contribution +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double ShippingFee =
                Merchandise.Fee.ShippingFeePerItem *
                (Merchandise.Fee.Quantity + Merchandise.Fee.Quantity + 1 + 1);

            private const double SubTotal = DiscountedFee + NonDiscountedFee;

            private const double TaxOne = Tax.Percentage.TaxOne * (SubTotal - AgendaItem.Fee.Contribution);

            private const double TaxTwo = Tax.Percentage.TaxTwo * (SubTotal - AgendaItem.Fee.Contribution);

            private const double LodgingSubTotal = LT.Nights * LT.HotelFeePerNight;

            private const double LodgingFeeTotal = LodgingSubTotal + LT.BookingFee;

            private readonly static double TransactionFees = Math.Round(
                CCChargePercentage * (SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal),
                2);

            private readonly static double ServiceFee = PerRegFee + TransactionFees;

            private readonly static double TotalFee = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal + ServiceFee;

            private const double DiscountSaving = FeeItemsWithDiscount * DiscountPercentage;

            private const double TransactionAmount_UpdateReg = SubTotal + TaxOne + TaxTwo + LodgingFeeTotal - TransactionAmount_InitialReg;

            private readonly static double TransactionFee_UpdateReg = ServiceFee - ServiceFee_InitialReg;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[25]
            { 
                MoneyTool.FormatMoney(-TotalFee), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //5.Net Amount Due/Owed

                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //6.Transaction %
                MoneyTool.FormatMoney(-TransactionAmount_InitialReg), //7.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees_InitialReg), //8.Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee_InitialReg), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(-ServiceFee_InitialReg), //14.Total Paid By Participant
                MoneyTool.FormatMoney(0), //15.Fees Due

                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //16.Transaction %
                MoneyTool.FormatMoney(-(TransactionAmount_UpdateReg + ShippingFee)), //17.Transaction Amount
                MoneyTool.FormatMoney(TransactionFee_UpdateReg), //18.Transaction Fee
                MoneyTool.FormatMoney(0), //19.Flat Fee
                null, //20.Per Reg Fee
                null, //21.Group Count
                MoneyTool.FormatMoney(TransactionFee_UpdateReg), //22.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //23.% Passed To Participant
                MoneyTool.FormatMoney(-TransactionFee_UpdateReg), //24.Total Paid By Participant
                MoneyTool.FormatMoney(0) //25.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[23]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TransactionAmount_InitialReg), MoneyTool.FormatMoney(TransactionAmount_InitialReg), 
                "Service Fee", MoneyTool.FormatMoney(ServiceFee_InitialReg), MoneyTool.FormatMoney(TotalFee_InitialReg), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalFee_InitialReg), MoneyTool.FormatMoney(0), null, 
                "Transaction Amount", MoneyTool.FormatMoney(TransactionAmount_UpdateReg), MoneyTool.FormatMoney(TransactionAmount_UpdateReg), 
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee), MoneyTool.FormatMoney(TransactionAmount_UpdateReg + ShippingFee), 
                "Service Fee", MoneyTool.FormatMoney(TransactionFee_UpdateReg), MoneyTool.FormatMoney(TransactionAmount_UpdateReg + ShippingFee + TransactionFee_UpdateReg), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-(TransactionAmount_UpdateReg + ShippingFee + TransactionFee_UpdateReg)), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_UpdateReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get { return TotalFee; }
            }

            public double? AgendaFeeTotal
            {
                get { return null; }
            }

            public double? MerchandiseFeeTotal
            {
                get { return null; }
            }

            public double? ShippingFeeTotal
            {
                get { return ShippingFee; }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return ServiceFee;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get { return TaxOne; }
            }

            public double? Tax2Amount
            {
                get { return TaxTwo; }
            }

            public double? LodgingBookingFee
            {
                get { return LT.BookingFee; }
            }

            public double? LodgingSubtotal
            {
                get { return LodgingSubTotal; }
            }

            public double? Subtotal
            {
                get { return SubTotal; }
            }

            public double? DiscountCodeSavings
            {
                get { return DiscountSaving; }
            }

            public double? GroupDiscountSavings
            {
                get { return null; }
            }

            public double? RecurringSubtotal
            {
                get { return null; }
            }

            public double? RecurringTax1Amount
            {
                get { return null; }
            }

            public double? RecurringTax2Amount
            {
                get { return null; }
            }

            public double? YearlyFees
            {
                get { return null; }
            }

            public double? YearlyFeesDiscount
            {
                get { return null; }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }
    }

    internal class Membership
    {
        public const string EventName = "TransactionIntegrityFixture_Membership";
        public const string DiscountCode = ProEvent.DiscountCode;
        public const double DiscountPercentage = ProEvent.DiscountPercentage;
        public const double PerRegFee = 3.6;
        public const double CCChargePercentage = ProEvent.CCChargePercentage;
        public const double ServiceFeePercentage = 0;

        public class MemberType
        {
            public class Name
            {
                public const string One = "Member Type 1";
                public const string Two = "Member Type 2";
            }
        }

        public class MembershipFees
        {
            public class Name
            {
                public const string Recurring_AlwaysDiscount = "Membership Checkbox Always Discounted";
                public const string Recurring_DiscountOnce = "Membership Checkbox One Discount";
                public const string OneTime = "One Time With Discount";
            }

            public class Fee
            {
                public const double Recurring_AlwaysDiscount = 10;
                public const double Recurring_DiscountOnce = 10;
                public const double OneTime = 10;
            }
        }

        public class Merchandise
        {
            public class Fee
            {
                public const double FixedPrice = ProEvent.Merchandise.Fee.FixedPrice;
                public const double FixedPriceWithMultiChoice = ProEvent.Merchandise.Fee.FixedPriceWithMultiChoice;
                public const double ShippingFeePerItem = ProEvent.Merchandise.Fee.ShippingFeePerItem;
                public const int Quantity = ProEvent.Merchandise.Fee.Quantity;
                public const double VariableAmount = ProEvent.Merchandise.Fee.VariableAmount;
            }

            public class Name
            {
                public const string MerchFixedPrice = ProEvent.Merchandise.Name.MerchFixedPrice;
                public const string MerchFixedPriceMC = ProEvent.Merchandise.Name.MerchFixedPriceMC;
                public const string MerchVariableAmount = ProEvent.Merchandise.Name.MerchVariableAmount;
                public const string MerchVariableAmountMC = ProEvent.Merchandise.Name.MerchVariableAmountMC;
                public const string FixedPriceMCSelection = ProEvent.Merchandise.Name.FixedPriceMCSelection;
                public const string VariableAmountMCSelection = ProEvent.Merchandise.Name.VariableAmountMCSelection;
            }
        }

        public class Tax
        {
            public class Name
            {
                public const string TaxOne = ProEvent.Tax.Name.TaxOne;
                public const string TaxTwo = ProEvent.Tax.Name.TaxTwo;
            }

            public class Percentage
            {
                public const double TaxOne = ProEvent.Tax.Percentage.TaxOne;
                public const double TaxTwo = ProEvent.Tax.Percentage.TaxTwo;
            }
        }

        public class FeeCalculation_InitialReg : IFeeCalculation
        {
            private const double FeeItemsWithDiscount_OneTime =
                (MembershipFees.Fee.OneTime +
                Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity);

            private const double DiscountedFee_OneTime = FeeItemsWithDiscount_OneTime * (1 - DiscountPercentage);

            private const double NonDiscountedFee_OneTime =
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double SubTotal_OneTime = DiscountedFee_OneTime + NonDiscountedFee_OneTime;

            private const double ShippingFee =
                Merchandise.Fee.ShippingFeePerItem *
                (Merchandise.Fee.Quantity + Merchandise.Fee.Quantity + 1 + 1);

            private const double TaxOne_OneTime = Tax.Percentage.TaxOne * SubTotal_OneTime;
            private const double TaxTwo_OneTime = Tax.Percentage.TaxTwo * SubTotal_OneTime;

            private const double Total_OneTime = SubTotal_OneTime + ShippingFee + TaxOne_OneTime + TaxTwo_OneTime;
            private const double DiscountSaving_OneTime = FeeItemsWithDiscount_OneTime * DiscountPercentage;

            private const double FeeItemsWithDiscount_Recurring =
                MembershipFees.Fee.Recurring_AlwaysDiscount +
                MembershipFees.Fee.Recurring_DiscountOnce;

            private const double DiscountedFee_Recurring = FeeItemsWithDiscount_Recurring * (1 - DiscountPercentage);

            private const double SubTotal_Recurring = DiscountedFee_Recurring;

            private const double TaxOne_Recurring = Tax.Percentage.TaxOne * SubTotal_Recurring;
            private const double TaxTwo_Recurring = Tax.Percentage.TaxTwo * SubTotal_Recurring;

            private const double Total_Recurring = SubTotal_Recurring + TaxOne_Recurring + TaxTwo_Recurring;

            private const double DiscountSaving_Recurring = FeeItemsWithDiscount_Recurring * DiscountPercentage;

            private const double TotalCharge = Total_OneTime + Total_Recurring;

            private readonly static double TransactionFees = Math.Round(
                CCChargePercentage * TotalCharge,
                2);

            private readonly static double ServiceFee = TransactionFees + PerRegFee;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[15]
            {
                MoneyTool.FormatMoney(-TotalCharge), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalCharge - ServiceFee)), //5.Net Amount Due/Owed

                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //6.Transaction %
                MoneyTool.FormatMoney(-TotalCharge), //7.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees), //8.Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(0), //14.Total Paid By Participant
                MoneyTool.FormatMoney(ServiceFee) //15.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[10]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalCharge - ShippingFee), MoneyTool.FormatMoney(TotalCharge - ShippingFee),
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee), MoneyTool.FormatMoney(TotalCharge),
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalCharge), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_InitialReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get { return Total_OneTime; }
            }

            public double? AgendaFeeTotal
            {
                get { return null; }
            }

            public double? MerchandiseFeeTotal
            {
                get { return null; }
            }

            public double? ShippingFeeTotal
            {
                get { return ShippingFee; }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return null;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get { return TaxOne_OneTime; }
            }

            public double? Tax2Amount
            {
                get { return TaxTwo_OneTime; }
            }

            public double? LodgingBookingFee
            {
                get { return null; }
            }

            public double? LodgingSubtotal
            {
                get { return null; }
            }

            public double? Subtotal
            {
                get { return SubTotal_OneTime; }
            }

            public double? DiscountCodeSavings
            {
                get { return DiscountSaving_OneTime; }
            }

            public double? GroupDiscountSavings
            {
                get { return null; }
            }

            public double? RecurringSubtotal
            {
                get { return SubTotal_Recurring; }
            }

            public double? RecurringTax1Amount
            {
                get { return TaxOne_Recurring; }
            }

            public double? RecurringTax2Amount
            {
                get { return TaxTwo_Recurring; }
            }

            public double? YearlyFees
            {
                get { return Total_Recurring; }
            }

            public double? YearlyFeesDiscount
            {
                get { return DiscountSaving_Recurring; }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }

        public class FeeCalculation_Renew : IFeeCalculation
        {
            private const double FeeItems_Recurring_Renew =
                MembershipFees.Fee.Recurring_AlwaysDiscount * (1 - DiscountPercentage) +
                MembershipFees.Fee.Recurring_DiscountOnce;

            private const double TaxOne_Recurring_Renew = Tax.Percentage.TaxOne * FeeItems_Recurring_Renew;
            private const double TaxTwo_Recurring_Renew = Tax.Percentage.TaxTwo * FeeItems_Recurring_Renew;

            private const double NetChangesToRecurringFees = FeeItems_Recurring_Renew + TaxOne_Recurring_Renew + TaxTwo_Recurring_Renew;

            private readonly static double TransactionFees_Renew = Math.Round(
                CCChargePercentage * NetChangesToRecurringFees,
                2);

            private readonly static double TotalCharge = 
                FeeCalculation_InitialReg.Default.TotalCharges.Value +
                FeeCalculation_InitialReg.Default.YearlyFees.Value +
                NetChangesToRecurringFees;

            private readonly static double TransactionFees = Math.Round(
                CCChargePercentage * TotalCharge,
                2);

            private readonly static double ServiceFee = TransactionFees + PerRegFee;

            private readonly static double ShippingFee = FeeCalculation_InitialReg.Default.ShippingFeeTotal.Value;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[25]
            {
                MoneyTool.FormatMoney(-TotalCharge), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalCharge - ServiceFee)), //5.Net Amount Due/Owed

                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //6.Transaction %
                MoneyTool.FormatMoney(-(TotalCharge - NetChangesToRecurringFees)), //7.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees - TransactionFees_Renew), //8.Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee - TransactionFees_Renew), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(0), //14.Total Paid By Participant
                MoneyTool.FormatMoney(ServiceFee - TransactionFees_Renew), //15.Fees Due

                string.Format("{0} %", (CCChargePercentage * 100).ToString()), //16.Transaction %
                MoneyTool.FormatMoney(-NetChangesToRecurringFees), //17.Transaction Amount
                MoneyTool.FormatMoney(TransactionFees_Renew), //18.Transaction Fee
                MoneyTool.FormatMoney(0), //19.Flat Fee
                null, //20.Per Reg Fee
                null, //21.Group Count
                MoneyTool.FormatMoney(TransactionFees_Renew), //22.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //23.% Passed To Participant
                MoneyTool.FormatMoney(0), //24.Total Paid By Participant
                MoneyTool.FormatMoney(TransactionFees_Renew) //25.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[17]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalCharge - ShippingFee - NetChangesToRecurringFees), MoneyTool.FormatMoney(TotalCharge - ShippingFee - NetChangesToRecurringFees),
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee), MoneyTool.FormatMoney(TotalCharge - NetChangesToRecurringFees),
                "Online Credit Card Payment", MoneyTool.FormatMoney(-(TotalCharge - NetChangesToRecurringFees)), MoneyTool.FormatMoney(0), null,
                "Transaction Amount", MoneyTool.FormatMoney(NetChangesToRecurringFees), MoneyTool.FormatMoney(NetChangesToRecurringFees),
                "Online Credit Card Payment", MoneyTool.FormatMoney(-NetChangesToRecurringFees), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_Renew();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get { return TotalCharge; }
            }

            public double? AgendaFeeTotal
            {
                get { return null; }
            }

            public double? MerchandiseFeeTotal
            {
                get { return null; }
            }

            public double? ShippingFeeTotal
            {
                get { return null; }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return null;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get { return null; }
            }

            public double? Tax2Amount
            {
                get { return null; }
            }

            public double? LodgingBookingFee
            {
                get { return null; }
            }

            public double? LodgingSubtotal
            {
                get { return null; }
            }

            public double? Subtotal
            {
                get { return null; }
            }

            public double? DiscountCodeSavings
            {
                get { return null; }
            }

            public double? GroupDiscountSavings
            {
                get { return null; }
            }

            public double? RecurringSubtotal
            {
                get { return null; }
            }

            public double? RecurringTax1Amount
            {
                get { return null; }
            }

            public double? RecurringTax2Amount
            {
                get { return null; }
            }

            public double? YearlyFees
            {
                get { return null; }
            }

            public double? YearlyFeesDiscount
            {
                get { return null; }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }
    }

    internal class EnduranceEvent_USD
    {
        public const string EventName = "TransactionIntegrityFixture_Endurance_USD";
        public const string DiscountCode = ProEvent.DiscountCode;
        public const double DiscountPercentage = ProEvent.DiscountPercentage;
        public const double GroupDiscountPercentage = ProEvent.GroupDiscountPercentage;
        public const double ServiceFeePercentage = ProEvent.ServiceFeePercentage;
        public const double PerRegFee = 3.95;
        
        public class CCChargePercentage
        {
            public const double NonMerch = 0.08;
            public const double Merch = 0.045;
        }

        public class Tax
        {
            public class Name
            {
                public const string TaxOne = ProEvent.Tax.Name.TaxOne;
                public const string TaxTwo = ProEvent.Tax.Name.TaxTwo;
            }

            public class Percentage
            {
                public const double TaxOne = ProEvent.Tax.Percentage.TaxOne;
                public const double TaxTwo = ProEvent.Tax.Percentage.TaxTwo;
            }
        }

        public class RegType
        {
            public class Name
            {
                public const string One = ProEvent.RegType.Name.One;
                public const string Two = ProEvent.RegType.Name.Two;
            }

            public class Fee
            {
                public const double One = ProEvent.RegType.Fee.One;
                public const double Two = ProEvent.RegType.Fee.Two;
            }
        }

        public class AgendaItem
        {
            public class Fee
            {
                public const double Checkbox = ProEvent.AgendaItem.Fee.Checkbox;
                public const double RadioButtonOnParent = ProEvent.AgendaItem.Fee.RadioButtonOnParent;
                public const double RadioButtonOnItems_Ex = ProEvent.AgendaItem.Fee.RadioButtonOnItems_Ex;
                public const double RadioButtonOnItems_Less = ProEvent.AgendaItem.Fee.RadioButtonOnItems_Less;
                public const double AlwaysSelected = ProEvent.AgendaItem.Fee.AlwaysSelected;
                public const double DropdownOnParent = ProEvent.AgendaItem.Fee.DropdownOnParent;
                public const double DropdownOnItems_Fair = ProEvent.AgendaItem.Fee.DropdownOnItems_Fair;
                public const double DropdownOnItems_Ade = ProEvent.AgendaItem.Fee.DropdownOnItems_Ade;
                public const double Contribution = ProEvent.AgendaItem.Fee.Contribution;
            }

            public class Name
            {
                public const string CheckboxAgendaPage = ProEvent.AgendaItem.Name.CheckboxAgendaPage;
                public const string RadioButtonsParentPrice = ProEvent.AgendaItem.Name.RadioButtonsParentPrice;
                public const string RadioButtonsParentPriceSelection = ProEvent.AgendaItem.Name.RadioButtonsParentPriceSelection;
                public const string RadionButtonsPriceOnItem = ProEvent.AgendaItem.Name.RadionButtonsPriceOnItem;
                public const string RadioButtonsOnItemSelection = ProEvent.AgendaItem.Name.RadioButtonsOnItemSelection;
                public const string ContributionAgendaPage = ProEvent.AgendaItem.Name.ContributionAgendaPage;
                public const string DropDownParentPrice = ProEvent.AgendaItem.Name.DropDownParentPrice;
                public const string DropDownParentSelection = ProEvent.AgendaItem.Name.DropDownParentSelection;
                public const string DropDownPriceOnItem = ProEvent.AgendaItem.Name.DropDownPriceOnItem;
                public const string DropDownOnItemSelection = ProEvent.AgendaItem.Name.DropDownOnItemSelection;
                public const string AlwaysSelectedAgenda = ProEvent.AgendaItem.Name.AlwaysSelectedAgenda;
            }
        }

        public class LT
        {
            public const string CheckinDate = ProEvent.LT.CheckinDate;
            public const string CheckoutDate = ProEvent.LT.CheckoutDate;
            public const string RoomPreference = ProEvent.LT.RoomPreference;
            public const double HotelFeePerNight = ProEvent.LT.HotelFeePerNight;
            public const int Nights = ProEvent.LT.Nights;
            public const double BookingFee = ProEvent.LT.BookingFee;
        }

        public class Merchandise
        {
            public class Fee
            {
                public const double FixedPrice = ProEvent.Merchandise.Fee.FixedPrice;
                public const double FixedPriceWithMultiChoice = ProEvent.Merchandise.Fee.FixedPriceWithMultiChoice;
                public const double ShippingFeePerItem = ProEvent.Merchandise.Fee.ShippingFeePerItem;
                public const int Quantity = ProEvent.Merchandise.Fee.Quantity;
                public const double VariableAmount = ProEvent.Merchandise.Fee.VariableAmount;
            }

            public class Name
            {
                public const string MerchFixedPrice = ProEvent.Merchandise.Name.MerchFixedPrice;
                public const string MerchFixedPriceMC = ProEvent.Merchandise.Name.MerchFixedPriceMC;
                public const string MerchVariableAmount = ProEvent.Merchandise.Name.MerchVariableAmount;
                public const string MerchVariableAmountMC = ProEvent.Merchandise.Name.MerchVariableAmountMC;
                public const string FixedPriceMCSelection = ProEvent.Merchandise.Name.FixedPriceMCSelection;
                public const string VariableAmountMCSelection = ProEvent.Merchandise.Name.VariableAmountMCSelection;
            }
        }

        public class FeeCalculation_SingleReg : IFeeCalculation
        {
            private const double FeeItemsWithDiscount =
                (RegType.Fee.One +
                AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade +
                Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity);

            private const double DiscountedFee = FeeItemsWithDiscount * (1 - DiscountPercentage);

            private const double NonDiscountedFee =
                AgendaItem.Fee.Contribution +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double _AgendaFeeTotal =
                (AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade) * (1 - DiscountPercentage) +
                AgendaItem.Fee.Contribution;

            private const double ShippingFee =
                Merchandise.Fee.ShippingFeePerItem *
                (Merchandise.Fee.Quantity + Merchandise.Fee.Quantity + 1 + 1);

            private const double MerchandiseFeeItems =
                (Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity) * (1 - DiscountPercentage) +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double _MerchandiseFeeTotal = MerchandiseFeeItems + ShippingFee;

            private const double SubTotal = DiscountedFee + NonDiscountedFee;

            private const double TaxOne_NonMerch = Tax.Percentage.TaxOne * (SubTotal - AgendaItem.Fee.Contribution - MerchandiseFeeItems);
            private const double TaxTwo_NonMerch = Tax.Percentage.TaxTwo * (SubTotal - AgendaItem.Fee.Contribution - MerchandiseFeeItems);
            private const double Tax_NonMerch = TaxOne_NonMerch + TaxTwo_NonMerch;

            private const double TaxOne_Merch = Tax.Percentage.TaxOne * MerchandiseFeeItems;
            private const double TaxTwo_Merch = Tax.Percentage.TaxTwo * MerchandiseFeeItems;
            private const double Tax_Merch = TaxOne_Merch + TaxTwo_Merch;

            private const double TaxOne = Tax.Percentage.TaxOne * (SubTotal - AgendaItem.Fee.Contribution);
            private const double TaxTwo = Tax.Percentage.TaxTwo * (SubTotal - AgendaItem.Fee.Contribution);

            private const double LodgingSubTotal = LT.Nights * LT.HotelFeePerNight;
            private const double LodgingFeeTotal = LodgingSubTotal + LT.BookingFee;

            private const double NonMerchAmount = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal - MerchAmount;
            private const double MerchAmount = MerchandiseFeeItems + Tax_Merch;

            private readonly static double TransactionFees_NonMerch = Math.Round(
                CCChargePercentage.NonMerch * NonMerchAmount,
                2);

            private readonly static double TransactionFees_Merch = Math.Round(
                CCChargePercentage.Merch * MerchAmount,
                2);

            private readonly static double TransactionFees = TransactionFees_NonMerch + TransactionFees_Merch;

            private readonly static double ServiceFee = PerRegFee + TransactionFees;

            private readonly static double TotalFee = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal + ServiceFee;

            private const double DiscountSaving = FeeItemsWithDiscount * DiscountPercentage;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[18]
            { 
                MoneyTool.FormatMoney(-TotalFee), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(TotalFee - ServiceFee)), //5.Net Amount Due/Owed
                string.Format("{0} %", (CCChargePercentage.NonMerch * 100).ToString("0.00")), //6.Non Merchandise %
                MoneyTool.FormatMoney(-NonMerchAmount), //7.Event/Activity Price
                MoneyTool.FormatMoney(TransactionFees_NonMerch), //8.Event/Activity Transaction Fee
                string.Format("{0} %", (CCChargePercentage.Merch * 100).ToString("0.00")), //6.Merchandise %
                MoneyTool.FormatMoney(-MerchAmount), //7.Merchandise Amount
                MoneyTool.FormatMoney(TransactionFees_Merch), //8.Merchandise Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(-ServiceFee), //14.Total Paid By Participant
                MoneyTool.FormatMoney(0) //15.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[13]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee), MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee), 
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee), MoneyTool.FormatMoney(TotalFee - ServiceFee), 
                "Service Fee", MoneyTool.FormatMoney(ServiceFee), MoneyTool.FormatMoney(TotalFee), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalFee), MoneyTool.FormatMoney(0), null
            };

            public static IFeeCalculation Default = new FeeCalculation_SingleReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get
                {
                    return TotalFee;
                }
            }

            public double? AgendaFeeTotal
            {
                get
                {
                    return null;
                }
            }

            public double? MerchandiseFeeTotal
            {
                get
                {
                    return null;
                }
            }

            public double? ShippingFeeTotal
            {
                get
                {
                    return ShippingFee;
                }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return ServiceFee;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get
                {
                    return TaxOne;
                }
            }

            public double? Tax2Amount
            {
                get
                {
                    return TaxTwo;
                }
            }

            public double? LodgingBookingFee
            {
                get
                {
                    return LT.BookingFee;
                }
            }

            public double? LodgingSubtotal
            {
                get
                {
                    return LodgingSubTotal;
                }
            }

            public double? Subtotal
            {
                get
                {
                    return SubTotal;
                }
            }

            public double? DiscountCodeSavings
            {
                get
                {
                    return DiscountSaving;
                }
            }

            public double? GroupDiscountSavings
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringSubtotal
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringTax1Amount
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringTax2Amount
            {
                get
                {
                    return null;
                }
            }

            public double? YearlyFees
            {
                get
                {
                    return null;
                }
            }

            public double? YearlyFeesDiscount
            {
                get
                {
                    return null;
                }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }
    }

    internal class EnduranceEvent_Pound
    {
        public const string EventName = "TransactionIntegrityFixture_Endurance_Pound";
        public const string DiscountCode = ProEvent.DiscountCode;
        public const double DiscountPercentage = ProEvent.DiscountPercentage;
        public const double GroupDiscountPercentage = ProEvent.GroupDiscountPercentage;
        public const double ServiceFeePercentage = ProEvent.ServiceFeePercentage;
        public const double PerRegFee_USD = 3.95;

        public readonly static double PerRegFee = MoneyTool.ConvertAmount(
            PerRegFee_USD, 
            MoneyTool.CurrencyCode.USD, 
            MoneyTool.CurrencyCode.GBP);

        public class CCChargePercentage
        {
            public const double NonMerch = 0.08;
            public const double Merch = 0.045;
        }

        public class Tax
        {
            public class Name
            {
                public const string TaxOne = ProEvent.Tax.Name.TaxOne;
                public const string TaxTwo = ProEvent.Tax.Name.TaxTwo;
            }

            public class Percentage
            {
                public const double TaxOne = ProEvent.Tax.Percentage.TaxOne;
                public const double TaxTwo = ProEvent.Tax.Percentage.TaxTwo;
            }
        }

        public class RegType
        {
            public class Name
            {
                public const string One = ProEvent.RegType.Name.One;
                public const string Two = ProEvent.RegType.Name.Two;
            }

            public class Fee
            {
                public const double One = ProEvent.RegType.Fee.One;
                public const double Two = ProEvent.RegType.Fee.Two;
            }
        }

        public class AgendaItem
        {
            public class Fee
            {
                public const double Checkbox = ProEvent.AgendaItem.Fee.Checkbox;
                public const double RadioButtonOnParent = ProEvent.AgendaItem.Fee.RadioButtonOnParent;
                public const double RadioButtonOnItems_Ex = ProEvent.AgendaItem.Fee.RadioButtonOnItems_Ex;
                public const double RadioButtonOnItems_Less = ProEvent.AgendaItem.Fee.RadioButtonOnItems_Less;
                public const double AlwaysSelected = ProEvent.AgendaItem.Fee.AlwaysSelected;
                public const double DropdownOnParent = ProEvent.AgendaItem.Fee.DropdownOnParent;
                public const double DropdownOnItems_Fair = ProEvent.AgendaItem.Fee.DropdownOnItems_Fair;
                public const double DropdownOnItems_Ade = ProEvent.AgendaItem.Fee.DropdownOnItems_Ade;
                public const double Contribution = ProEvent.AgendaItem.Fee.Contribution;
            }

            public class Name
            {
                public const string CheckboxAgendaPage = ProEvent.AgendaItem.Name.CheckboxAgendaPage;
                public const string RadioButtonsParentPrice = ProEvent.AgendaItem.Name.RadioButtonsParentPrice;
                public const string RadioButtonsParentPriceSelection = ProEvent.AgendaItem.Name.RadioButtonsParentPriceSelection;
                public const string RadionButtonsPriceOnItem = ProEvent.AgendaItem.Name.RadionButtonsPriceOnItem;
                public const string RadioButtonsOnItemSelection = "Less Expensive: £10.00";
                public const string ContributionAgendaPage = ProEvent.AgendaItem.Name.ContributionAgendaPage;
                public const string DropDownParentPrice = ProEvent.AgendaItem.Name.DropDownParentPrice;
                public const string DropDownParentSelection = ProEvent.AgendaItem.Name.DropDownParentSelection;
                public const string DropDownPriceOnItem = ProEvent.AgendaItem.Name.DropDownPriceOnItem;
                public const string DropDownOnItemSelection = "Adequately Priced: £10.00";
                public const string AlwaysSelectedAgenda = ProEvent.AgendaItem.Name.AlwaysSelectedAgenda;
            }
        }

        public class LT
        {
            public const string CheckinDate = ProEvent.LT.CheckinDate;
            public const string CheckoutDate = ProEvent.LT.CheckoutDate;
            public const string RoomPreference = ProEvent.LT.RoomPreference;
            public const double HotelFeePerNight = ProEvent.LT.HotelFeePerNight;
            public const int Nights = ProEvent.LT.Nights;
            public const double BookingFee = ProEvent.LT.BookingFee;
        }

        public class Merchandise
        {
            public class Fee
            {
                public const double FixedPrice = ProEvent.Merchandise.Fee.FixedPrice;
                public const double FixedPriceWithMultiChoice = ProEvent.Merchandise.Fee.FixedPriceWithMultiChoice;
                public const double ShippingFeePerItem = ProEvent.Merchandise.Fee.ShippingFeePerItem;
                public const int Quantity = ProEvent.Merchandise.Fee.Quantity;
                public const double VariableAmount = ProEvent.Merchandise.Fee.VariableAmount;
            }

            public class Name
            {
                public const string MerchFixedPrice = ProEvent.Merchandise.Name.MerchFixedPrice;
                public const string MerchFixedPriceMC = ProEvent.Merchandise.Name.MerchFixedPriceMC;
                public const string MerchVariableAmount = ProEvent.Merchandise.Name.MerchVariableAmount;
                public const string MerchVariableAmountMC = ProEvent.Merchandise.Name.MerchVariableAmountMC;
                public const string FixedPriceMCSelection = ProEvent.Merchandise.Name.FixedPriceMCSelection;
                public const string VariableAmountMCSelection = ProEvent.Merchandise.Name.VariableAmountMCSelection;
            }
        }

        public class FeeCalculation_SingleReg : IFeeCalculation
        {
            private const double FeeItemsWithDiscount =
                (RegType.Fee.One +
                AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade +
                Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity);

            private const double DiscountedFee = FeeItemsWithDiscount * (1 - DiscountPercentage);

            private const double NonDiscountedFee =
                AgendaItem.Fee.Contribution +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double _AgendaFeeTotal =
                (AgendaItem.Fee.Checkbox +
                AgendaItem.Fee.AlwaysSelected +
                AgendaItem.Fee.RadioButtonOnParent +
                AgendaItem.Fee.RadioButtonOnItems_Less +
                AgendaItem.Fee.DropdownOnParent +
                AgendaItem.Fee.DropdownOnItems_Ade) * (1 - DiscountPercentage) +
                AgendaItem.Fee.Contribution;

            private const double ShippingFee =
                Merchandise.Fee.ShippingFeePerItem *
                (Merchandise.Fee.Quantity + Merchandise.Fee.Quantity + 1 + 1);

            private const double MerchandiseFeeItems =
                (Merchandise.Fee.FixedPrice * Merchandise.Fee.Quantity +
                Merchandise.Fee.FixedPriceWithMultiChoice * Merchandise.Fee.Quantity) * (1 - DiscountPercentage) +
                Merchandise.Fee.VariableAmount +
                Merchandise.Fee.VariableAmount;

            private const double _MerchandiseFeeTotal = MerchandiseFeeItems + ShippingFee;

            private const double SubTotal = DiscountedFee + NonDiscountedFee;

            private const double TaxOne_NonMerch = Tax.Percentage.TaxOne * (SubTotal - AgendaItem.Fee.Contribution - MerchandiseFeeItems);
            private const double TaxTwo_NonMerch = Tax.Percentage.TaxTwo * (SubTotal - AgendaItem.Fee.Contribution - MerchandiseFeeItems);
            private const double Tax_NonMerch = TaxOne_NonMerch + TaxTwo_NonMerch;

            private const double TaxOne_Merch = Tax.Percentage.TaxOne * MerchandiseFeeItems;
            private const double TaxTwo_Merch = Tax.Percentage.TaxTwo * MerchandiseFeeItems;
            private const double Tax_Merch = TaxOne_Merch + TaxTwo_Merch;

            private const double TaxOne = Tax.Percentage.TaxOne * (SubTotal - AgendaItem.Fee.Contribution);
            private const double TaxTwo = Tax.Percentage.TaxTwo * (SubTotal - AgendaItem.Fee.Contribution);

            private const double LodgingSubTotal = LT.Nights * LT.HotelFeePerNight;
            private const double LodgingFeeTotal = LodgingSubTotal + LT.BookingFee;

            private const double NonMerchAmount = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal - MerchAmount;
            private const double MerchAmount = MerchandiseFeeItems + Tax_Merch;

            private readonly static double NonMerchAmount_USD = MoneyTool.ConvertAmount(
                NonMerchAmount,
                MoneyTool.CurrencyCode.GBP,
                MoneyTool.CurrencyCode.USD);

            private readonly static double MerchAmount_USD = MoneyTool.ConvertAmount(
                MerchAmount,
                MoneyTool.CurrencyCode.GBP,
                MoneyTool.CurrencyCode.USD);

            private readonly static double TransactionFees_NonMerch = Math.Round(
                CCChargePercentage.NonMerch * NonMerchAmount,
                2);

            private readonly static double TransactionFees_Merch = Math.Round(
                CCChargePercentage.Merch * MerchAmount,
                2);

            private readonly static double TransactionFees_NonMerch_USD = Math.Round(
                CCChargePercentage.NonMerch * NonMerchAmount_USD,
                2);

            private readonly static double TransactionFees_Merch_USD = Math.Round(
                CCChargePercentage.Merch * MerchAmount_USD,
                2);

            private readonly static double TransactionFees = TransactionFees_NonMerch + TransactionFees_Merch;
            private readonly static double TransactionFees_USD = TransactionFees_NonMerch_USD + TransactionFees_Merch_USD;

            private readonly static double TransactionFees_Converted = MoneyTool.ConvertAmount(
                TransactionFees,
                MoneyTool.CurrencyCode.GBP,
                MoneyTool.CurrencyCode.USD);

            private readonly static double ServiceFee_USD = PerRegFee_USD + TransactionFees_USD;

            private readonly static double ServiceFee = PerRegFee + TransactionFees;

            private readonly static double ServiceFee_Converted = MoneyTool.ConvertAmount(
                ServiceFee,
                MoneyTool.CurrencyCode.GBP,
                MoneyTool.CurrencyCode.USD);

            private readonly static double TotalFee = SubTotal + ShippingFee + TaxOne + TaxTwo + LodgingFeeTotal + ServiceFee;
            private readonly static double TotalFee_USD = NonMerchAmount_USD + MerchAmount_USD + ServiceFee_USD;

            private readonly static double ConvertedAmount = MoneyTool.ConvertAmount(
                TotalFee, 
                MoneyTool.CurrencyCode.GBP, 
                MoneyTool.CurrencyCode.USD);

            private const double DiscountSaving = FeeItemsWithDiscount * DiscountPercentage;

            private readonly static string[] _ExpectedTransactionFeesReportData = new string[18]
            { 
                MoneyTool.FormatMoney(-ConvertedAmount), //1.Total Amount Collected
                MoneyTool.FormatMoney(TransactionFees_Converted), //2.Transaction Fees
                "1", //3.Registrants in group
                MoneyTool.FormatMoney(PerRegFee_USD), //4.Total per Reg Fee
                MoneyTool.FormatMoney(-(ConvertedAmount - ServiceFee_Converted)), //5.Net Amount Due/Owed
                string.Format("{0} %", (CCChargePercentage.NonMerch * 100).ToString("0.00")), //6.Non Merchandise %
                MoneyTool.FormatMoney(-NonMerchAmount_USD), //7.Event/Activity Price
                MoneyTool.FormatMoney(TransactionFees_NonMerch_USD), //8.Event/Activity Transaction Fee
                string.Format("{0} %", (CCChargePercentage.Merch * 100).ToString("0.00")), //6.Merchandise %
                MoneyTool.FormatMoney(-MerchAmount_USD), //7.Merchandise Amount
                MoneyTool.FormatMoney(TransactionFees_Merch_USD), //8.Merchandise Transaction Fee
                MoneyTool.FormatMoney(0), //9.Flat Fee
                MoneyTool.FormatMoney(PerRegFee_USD), //10.Per Reg Fee
                "1", //11.Group Count
                MoneyTool.FormatMoney(ServiceFee_Converted), //12.Total Fees
                string.Format("{0} %", (ServiceFeePercentage * 100).ToString("0.00")), //13.% Passed To Participant
                MoneyTool.FormatMoney(-ServiceFee_Converted), //14.Total Paid By Participant
                MoneyTool.FormatMoney(0) //15.Fees Due
            };

            private readonly static string[] _ExpectedTransactionReportData = new string[14]
            {
                "Transaction Amount", MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee, currencyCode:MoneyTool.CurrencyCode.GBP), MoneyTool.FormatMoney(TotalFee - ServiceFee - ShippingFee, currencyCode:MoneyTool.CurrencyCode.GBP), 
                "Merchandise Shipping Fee", MoneyTool.FormatMoney(ShippingFee, currencyCode:MoneyTool.CurrencyCode.GBP), MoneyTool.FormatMoney(TotalFee - ServiceFee, currencyCode:MoneyTool.CurrencyCode.GBP), 
                "Service Fee", MoneyTool.FormatMoney(ServiceFee, currencyCode:MoneyTool.CurrencyCode.GBP), MoneyTool.FormatMoney(TotalFee, currencyCode:MoneyTool.CurrencyCode.GBP), 
                "Online Credit Card Payment", MoneyTool.FormatMoney(-TotalFee, currencyCode:MoneyTool.CurrencyCode.GBP), MoneyTool.FormatMoney(0, currencyCode:MoneyTool.CurrencyCode.GBP), MoneyTool.FormatMoney(ConvertedAmount), null
            };

            public static IFeeCalculation Default = new FeeCalculation_SingleReg();

            #region IFeeCalculation interface implementation
            public double? TotalCharges
            {
                get
                {
                    return TotalFee;
                }
            }

            public double? AgendaFeeTotal
            {
                get
                {
                    return _AgendaFeeTotal;
                }
            }

            public double? MerchandiseFeeTotal
            {
                get
                {
                    return null;
                }
            }

            public double? ShippingFeeTotal
            {
                get
                {
                    return ShippingFee;
                }
            }

            public double? ServiceFeeTotal
            {
                get
                {
                    return ServiceFee;
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double? Tax1Amount
            {
                get
                {
                    return TaxOne;
                }
            }

            public double? Tax2Amount
            {
                get
                {
                    return TaxTwo;
                }
            }

            public double? LodgingBookingFee
            {
                get
                {
                    return LT.BookingFee;
                }
            }

            public double? LodgingSubtotal
            {
                get
                {
                    return LodgingSubTotal;
                }
            }

            public double? Subtotal
            {
                get
                {
                    return SubTotal;
                }
            }

            public double? DiscountCodeSavings
            {
                get
                {
                    return DiscountSaving;
                }
            }

            public double? GroupDiscountSavings
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringSubtotal
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringTax1Amount
            {
                get
                {
                    return null;
                }
            }

            public double? RecurringTax2Amount
            {
                get
                {
                    return null;
                }
            }

            public double? YearlyFees
            {
                get
                {
                    return null;
                }
            }

            public double? YearlyFeesDiscount
            {
                get
                {
                    return null;
                }
            }

            public string[] ExpectedTransactionFeesReportData
            {
                get { return _ExpectedTransactionFeesReportData; }
            }

            public string[] ExpectedTransactionReportData
            {
                get { return _ExpectedTransactionReportData; }
            }
            #endregion
        }
    }

    public interface IFeeCalculation
    {
        double? TotalCharges { get; }
        double? AgendaFeeTotal { get; }
        double? MerchandiseFeeTotal { get; }
        double? ShippingFeeTotal { get; }
        double? ServiceFeeTotal { get; set; }
        double? Tax1Amount { get; }
        double? Tax2Amount { get; }
        double? LodgingBookingFee { get; }
        double? LodgingSubtotal { get; }
        double? Subtotal { get; }
        double? DiscountCodeSavings { get; }
        double? GroupDiscountSavings { get; }
        double? RecurringSubtotal { get; }
        double? RecurringTax1Amount { get; }
        double? RecurringTax2Amount { get; }
        double? YearlyFees { get; }
        double? YearlyFeesDiscount { get; }
        string[] ExpectedTransactionFeesReportData { get; }
        string[] ExpectedTransactionReportData { get; }
    }

    /// <summary>
    /// Will return all of the fees for a specific test, this is for the registration peice
    /// </summary>
    public class FeeTotals
    {
        public double? TotalCharges;
        public double? AgendaFeeTotal;
        public double? MerchandiseFeeTotal;
        public double? ShippingFeeTotal;
        public double? ServiceFeeTotal;
        public double? Tax1Amount;
        public double? Tax2Amount;
        public double? LodgingBookingFee;
        public double? LodgingSubtotal;
        public double? Subtotal;
        public double? DiscountCodeSavings;
        public double? GroupDiscountSavings;
        public double? RecurringSubtotal;
        public double? RecurringTax1Amount;
        public double? RecurringTax2Amount;
        public double? YearlyFees;
        public double? YearlyFeesDiscount;

        public static FeeTotals getDifferentCurrencies()
        {
            return new FeeTotals
            {
                TotalCharges = 279.20,
                ServiceFeeTotal = 18.40148,
                Tax1Amount = 5.50,
                Tax2Amount = 2.78,
                Subtotal = 125.00,
                ShippingFeeTotal = 15.00,
                LodgingBookingFee = 10.00,
                LodgingSubtotal = 100.00,
                DiscountCodeSavings = 75.00
            };
        }
    }

    /// <summary>
    /// Constants for the test
    /// </summary>
    public class TxnIntegrityConstants
    {
        //amount for backend charge, this is for the ChargeRemainingBalance method
        public const double BackEndChargeAmount = 73.05;
        public const double PerRegFee = 3.95;

        //Renewed Membership Fee Totals
        public const double MembershipRenewedTotalCharges = 111.77;

        //checkin page locators/strings
        public const string EventName = "Transaction Integrity Event";
        public const string MembershipName = "Transaction Integrity Membership";
        public const string ActiveEventName = "Merch and Non-Merch Fees Transaction Integrity";
        public const string DifferentCurrencies = "Transaction Integrity Different Currencies";
        public const string RegType = "Regstrant type 1";
        public const string MemberType = "Member Type 1";
        public const string DiscountCode = "half";

        //Agenda locators/strings
        public const string CheckboxAgendaPage = "Checkbox Agenda Page";
        public const string RadioButtonsParentPrice = "Radio Buttons Price On Parent";
        public const string RadioButtonsParentPriceSelection = "Medium";
        public const string RadionButtonsPriceOnItem = "Radio Buttons Price on Items";
        public const string RadioButtonsOnItemSelection = "Less Expensive: $10.00";
        public const string RadioButtonsOnItemSelectionPound = "Less Expensive: £10.00";
        public const string ContributionAgendaPage = "Contribution Agenda Page";
        public const string DropDownParentPrice = "Drop Down Price on Parent Item";
        public const string DropDownParentSelection = "Thursday";
        public const string DropDownPriceOnItem = "Drop Down Price on Item";
        public const string DropDownOnItemSelection = "Adequately Priced: $10.00";
        public const string DropDownOnItemSelectionPound = "Adequately Priced: £10.00";
        public const string AlwaysSelectedAgenda = "Always Selected Agenda With Fee";

        //Membership Locators/strings
        public const string MembershipCheckboxAlwaysDiscount = "Membership Checkbox Always Discounted";
        public const string MembershipCheckboxOneDiscount = "Membership Checkbox One Discount";
        public const string OneTimeFee = "One Time With Discount";

        //L&T page string helpers
        public const string RoomPreference = "Boulderado King Room";

        //Merch string helpers
        public enum MerchandiseNames
        {
            [StringValue("Merchandise Fixed Price")]
            MerchFixedPrice,

            [StringValue("Merchandise Fixed Price W/MC")]
            MerchFixedPriceMC,

            [StringValue("Merchandise Variable Amount")]
            MerchVariableAmount,

            [StringValue("Merchandise Variable Amount W/MC")]
            MerchVariableAmountMC,

            [StringValue("item 2")]
            FixedPriceMCSelection,

            [StringValue("Second Item")]
            VariableAmountMCSelection
        }

        //amounts and data to verify are present and correct in the database
        public const string MerchandId = "013611";
        public static string[] SingleRegTransactionAmount = new string[1] { "-294.8800" };
        public static string[] GroupRegTransactionAmount = new string[1] { "-526.7300" };
        public static string[] BackEndTransactionAmount = new string[1] { "-73.0500" };
        public static string[] UpdateTransactionAmounts = new string[2] { "-73.0500", "-229.7600" };
        public static string[] MembershipTransactionAmounts = new string[1] { "-95.6400" };
        public static string[] RenewedMemberTransactionAmount = new string[2] { "-95.6400", "-16.1300" };
        public static string[] MerchNonMerchTransactionAmount = new string[1] { "-280.6300" };
        public static string[] DiffCurrenciesTransactionAmount = new string[1] { "-279.2000" };

        public const string ccNumber = "4444444444444448";
        public const string ccExpDate = "12/1/2019 12:00:00 AM";
        public const string ccName = "test test";
        public const string ccType = "Visa";
        public const string LastFourCC = "4448";
        public const string SharedFeePercent = "100.00";
        public const string NoSharedFeePercent = "0.00";
        public const string CustomerMerchantId = "1218";
        public const string EnduranceCustomerMerchId = "1527";
        public const string EnduranceMerchAmount = "64.5000";
        public const string EnduranceNonMerchAmount = "193.7800";
        public const string USDExchangeRate = "1";
        public const string GBPExchangeRate = "";
        public static string[] SingleRegBillableAmount = new string[1] { "258.6600" };
        public static string[] GroupRegBillableAmount = new string[1] { "431.6800" };
        public static string[] BackEndBillableAmount = new string[1] { "64.5000" };
        public static string[] UpdateBillableAmount = new string[2] { "64.5000", "221.0300" };
        public static string[] MembershipBillableAmount = new string[1] { "95.6400" };
        public static string[] RenewedMemberBillableAmount = new string[2] { "95.6400", "16.1300" };
        public static string[] MerchNonMerchBillableAmount = new string[1] {"258.2800"};
        public static string[] AddAndModByAttendee = new string[1] { "Attendee" };
        public static string[] AddAndModByRegression = new string[1] { "regression" };
        public static string[] AddAndModByAttendee2 = new string[2] { "Attendee", "Attendee" };
        public static string[] AddAndModByMember = new string[1] { "Member" };
        public static string[] AddAndModByMember2 = new string[2] { "Member", "Member" };
        public static string[] AddAndModByParticpant = new string[1] { "Participant" };

        /// <summary>
        /// These arrays are for verifying data in our Transaction and Transaction fees reports.
        /// For the transaction fees report the data it is looking at in order is this:
        /// 
        /// Total Amount Collected, TransactionFees, Registrants in group, Total per Reg Fee,
        /// Net Amount Due/Owed, Transaction %, Transaction Fee, Per Reg Fee, Group Count, Total fees,
        /// % passed to participant, total paid by participant, fees due. 
        /// 
        /// Note: if the Transactions Report is failing it might be due to an outstanding balance due. 
        /// This test assumes everyone in the report is paid in full.
        /// </summary>
        public static string[] ExpectedTransactionFeeReportData = new string[15] { 
            "-$274.88", "$10.22", "1", "$6.00", "-$258.66", "3.95 %", "-$258.66", "$10.22", 
            "$0.00", "$6.00", "1", "$16.22", "100.00 %", "-$16.22", "$0.00" };
        public static string[] ExpectedGroupTransactionFeeData = new string[15] { 
            "-$466.73", "$17.05", "3", "$18.00", "-$431.68", "3.95 %", "-$431.68", "$17.05", 
            "$0.00", "$6.00", "3", "$35.05", "100.00 %", "-$35.05", "$0.00" };
        public static string[] BackendExpectedTransactionFeeReportData = new string[15] { 
            "-$73.05", "$2.55", "1", "$6.00", "-$64.50", "3.95 %", "-$64.50", "$2.55", 
            "$0.00", "$6.00", "1", "$8.55", "100.00 %", "-$8.55", "$0.00" };
        public static string[] ExpectedUpdateTransactionFeeReportData = new string[25] {
            "-$302.81", "$11.28", "1", "$6.00", "-$285.53", "3.95 %", "-$64.50", "$2.55", 
            "$0.00", "$6.00", "1", "$8.55", "100.00 %", "-$8.55", "$0.00", "3.95 %", "-$221.03",
            "$8.73", "$0.00", null, null, "$8.73", "100.00 %", "-$8.73", "$0.00" };
        public static string[] ExpectedMembershipTransactionFeeData = new string[15] {
            "-$95.64", "$3.78", "1", "$3.60", "-$88.26", "3.95 %", "-$95.64", "$3.78", 
            "$0.00", "$3.60", "1", "$7.38", "0.00 %", "$0.00", "$7.38"  };
        public static string[] ExpectedRenewedMemberTransactionFeeData = new string[25] {
            "-$111.77", "$4.41", "1", "$3.60", "-$103.75", "3.95 %", "-$95.64", "$3.78", 
            "$0.00", "$3.60", "1", "$7.38", "0.00 %", "$0.00", "$7.38", "3.95 %", "-$16.13",
            "$0.64", "$0.00", null, null, "$0.64", "0.00 %", "$0.00", "$0.64" };
        public static string[] ExpectedMerchNonMerchTxnFeesData = new string[18] {
            "-$280.63", "$18.40", "1", "$3.95", "-$258.28", "8.00 %", "-$193.78", "$15.50", 
            "4.50 %", "-$64.50", "$2.90", "$0.00", "$3.95", "1", "$22.35", "100.00 %", "-$22.35", "$0.00" };
        public static string[] ExpectedDiffCurrenciesTxnFeesData = new string[18] {
            "-$437.90", "$28.87", "1", "$3.95", "-$405.08", "8.00 %", "-$303.92", "$24.31", 
            "4.50 %", "-$101.16", "$4.55", "$0.00", "$3.95", "1", "$32.82", "100.00 %", "-$32.82", "$0.00" };        


        /// <summary>
        /// These arrays are for the expect data in the Transactions report. They check the: 
        /// Type, Amount, Running balance and Transaction notes columns. 
        /// </summary>
        public static string[] ExpectedTransactionsReportData = new string[16] { 
            "Transaction Amount", "$243.66", "$243.66", 
            "Merchandise Shipping Fee", "$15.00", "$258.66", 
            "Service Fee", "$16.22", "$274.88", 
            "Registration Protection", "$20.00", "$294.88", 
            "Online Credit Card Payment", "-$294.88", "$0.00", null };
        public static string[] ExpectedGroupTransactionsReportData = new string[16] { 
            "Transaction Amount", "$416.68", "$416.68", 
            "Merchandise Shipping Fee", "$15.00", "$431.68", 
            "Service Fee", "$35.05", "$466.73", 
            "Registration Protection", "$60.00", "$526.73", 
            "Online Credit Card Payment", "-$526.73", "$0.00", null };
        public static string[] BackendExpectedTransactionsReportData = new string[10] { 
            "Transaction Amount", "$64.50", "$64.50", 
            "Service Fee", "$8.55", "$73.05", 
            "Online Credit Card Payment", "-$73.05", "$0.00", null };
        public static string[] ExpectedUpdateTransactionsReportData = new string[23] { 
            "Transaction Amount", "$64.50", "$64.50", 
            "Service Fee", "$8.55", "$73.05", 
            "Online Credit Card Payment", "-$73.05", "$0.00", null, 
            "Transaction Amount", "$206.03", "$206.03", 
            "Merchandise Shipping Fee", "$15.00", "$221.03",
            "Service Fee", "$8.73", "$229.76", 
            "Online Credit Card Payment", "-$229.76", "$0.00", null };
        public static string[] ExpectedMembershipTransactionReportData = new string[10] {
            "Transaction Amount", "$80.64", "$80.64",
            "Merchandise Shipping Fee", "$15.00", "$95.64",
            "Online Credit Card Payment", "-$95.64", "$0.00", null };
        public static string[] ExpectedRenewedMemberTransactionReportData = new string[17] {
            "Transaction Amount", "$80.64", "$80.64",
            "Merchandise Shipping Fee", "$15.00", "$95.64",
            "Online Credit Card Payment", "-$95.64", "$0.00", null, 
            "Transaction Amount", "$16.13", "$16.13",
            "Online Credit Card Payment", "-$16.13", "$0.00", null };
        public static string[] ExpectedMerchNonMerchTransactionReportData = new string [13] {
            "Transaction Amount", "$243.28", "$243.28",
            "Merchandise Shipping Fee", "$15.00", "$258.28",
            "Service Fee", "$22.35", "$280.63",
            "Online Credit Card Payment", "-$280.63", "$0.00", null };
        public static string[] ExpectedDifferentCurrenciesTransactionReportData = new string[14] {
            "Transaction Amount", "£243.28", "£243.28",
            "Merchandise Shipping Fee", "£15.00", "£258.28",
            "Service Fee", null, "£279.20",
            "Online Credit Card Payment", "-£279.20", "£0.00", null , null };
        

        /// <summary>
        /// Data to verify against database for billing verification, these are in order of the database.
        /// </summary>
        public static decimal[] EventAmount = new decimal[6] { -80.0000m, 80.0000m, 36.0000m, -1197.4700m, 41.1000m, 80.0000m };
        public static decimal[] EventCCAmount = new decimal[6] { -80.0000m, 80.0000m, 0.0000m, -1197.4700m, 0.0000m, 80.0000m };
        public static decimal EventCCCost = 0.0000m;
        public static int[] EventRegCount = new int[6] { 4, 4, 6, 5, 5, 4 };
        public static int[] EventTypeId = new int[6] { 55, 56, 1, 2, 8, 56 };
        public static int[] EventCusomterId = new int[6] { 292618, 292618, 377977, 377977, 377977, 377977 };
        public static decimal[] EventFinalAmount = new decimal[4] { 36.0000m, -1197.4700m, 41.1000m, 80.0000m };
        public static decimal[] EventFinalCCAmount = new decimal[4] { 0.0000m, -1197.4700m, 0.0000m, 80.0000m };
        public static decimal EventFinalCCCost = 0.0000m;
        public static int[] EventFinalRegCount = new int[4] { 6, 5, 5, 4 };
        public static int[] EventFinalItemId = new int[4] { 1, 2, 8, 56 };

        /// <summary>
        /// Data to verify against database for billing verification
        /// </summary>
        public static decimal[] MembershipAmount = new decimal[4] { -111.7700m, 4.4200m, 3.6000m, 3.6000m };
        public static decimal[] MembershipCCAmount = new decimal[4] { -111.7700m, 0.0000m, 0.0000m, 0.0000m };
        public static decimal MembershipCCCost = 0.0000m;
        public static int[] MembershipRegCount = new int[4] { 2, 2, 1, 1 };
        public static int[] MembershipTypeId = new int[4] { 2, 17, 28, 16 };
        public static int[] MembershipCusomterId = new int[4] { 377977, 377977, 377977, 377977 };
        public static decimal[] MembershipFinalAmount = new decimal[4] { -111.7700m, 3.6000m, 4.4200m, 3.6000m };
        public static decimal[] MembershipFinalCCAmount = new decimal[4] { -111.7700m, 0.0000m, 0.0000m, 0.0000m };
        public static decimal MembershipFinalCCCost = 0.0000m;
        public static int[] MembershipFinalRegCount = new int[4] { 2, 1, 2, 1 };
        public static int[] MembershipFinalItemId = new int[4] { 2, 16, 17, 28 };

        /// <summary>
        /// Data to verify against database for billing verification
        /// </summary>
        public static decimal[] EnduranceAmount = new decimal[3] { 3.9500m, -280.6300m, 18.4000m };
        public static decimal[] EnduranceCCAmount = new decimal[3] { 0.0000m, -280.6300m, 0.0000m, };
        public static decimal EnduranceCCCost = 0.0000m;
        public static int[] EnduranceRegCount = new int[3] { 1, 1, 1 };
        public static int[] EnduranceTypeId = new int[3] { 1, 2, 8 };
        public static int[] EnduranceCusomterId = new int[3] { 378150, 378150, 378150 };
        public static decimal[] EnduranceFinalAmount = new decimal[3] { 3.9500m, -280.6300m, 18.4000m };
        public static decimal[] EnduranceFinalCCAmount = new decimal[3] { 0.0000m, -280.6300m, 0.0000m, };
        public static decimal EnduranceFinalCCCost = 0.0000m;
        public static int[] EnduranceFinalRegCount = new int[3] { 1, 1, 1 };
        public static int[] EnduranceFinalItemId = new int[3] { 1, 2, 8 };

        /// <summary>
        /// Data to verify against database for billing verification
        /// </summary>
        public static decimal[] DiffCurrencyAmount = new decimal[3] { 3.9500m, 0.0000m, 0.0000m };
        public static decimal[] DiffCurrencyCCAmount = new decimal[3] { 0.0000m, 0.0000m, 0.0000m };
        public static decimal DiffCurrencyCCCost = 0.0000m;
        public static int[] DiffCurrencyRegCount = new int[3] { 1, 1, 1 };
        public static int[] DiffCurrencyTypeId = new int[3] { 1, 2, 8 };
        public static int[] DiffCurrencyCusomterId = new int[3] { 378150, 378150, 378150 };
        public static decimal[] DiffCurrencyFinalAmount = new decimal[3] { 3.9500m, -279.2000m, 18.4079m };
        public static decimal[] DiffCurrencyFinalCCAmount = new decimal[3] { 0.0000m, -279.2000m, 0.0000m };
        public static decimal DiffCurrencyFinalCCCost = 0.0000m;
        public static int[] DiffCurrencyFinalRegCount = new int[3] { 1, 1, 1 };
        public static int[] DiffCurrencyFinalItemId = new int[3] { 1, 2, 8 };
    }
}
