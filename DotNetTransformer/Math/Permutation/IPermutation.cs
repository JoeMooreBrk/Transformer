using System;
using System.Collections.Generic;
using DotNetTransformer.Math.Group;

namespace DotNetTransformer.Math.Permutation {
	public interface IPermutation<T> : IFiniteGroupElement<T>
		, IEnumerable<int>
		where T : IPermutation<T>, new()
	{
		int this[int index] { get; }
		int SwapsCount { get; }

		T GetNextPermutation(int maxLength);
		T GetPreviousPermutation(int maxLength);

		List<T> GetCycles(Predicate<T> match);
		int GetCyclesCount(Predicate<int> match);

		int[] ToArray();
	}
}
