using System.Collections;

BitChain X1 = new("0000 0000 1111 1111");
BitChain X2 = new("0000 1111 0000 1111");
BitChain X3 = new("0011 0011 0011 0011");
BitChain X4 = new("0101 0101 0101 0101");

BitChain Y1 = new("1010 0111 0101 0100");
BitChain Y2 = new("1110 0100 0011 1001");
BitChain Y3 = new("1000 1110 1110 0001");
BitChain Y4 = new("0011 0110 1000 1101");

List<(BitChain X, BitChain Y)> xyPairs = new() { (X1, Y1), (X2, Y2), (X3, Y3), (X4, Y4) };

for (int i = 0; i < 16; i++)
{
	for (int j = 0; j < 16; j++)
		Console.Write($"{NL(new(i), new(j), xyPairs),2} ");
	Console.WriteLine();
}

Console.WriteLine();

for (int i = 0; i < 16; i++)
{
	for (int j = 0; j < 16; j++)
		Console.Write($"{NL(new(i), new(j), xyPairs) / 16.0 - 0.5:0.000}".PadLeft(6) + " ");
	Console.WriteLine();
}

//////////////////////////////

static int NL(BitBlock a, BitBlock b, List<(BitChain X, BitChain Y)> xyPairs)
{
	List<BitChain> activeValues = new();
	for (int i = 0; i < 4; i++)
		if (a.Bits[i])
			activeValues.Add(xyPairs[i].X);
	for (int i = 0; i < 4; i++)
		if (b.Bits[i])
			activeValues.Add(xyPairs[i].Y);

	if (activeValues.Count == 0)
		return 16;

	BitChain result = activeValues.Aggregate((accumulator, x) => accumulator ^ x);

	return result.Blocks.Sum(block => block.Bits.Count(bit => !bit));
}

class BitBlock
{
	public BitBlock(bool[] bits)
	{
		Bits = bits;
	}

	public BitBlock(int value)
	{
		SetDec(value);
	}

	public bool[] Bits { get; set; } = new bool[4];

	public void SetDec(int value)
	{
		var newBits = new BitArray(BitConverter.GetBytes(value));
		for (int i = 0; i < 4; i++)
			Bits[3 - i] = newBits[i];
	}

	public void SetHex(char value) => SetDec(Convert.ToInt32(value.ToString(), 16));

	public static implicit operator int(BitBlock b) => b.Bits.Aggregate(0, (acc, bit) => (acc << 1) | (bit ? 1 : 0));

	public override string ToString() => new(Bits.Take(4).Select(bit => bit ? '1' : '0').ToArray());
}

class BitChain
{
	public BitChain(List<BitBlock> blocks)
	{
		Blocks = blocks;
	}

	public BitChain(string binary)
	{
		Blocks.AddRange(binary.Split(' ').Select(block => new BitBlock(Convert.ToInt32(block, 2))));
	}

	public List<BitBlock> Blocks { get; set; } = new();

	public int Length() => Blocks.Sum(block => block.Bits.Length);

	public double Probability(bool valueBit) =>
		Blocks.Sum(block => block.Bits.Count(bit => bit == valueBit)) / (double)Length();

	public double Deviation(bool valueBit) => Probability(valueBit) - 0.5;

	public static BitChain operator ^(BitChain a, BitChain b)
	{
		List<BitBlock> result = new();
		for (int i = 0; i < a.Blocks.Count; i++)
		{
			bool[] bits = new bool[4];
			for (int j = 0; j < a.Blocks[i].Bits.Length; j++)
				bits[j] = a.Blocks[i].Bits[j] ^ b.Blocks[i].Bits[j];
			result.Add(new(bits));
		}

		return new(result);
	}

	public override string ToString() => string.Join(" ", Blocks.Select(block => block.ToString()));
}
