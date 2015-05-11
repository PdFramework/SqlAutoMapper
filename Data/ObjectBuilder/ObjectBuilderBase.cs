namespace PeinearyDevelopment.Framework.Data.ObjectBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	internal class ObjectBuilderBase
    {
        internal PropertyInfo[] ObjectProperties { get; set; }
        internal IDictionary<string, string> OutParameterMapper { get; set; }
        internal Func<object, object> ObjectKeyFunction { get; set; }
        internal Action<object, object> PropertySetterAction { get; set; }
    }
}
