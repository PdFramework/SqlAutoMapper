namespace PeinearyDevelopment.Framework.Data.ObjectBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	internal class TypedObjectBuilderBase<T> : ObjectBuilderBase
	{
		    internal Type ObjectType { get; set; }
				internal Action<object, object>[] ObjectPropertySettingActions { get; set; }
				internal Func<object> ObjectConstructor { get; set; }

				internal TypedObjectBuilderBase()
        {
            ObjectType = typeof(T);
            ObjectProperties = ObjectType.GetProperties();
            ObjectConstructor = Expression.Lambda<Func<object>>(Expression.New(ObjectType.GetConstructor(Type.EmptyTypes))).Compile();
            ObjectPropertySettingActions = ObjectProperties.Select(objectProperty => (Action<object, object>)(objectProperty.SetValue)).ToArray();
            OutParameterMapper = new Dictionary<string, string>();
        }
	}
}
