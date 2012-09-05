namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using System.Net;

    [NUnitAddin(Type = ExtensionType.Core, Name = "TestResultCommunicator", Description = "Report test case's execution results back to where you want")]
    public class TestAddin : IAddin
    {
        internal const string SOURCE_NAME = "TestResultCommunicator";
        private const string CLASS_NAME = "TestAddin::";

        #region IAddin Members
        public bool Install(IExtensionHost host)
        {
            const string METHOD_NAME = "Install: ";

            try
            {
                ////IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");
                
                ////if (listeners == null)
                ////{
                ////    return false;
                ////}

                ////listeners.Install(new NUnitTestEventListener());

                // Instantiate the test decorator extension point
                IExtensionPoint decorators = host.GetExtensionPoint("TestDecorators");

                if (decorators == null)
                {
                    return false;
                }

                // Install the SpiraTest Test Case decorator
                decorators.Install(new NUnitTestCaseDecorator());

                return true;
            }
            catch (Exception exception)
            {
                // Log error then rethrow
                System.Diagnostics.EventLog.WriteEntry(SOURCE_NAME, CLASS_NAME + METHOD_NAME + exception.Message, System.Diagnostics.EventLogEntryType.Error);
                throw exception;
            }
        }
        #endregion
    }
}