namespace RegOnline.RegressionTest.UIUtility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;
    using RegOnline.RegressionTest.Attributes;
    using RegOnline.RegressionTest.Configuration;
    using RegOnline.RegressionTest.Utilities;

    public class WebDriverUtility
    {
        private static WebDriverUtility Default = new WebDriverUtility();
        private TimeSpan timeOutSpan;
        private IWebDriver driver;
        private ConfigReader.BrowserEnum browser;

        public static WebDriverUtility DefaultProvider
        {
            get
            {
                return WebDriverUtility.Default;
            }
        }

        private By GetLocatorFinder(string locator, LocateBy locateBy)
        {
            switch (locateBy)
            {
                case LocateBy.XPath:
                    return By.XPath(locator);

                case LocateBy.Id:
                    return By.Id(locator);

                case LocateBy.LinkText:
                    return By.LinkText(locator);

                case LocateBy.PartialLinkText:
                    return By.PartialLinkText(locator);

                case LocateBy.CssSelector:
                    return By.CssSelector(locator);

                case LocateBy.ClassName:
                    return By.ClassName(locator);

                case LocateBy.Name:
                    return By.Name(locator);

                case LocateBy.TagName:
                    return By.TagName(locator);

                default:
                    this.FailTest(string.Format("Cannot determine locator finder type for '{0}' located by '{1}'", locator, locateBy.ToString()));
                    return null;
            }
        }

        public void Initialize()
        {
            this.StartWebDriver();
            this.SetTimeoutSpan();

            if (this.browser != ConfigReader.BrowserEnum.HtmlUnit)
            {
                this.MaximizeWindow();
            }
        }

        private void StartWebDriver()
        {
            Enum.TryParse<ConfigReader.BrowserEnum>(ConfigReader.DefaultProvider.CurrentBrowser.Name, out this.browser);
            IGetWebDriver br;

            switch (browser)
            {
                case ConfigReader.BrowserEnum.Firefox:
                    br = new Browser_Firefox();
                    break;

                case ConfigReader.BrowserEnum.Chrome:
                    br = new Browser_Chrome();
                    break;

                case ConfigReader.BrowserEnum.HtmlUnit:
                    br = new Browser_HtmlUnit();
                    break;

                default:
                    throw new Exception(string.Format("No matching browser for current browser name: {0}", browser.ToString()));
            }

            this.driver = br.GetWebDriver();
        }

        public void Exit()
        {
            this.driver.Quit();
        }

        /// <summary>
        /// There is no other way to clear cache, so you must restart firefox. This method should ONLY be used when you need to clear the cache
        /// </summary>
        public void ClearCookiesAndRestart()
        {
            this.driver.Manage().Cookies.DeleteAllCookies();
            this.Exit();
            this.Initialize();
        }

        public void ExecuteJavaScript(string script)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript(script);
        }

        /// <summary>
        /// Hide the footer to avoid invalid click action (some elements may be covered by footer)
        /// </summary>
        /// <param name="hide">true: hide, false: display</param>
        public void HideActiveSpecificFooter(bool hide)
        {
            string footerLocator_Id = "activeSpecificFooter";
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            string script = string.Format(
                "document.getElementById('{0}').setAttribute('style', '{1}')", 
                footerLocator_Id, 
                hide ? "display: none" : string.Empty);

            js.ExecuteScript(script);
        }

        public void SetTimeoutSpan()
        {
            this.SetTimeoutSpan(TimeSpan.FromMilliseconds(UIUtilityHelper.DefaultTimeoutMilliSeconds));
        }

        public void SetTimeoutSpan(TimeSpan timeOut)
        {
            this.timeOutSpan = timeOut;
        }

        public void CaptureScreenshot()
        {
            this.CaptureScreenshot(string.Format(
                UIUtilityHelper.ScreenshotFileNameFormat, 
                DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss tt")));
        }

        public void CaptureScreenshot(string fileName)
        {
            this.CaptureScreenshot(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Screenshot"), fileName);
        }

        private void CaptureScreenshot(string directoryName, string fileName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            string fullFileName = Path.Combine(directoryName, fileName);

            // Wait for 1 second before taking screenshot till the page can fully load
            Utility.ThreadSleep(1);

            Screenshot srcFile = ((ITakesScreenshot)driver).GetScreenshot();
            srcFile.SaveAsFile(fullFileName, ImageFormat.Png);
        }

        public void MaximizeWindow()
        {
            driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
            
            driver.Manage().Window.Size = new System.Drawing.Size(
                System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, 
                System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 40);
        }

        public void MaximizeRADWindow()
        {
            this.WaitForDisplayAndClick("//a[@class='rwMaximizeButton']", LocateBy.XPath);
            Utility.ThreadSleep(1);
        }

        private void ClickAdvancedHeader()
        {
            this.WaitForDisplayAndClick(UIUtilityHelper.AdvancedHeaderLocator, LocateBy.Id);
            WaitForAJAXRequest();
        }

        public void ExpandAdvanced()
        {
            if (!this.IsElementDisplay(UIUtilityHelper.AdvancedSectionDIVLocator, LocateBy.Id))
            {
                this.ClickAdvancedHeader();
            }
        }

        public void CollapseAdvanced()
        {
            if (this.IsElementDisplay(UIUtilityHelper.AdvancedSectionDIVLocator, LocateBy.Id))
            {
                this.ClickAdvancedHeader();
            }
        }

        public List<IWebElement> GetElements(string locator, LocateBy LocatorType)
        {
            ReadOnlyCollection<IWebElement> elements = this.driver.FindElements(this.GetLocatorFinder(locator, LocatorType));

            List<IWebElement> elementList = new List<IWebElement>();

            foreach (IWebElement element in elements)
            {
                elementList.Add(element);
            }

            return elementList;
        }

        public int GetElementsCount(string locator, LocateBy LocatorType)
        {
            return this.GetElements(locator, LocatorType).Count;
        }

        public bool IsElementPresent(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsElementDisplay(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            IWebElement element;

            try
            {
                element = driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return element.Displayed;
        }

        /// <summary>
        /// Determine whether a element is hidden using its style attibute.
        /// "display: none;" = not hidden
        /// "display: block;" = hidden
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public bool IsElementHidden(string locator, LocateBy locateBy)
        {

            string styleAttributeText = string.Empty;
            string classAttributeText = string.Empty;

            try
            {
                styleAttributeText = GetAttribute(locator, "style", locateBy);
            }
            catch
            {
                styleAttributeText = string.Empty;
            }

            try
            {
                classAttributeText = GetAttribute(locator, "class", locateBy);
            }
            catch
            {
                classAttributeText = string.Empty;
            }

            if (styleAttributeText.Contains("display: none;") 
                || styleAttributeText.Contains("display:none;") 
                || classAttributeText.Contains("hidden"))
            {
                return true;
            }
            else if (styleAttributeText.Contains("display: block;") || styleAttributeText.Contains("display:block;"))
            {
                return false;
            }
            else
            {
                return !IsElementPresent(locator, locateBy);
            }
        }

        [Verify]
        public bool IsTextPresent(string expectedText)
        {
            if (this.GetPageSource().Contains(expectedText) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetPageSource()
        {
            return driver.PageSource;
        }

        public void WaitForElementPresent(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
        }

        private IWebElement WaitForElementPresent(By by)
        {
            IWebElement element = null;

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, this.timeOutSpan);
                element = wait.Until(d => d.FindElement(by));
            }
            catch
            {
                this.FailTest(string.Format("Unable to locate or timed out when waiting for element PRESENT: '{0}'", by.ToString()));
            }

            return element;
        }

        public void WaitForElementDisplay(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementDisplay(by);
        }

        private IWebElement WaitForElementDisplay(By by)
        {
            IWebElement element = null;

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, this.timeOutSpan);
                bool isDisplayed = wait.Until(d => d.FindElement(by).Displayed == true);

                if (isDisplayed)
                {
                    element = driver.FindElement(by);
                }
            }
            catch
            {
                this.FailTest(string.Format("Unable to locate or timed out when waiting for element DISPLAY: '{0}'", by.ToString()));
            }

            return element;
        }

        [Step]
        public void WaitForDisplayAndClick(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            IWebElement element = this.WaitForElementDisplay(by);
            element.Click();
        }

        /// <summary>
        /// Click on an element without any wait.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="locateBy"></param>
        public void Click(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            driver.FindElement(by).Click();
        }

        [Step]
        public void WaitForPageToLoad()
        {
            this.WaitForPageToLoad(TimeSpan.FromMilliseconds(UIUtilityHelper.DefaultTimeoutMilliSeconds));
        }

        public void WaitForPageToLoad(TimeSpan timeOutSpan)
        {
            int timeOutSpanInMilliSeconds = Convert.ToInt32(timeOutSpan.TotalMilliseconds);
            object result = null;
            int elapsedTimeSpanInMilliSeconds = 0;

            while (elapsedTimeSpanInMilliSeconds <= timeOutSpanInMilliSeconds)
            {
                result = ((IJavaScriptExecutor)driver).ExecuteScript("return document['readyState'] ? 'complete' == document.readyState : true");
                
                if ((bool)result)
                {
                    return;
                }
                else
                {
                    Utility.ThreadSleep(0.25);
                    elapsedTimeSpanInMilliSeconds += 250;
                }
            }

            if (elapsedTimeSpanInMilliSeconds > timeOutSpanInMilliSeconds)
            {
                this.FailTest(string.Format("Timed out after {0}ms", timeOutSpanInMilliSeconds.ToString()));
            }
        }

        /// <summary>
        /// Waits for a RAD window to load.
        /// </summary>
        public void WaitForRADWindow()
        {
            TryWaitForRADWindowOpening();
            WebDriverWait wait = new WebDriverWait(driver, this.timeOutSpan);
            wait.Until(d => (bool)((IJavaScriptExecutor)d).ExecuteScript("return window.top.radWindowLoading") == false);
        }

        private void TryWaitForRADWindowOpening()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                wait.Until(d => (bool)((IJavaScriptExecutor)d).ExecuteScript("return window.top.radWindowLoading") == true);
            }
            catch (TimeoutException) { /* ignore this timeout */ }
        }

        /// <summary>
        /// Waits for an AJAX request to complete.
        /// </summary>
        [Step]
        public void WaitForAJAXRequest()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            try
            {
                wait.Until(d => (string)((IJavaScriptExecutor)d).ExecuteScript(
                        "return document.readyState") != "complete");
            }
            catch (TimeoutException) {/*ignore*/}

            wait = new WebDriverWait(driver, this.timeOutSpan);
            try
            {
                wait.Until(d => (string)((IJavaScriptExecutor)d).ExecuteScript(
                        "return document.readyState") == "complete");
            }
            catch (TimeoutException) {/*ignore*/}
        }

        public void GoBackToPreviousPage()
        {
            driver.Navigate().Back();
        }

        public bool UrlContainsPath(string path)
        {
            return GetLocation().Contains(path.ToLower());
        }

        public bool UrlContainsAbsolutePath(string path)
        {
            return GetCurrentAbsolutePath().Contains(path.ToLower());
        }

        /// <summary>
        /// Get the current url
        /// </summary>
        /// <returns>Current url in lower case</returns>
        public string GetLocation()
        {
            return driver.Url.ToLower();
        }

        public string GetCurrentAbsolutePath()
        {
            Uri uri = new Uri(this.GetLocation());
            return uri.AbsolutePath.ToLower();
        }

        public string GetQueryStringValue(string queryStringParameter)
        {
            return this.ExtractElementInQueryString(this.GetLocation(), queryStringParameter);
        }

        /// <summary>
        /// Extracts an element in the query string
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="target">target</param>
        /// <returns></returns>
        public string ExtractElementInQueryString(string input, string target)
        {
            input = input.ToLower();
            target = target.ToLower();
            string ret = string.Empty;
            string pattern = @"[a-zA-Z]+=[a-zA-Z0-9]*";
            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

            foreach (Match mt in matches)
            {
                string matched = mt.Value;

                if (matched.IndexOf(target) >= 0)
                {
                    ret = matched.Substring(target.Length + 1, matched.Length - target.Length - 1);
                    break;
                }
            }

            return ret;
        }

        [Step]
        public void Type(string locator, object value, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            IWebElement element = this.WaitForElementDisplay(by);

            try
            {
                element.Clear();
            }
            catch
            {
                // Ignore exception such as:
                // 'Element must be user-editable in order to clear'
            }

            element.SendKeys(Convert.ToString(value));
        }

        //This has to exist for uploading photos, do not mess with it please! -ed-
        [Step]
        public void SendKeys(string locator, object value, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            driver.FindElement(by).SendKeys(Convert.ToString(value));
        }

        public void TypeRADNumericById(string locator_Id, object value)
        {
            string stringValue = Convert.ToString(value);
            this.Type(locator_Id + UIUtilityHelper.RADNumericLocatorSuffix_text, stringValue, LocateBy.Id);

            this.SetValue(locator_Id, stringValue);
        }

        private void SetValue(string locator_Id, string value)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("window.$find('{0}').set_value({1})", locator_Id, value));
        }

        public string GetValue(string locator, LocateBy locateBy)
        {
            return this.GetAttribute(locator, "value", locateBy);
        }

        public string GetId(string locator, LocateBy locateBy)
        {
            return this.GetAttribute(locator, "id", locateBy);
        }

        public string GetText(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            return driver.FindElement(by).Text;
        }

        public string GetAttribute(string locator, string attribute, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);

            try
            {
                return driver.FindElement(by).GetAttribute(attribute);
            }
            catch
            {
                this.FailTest("Failed to get attribute '" + attribute + "' for element: " + by.ToString());
                return string.Empty;
            }
        }

        public void SetDateTimeById(string locator_Id, DateTime dateTime)
        {
            string value = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", dateTime);
            this.Type(locator_Id + "_dateInput_text", value, LocateBy.Id);
        }

        public void SetDateForDatePicker(string locator_Id_Picker, DateTime date)
        {
            SetDateForDatePicker(locator_Id_Picker, date.Year, date.Month, date.Day);
        }

        public void SetTimeForTimePicker(string locator_Id_Picker, DateTime dateTime)
        {
            SetDateForDatePicker(locator_Id_Picker, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        /// <summary>
        /// Enters the given date into a Telerik date picker.
        /// </summary>
        /// <param name="locator_Id_Picker">The ID of the date picker</param>
        /// <param name="y">Year</param>
        /// <param name="m">Month</param>
        /// <param name="d">Day</param>
        public void SetDateForDatePicker(string locator_Id_Picker, int y, int m, int d)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "$find('{0}').set_selectedDate(new Date({1}, {2}, {3}));", 
                locator_Id_Picker, 
                y.ToString(), 
                (m - 1).ToString(), 
                d.ToString()));
        }

        /// <summary>
        /// Enters the given date and time into a Telerik date picker.
        /// </summary>
        /// <param name="locator_Id_Picker">The ID of the date picker</param>
        /// <param name="y">Year</param>
        /// <param name="m">Month</param>
        /// <param name="d">Day</param>
        /// <param name="h">Hours</param>
        /// <param name="minutes">Minutes</param>
        /// <param name="s">Seconds</param>
        public void SetDateForDatePicker(string locator_Id_Picker, int y, int m, int d, int h, int minutes, int s)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "$find('{0}').set_selectedDate(new Date({1}, {2}, {3}, {4}, {5}, {6}));",
                locator_Id_Picker,
                y.ToString(),
                (m - 1).ToString(),
                d.ToString(),
                h.ToString(),
                minutes.ToString(),
                s.ToString()));
        }

        /// <summary>
        /// Enters the given start and end dates into Builder's DateTimePickerWrapper control.
        /// </summary>
        public void SetDatesTimesForDatePickerWrapper(DateTime startDate, DateTime endDate)
        {
            string startDateId = "ctl00_cph_ucCF_dtpSD_dateInput";
            string endDateId = "ctl00_cph_ucCF_dtpED_dateInput";

            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "$find('{0}').set_selectedDate(new Date({1},{2},{3}))",
                startDateId,
                startDate.Year,
                startDate.Month - 1,
                startDate.Day));

            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "$find('{0}').set_selectedDate(new Date({1},{2},{3}))",
                endDateId,
                endDate.Year,
                endDate.Month - 1,
                endDate.Day));

            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "UpdateTimeFields(new Date({0}, {1}, {2}, {3}, {4}, 0), new Date({5}, {6}, {7}, {8}, {9}, 0));",
                startDate.Year, 
                startDate.Month - 1, 
                startDate.Day,
                startDate.Hour, 
                startDate.Minute,
                endDate.Year, 
                endDate.Month - 1, 
                endDate.Day,
                endDate.Hour, 
                endDate.Minute));
        }

        [Verify]
        public bool IsChecked(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            return driver.FindElement(by).Selected;
        }

        public bool IsEditable(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            return driver.FindElement(by).Enabled;
        }

        /// <summary>
        /// Refresh the page
        /// </summary>
        [Step]
        public void RefreshPage()
        {
            driver.Navigate().Refresh();
            this.WaitForPageToLoad();
        }

        [Step]
        public void OpenUrl(string targetUrl)
        {
            try
            {
                driver.Navigate().GoToUrl(targetUrl);
                this.WaitForPageToLoad();
            }
            catch (WebDriverException e)
            {
                this.FailTest(string.Format("Exception when trying to open '{0}': {1}", targetUrl, e.Message));
            }
        }

        [Step]
        public void GetConfirmation()
        {
            ////driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            driver.SwitchTo().Alert().Accept();
            ////driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        public string GetConfirmationText()
        {
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            string text = driver.SwitchTo().Alert().Text;
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));

            return text;
        }

        public void ChooseCancelOnNextConfirmation()
        {
            driver.SwitchTo().Alert().Dismiss();
        }

        public bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string> GetAllWindows()
        {
            List<string> windowHandles = new List<string>();

            foreach (string windowHandle in driver.WindowHandles)
            {
                windowHandles.Add(windowHandle);
            }

            return windowHandles;
        }

        [Step]
        public void SelectOriginalWindow()
        {
            try
            {
                ReadOnlyCollection<string> windows = driver.WindowHandles;
                driver.SwitchTo().Window(windows[0]);
            }
            catch (WebDriverException)
            {
                this.FailTest("Failed to select the original window");
            }
        }

        public void SelectTopWindow()
        {
            try
            {
                ReadOnlyCollection<string> windows = driver.WindowHandles;
                driver.SwitchTo().Window(windows[windows.Count - 1]);
            }
            catch (WebDriverException)
            {
                this.FailTest("Failed to select the first non-top POPUP window");
            }
        }

        public void SelectWindowByIndex(int index)
        {
            try
            {
                List<string> popupWindowNames = this.GetAllWindows();
                this.driver.SwitchTo().Window(popupWindowNames[index]);
            }
            catch (WebDriverException)
            {
                this.FailTest(string.Format("Failed to select the window by index: {0}", index.ToString()));
            }
        }

        public void SelectWindowByName(string windowName)
        {
            try
            {
                this.driver.SwitchTo().Window(windowName);
            }
            catch (WebDriverException)
            {
                this.FailTest(string.Format("Failed to select the window by name: {0}", windowName));
            }
        }

        [Step]
        public void SelectWindowByTitle(string windowTitle)
        {
            try
            {
                ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

                if (windowHandles.Count != 0)
                {
                    foreach (string windowId in windowHandles)
                    {
                        if (driver.SwitchTo().Window(windowId).Title == (windowId))
                        {
                            break;
                        }
                    }
                }
            }
            catch (WebDriverException)
            {
                this.FailTest(string.Format("Failed to select the window by title: {0}", windowTitle));
            }
        }

        public void ClosePopUpWindow()
        {
            SelectTopWindow();
            CloseWindow();
            SelectOriginalWindow();
        }

        [Step]
        public void CloseWindow()
        {
            driver.Close();
        }

        public void SelectUpperFrame()
        {
            try
            {
                driver.SwitchTo().Frame(0);
            }
            catch (NoSuchFrameException)
            {
                this.FailTest("Failed to select the UPPER popup frame");
            }
        }

        [Step]
        public void SelectPopUpFrameByName(string frameName)
        {
            SwitchToMainContent();
            IWebElement frame = this.WaitForElementDisplay(By.Name(frameName));

            try
            {
                driver.SwitchTo().Frame(frameName);
            }
            catch (WebDriverException)
            {
                this.FailTest(string.Format("Failed to select the popup frame by name: {0}", frameName));
            }
        }

        public void SelectPopUpFrameById(string frameId)
        {
            //SwitchToMainContent();
            IWebElement frame = this.WaitForElementDisplay(By.Id(frameId));

            try
            {
                driver.SwitchTo().Frame(frame);
            }
            catch (WebDriverException)
            {
                this.FailTest(string.Format("Failed to select the popup frame by id: {0}", frameId));
            }
        }

        public void SelectIFrame(int index)
        {
            SwitchToMainContent();
            ReadOnlyCollection<IWebElement> iFrames = GetAllIFrames();
            driver.SwitchTo().Frame(iFrames[index]);
        }

        public void SelectIFrameOnCurrentIFrame(int index)
        {
            ReadOnlyCollection<IWebElement> iFrames = this.GetAllIFrames();
            driver.SwitchTo().Frame(iFrames[index]);
        }

        public ReadOnlyCollection<IWebElement> GetAllIFrames()
        {
            return driver.FindElements(By.TagName("iframe"));
        }

        public void SelectWithValue(string locator, string value, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            IWebElement select = driver.FindElement(by);
            SelectElement item = new SelectElement(select);
            string val = Convert.ToString(value);
            item.SelectByValue(val);
        }

        public void SelectWithIndex(string locator, int index, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            IWebElement select = driver.FindElement(by);
            SelectElement item = new SelectElement(select);
            item.SelectByIndex(index);
        }

        [Step]
        public void SelectWithText(string locator, object value, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            IWebElement select = driver.FindElement(by);
            SelectElement item = new SelectElement(select);
            string val = Convert.ToString(value);
            item.SelectByText(val);
        }

        public string GetSelectedLabel(string locator, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            IWebElement select = driver.FindElement(by);
            SelectElement item = new SelectElement(select);
            select = item.SelectedOption;
            return select.Text;
        }

        public string GetSelectedOptionFromDropdownByXPath(string xPath)
        {
            xPath += "/option[@selected='selected']";
            return this.GetText(xPath, LocateBy.XPath);
        }

        public bool IsAnySelectionMadeOnDropDownByXPath(string xPath)
        {
            xPath += "/option[@selected='selected']";
            bool selections = this.IsElementPresent(xPath, LocateBy.XPath);
            return selections;
        }

        public bool IsOptionExistInSelect(string locator, object value, LocateBy locateBy)
        {
            bool ret = false;
            By by = this.GetLocatorFinder(locator, locateBy);
            this.WaitForElementPresent(by);
            IWebElement select = driver.FindElement(by);
            SelectElement item = new SelectElement(select);
            string val = Convert.ToString(value);
            try
            {
                item.SelectByText(val);
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public void TypeContentEditorOnFrame(string text)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "window.frames[1].$find('ctl00_cphDialog_ucContent_reContentEditor').set_html('{0}')",
                text));
        }

        public void SaveAndCloseContentEditorFrame()
        {
            this.SelectPopUpFrameByName("dialog2");
            this.ClickSaveAndClose();
            this.SwitchToMainContent();
            this.WaitForAJAXRequest();
        }

        public void TypeContentEditorOnWindowById(string locator_Id, string text)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "window.$find('{0}').set_html('{1}')",
                locator_Id,
                text));
        }

        //Might need to add index parameter...
        public void TypeWizardContentEditorOnFrame(string text)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format(
                "window.frames[0].$find(window.frames[0].document.getElementsByClassName('RadEditor')[0].id).set_html('{0}')",
                text));
        }

        public void SetCheckbox(string locator, bool check, LocateBy locateBy)
        {
            By by = this.GetLocatorFinder(locator, locateBy);
            IWebElement element = this.WaitForElementDisplay(by);

            if (check)
            {
                if (!element.Selected)
                {
                    element.Click();
                }
            }
            else
            {
                if (element.Selected)
                {
                    element.Click();
                }
            }
        }

        [Step]
        public void SwitchToMainContent()
        {
            driver.SwitchTo().DefaultContent();
        }

        public void ClickSaveAndNew()
        {
            this.WaitForDisplayAndClick(UIUtilityHelper.SaveAndNewButtonLocator, LocateBy.Id);
        }

        public void ClickSaveAndStay()
        {
            this.WaitForDisplayAndClick(UIUtilityHelper.SaveAndStayButtonLocator, LocateBy.Id);
        }

        public void ClickSaveAndClose()
        {
            this.WaitForDisplayAndClick(UIUtilityHelper.SaveAndCloseButtonLocator, LocateBy.Id);
        }

        public void ClickCancel()
        {
            this.WaitForDisplayAndClick(UIUtilityHelper.CancelButtonLocator, LocateBy.Id);
        }

        public string GetTable(string tableId, int rowIndex, int columnIndex)
        {
            string results = string.Empty;
            this.WaitForElementPresent(tableId, LocateBy.Id);

            try
            {
                if (columnIndex == 2)
                {
                    results = GetText(string.Format("//table[@id='" + tableId + "']//tr[{0}]/td[{1}]//b", rowIndex, columnIndex), LocateBy.XPath);
                }
                else
                {
                    results = GetText(string.Format("//table[@id='" + tableId + "']//tr[{0}]/td[{1}]", rowIndex, columnIndex), LocateBy.XPath);
                }
            }
            catch
            {
                Assert.Fail("Failed Retrieving Data from table: " + tableId + ", row: " + rowIndex + ", column: " + columnIndex);
            }

            return results;
        }

        /// <summary>
        /// drag and drops from the initial to the end locator
        /// </summary>
        /// <param name="initialLocator">where to click and hold</param>
        /// <param name="endLocator">where to release the mouse click</param>
        public void DragAndDrop(string initialLocator, string endLocator)
        {
            Actions action = new Actions(driver);
            WaitForDisplayAndClick(initialLocator, LocateBy.XPath);
            IWebElement start = WaitForElementPresent(By.XPath(initialLocator));
            IWebElement end = WaitForElementPresent(By.XPath(endLocator));
            action.DragAndDrop(start, end).Build().Perform();
            WaitForAJAXRequest();
        }

        //I need to clean this up and put it somewhere else. For now though it can stay here and should fit within Bruce's new schema. 
        public void UploadEmailLogo(string filePath)
        {
            driver.SwitchTo().Frame(driver.FindElements(By.TagName("iframe"))[0]);
            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('ctl00_cphDialog_upload1file0').style='position: absolute; left: 10px; top: 10px;'");
            driver.FindElement(By.Id("ctl00_cphDialog_upload1file0")).SendKeys(filePath);
            this.WaitForDisplayAndClick("ctl00_cphDialog_btnSubmit", LocateBy.Id);
            SwitchToMainContent();
            driver.SwitchTo().Frame("plain");
            this.WaitForDisplayAndClick("//a/span[text()='Close']", LocateBy.XPath);
            SwitchToMainContent();
            driver.SwitchTo().Frame("plain");
        }

        #region Verify
        public void VerifyConfirmation(string confirmation)
        {
            VerifyTool.VerifyValue(driver.SwitchTo().Alert().Text, confirmation, "Alert: {0}");
        }

        public void VerifyElementPresent(string locator, bool expected, LocateBy locateBy)
        {
            bool actual = this.IsElementPresent(locator, locateBy);

            if (actual != expected)
            {
                this.FailTest(string.Format("Element '{0}' is{1} present!", locator, (expected ? " NOT" : string.Empty)));
            }
        }

        public void VerifyElementDisplay(string locator, bool expected, LocateBy locateBy)
        {
            bool actual = this.IsElementDisplay(locator, locateBy);

            if (actual != expected)
            {
                this.FailTest(string.Format("Element '{0}' is{1} displayed!", locator, (expected ? " NOT" : string.Empty)));
            }
        }

        public void VerifyElementDisplay(string elementName, bool expected, bool actual)
        {
            if (expected != actual)
            {
                string message = string.Format("Element:'{0}' is{1} displayed!", elementName, (expected ? " NOT" : string.Empty));
                this.FailTest(message);
            }
        }

        public void VerifyElementEditable(string elementName, bool expected, bool actual)
        {
            if (expected != actual)
            {
                string message = string.Format("Element:'{0}' is{1} editable!", elementName, (expected ? " NOT" : string.Empty));
                this.FailTest(message);
            }
        }

        public int GetXPathCountByXPath(string xPath)
        {
            By by = this.GetLocatorFinder(xPath, LocateBy.XPath);

            try
            {
                return driver.FindElements(by).Count;
            }
            catch (NoSuchElementException)
            {
                FailTest("Could not find element " + xPath);
                return 0; 
            }
        }

        [Verify]
        public void VerifyOnPage(bool onPage, string pageName)
        {
            if (!onPage)
            {
                this.FailTest(string.Format("Not on '{0}' page", pageName));
            }
        }

        private bool OnErrorPage()
        {
            return this.GetCurrentAbsolutePath().ToLower().Contains("error.aspx");
        }

        private string GetSiteErrorId()
        {
            return GetQueryStringValue("errorid").Trim();
        }

        private string GetCurrentPageOrSiteErrorIdIfNecessary()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (this.OnErrorPage())
            {
                errorMessage.Append(string.Format(", errorID: {0}", this.GetSiteErrorId()));
            }
            else
            {
                errorMessage.Append(string.Format(", current page: '{0}'", this.GetCurrentAbsolutePath()));
            }

            return errorMessage.ToString();
        }

        public void FailTest(string message)
        {
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append(message);
            errorMessage.Append(this.GetCurrentPageOrSiteErrorIdIfNecessary());
            throw new Exception(errorMessage.ToString());
        }

        public void VerifyValue(string expectedValue, string actualValue, string message)
        {
            FailTest(string.Format("{0} Expected value:{1} Actual value:{2}", message, expectedValue, actualValue));
        }

        public void VerifyValue(bool expectedValue, bool actualValue, string message)
        {
            VerifyValue(expectedValue.ToString(), actualValue.ToString(), message);
        }

        public void VerifyValue(int expectedValue, int actualValue, string message)
        {
            VerifyValue(expectedValue.ToString(), actualValue.ToString(), message);
        }

        public void VerifyValue(double expectedValue, double actualValue, string message)
        {
            VerifyValue(expectedValue.ToString(), actualValue.ToString(), message);
        }
        #endregion
    }
}
