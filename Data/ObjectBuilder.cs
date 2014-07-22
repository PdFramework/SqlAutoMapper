namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class ObjectBuilder<T> : ObjectBuilderBase
    {
        private Type ObjectType { get; set; }
        private Action<object, object>[] ObjectPropertySettingActions { get; set; }
        private Func<object> ObjectConstructor { get; set; }

        internal ObjectBuilder()
        {
            ObjectType = typeof(T);
            ObjectProperties = ObjectType.GetProperties();
            ObjectConstructor = Expression.Lambda<Func<object>>(Expression.New(ObjectType.GetConstructor(Type.EmptyTypes))).Compile();
            ObjectPropertySettingActions = ObjectProperties.Select(objectProperty => (Action<object, object>)(objectProperty.SetValue)).ToArray();
            ObjectConstructWithAssignments = (tObjPropertyValues) =>
            {
                var tObj = ObjectConstructor();

                for (var i = 0; i < tObjPropertyValues.Count(); i++)
                {
                    ObjectPropertySettingActions[i](tObj, tObjPropertyValues[i]);
                }

                return tObj;
            };

            OutParameterMapper = new Dictionary<string, string>();
            ReaderToObjectMapper = new Dictionary<string, Func<object>>();
        }
    }
}
