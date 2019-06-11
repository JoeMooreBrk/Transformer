using System;

namespace DotNetTransformer.Math.Group {
	public interface IGroupElement<T> : IEquatable<T>
		where T : IGroupElement<T>
	{
		T InverseElement { get; }
		T Add(T other);
		T Times(int count);
	}
}
