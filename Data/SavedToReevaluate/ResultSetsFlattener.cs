namespace PeinearyDevelopment.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ResultSetsFlattener
    {
        public static IEnumerable<TParent> Flatten<TParent, TChild, TCompare>(this IEnumerable<object>[] results, int parentResultSetIndex, Func<TParent, TCompare> parentIdentifier, Action<TParent, TChild> setMappedChildProperty, int childResultSetIndex, Func<TChild, TCompare> childMappingIdentifier)
        {
            var parents = new List<TParent>();
            var groupedChildren = results[childResultSetIndex].Select(result => (TChild)result).ToLookup(childMappingIdentifier);
            foreach (TParent parent in results[parentResultSetIndex])
            {
                if (groupedChildren.Contains(parentIdentifier(parent)))
                {
                    setMappedChildProperty(parent, groupedChildren[parentIdentifier(parent)].ToList().First());
                }

                parents.Add(parent);
            }

            return parents;
        }

        public static IEnumerable<TParent> Flatten<TParent, TChild, TCompare>(this IEnumerable<object>[] results, int parentResultSetIndex, Func<TParent, TCompare> parentIdentifier, Action<TParent, IEnumerable<TChild>> setMappedChildProperty, int childResultSetIndex, Func<TChild, TCompare> childMappingIdentifier)
        {
            var parents = new List<TParent>();
            var groupedChildren = results[childResultSetIndex].Select(result => (TChild)result).ToLookup(childMappingIdentifier);
            foreach (TParent parent in results[parentResultSetIndex])
            {
                if (groupedChildren.Contains(parentIdentifier(parent)))
                {
                    setMappedChildProperty(parent, groupedChildren[parentIdentifier(parent)].ToList());
                }

                parents.Add(parent);
            }

            return parents;
        }

        public static IEnumerable<TParent> Flatten<TParent, TChild, TCompare>(this IEnumerable<TParent> parents, Func<TParent, TCompare> parentIdentifier, Action<TParent, TChild> setMappedChildProperty, IEnumerable<object> childrenResultSet, Func<TChild, TCompare> childMappingIdentifier)
        {
            var groupedChildren = childrenResultSet.Select(result => (TChild)result).ToLookup(childMappingIdentifier);
            foreach (var parent in parents.Where(parent => groupedChildren.Contains(parentIdentifier(parent))))
            {
                setMappedChildProperty(parent, groupedChildren[parentIdentifier(parent)].ToList().First());
            }

            return parents;
        }

        public static IEnumerable<TParent> Flatten<TParent, TChild, TCompare>(this IEnumerable<TParent> parents, Func<TParent, TCompare> parentIdentifier, Action<TParent, IEnumerable<TChild>> setMappedChildProperty, IEnumerable<object> childrenResultSet, Func<TChild, TCompare> childMappingIdentifier)
        {
            var groupedChildren = childrenResultSet.Select(result => (TChild)result).ToLookup(childMappingIdentifier);
            foreach (var parent in parents.Where(parent => groupedChildren.Contains(parentIdentifier(parent))))
            {
                setMappedChildProperty(parent, groupedChildren[parentIdentifier(parent)].ToList());
            }

            return parents;
        }
    }
}
