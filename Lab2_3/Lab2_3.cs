static string PermutationEncrypt(string plaintext, int[] key)
{
	char[] result = new char[plaintext.Length];
	for (int i = 0; i < plaintext.Length; i += key.Length)
		for (int j = 0; j < key.Length; j++)
			result[i + key[j] - 1] = plaintext[i + j];
	return new string(result).ToUpper();
}

static string PermutationDecrypt(string plaintext, int[] key)
{
	char[] result = new char[plaintext.Length];
	int[] invKey = new int[key.Length];
	for (int i = 0; i < invKey.Length; i++)
		invKey[key[i] - 1] = i + 1;
	for (int i = 0; i < plaintext.Length; i += invKey.Length)
		for (int j = 0; j < invKey.Length; j++)
			result[i + invKey[j] - 1] = plaintext[i + j];
	return new string(result).ToLower();
}

string cipher = "TGEEMNELNNTDROEOAAHDOETCSHAEIRLM";
int[] key = { 2, 4, 6, 1, 8, 3, 5, 7 };
string decrypted = PermutationDecrypt(cipher, key);
string encrypted = PermutationEncrypt(decrypted, key);

Console.WriteLine("Decryption");
Console.WriteLine($"Plaintext: {decrypted}");
Console.WriteLine($"Cyphertext:{cipher}");
Console.WriteLine();
Console.WriteLine("Encryption");
Console.WriteLine($"Plaintext: {decrypted}");
Console.WriteLine($"Cyphertext:{encrypted}");
