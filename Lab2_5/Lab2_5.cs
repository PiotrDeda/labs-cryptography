using System.Text;

const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

static int AlphabetModulo(int a) => (a % Alphabet.Length + Alphabet.Length) % Alphabet.Length;

static int LetterToNumber(char letter) => Alphabet.IndexOf(char.ToUpper(letter));

static char NumberToLetter(int number) => Alphabet[AlphabetModulo(number)];

static string RandomString(int length, Random random)
{
	var sb = new StringBuilder();
	for (int i = 0; i < length; i++)
		sb.Append(Alphabet[random.Next(Alphabet.Length)]);
	return sb.ToString().ToLower();
}

static string EncryptTranspositionCipher(string text, int key)
{
	StringBuilder sb = new();
	foreach (char c in text)
		sb.Append(NumberToLetter(LetterToNumber(c) + key));
	return sb.ToString().ToLower();
}

static string DecryptTranspositionCipher(string text, int key)
{
	StringBuilder sb = new();
	foreach (char c in text)
		sb.Append(NumberToLetter(LetterToNumber(c) - key));
	return sb.ToString().ToLower();
}

static (int key, string decrypted)[] FindTranspositionCipherKey(string cipher)
{
	var results = new (int key, string decrypted)[26];
	for (int i = 0; i < 26; ++i)
		results[i] = (i, DecryptTranspositionCipher(cipher, i));
	return results;
}

int sum = 0;

Random random = new();

for (int i = 0; i < 1000; i++)
{
	string randomText = RandomString(100, random);
	string encrypted = EncryptTranspositionCipher(randomText, random.Next(26));
	var results = FindTranspositionCipherKey(encrypted);
	foreach ((int key, string decrypted) in results)
		if (decrypted == randomText)
		{
			sum += key;
			break;
		}
}

Console.WriteLine($"Average: {sum / 1000.0}");
