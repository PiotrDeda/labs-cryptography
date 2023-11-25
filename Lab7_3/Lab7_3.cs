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
Dictionary<int, int> pBox = new() {
	{ 0, 0 },
	{ 1, 4 },
	{ 2, 8 },
	{ 3, 12 },
	{ 4, 1 },
	{ 5, 5 },
	{ 6, 9 },
	{ 7, 13 },
	{ 8, 2 },
	{ 9, 6 },
	{ 10, 10 },
	{ 11, 14 },
	{ 12, 3 },
	{ 13, 7 },
	{ 14, 11 },
	{ 15, 15 }
};
var reverseSBox = sBox.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
BitChain key = new("0011 1010 1001 0100 1101 0110 0011 1111");
BitChain xprim = new("0000 1011 0000 0000");

List<(BitChain, BitChain, BitChain, BitChain)> T = new(200);

Random random = new();
for (int i = 0; i < 500; i++)
{
	BitChain plaintext = new(string.Join(" ",
		Enumerable.Range(0, 4).Select(_ => string.Join("", Enumerable.Range(0, 4).Select(_ => random.Next(2))))));
	T.Add((plaintext, SPN(plaintext, sBox, pBox, key), plaintext ^ xprim, SPN(plaintext ^ xprim, sBox, pBox, key)));
}

(BitBlock, BitBlock) result = LinearAttack(T, reverseSBox);
Console.WriteLine($"L1max= {result.Item1.ToInt()}, L2max={result.Item2.ToInt()}");
Console.WriteLine($"K5(5-8 bits):{result.Item1}");
Console.WriteLine($"K5(13-16 bits):{result.Item2}");

//////////////////////////////

static (BitBlock, BitBlock) LinearAttack(List<(BitChain, BitChain, BitChain, BitChain)> T,
	Dictionary<int, int> reverseSBox)
{
	int[,] count = new int[16, 16];

	for (int L1 = 0; L1 < 16; L1++)
		for (int L2 = 0; L2 < 16; L2++)
			count[L1, L2] = 0;

	foreach ((BitChain x, BitChain y, BitChain xstar, BitChain ystar) in T)
		if (y.Blocks[0].Equals(ystar.Blocks[0]) && y.Blocks[2].Equals(ystar.Blocks[2]))
			for (int L1 = 0; L1 < 16; L1++)
				for (int L2 = 0; L2 < 16; L2++)
				{
					BitBlock v42 = new(L1 ^ y.Blocks[1]);
					BitBlock v44 = new(L2 ^ y.Blocks[3]);
					BitBlock u42 = new(reverseSBox[v42]);
					BitBlock u44 = new(reverseSBox[v44]);
					BitBlock v42star = new(L1 ^ ystar.Blocks[1]);
					BitBlock v44star = new(L2 ^ ystar.Blocks[3]);
					BitBlock u42star = new(reverseSBox[v42star]);
					BitBlock u44star = new(reverseSBox[v44star]);
					BitBlock u42prim = new(u42 ^ u42star);
					BitBlock u44prim = new(u44 ^ u44star);
					if (u42prim.Equals(new(6)) && u44prim.Equals(new(6)))
						count[L1, L2]++;
				}

	int max = -1;
	(int, int) maxkey = (-1, -1);
	for (int L1 = 0; L1 < 16; L1++)
		for (int L2 = 0; L2 < 16; L2++)
			if (count[L1, L2] > max)
			{
				max = count[L1, L2];
				maxkey = (L1, L2);
			}

	return (new(maxkey.Item1), new(maxkey.Item2));
}

static BitChain SPN(BitChain x, Dictionary<int, int> sBox, Dictionary<int, int> pBox, BitChain key)
{
	var keys = DivideKeys(x, key);

	for (int r = 0; r < keys.Count - 2; r++)
	{
		BitChain u = x ^ keys[r];
		BitChain v = SwitchBits(u, sBox);
		BitChain w = PermuteBits(v, pBox);
		x = w;
	}

	BitChain u4 = x ^ keys[3];
	BitChain v4 = SwitchBits(u4, sBox);
	x = v4;

	return x ^ keys[4];
}

static List<BitChain> DivideKeys(BitChain x, BitChain key)
{
	List<BitChain> dividedKeys = new();

	for (int i = 0; i <= key.Blocks.Count - x.Blocks.Count; i++)
		dividedKeys.Add(new(key.Blocks.GetRange(i, x.Blocks.Count)));

	return dividedKeys;
}

static BitChain SwitchBits(BitChain u, Dictionary<int, int> sBox)
{
	List<BitBlock> result = new();
	for (int i = 0; i < u.Blocks.Count; i++)
		result.Add(new(sBox[u.Blocks[i]]));
	return new(result);
}

static BitChain PermuteBits(BitChain v, Dictionary<int, int> pBox)
{
	List<BitBlock> result = new();
	bool[] bits = new bool[v.Blocks.Count * 4];
	for (int i = 0; i < v.Blocks.Count; i++)
		for (int j = 0; j < 4; j++)
			bits[pBox[i * 4 + j]] = v.Blocks[i].Bits[j];
	for (int i = 0; i < v.Blocks.Count; i++)
		result.Add(new(new[] { bits[i * 4], bits[i * 4 + 1], bits[i * 4 + 2], bits[i * 4 + 3] }));
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

	public bool Equals(BitBlock other) => Bits.SequenceEqual(other.Bits);

	public int ToInt() => Bits.Aggregate(0, (acc, bit) => (acc << 1) | (bit ? 1 : 0));

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
