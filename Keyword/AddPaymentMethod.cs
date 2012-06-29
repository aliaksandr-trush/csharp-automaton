namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;
    using RegOnline.RegressionTest.Utilities;

    public class AddPaymentMethod
    {
        private Checkout Checkout = new Checkout();

        public void AddPaymentMethods(PaymentMethod method)
        {
            Checkout.AddPaymentMethod_Click();
            Checkout.PaymentMethodSelections.SelectByName();
            Checkout.PaymentMethodSelections.PaymentMethods.SelectWithText(CustomStringAttribute.GetCustomString(method.PMethod));
            Checkout.PaymentMethodSelections.SaveAndClose_Click();
        }
    }
}
