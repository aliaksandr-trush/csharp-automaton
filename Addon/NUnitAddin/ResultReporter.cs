namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using System.Net;
    using NUnit.Core;

    public class ResultReporter
    {
        private enum ExecutionStatus
        {
            Unknown = -1,
            NotRun = 3,
            Blocked = 5,
            Failed = 1,
            Passed = 2
        }

        internal abstract class SpiraTeam
        {
            public const string WebServiceUrl = "http://10.119.33.66/SpiraTeam/Services/v2_2/ImportExport.asmx";
            public const string Login = "AutoReg";
            public const string Password = "AutoReg";
            public const int ProjectId = 5;
            public const int ReleaseId = -1;
            public const int TestSetId = 72;
            public const string RunnerName = "NUnit";
        }

        public static void ReportResultToSpiraTeam(TestResult result)
        {
            string url = SpiraTeam.WebServiceUrl;
            string login = SpiraTeam.Login;
            string password = SpiraTeam.Password;
            int projectId = Convert.ToInt32(SpiraTeam.ProjectId);
            Nullable<int> releaseId = Convert.ToInt32(SpiraTeam.ReleaseId);
            Nullable<int> testSetId = Convert.ToInt32(SpiraTeam.TestSetId);
            string runnerName = SpiraTeam.RunnerName;

            // Now we need to extract the result information
            ExecutionStatus executionStatus = ExecutionStatus.Unknown;
            
            if (!result.Executed)
            {
                // Set status to 'Not Run'
                executionStatus = ExecutionStatus.NotRun;
            }
            else
            {
                // If no codes are found, default to blocked;
                executionStatus = ExecutionStatus.Blocked;

                if (result.IsFailure)
                {
                    // Set status to 'Failed'
                    executionStatus = ExecutionStatus.Failed;
                }

                if (result.IsSuccess)
                {
                    // Set status to 'Passed'
                    executionStatus = ExecutionStatus.Passed;
                }

                if (result.IsError)
                {
                    // Set status to 'Failed'
                    executionStatus = ExecutionStatus.Failed;
                }
            }

            // Extract the other information
            string testCaseId = string.Empty;

            if (!string.IsNullOrEmpty(result.Description))
            {
                testCaseId = result.Description;
            }
            else
            {
                // If there's no corresponding test case id, do not report back to SpiraTeam
                return;
            }

            string testCaseName = result.Name;
            string message = result.Message;
            string stackTrace = result.StackTrace;
            int assertCount = result.AssertCount;
            DateTime startDate = DateTime.Now.AddSeconds(-result.Time);
            DateTime endDate = DateTime.Now;

            // Instantiate the web-service proxy class and set the URL from the text box
            bool success = false;
            SpiraImportExport.ImportExport spiraTestExecuteProxy = new SpiraImportExport.ImportExport();
            spiraTestExecuteProxy.Url = url;

            // Create a new cookie container to hold the session handle
            CookieContainer cookieContainer = new CookieContainer();
            spiraTestExecuteProxy.CookieContainer = cookieContainer;

            // Attempt to authenticate the user
            success = spiraTestExecuteProxy.Connection_Authenticate(login, password);
            if (!success)
            {
                throw new Exception("Cannot authenticate with SpiraTeam, check the URL, login and password");
            }

            // Now connect to the specified project
            success = spiraTestExecuteProxy.Connection_ConnectToProject(projectId);
            if (!success)
            {
                throw new Exception("Cannot connect to the specified project, check permissions of user!");
            }

            // Now actually record the test run itself
            SpiraImportExport.RemoteTestRun remoteTestRun = new SpiraImportExport.RemoteTestRun();
            remoteTestRun.TestCaseId = Convert.ToInt32(testCaseId);
            remoteTestRun.ReleaseId = releaseId;
            remoteTestRun.TestSetId = testSetId;
            remoteTestRun.StartDate = startDate;
            remoteTestRun.EndDate = endDate;
            remoteTestRun.ExecutionStatusId = (int)executionStatus;
            remoteTestRun.RunnerName = runnerName;
            remoteTestRun.RunnerTestName = testCaseName;
            remoteTestRun.RunnerAssertCount = assertCount;
            remoteTestRun.RunnerMessage = message;
            remoteTestRun.RunnerStackTrace = stackTrace;
            spiraTestExecuteProxy.TestRun_RecordAutomated1(remoteTestRun);

            // Close the SpiraTest connection
            spiraTestExecuteProxy.Connection_Disconnect();

            // Report result to CIP
            CIPReporter.RegressionTestServiceSoapClient soap = new CIPReporter.RegressionTestServiceSoapClient(
                "RegressionTestServiceSoap", 
                "http://10.119.33.66:8080/Services/RegressionTestService.asmx");

            soap.InsertTestResult(result.FullName, "Automation", result.IsSuccess, result.StackTrace, result.Time);
        }
    }
}
