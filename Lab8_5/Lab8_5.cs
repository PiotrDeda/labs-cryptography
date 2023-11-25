using System.Collections;
using System.Security.Cryptography;
using System.Text;

string[] texts = {
	"The quick brown fox jumps over the lazy dog",
	"The quick brown fox jumps over the lazy cog",
	""
};

foreach (string text in texts)
{
	Console.WriteLine($"Text: {text}");
	Console.WriteLine($"SHA1 built-in: {SHA1BuiltIn(text)}");
	Console.WriteLine($"SHA1 custom:   {SHA1Custom(text)}");
	Console.WriteLine();
}

//////////////////////////////

static string SHA1BuiltIn(string str)
{
	using var sha1 = SHA1.Create();
	return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
}

static string SHA1Custom(string str)
{
	var bits = str.ToBitArray();
	int initialLength = bits.Length;

	BitArray h0 = new BitArray(BitConverter.GetBytes(0x67452301)).ReverseBits();
	BitArray h1 = new BitArray(BitConverter.GetBytes(0xEFCDAB89)).ReverseBits();
	BitArray h2 = new BitArray(BitConverter.GetBytes(0x98BADCFE)).ReverseBits();
	BitArray h3 = new BitArray(BitConverter.GetBytes(0x10325476)).ReverseBits();
	BitArray h4 = new BitArray(BitConverter.GetBytes(0xC3D2E1F0)).ReverseBits();

	bits.Length += 1;
	bits[bits.Length - 1] = true;

	while (bits.Length % 512 != 480) // 480 instead of 448 because the length of a BitArray is just 32-bits in C#
	{
		bits.Length += 1;
		bits[bits.Length - 1] = false;
	}

	BitArray initialLengthBits = new BitArray(BitConverter.GetBytes(initialLength)).ReverseBits();
	for (int i = 0; i < 32; i++)
	{
		bits.Length += 1;
		bits[bits.Length - 1] = initialLengthBits[i];
	}

	List<List<BitArray>> blocks = new();
	for (int i = 0; i < bits.Length; i += 512)
	{
		blocks.Add(new());
		for (int j = 0; j < 512; j += 32)
		{
			blocks[^1].Add(new(32));
			for (int k = 0; k < 32; k++)
				blocks[^1][^1][k] = bits[i + j + k];
		}
	}

	foreach (var block in blocks)
		for (int i = 16; i < 80; i++)
			block.Add(block[i - 3].CloneBA().Xor(block[i - 8]).Xor(block[i - 14]).Xor(block[i - 16]).LeftRotate(1));

	foreach (var block in blocks)
	{
		BitArray a = h0.CloneBA();
		BitArray b = h1.CloneBA();
		BitArray c = h2.CloneBA();
		BitArray d = h3.CloneBA();
		BitArray e = h4.CloneBA();

		for (int i = 0; i < 80; i++)
		{
			BitArray f = b.CloneBA();
			BitArray k;
			if (i < 20)
			{
				f.And(c).Or(b.CloneBA().Not().And(d));
				k = new BitArray(BitConverter.GetBytes(0x5A827999)).ReverseBits();
			}
			else if (i < 40)
			{
				f.Xor(c).Xor(d);
				k = new BitArray(BitConverter.GetBytes(0x6ED9EBA1)).ReverseBits();
			}
			else if (i < 60)
			{
				f.And(c).Or(b.CloneBA().And(d)).Or(c.CloneBA().And(d));
				k = new BitArray(BitConverter.GetBytes(0x8F1BBCDC)).ReverseBits();
			}
			else
			{
				f.Xor(c).Xor(d);
				k = new BitArray(BitConverter.GetBytes(0xCA62C1D6)).ReverseBits();
			}

			BitArray temp = a.CloneBA().LeftRotate(5).Add(f).Add(e).Add(k).Add(block[i]);
			e = d;
			d = c;
			c = b.CloneBA().LeftRotate(30);
			b = a;
			a = temp;
		}

		h0 = h0.Add(a);
		h1 = h1.Add(b);
		h2 = h2.Add(c);
		h3 = h3.Add(d);
		h4 = h4.Add(e);
	}

	return h0.Concat(h1).Concat(h2).Concat(h3).Concat(h4).ToHexRepresentation();
}

static class ExtensionMethods
{
	public static BitArray CloneBA(this BitArray bits) => (BitArray)bits.Clone();

	public static BitArray Add(this BitArray bits, BitArray other)
	{
		int x = bits.ToInt();
		int y = other.ToInt();
		int z = (x + y) % (int)Math.Pow(2, bits.Length);
		return new BitArray(BitConverter.GetBytes(z)).ReverseBits();
	}

	public static BitArray Concat(this BitArray bits, BitArray other)
	{
		BitArray result = new(bits.Length + other.Length);
		for (int i = 0; i < bits.Length; i++)
			result[i] = bits[i];
		for (int i = 0; i < other.Length; i++)
			result[bits.Length + i] = other[i];
		return result;
	}

	public static BitArray ReverseBits(this BitArray bits)
	{
		var result = new BitArray(bits.Length);
		for (int i = 0; i < bits.Length; i++)
			result[i] = bits[bits.Length - i - 1];
		return result;
	}

	public static BitArray ReverseBytes(this BitArray bits)
	{
		var result = new BitArray(bits.Length);
		for (int i = 0; i < bits.Length; i += 8)
			for (int j = 0; j < 8; j++)
				result[i + j] = bits[bits.Length - 8 - i + j];
		return result;
	}

	public static BitArray LeftRotate(this BitArray bits, int n) =>
		bits.CloneBA().RightShift(n).Or(bits.CloneBA().LeftShift(bits.Length - n));

	public static int ToInt(this BitArray bits)
	{
		int[] result = new int[1];
		bits.CloneBA().ReverseBits().CopyTo(result, 0);
		return result[0];
	}

	public static string ToHexRepresentation(this BitArray bits)
	{
		StringBuilder sb = new();

		for (int i = 0; i < bits.Length; i += 4)
		{
			int v = 0;
			for (int j = 0; j < 4; j++)
			{
				v <<= 1;
				if (bits[i + j])
					v |= 1;
			}

			sb.Append(v.ToString("X"));
		}

		return sb.ToString().ToLower();
	}

	public static BitArray ToBitArray(this string str) =>
		new BitArray(Encoding.UTF8.GetBytes(str)).ReverseBits().ReverseBytes();
}
