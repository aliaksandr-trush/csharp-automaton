namespace RegOnline.RegressionTest.Attributes
{
    using System;

    /// <summary>
    /// Allows addition of a description to
    /// explain the purpose of a test case,
    /// test step, or test verification
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initialize the attribute
        /// </summary>
        /// <param name="text">Text of the description</param>
        public DescriptionAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the text of the description
        /// </summary>
        public string Text
        {
            get;
            private set;
        }
    }
}
