namespace MouseAhead
{
	static class Tuple
	{
		public static Tuple<T1, T2> Make<T1, T2>(T1 _1, T2 _2)
		{
			return new Tuple<T1, T2>(_1, _2);
		}

		public static Tuple<T1, T2, T3> Make<T1, T2, T3>(T1 _1, T2 _2, T3 _3)
		{
			return new Tuple<T1, T2, T3>(_1, _2, _3);
		}
	}

	struct Tuple<T1, T2>
	{
		public T1 _1 { get; private set; }
		public T2 _2 { get; private set; }

		public Tuple(T1 _1, T2 _2) : this()
		{
			this._1 = _1;
			this._2 = _2;
		}
	}

	struct Tuple<T1, T2, T3>
	{
		public T1 _1 { get; private set; }
		public T2 _2 { get; private set; }
		public T3 _3 { get; private set; }

		public Tuple(T1 _1, T2 _2, T3 _3) : this()
		{
			this._1 = _1;
			this._2 = _2;
			this._3 = _3;
		}
	}
}