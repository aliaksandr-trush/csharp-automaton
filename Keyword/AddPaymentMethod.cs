namespace RegOnline.RegressionTest.Keyword
{
    using RegOnline.RegressionTest.DataCollection;
    using RegOnline.RegressionTest.PageObject.Builder;
    using RegOnline.RegressionTest.Utilities;

    public class AddPaymentMethod
    {
        public void AddPaymentMethods(PaymentMethod method)
        {
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.AddPaymentMethod_Click();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.PaymentMethodSelections.SelectByName();
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.PaymentMethodSelections.PaymentMethods.SelectWithText(CustomStringAttribute.GetCustomString(method.PMethod));
            PageObject.PageObjectProvider.Builder.EventDetails.FormPages.CheckoutPage.PaymentMethodSelections.SaveAndClose_Click();
        }
    }
}
