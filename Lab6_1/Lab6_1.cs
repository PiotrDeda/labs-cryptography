using System.Collections;

string x1 = "0000 0000 1111 1111";
string x2 = "0000 1111 0000 1111";
string x3 = "0011 0011 0011 0011";
string x4 = "0101 0101 0101 0101";

string y1 = "1010 0111 0101 0100";
string y2 = "1110 0100 0011 1001";
string y3 = "1000 1110 1110 0001";
string y4 = "0011 0110 1000 1101";

BitChain X1 = new(x1);
BitChain X2 = new(x2);
BitChain X3 = new(x3);
BitChain X4 = new(x4);

BitChain Y1 = new(y1);
BitChain Y2 = new(y2);
BitChain Y3 = new(y3);
BitChain Y4 = new(y4);

Console.WriteLine($"x1 = {X1}");
Console.WriteLine($"x2 = {X2}");
Console.WriteLine($"x3 = {X3}");
Console.WriteLine($"x4 = {X4}");
Console.WriteLine();
Console.WriteLine($"y1 = {Y1}");
Console.WriteLine($"y2 = {Y2}");
Console.WriteLine($"y3 = {Y3}");
Console.WriteLine($"y4 = {Y4}");
Console.WriteLine();

BitChain var1 = X1 ^ X4 ^ Y2;

Console.WriteLine($"X1 ⊕ X4 ⊕ Y2 = {var1}");
Console.WriteLine();

Console.WriteLine($"Pr[X1 ⊕ X4 ⊕ Y2 = 0] = {var1.Probability(false):N2}");
Console.WriteLine($"Pr[X1 ⊕ X4 ⊕ Y2 = 1] = {var1.Probability(true):N2}");
Console.WriteLine($"eps[X1 ⊕ X4 ⊕ Y2 = 0] = {var1.Deviation(false):N2}");
Console.WriteLine($"eps[X1 ⊕ X4 ⊕ Y2 = 1] = {var1.Deviation(true):N2}");
Console.WriteLine();

BitChain var2 = X3 ^ X4 ^ Y1 ^ Y4;

Console.WriteLine($"X3 ⊕ X4 ⊕ Y1 ⊕ Y4 = {var2}");
Console.WriteLine();

Console.WriteLine($"Pr[X3 ⊕ X4 ⊕ Y1 ⊕ Y4 = 0] = {var2.Probability(false)}");
Console.WriteLine($"Pr[X3 ⊕ X4 ⊕ Y1 ⊕ Y4 = 1] = {var2.Probability(true)}");
Console.WriteLine($"eps[X3 ⊕ X4 ⊕ Y1 ⊕ Y4 = 0] = {var2.Deviation(false)}");
Console.WriteLine($"eps[X3 ⊕ X4 ⊕ Y1 ⊕ Y4 = 1] = {var2.Deviation(true)}");

//////////////////////////////

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
