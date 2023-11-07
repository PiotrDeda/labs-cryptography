using System.Collections;

string keyString = "0011 1010 1001 0100 1101 0110 0011 1111";
string plaintextString = "0010 0110 1011 0111";
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

BitChain key = new(keyString);
BitChain plaintext = new(plaintextString);

Console.WriteLine($"Key: {key}");
Console.WriteLine();
Console.WriteLine($"x : {plaintext}");
Console.WriteLine();

Console.WriteLine($"y : {SPN(plaintext, sBox, pBox, key)}");

//////////////////////////////

static BitChain SPN(BitChain x, Dictionary<int, int> sBox, Dictionary<int, int> pBox, BitChain key)
{
	var keys = DivideKeys(x, key);

	Console.WriteLine($"w0: {x}");
	Console.WriteLine();

	for (int r = 0; r < keys.Count - 2; r++)
	{
		Console.WriteLine($"K{r + 1}: {keys[r]}");
		BitChain u = x ^ keys[r];
		Console.WriteLine($"u{r + 1}: {u}");
		BitChain v = SwitchBits(u, sBox);
		Console.WriteLine($"v{r + 1}: {v}");
		BitChain w = PermuteBits(v, pBox);
		Console.WriteLine($"w{r + 1}: {w}");
		Console.WriteLine();
		x = w;
	}

	Console.WriteLine($"K4: {keys[3]}");
	BitChain u4 = x ^ keys[3];
	Console.WriteLine($"u4: {u4}");
	BitChain v4 = SwitchBits(u4, sBox);
	Console.WriteLine($"v4: {v4}");
	x = v4;

	Console.WriteLine($"K5: {keys[4]}");
	Console.WriteLine();

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
