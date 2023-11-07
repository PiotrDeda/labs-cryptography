List<(string, string)> samples = new() {
	("Attack at dawn", "Secret"),
	("Plaintext", "Key"),
	("pedia", "Wiki")
};

foreach ((string plaintext, string key) in samples)
{
	int[] plaintextBytes = plaintext.Select(c => (int)c).ToArray();
	int[] keyBytes = key.Select(c => (int)c).ToArray();
	int[] ciphertextBytes = new int[plaintextBytes.Length];
	int[] keystreamBytes = new int[plaintextBytes.Length];

	StreamGenerator streamGenerator = new(keyBytes);
	for (int i = 0; i < plaintextBytes.Length; i++)
		(ciphertextBytes[i], keystreamBytes[i]) = streamGenerator.Next(plaintextBytes[i]);

	Console.Write("Keystream: ");
	foreach (int c in keystreamBytes)
		Console.Write($"{c:X2}");
	Console.WriteLine();
	Console.WriteLine($"Plaintext: {plaintext}");
	Console.WriteLine($"Key: {key}");
	Console.Write("Ciphertext: ");
	foreach (int c in ciphertextBytes)
		Console.Write($"{c:X2}");
	Console.WriteLine("\n");
}

//////////////////////////////

class StreamGenerator
{
	public StreamGenerator(int[] key)
	{
		for (int i = 0; i < 256; i++)
			S[i] = i;

		int j = 0;
		for (int i = 0; i < 256; i++)
		{
			j = (j + S[i] + key[i % key.Length]) % 256;
			(S[j], S[i]) = (S[i], S[j]);
		}
	}

	int I { get; set; }
	int J { get; set; }
	int[] S { get; } = new int[256];

	public (int cipher, int byteKey) Next(int c)
	{
		I = (I + 1) % 256;
		J = (J + S[I]) % 256;
		(S[I], S[J]) = (S[J], S[I]);
		int byteKey = S[(S[I] + S[J]) % 256];
		return (c ^ byteKey, byteKey);
	}
}
