//	ISet.cs
//	
//	Based on :
//		Math
//			Set theory
//	
//	Author   : leofun01
//	Created  : 2018-07-02
//	Modified : 2018-07-15

using System;

namespace DotNetTransformer.Math.Set {
	// T is contravariant
	public interface ISet<in T>
		where T : IEquatable<T>
	{
		//bool IsCountable { get; }
		//bool IsFinite { get; }
		//bool IsEmpty { get; }
		bool Contains(T item);
	}
}
