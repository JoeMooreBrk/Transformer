using System;
using System.Collections.Generic;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public static class FiniteGroupExtension
	{
		private sealed class InternalGroup<T> : FiniteGroup<T>
			where T : IFiniteGroupElement<T>, new()
		{
			private readonly T _identity;
			private readonly FiniteSet<T> _collection;

			internal InternalGroup(FiniteSet<T> collection) {
				_identity = new T();
				_collection = collection;
			}

			public override T IdentityElement { get { return _identity; } }
			public override sealed long Count { get { return _collection.Count; } }
			public override sealed bool Contains(T item) {
				return _collection.Contains(item);
			}
			public override sealed IEnumerator<T> GetEnumerator() {
				return _collection.GetEnumerator();
			}
			public override sealed bool Equals(FiniteSet<T> other) {
				return IsMatch<FiniteSet<T>>(other, base.Equals);
			}
			public override sealed int GetHashCode() {
				return _collection.GetHashCode();
			}
			public override sealed bool IsSubsetOf(ISet<T> other) {
				return IsMatch<ISet<T>>(other, base.IsSubsetOf);
			}
			public override sealed bool IsSubsetOf(FiniteSet<T> other) {
				return IsMatch<FiniteSet<T>>(other, base.IsSubsetOf);
			}
			public override sealed bool IsSupersetOf(FiniteSet<T> other) {
				return IsMatch<FiniteSet<T>>(other, base.IsSupersetOf);
			}
			private bool IsMatch<TSet>(TSet other, Predicate<TSet> match)
				where TSet : ISet<T>
			{
				InternalGroup<T> o = other as InternalGroup<T>;
				return ReferenceEquals(_collection, other)
					|| !ReferenceEquals(o, null) && ReferenceEquals(_collection, o._collection)
					|| match(other);
			}
		}

		internal static FiniteGroup<T> ToFiniteGroup<T>(this FiniteSet<T> collection)
			where T : IFiniteGroupElement<T>, new()
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalGroup<T>(collection);
		}
		internal static FiniteGroup<T> ToFiniteGroup<T>(this ICollection<T> collection)
			where T : IFiniteGroupElement<T>, new()
		{
			return ToFiniteGroup<T>(collection.ToFiniteSet<T>());
		}
		internal static FiniteGroup<T> ToFiniteGroup<T>(this IEnumerable<T> collection,
			long count, Predicate<T> contains
		)
			where T : IFiniteGroupElement<T>, new()
		{
			return ToFiniteGroup<T>(collection.ToFiniteSet<T>(count, contains));
		}

		public static FiniteGroup<T> CreateGroup<T>(this IEnumerable<T> collection)
			where T : IFiniteGroupElement<T>, new()
		{
			List<T> list = new List<T>();
			list.Add(new T());
			if(!ReferenceEquals(collection, null)) {
				foreach(T a in collection)
					if(!list.Contains(a)) list.Add(a);
				int i = 1, count;
				do {
					for(count = list.Count; i < count; ++i) {
						T a = list[i];
						for(int j = 1; j < count; ++j) {
							T b = list[j], c;
							if(!list.Contains(c = a.Add(b))) list.Add(c);
							if(!list.Contains(c = b.Add(a))) list.Add(c);
						}
					}
				} while(count < list.Count);
			}
			FiniteGroup<T> group = ToFiniteGroup<T>(list);
			list[0] = group.IdentityElement;
			return group;
		}
		public static bool IsGeneratingSetOf<T>(this IEnumerable<T> collection, FiniteGroup<T> group)
			where T : IFiniteGroupElement<T>, new()
		{
			return !ReferenceEquals(collection, null)
				&& !ReferenceEquals(group, null)
				&& group.IsSubsetOf(CreateGroup<T>(collection));
		}

		public static int GetLengthTo<T>(this T t, T o)
			where T : IFiniteGroupElement<T>, new()
		{
			int cLen = 1;
			T sum = t;
			while(!sum.Equals(o)) {
				sum = sum.Add(t);
				++cLen;
			}
			return cLen;
		}
		public static T Times<T>(this T t, int count)
			where T : IFiniteGroupElement<T>, new()
		{
			int c = t.CycleLength;
			count = (count % c + c) % c;
			return GroupExtension.Times<T>(t, count);
		}
	}
}
