﻿namespace RegOnline.RegressionTest.PageObject.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RegOnline.RegressionTest.WebElements;
    using RegOnline.RegressionTest.Configuration;

    public class EventCalendar : Window
    {
        public RadioButton CalendarView = new RadioButton("viewList_0", UIUtility.LocateBy.Id);
        public RadioButton ViewByLocation = new RadioButton("viewList_1", UIUtility.LocateBy.Id);
        public RadioButton ViewByMonth = new RadioButton("viewList_2", UIUtility.LocateBy.Id);
        public RadioButton ViewByDay = new RadioButton("viewList_3", UIUtility.LocateBy.Id);
        public RadioButton ViewByCategory = new RadioButton("viewList_4", UIUtility.LocateBy.Id);

        private ButtonOrLink Register;

        public void SelectView(DataCollection.FormData.EventCalendarView view)
        {
            switch (view)
            {
                case RegOnline.RegressionTest.DataCollection.FormData.EventCalendarView.Calendar:
                    this.CalendarView.WaitForDisplay();
                    this.CalendarView.Click();
                    WaitForLoad();
                    break;

                case RegOnline.RegressionTest.DataCollection.FormData.EventCalendarView.Location:
                    this.ViewByLocation.WaitForDisplay();
                    this.ViewByLocation.Click();
                    WaitForLoad();
                    break;

                case RegOnline.RegressionTest.DataCollection.FormData.EventCalendarView.Month:
                    this.ViewByMonth.WaitForDisplay();
                    this.ViewByMonth.Click();
                    WaitForLoad();
                    break;

                case RegOnline.RegressionTest.DataCollection.FormData.EventCalendarView.Day:
                    this.ViewByDay.WaitForDisplay();
                    this.ViewByDay.Click();
                    WaitForLoad();
                    break;

                case RegOnline.RegressionTest.DataCollection.FormData.EventCalendarView.Category:
                    this.ViewByCategory.WaitForDisplay();
                    this.ViewByCategory.Click();
                    WaitForLoad();
                    break;

                default:
                    break;
            }
        }

        public void ClickToRegister(DataCollection.Event evt)
        {
            this.Register = new ButtonOrLink(
                string.Format("//a[@href='{0}?{1}']", ConfigurationProvider.XmlConfig.AccountConfiguration.BaseUrlWithHttps, evt.Id), 
                UIUtility.LocateBy.XPath);

            this.Register.WaitForDisplay();
            this.Register.Click();
            WaitForLoad();
        }
    }
}
