namespace RegOnline.RegressionTest.Managers.Builder
{
    //using RegOnline.RegressionTest.Fixtures;
    using RegOnline.RegressionTest.UIUtility;
    using RegOnline.RegressionTest.Utilities;

    public class BibNumberingToolManager : ManagerBase
    {
        private const string BibLocator = "ctl00_cphDialog_rblTeamNumbers_{0}";
        private const string StartingNumberLocator = "ctl00_cphDialog_rptRegType_ctl0{0}_txtRegTypeStart_text";
        private const string DefaultStartingNumberLocator = "ctl00_cphDialog_txtEventStart_text";

        /// <summary>
        /// This class is the proxy for a RegType / BibNumber in Core.
        /// This is the contract by which the Fixture can communicate with the Builder and Manager managers.
        /// </summary>
        public class TeamWithRegTypes : ManagerBase.RegType
        {
            public bool CollectTeamName { get; set; }
            public int StartingNumber { get; set; }
        }

        public enum AssignNumberToMember
        {
            UniqueToEach = 0,
            SameToEvery = 1
        }
        
        public void SetTeamNumbers(AssignNumberToMember which)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(string.Format(BibLocator, (int)which), LocateBy.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regTypeSequence">The zero-based sequence of the list of reg types.</param>
        /// <param name="num"></param>
        public void SetStartingNumber(int regTypeSequence, int num)
        {
            WebDriverUtility.DefaultProvider.Type(string.Format(StartingNumberLocator, regTypeSequence), num, LocateBy.Id);
        }

        public void SetStartingNumberDefault(int num)
        {
            WebDriverUtility.DefaultProvider.Type(DefaultStartingNumberLocator, num, LocateBy.Id);
        }

        public void SaveAndClose()
        {
            SaveAndClose(true);
        }

        public void SaveAndClose(bool reassign)
        {
            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Assign", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            WebDriverUtility.DefaultProvider.WaitForAJAXRequest();

            string subElementOKbutton = "OK";

            if (WebDriverUtility.DefaultProvider.IsElementPresent(subElementOKbutton, LocateBy.LinkText))
            {
                if (reassign)
                {
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick(subElementOKbutton, LocateBy.LinkText);
                }
                else
                {
                    WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Cancel", LocateBy.LinkText);
                }

                Utility.ThreadSleep(1);
                WebDriverUtility.DefaultProvider.WaitForAJAXRequest();
            }

            WebDriverUtility.DefaultProvider.WaitForDisplayAndClick("Close", LocateBy.LinkText);
            Utility.ThreadSleep(1);
            //the calling form will switch back to the main
            WebDriverUtility.DefaultProvider.SelectOriginalWindow();
        }
    }
}
