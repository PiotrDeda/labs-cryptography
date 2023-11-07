using System.Text;

const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

static int AlphabetModulo(int a) => (a % Alphabet.Length + Alphabet.Length) % Alphabet.Length;

static int LetterToNumber(char letter) => Alphabet.IndexOf(char.ToUpper(letter));

static char NumberToLetter(int number) => Alphabet[AlphabetModulo(number)];

static int Determinant((int k11, int k12, int k21, int k22) matrix) =>
	AlphabetModulo(matrix.k11 * matrix.k22 - matrix.k21 * matrix.k12);

static (int, int)[] ParcelPlaintext(string plaintext)
{
	var parcels = new (int, int)[plaintext.Length / 2];
	for (int i = 0; i < plaintext.Length; i += 2)
		parcels[i / 2] = (LetterToNumber(plaintext[i]), LetterToNumber(plaintext[i + 1]));
	return parcels;
}

static (int k11, int k12, int k21, int k22) InverseMatrix((int k11, int k12, int k21, int k22) matrix)
{
	int det = Determinant(matrix);
	return (AlphabetModulo(matrix.k22 / det), AlphabetModulo(-matrix.k12 / det),
		AlphabetModulo(-matrix.k21 / det), AlphabetModulo(matrix.k11 / det));
}

static (int, int) MatrixMultiplication((int a, int b) parcel, (int k11, int k12, int k21, int k22) matrix) => (
	AlphabetModulo(matrix.k11 * parcel.a + matrix.k21 * parcel.b),
	AlphabetModulo(matrix.k12 * parcel.a + matrix.k22 * parcel.b)
);

static string HillCipherEncrypt(string plaintext, (int, int, int, int) key)
{
	StringBuilder result = new();
	var parcels = ParcelPlaintext(plaintext);
	foreach ((int, int) parcel in parcels)
	{
		(int, int) mult = MatrixMultiplication(parcel, key);
		result.Append(NumberToLetter(mult.Item1));
		result.Append(NumberToLetter(mult.Item2));
	}

	return result.ToString();
}

static string HillCipherDecrypt(string plaintext, (int, int, int, int) key)
{
	StringBuilder result = new();
	(int k11, int k12, int k21, int k22) invKey = InverseMatrix(key);
	var parcels = ParcelPlaintext(plaintext);
	foreach ((int, int) parcel in parcels)
	{
		(int, int) mult = MatrixMultiplication(parcel, invKey);
		result.Append(NumberToLetter(mult.Item1));
		result.Append(NumberToLetter(mult.Item2));
	}

	return result.ToString().ToLower();
}

string plaintext = "july";
var key = (k11: 11, k12: 8, k21: 3, k22: 7);
(int k11, int k12, int k21, int k22) invKey = InverseMatrix(key);
string encrypted = HillCipherEncrypt(plaintext, key);
string decrypted = HillCipherDecrypt(encrypted, key);

Console.WriteLine($"det = {Determinant(key)}");
Console.WriteLine($"det_inv = {Determinant(invKey)}");
Console.WriteLine();
Console.WriteLine("K:");
Console.WriteLine($"{key.k11} {key.k12}");
Console.WriteLine($"{key.k21} {key.k22}");
Console.WriteLine();
Console.WriteLine("K_inv:");
Console.WriteLine($"{invKey.k11} {invKey.k12}");
Console.WriteLine($"{invKey.k21} {invKey.k22}");
Console.WriteLine();
Console.WriteLine("Encryption");
Console.WriteLine($"Plaintext: {plaintext}");
Console.WriteLine($"Cyphertext:{encrypted}");
Console.WriteLine();
Console.WriteLine("Decryption");
Console.WriteLine($"Plaintext: {decrypted}");
Console.WriteLine($"Cyphertext:{encrypted}");
