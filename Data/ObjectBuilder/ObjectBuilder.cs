namespace PeinearyDevelopment.Framework.Data.ObjectBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	internal class ObjectBuilder<T> : TypedObjectBuilderBase<T>
    {
			internal Dictionary<string, Func<object>> ReaderToObjectMapper { get; set; }
			internal virtual Func<object[], object> ObjectConstructWithAssignments { get; set; }
			internal IEnumerable<object> MappedProperties { get; set; }
			
			internal ObjectBuilder()
        {
            ObjectConstructWithAssignments = (tObjPropertyValues) =>
            {
                var tObj = ObjectConstructor();

                for (var i = 0; i < tObjPropertyValues.Count(); i++)
                {
                    ObjectPropertySettingActions[i](tObj, tObjPropertyValues[i]);
                }

                return tObj;
            };

            ReaderToObjectMapper = new Dictionary<string, Func<object>>();
        }
    }
}
