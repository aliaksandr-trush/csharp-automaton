namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class KeywordProvider
    {
        public static SignInToManager SignIn = new SignInToManager();
        public static EventCreation Event_Creator = new EventCreation();
        public static VerifyDisplay Verify_Display = new VerifyDisplay();
        public static RegistrationCreation Registration_Creation = new RegistrationCreation();
        public static AddRegType Add_RegType = new AddRegType();
        public static AddDiscountCode Add_DiscountCode = new AddDiscountCode();
        public static AddEarlyLatePrice Add_EarlyLatePrice = new AddEarlyLatePrice();
        public static AddTaxRate Add_TaxRate = new AddTaxRate();
        public static AddAgendaItem Add_AgendaItem = new AddAgendaItem();
        public static AddMerchandise Add_Merchandise = new AddMerchandise();
        public static AddPaymentMethod Add_PaymentMethod = new AddPaymentMethod();
        public static ManagerCommon Manager_Common = new ManagerCommon();
        public static RegisterCommon Register_Common = new RegisterCommon();
        public static BuilderCommon Builder_Common = new BuilderCommon();
        public static AddAttendeeDirectory Add_AttendeeDirectory = new AddAttendeeDirectory();
        public static ChangeEventStatus Change_EventStatus = new ChangeEventStatus();
        public static LaunchKiosk Launch_Kiosk = new LaunchKiosk();
        public static VerifyStandardReports Verify_StandardReports = new VerifyStandardReports();
        public static CustomFieldCreation CustomField_Creation = new CustomFieldCreation();
        public static BackendUpdate Backend_Update = new BackendUpdate();
        public static AddGroupDiscount Add_GroupDiscount = new AddGroupDiscount();
        public static CalculateFee Calculate_Fee = new CalculateFee();
    }
}
