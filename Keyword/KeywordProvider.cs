namespace RegOnline.RegressionTest.Keyword
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class KeywordProvider
    {
        public static SignInToManager SignIn = new SignInToManager();
        public static EventCreation EventCreator = new EventCreation();
        public static VerifyDisplay Display = new VerifyDisplay();
        public static RegistrationCreation RegistrationCreation = new RegistrationCreation();
        public static AddRegType AddRegType = new AddRegType();
        public static AddDiscountCode AddDiscountCode = new AddDiscountCode();
        public static AddEarlyLatePrice AddEarlyLatePrice = new AddEarlyLatePrice();
        public static AddTaxRate AddTaxRate = new AddTaxRate();
        public static AddAgendaItem AddAgendaItem = new AddAgendaItem();
        public static AddMerchandise AddMerchandise = new AddMerchandise();
        public static AddPaymentMethod AddPaymentMethod = new AddPaymentMethod();
        public static ManagerDefault ManagerDefault = new ManagerDefault();
        public static RegisterDefault RegisterDefault = new RegisterDefault();
        public static BuilderDefault BuilderDefault = new BuilderDefault();
        public static AddAttendeeDirectory AddAttendeeDirectory = new AddAttendeeDirectory();
        public static ChangeEventStatus ChangeEventStatus = new ChangeEventStatus();
        public static LaunchKiosk LaunchKiosk = new LaunchKiosk();
        public static VerifyStandardReports VerifyStandardReports = new VerifyStandardReports();
        public static CustomFieldCreation CustomFieldCreation = new CustomFieldCreation();
        public static BackendUpdate BackendUpdate = new BackendUpdate();
        public static AddGroupDiscount AddGroupDiscount = new AddGroupDiscount();
        public static CalculateFee CalculateFee = new CalculateFee();
        public static AddHotel AddHotel = new AddHotel();
    }
}
