namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using System.Collections.Generic;

    public sealed class EnumValueSet<TKey, TValue> 
    {
        private Dictionary<TKey, TValue> myEnumValues = new Dictionary<TKey,TValue>();

        private int _count;
        public int Count
        {
            get { return _count; }
        }

        private Type _valueType;
        public Type ValueType
        {
            get { return _valueType; }
        }

        private TKey[] _keys;
        public TKey[] Keys
        {
            get { return _keys; }
        }

        public EnumValueSet(TValue defaultValue) 
        {
            string[] enumNames = Enum.GetNames(typeof(TKey));
            foreach (string enumName in enumNames)
            {
                myEnumValues.Add((TKey)Enum.Parse(typeof(TKey), enumName),defaultValue);
            }
            _count = myEnumValues.Count;
            _valueType = typeof(TValue);
            _keys = new TKey[_count];
            myEnumValues.Keys.CopyTo(_keys, 0);
        }

        public void SetValue(TKey enumField, TValue setValue)
        {
            if (myEnumValues.ContainsKey(enumField))
            {
                myEnumValues.Remove(enumField);
                myEnumValues.Add(enumField, setValue);
            }
            else
            {
                throw new System.ArgumentException("Invalid key value");
            }
        }

        public TValue GetValue(TKey enumField)
        {
            TValue getValue;

            if (!myEnumValues.TryGetValue(enumField, out getValue))
            {
                throw new System.ArgumentException("Invalid key value");
            }

            return getValue;
        }
    }
}
