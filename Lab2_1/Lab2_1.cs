using System.Text;

const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

static int LetterToNumber(char letter) => Alphabet.IndexOf(char.ToUpper(letter));

static int[] LettersToNumbers(string text)
{
	int[] numbers = new int[text.Length];
	for (int i = 0; i < numbers.Length; i++)
		numbers[i] = LetterToNumber(text[i]);
	return numbers;
}

static char NumberToLetter(int number) => Alphabet[(number % Alphabet.Length + Alphabet.Length) % Alphabet.Length];

static string VigenereCipherEncrypt(string text, string key)
{
	StringBuilder result = new();
	int[] shifts = LettersToNumbers(key);
	for (int i = 0; i < text.Length; i++)
		result.Append(NumberToLetter(LetterToNumber(text[i]) + shifts[i % shifts.Length]));
	return result.ToString();
}

static string VigenereCipherDecrypt(string text, string key)
{
	StringBuilder result = new();
	int[] shifts = LettersToNumbers(key);
	for (int i = 0; i < text.Length; i++)
		result.Append(NumberToLetter(LetterToNumber(text[i]) - shifts[i % shifts.Length]));
	return result.ToString().ToLower();
}

string plaintext = "thiscryptosystemisnotsecure";
string key = "CIPHER";
string encrypted = VigenereCipherEncrypt(plaintext, key);
string decrypted = VigenereCipherDecrypt(encrypted, key);

Console.WriteLine($"key: {key}");
Console.WriteLine("Encryption");
Console.WriteLine($"Plaintext: {plaintext}");
Console.WriteLine($"Cyphertext:{encrypted}");
Console.WriteLine();
Console.WriteLine("Decryption");
Console.WriteLine($"Plaintext: {decrypted}");
Console.WriteLine($"Cyphertext:{encrypted}");
