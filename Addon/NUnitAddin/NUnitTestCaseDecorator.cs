namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using System.Reflection;
    using NUnit.Core;
    using NUnit.Core.Extensibility;

    public class NUnitTestCaseDecorator : ITestDecorator
    {
        private const string CLASS_NAME = "NUnitTestCaseDecorator::";

        /// <summary>
        /// Instantiates the extended Test Case class and passes it the SpiraTeam Test Case ID
        /// </summary>
        /// <param name="test">The NUnit Test</param>
        /// <param name="member">The method whose attribute we need to read</param>
        /// <returns></returns>
        public Test Decorate(Test test, System.Reflection.MemberInfo member)
        {
            const string METHOD_NAME = "Decorate: ";

            try
            {
                if (test is TestMethod)
                {
                    ////Attribute attribute = Reflect.GetAttribute(member, "SpiraTeamTestCaseId", false);

                    ////if (attribute != null)
                    ////{
                    ////    // Get the test case id from the test case attribute
                    ////    int testCaseId = (int)Reflect.GetPropertyValue(attribute, "TestCaseId", BindingFlags.Public | BindingFlags.Instance);
                    ////    test = new ExtendedNUnitTestCase((TestMethod)test, testCaseId);
                    ////}

                    Attribute attribute = Reflect.GetAttribute(member, SpiraTeamTestCaseIdAttribute.AttributeFullName, false);

                    if (attribute != null)
                    {
                        // Get the test case id from the test case attribute
                        int testCaseId = SpiraTeamTestCaseIdAttribute.GetTestCaseId(member);
                        test = new ExtendedNUnitTestCase((TestMethod)test, testCaseId);
                    }
                    else
                    {
                        throw new Exception(string.Format("'{0}' attribute not set!", SpiraTeamTestCaseIdAttribute.AttributeFullName));
                    }
                }

                return test;
            }
            catch (Exception exception)
            {
                // Log error then rethrow
                System.Diagnostics.EventLog.WriteEntry(TestAddin.SOURCE_NAME, CLASS_NAME + METHOD_NAME + exception.Message, System.Diagnostics.EventLogEntryType.Error);
                throw exception;
            }
        }
    }
}