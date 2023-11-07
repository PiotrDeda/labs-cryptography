KeyGenerator keygenA = new((0, 1, 0, 0), (1, 1, 1, 1));
KeyGenerator keygenB = new((1, 0, 0, 0), (1, 0, 0, 1));

Console.WriteLine($"wektor:{KeyToString(keygenA.Key)}");
Console.Write("strumien klucza:");
for (int i = 0; i < 54; i++)
	Console.Write(keygenA.Next());
Console.WriteLine("\n");

Console.WriteLine($"wektor:{KeyToString(keygenB.Key)}");
Console.Write("strumien klucza:");
for (int i = 0; i < 54; i++)
	Console.Write(keygenB.Next());
Console.WriteLine("\n");

for ((int k1, int k2, int k3, int k4) key = (0, 0, 0, 0); key != (-1, 0, 0, 0); key = IncrementKey(key))
{
	KeyGenerator keygen = new(key, (1, 1, 1, 1));
	(int f1, int f2, int f3, int f4) frame = (keygen.Next(), keygen.Next(), keygen.Next(), keygen.Next());
	int count = 0;
	while (true)
	{
		count++;
		frame = (frame.f2, frame.f3, frame.f4, keygen.Next());
		if (frame == key)
		{
			Console.WriteLine($"P({KeyToString(key)}) = {count}");
			break;
		}
	}
}

Console.WriteLine();

for ((int k1, int k2, int k3, int k4) key = (0, 0, 0, 0); key != (-1, 0, 0, 0); key = IncrementKey(key))
{
	KeyGenerator keygen = new(key, (1, 0, 0, 1));
	(int f1, int f2, int f3, int f4) frame = (keygen.Next(), keygen.Next(), keygen.Next(), keygen.Next());
	int count = 0;
	while (true)
	{
		count++;
		frame = (frame.f2, frame.f3, frame.f4, keygen.Next());
		if (frame == key)
		{
			Console.WriteLine($"P({KeyToString(key)}) = {count}");
			break;
		}
	}
}

//////////////////////////////


static (int, int, int, int) IncrementKey((int k1, int k2, int k3, int k4) key)
{
	string keyString = KeyToString(key);
	int keyInt = Convert.ToInt32(keyString, 2);
	keyInt++;
	keyString = Convert.ToString(keyInt, 2);
	if (keyString.Length < 4)
		keyString = keyString.PadLeft(4, '0');
	if (keyString.Length > 4)
		return (-1, 0, 0, 0);
	return StringToKey(keyString);
}

static string KeyToString((int k1, int k2, int k3, int k4) key) => $"{key.k1}{key.k2}{key.k3}{key.k4}";

static (int, int, int, int) StringToKey(string key) => (key[0] - '0', key[1] - '0', key[2] - '0', key[3] - '0');

class KeyGenerator
{
	public KeyGenerator((int, int, int, int) startingKey, (int, int, int, int) consts)
	{
		Key = startingKey;
		Consts = consts;
	}

	public (int k1, int k2, int k3, int k4) Key { get; set; }
	public (int c1, int c2, int c3, int c4) Consts { get; set; }

	public int Next()
	{
		int k = Key.k1;
		Key = (Key.k2, Key.k3, Key.k4,
			(Key.k1 * Consts.c1 + Key.k2 * Consts.c2 + Key.k3 * Consts.c3 + Key.k4 * Consts.c4) % 2);
		return k;
	}
}
