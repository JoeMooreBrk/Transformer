namespace DotNetTransformer.Math.Group {
	public interface IFiniteGroupElement<T> : IGroupElement<T>
		where T : IFiniteGroupElement<T>, new()
	{
		int CycleLength { get; }
		T Subtract(T other);
	}
}
