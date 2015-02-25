namespace PeinearyDevelopment.Framework.Data.ObjectBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	internal class AsyncObjectBuilder<T> : TypedObjectBuilderBase<T>
    {
			internal IEnumerable<Task<object>> MappedProperties { get; set; }
			internal Dictionary<string, Func<Task<object>>> ReaderToObjectMapper { get; set; }
			internal Func<Task<object>[], Task<object>> ObjectConstructWithAssignments { get; set; }
			
			internal AsyncObjectBuilder()
        {
						ObjectConstructWithAssignments = async (tObjPropertyValues) =>
            {
                var tObj = ObjectConstructor();

                for (var i = 0; i < tObjPropertyValues.Count(); i++)
                {
									ObjectPropertySettingActions[i](tObj, await tObjPropertyValues[i]);
                }

                return tObj;
            };

						ReaderToObjectMapper = new Dictionary<string, Func<Task<object>>>();
        }
    }
}
