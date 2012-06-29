namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using NUnit.Core;
    using RegOnline.RegressionTest.Configuration;

    public class NUnitTestEventListener : EventListener
    {
        #region EventListener Members
        public void RunFinished(Exception exception) { }

        public void RunFinished(TestResult result) { }

        public void RunStarted(string name, int testCount) { }

        public void SuiteFinished(TestResult result) { }

        public void SuiteStarted(TestName testName) { }

        public void TestFinished(TestResult result)
        {
            if (ConfigurationProvider.XmlConfig.AllConfiguration.NUnitAddin.ReportBack)
            {
                ResultReporter.ReportResultToSpiraTeam(result);
            }
        }

        public void TestOutput(TestOutput testOutput) { }

        public void TestStarted(TestName testName) { }

        public void UnhandledException(Exception exception) { }
        #endregion
    }
}
