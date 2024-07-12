namespace LitJson
{
	internal enum ParserToken
	{
		None = 65536,
		Number = 65537,
		True = 65538,
		False = 65539,
		Null = 65540,
		CharSeq = 65541,
		Char = 65542,
		Text = 65543,
		Object = 65544,
		ObjectPrime = 65545,
		Pair = 65546,
		Colon = 65547,
		ColonRest = 65548,
		PairRest = 65549,
		Array = 65550,
		ArrayPrime = 65551,
		Value = 65552,
		ValueRest = 65553,
		String = 65554,
		End = 65555,
		Epsilon = 65556
	}
}
