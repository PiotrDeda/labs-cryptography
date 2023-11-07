KeyGenerator keygen = new(new(1, 1, 0, 1, 0), new(1, 0, 0, 1, 0));
string plaintext = "011001111111000";

int[] keyStream = new int[plaintext.Length];
int[] ciphertext = new int[plaintext.Length];
for (int i = 0; i < plaintext.Length; i++)
{
	keyStream[i] = keygen.Next();
	ciphertext[i] = EncryptBit(plaintext[i] - '0', keyStream[i]);
}

Console.WriteLine("Encryption");
Console.WriteLine($"Plaintext: {plaintext}");
Console.WriteLine($"Key stream:{string.Join("", keyStream)}");
Console.WriteLine($"Ciphertext:{string.Join("", ciphertext)}");
Console.WriteLine();

int[] decrypted = new int[plaintext.Length];
for (int i = 0; i < ciphertext.Length; i++)
	decrypted[i] = DecryptBit(ciphertext[i], keyStream[i]);

Console.WriteLine("Decryption");
Console.WriteLine($"Ciphertext:{string.Join("", ciphertext)}");
Console.WriteLine($"Key stream:{string.Join("", keyStream)}");
Console.WriteLine($"Plaintext: {string.Join("", decrypted)}");

//////////////////////////////

static int EncryptBit(int value, int key) => ((value + key) % 2 + 2) % 2;

static int DecryptBit(int value, int key) => ((value - key) % 2 + 2) % 2;

class KeySeq
{
	public int k1, k2, k3, k4, k5;

	public KeySeq(int k1, int k2, int k3, int k4, int k5)
	{
		this.k1 = k1;
		this.k2 = k2;
		this.k3 = k3;
		this.k4 = k4;
		this.k5 = k5;
	}

	public void Increment()
	{
		string keyString = ToString();
		int keyInt = Convert.ToInt32(keyString, 2);
		keyInt++;
		keyString = Convert.ToString(keyInt, 2);
		if (keyString.Length < 5)
			keyString = keyString.PadLeft(5, '0');
		if (keyString.Length > 5)
		{
			k1 = -1;
			k2 = 0;
			k3 = 0;
			k4 = 0;
			k5 = 0;
		}
	}

	public override string ToString() => $"{k1}{k2}{k3}{k4}{k5}";

	public static KeySeq FromString(string key) =>
		new(key[0] - '0', key[1] - '0', key[2] - '0', key[3] - '0', key[4] - '0');
}

class KeyGenerator
{
	public KeyGenerator(KeySeq startingKey, KeySeq consts)
	{
		Key = startingKey;
		Consts = consts;
	}

	public KeySeq Key { get; set; }
	public KeySeq Consts { get; set; }

	public int Next()
	{
		int k = Key.k1;
		Key = new(Key.k2, Key.k3, Key.k4, Key.k5,
			(Key.k1 * Consts.k1 + Key.k2 * Consts.k2 + Key.k3 * Consts.k3 + Key.k4 * Consts.k4 + Key.k5 * Consts.k5) % 2
		);
		return k;
	}
}
