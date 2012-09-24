namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using NUnit.Core;
    using NUnit.Core.Builders;
    using System.Reflection;

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

                Communicator.ReportResultToSpiraTeam(result, this.testCaseId);
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

    ////public class TestMethodBuilder : NUnitTestCaseBuilder
    ////{
    ////    private const string CLASS_NAME = "TestMethodBuilder::";

    ////    public new Test BuildFrom(MethodInfo method)
    ////    {
    ////        const string METHOD_NAME = "BuildFrom: ";

    ////        try
    ////        {
    ////            //Determines if it can build from the passed in type
    ////            if (CanBuildFrom(method))
    ////            {
    ////                Attribute attribute = Reflect.GetAttribute(method, SpiraTeamTestCaseIdAttribute.AttributeFullName, false);
    ////                int testCaseId = 0;

    ////                if (attribute != null)
    ////                {
    ////                    // Get the test case id from the test case attribute
    ////                    testCaseId = SpiraTeamTestCaseIdAttribute.GetTestCaseId(method);
    ////                }
    ////                else
    ////                {
    ////                    throw new Exception(string.Format("'{0}' attribute not set!", SpiraTeamTestCaseIdAttribute.AttributeFullName));
    ////                }

    ////                return new ExtendedNUnitTestCase(new TestMethod(method), testCaseId);
    ////            }
    ////            else
    ////            {
    ////                return null;
    ////            }
    ////        }
    ////        catch (Exception exception)
    ////        {
    ////            //Log error then rethrow
    ////            System.Diagnostics.EventLog.WriteEntry(TestAddin.SOURCE_NAME, CLASS_NAME + METHOD_NAME + exception.Message, System.Diagnostics.EventLogEntryType.Error);
    ////            throw exception;
    ////        }
    ////    }

    ////    public new bool CanBuildFrom(Type type)
    ////    {
    ////        return Reflect.HasAttribute(type, "RegOnline.RegressionTest.NUnitAddin.SpiraTeamTestCaseIdAttribute", false);
    ////    }
    ////}
}
