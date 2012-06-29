namespace RegOnline.RegressionTest.Attributes
{
    using System;

    /// <summary>
    /// Marks a method as one that is a test case
    /// which should be run manually; replace this 
    /// attribute with the NUnit [Test] attribute 
    /// when the test case becomes automated
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ManualAttribute : Attribute
    {
    }
}
