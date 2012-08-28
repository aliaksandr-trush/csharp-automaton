namespace RegOnline.RegressionTest.Configuration
{
    public partial class Account
    {
        public string BaseUrl
        {
            get
            {
                return string.Format("http://{0}/", this.DomainName);
            }
        }

        public string BaseUrlWithHttps
        {
            get
            {
                return string.Format("https://{0}/", this.DomainName);
            }
        }
    }
}
