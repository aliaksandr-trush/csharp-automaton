namespace RegOnline.RegressionTest.Attributes
{
    using System;

    /// <summary>
    /// Marks a method as one that can be used
    /// to verify the expected behavior of a 
    /// test case
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class VerifyAttribute : Attribute
    {
    }
}
