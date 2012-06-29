namespace RegOnline.RegressionTest.Utilities
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public abstract class VerifyTool
    {
        public VerifyTool() { }

        public static bool AreListContentsEqual<T>(List<T> list1, List<T> list2)
        {
            bool areEqual = false;

            if (list1.Count == list2.Count)
            {
                areEqual = true;
                int i = 0;
                while ((i < list1.Count) && (areEqual == true))
                {
                    areEqual = list1[i].Equals(list2[i]);
                    i++;
                }
            }

            return areEqual;
        }

        public static void VerifyList<T>(List<T> list1, List<T> list2)
        {
            Assert.AreEqual(list1.Count, list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Assert.AreEqual(list1[i], list2[i]);
            }
        }

        public static void VerifyValue(string expectedValue, string actualValue, string messageFormat)
        {
            if (expectedValue == null || actualValue == null)
                return;

            string mssgExpected = string.Format(messageFormat, expectedValue);
            string mssgActual = string.Format(messageFormat, actualValue);

            Assert.AreEqual(mssgExpected, mssgActual);
        }

        public static void VerifyValue(bool? expectedValue, bool? actualValue, string messageFormat)
        {
            VerifyValue(expectedValue.ToString(), actualValue.ToString(), messageFormat);
        }

        public static void VerifyValue(int? expectedValue, int? actualValue, string messageFormat)
        {
            VerifyValue(expectedValue.ToString(), actualValue.ToString(), messageFormat);
        }

        public static void VerifyValue(double? expectedValue, double? actualValue, string messageFormat)
        {
            VerifyValue(expectedValue.ToString(), actualValue.ToString(), messageFormat);
        }
    }
}
