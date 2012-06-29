namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using System.Reflection;

    public class CustomStringAttribute : Attribute
    {
        public CustomStringAttribute(string value)
        {
            this.CustomStringValue = value;
        }

        public string CustomStringValue
        {
            get;
            set;
        }

        public static string GetCustomString(Enum value)
        {
            string customStringValue = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            CustomStringAttribute[] attrs = fi.GetCustomAttributes(typeof(CustomStringAttribute), false) as CustomStringAttribute[];
            if (attrs.Length > 0)
                customStringValue = attrs[0].CustomStringValue;

            return customStringValue;
        }
    }
}
