using System.Text;

const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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

static (int key, string decrypted)[] FindTranspositionCipherKey(string cipher)
{
	var results = new (int key, string decrypted)[26];
	for (int i = 0; i < 26; ++i)
		results[i] = (i, DecryptTranspositionCipher(cipher, i));
	return results;
}

string cipher = "BEEAKFYDJXUQYHYJIQRYHTYJIQFBQDUYJIIKFUHCQD";
var potentialResults = FindTranspositionCipherKey(cipher);

foreach ((int key, string decrypted) in potentialResults)
	Console.WriteLine($"Key: {key}, Decrypted: {decrypted}");

// lookupintheairitsabirditsaplaneitssuperman
