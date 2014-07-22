namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class ObjectBuilderBase
    {
        internal PropertyInfo[] ObjectProperties { get; set; }
        internal Func<object[], object> ObjectConstructWithAssignments { get; set; }
        internal IDictionary<string, string> OutParameterMapper { get; set; }
        internal Dictionary<string, Func<object>> ReaderToObjectMapper { get; set; }
        internal IEnumerable<object> MappedProperties { get; set; }

        internal Func<object, object> ObjectKeyFunction { get; set; }
        internal Action<object, object> PropertySetterAction { get; set; }
    }
}
