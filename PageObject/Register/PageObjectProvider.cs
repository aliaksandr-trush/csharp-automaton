namespace RegOnline.RegressionTest.PageObject.Register
{
    public class PageObjectProvider
    {
        public Checkin Checkin = new Checkin();
        public Login Login = new Login();
        public PersonalInfo PersonalInfo = new PersonalInfo();
        public Checkout Checkout = new Checkout();
        public Confirmation Confirmation = new Confirmation();
        public AttendeeCheck AttendeeCheck = new AttendeeCheck();
        public PageObjectHelper RegisterHelper = new PageObjectHelper();
    }
}
