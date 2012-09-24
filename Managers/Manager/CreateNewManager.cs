namespace RegOnline.RegressionTest.Managers.Manager
{
    ////using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;

    public class CreateNewManager
    {
        public const string AddProEventLocator = "ctl00_cphDialog_uclCreateNewForm_btnAddEvent";
        /*
        <li>Welcome 
                    <a id="ctl00_hpEditUser" title="Edit User" href="javascript:editCurrentUser(224178,setUserName);">Selenium</a>
                    (Account: 378575)&nbsp;|</li>*/
        public const string HeaderAccountLocator = "//li[a[@id='ctl00_hpEditUser']][last()]";

        /// <summary>
        /// Extract the new [Customers].[ID] from the CreateNew.aspx header
        /// </summary>
        /// <returns></returns>
        public int GetCurrentAccountId()
        {
            string accountHeader = UIUtil.DefaultProvider.GetText(HeaderAccountLocator, LocateBy.XPath);
            int start = accountHeader.IndexOf("Account:") + 9;
            int end = accountHeader.IndexOf(")");
            accountHeader = accountHeader.Substring(start, end - start);
            int accountId;

            if (!int.TryParse(accountHeader, out accountId))
            {
                throw new System.Exception("Problem parsing Account number from Header");
            }

            return accountId;
        }

        public void AddProEvent()
        {
            UIUtil.DefaultProvider.WaitForDisplayAndClick(AddProEventLocator, LocateBy.Id);
            UIUtil.DefaultProvider.WaitForPageToLoad();
        }
    }
}
