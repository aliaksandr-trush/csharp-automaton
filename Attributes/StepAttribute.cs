namespace RegOnline.RegressionTest.Attributes
{
    using System;

    /// <summary>
    /// Marks a method as one that can be used
    /// as a test step in a test case
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class StepAttribute : Attribute
    {
    }
}
