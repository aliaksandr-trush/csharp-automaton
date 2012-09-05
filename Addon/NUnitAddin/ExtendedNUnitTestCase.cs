namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using NUnit.Core;

    /// <summary>
    /// This class extends the built-in NUnit test case to provide support for logging the execution
    /// results with the configured instance of SpiraTeam
    /// </summary>
    public class ExtendedNUnitTestCase : TestMethod
    {
        private const string CLASS_NAME = "ExtendedNUnitTestCase::";

        protected TestMethod testMethod;
        protected int testCaseId = 0;

        /// <summary>
        /// The constructor method for the class. Sets the local member variables with the passed in values
        /// </summary>
        /// <param name="testMethod">The NUnit Test Method</param>
        /// <param name="testCaseId">The ID of the matching SpiraTeam test case</param>
        public ExtendedNUnitTestCase(TestMethod testMethod, int testCaseId)
            : base(testMethod.Method)
        {
            this.testMethod = testMethod;
            this.testCaseId = testCaseId;
        }

        /// <summary>
        /// Executes the NUnit test case
        /// </summary>
        /// <param name="result">The test case result</param>
        public override TestResult RunTest()
        {
            const string METHOD_NAME = "Run: ";

            TestResult result;

            try
            {
                // Call the base method to log the result within NUnit
                result = base.RunTest();

                ResultReporter.ReportResultToSpiraTeam(result, this.testCaseId);
            }
            catch (Exception exception)
            {
                // Log error then rethrow
                System.Diagnostics.EventLog.WriteEntry(TestAddin.SOURCE_NAME, CLASS_NAME + METHOD_NAME + exception.Message, System.Diagnostics.EventLogEntryType.Error);
                throw exception;
            }

            return result;
        }
    }
}
