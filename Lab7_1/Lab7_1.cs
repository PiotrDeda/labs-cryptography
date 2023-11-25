using System.Collections;

Dictionary<int, int> sBox = new() {
	{ 0, 14 },
	{ 1, 4 },
	{ 2, 13 },
	{ 3, 1 },
	{ 4, 2 },
	{ 5, 15 },
	{ 6, 11 },
	{ 7, 8 },
	{ 8, 3 },
	{ 9, 10 },
	{ 10, 6 },
	{ 11, 12 },
	{ 12, 5 },
	{ 13, 9 },
	{ 14, 0 },
	{ 15, 7 }
};

BitChain xprim = new("1011");
Dictionary<int, int> yprims = new();
for (int i = 0; i < 16; i++)
	yprims[i] = 0;

for (int i = 0; i < 16; i++)
{
	BitChain x = new(new BitBlock(i));
	BitChain xstar = x ^ xprim;
	BitChain y = SwitchBits(x, sBox);
	BitChain ystar = SwitchBits(xstar, sBox);
	BitChain yprim = y ^ ystar;
	yprims[yprim.Blocks[0]]++;
	Console.WriteLine($"{i:X}:{x}|{xstar}|{y}|{ystar}|{yprim}");
}

Console.WriteLine();

Console.WriteLine(string.Join(" ", yprims.Keys.Select(k => new BitChain(new BitBlock(k)))));
Console.WriteLine(string.Join(" ", yprims.Values.Select(v => v.ToString().PadLeft(4))));

//////////////////////////////

static BitChain SwitchBits(BitChain u, Dictionary<int, int> sBox)
{
	List<BitBlock> result = new();
	for (int i = 0; i < u.Blocks.Count; i++)
		result.Add(new(sBox[u.Blocks[i]]));
	return new(result);
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
	public BitChain(BitBlock block)
	{
		Blocks = new() { block };
	}

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
