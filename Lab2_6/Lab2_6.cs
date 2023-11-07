using System.Text;

const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
const string CommonLetters = "ETAOINSHRDLCUMWFGYPBVKJXQZ";

static int AlphabetModulo(int a) => (a % Alphabet.Length + Alphabet.Length) % Alphabet.Length;

static int LetterToNumber(char letter) => Alphabet.IndexOf(char.ToUpper(letter));

static char NumberToLetter(int number) => Alphabet[AlphabetModulo(number)];

static string DecryptTranspositionCipher(string text, int key)
{
	StringBuilder sb = new();
	foreach (char c in text)
		sb.Append(NumberToLetter(LetterToNumber(c) - key));
	return sb.ToString().ToLower();
}

static List<KeyValuePair<char, int>> CountLetters(string text)
{
	Dictionary<char, int> counts = new();
	foreach (char c in text)
		if (counts.ContainsKey(c))
			counts[c]++;
		else
			counts[c] = 1;
	return counts.OrderByDescending(x => x.Value).ToList();
}

static (int key, string decrypted)[] FindTranspositionCipherKey(string cipher)
{
	var results = new List<(int key, string decrypted)>();
	var counts = CountLetters(cipher);
	foreach (char letter in CommonLetters)
	{
		int key = AlphabetModulo(LetterToNumber(letter) - LetterToNumber(counts[0].Key));
		results.Add((key, DecryptTranspositionCipher(cipher, key)));
	}

	return results.ToArray();
}

string cipher = "BEEAKFYDJXUQYHYJIQRYHTYJIQFBQDUYJIIKFUHCQD";
var results = FindTranspositionCipherKey(cipher);

Console.WriteLine($"Ciphertext: {cipher}");
foreach ((int key, string decrypted) result in results)
	Console.WriteLine($"Key: {result.key}, Decrypted: {result.decrypted}");

// lookupintheairitsabirditsaplaneitssuperman
