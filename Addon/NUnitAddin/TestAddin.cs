namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using System.Net;

    [NUnitAddin(Type = ExtensionType.Core, Name = "ResultReporter", Description = "Report test case's execution results")]
    public class TestAddin : IAddin
    {
        internal const string SOURCE_NAME = "ResultReporter";
        private const string CLASS_NAME = "TestAddin::";

        #region IAddin Members
        public bool Install(IExtensionHost host)
        {
            const string METHOD_NAME = "Install: ";

            try
            {
                IExtensionPoint listeners = host.GetExtensionPoint("EventListeners");
                
                if (listeners == null)
                {
                    return false;
                }

                listeners.Install(new NUnitTestEventListener());
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