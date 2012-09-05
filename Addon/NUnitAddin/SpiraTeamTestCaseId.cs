namespace RegOnline.RegressionTest.NUnitAddin
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SpiraTeamTestCaseIdAttribute : Attribute
    {
        public const string AttributeFullName = "RegOnline.RegressionTest.NUnitAddin.SpiraTeamTestCaseIdAttribute";

        public int TestCaseId { get; set; }

        public SpiraTeamTestCaseIdAttribute(int testCaseId)
        {
            this.TestCaseId = testCaseId;
        }

        public static int GetTestCaseId(MemberInfo method)
        {
            SpiraTeamTestCaseIdAttribute[] attrs = method.GetCustomAttributes(typeof(SpiraTeamTestCaseIdAttribute), false) as SpiraTeamTestCaseIdAttribute[];
            return attrs[0].TestCaseId;
        }
    }
}