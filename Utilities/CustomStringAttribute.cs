namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
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
            return GetCustomStrings(value)[0];
        }

        public static string[] GetCustomStrings(Enum value)
        {
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            CustomStringAttribute[] attrs = fi.GetCustomAttributes(typeof(CustomStringAttribute), false) as CustomStringAttribute[];
            string[] customStringValues = new string[attrs.Length];

            for (int cnt = 0; cnt < attrs.Length; cnt++)
            {
                customStringValues[cnt] = attrs[cnt].CustomStringValue;
            }

            return customStringValues;
        }
    }
}
