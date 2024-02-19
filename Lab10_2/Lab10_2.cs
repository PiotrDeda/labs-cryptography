using System.Security.Cryptography;

using (var aes = Aes.Create())
{
    aes.BlockSize = 128;
    aes.KeySize /= 2;
    string plaintext = "00000000000000000000000000000000";

    byte[] encryptedEcb = aes.EncryptEcb(StringToBytes(plaintext), PaddingMode.None);
    byte[] decryptedEcb = aes.DecryptEcb(encryptedEcb, PaddingMode.None);
    Console.WriteLine($"k = {BytesToString(aes.Key)}");
    Console.WriteLine($"enc({plaintext}) = {BytesToString(encryptedEcb)}");
    Console.WriteLine($"dec({BytesToString(encryptedEcb)}) = {BytesToString(decryptedEcb)}");
    Console.WriteLine();
}

using (var aes = Aes.Create())
{
    aes.BlockSize = 128;
    aes.KeySize /= 2;
    string plaintext = "0000000000000000000000000000000000000000000000000000000000000000";

    byte[] encryptedEcb = aes.EncryptEcb(StringToBytes(plaintext), PaddingMode.None);
    Console.WriteLine($"k = {BytesToString(aes.Key)}");
    Console.WriteLine($"enc({HalveString(plaintext)})");
    Console.WriteLine($"=   {HalveString(BytesToString(encryptedEcb))}");
    Console.WriteLine();
}

using (var aes = Aes.Create())
{
    aes.BlockSize = 128;
    aes.KeySize /= 2;
    string plaintext = "0000000000000000000000000000000000000000000000000000000000000000";

    byte[] encryptedCbc1 = aes.EncryptCbc(StringToBytes(plaintext), aes.IV, PaddingMode.None);
    Console.WriteLine($"k = {BytesToString(aes.Key)}");
    Console.WriteLine();
    Console.WriteLine($"iv = {BytesToString(aes.IV)}");
    Console.WriteLine($"enc({HalveString(plaintext)})");
    Console.WriteLine($"=   {HalveString(BytesToString(encryptedCbc1))}");
    Console.WriteLine();

    aes.GenerateIV();
    byte[] encryptedCbc2 = aes.EncryptCbc(StringToBytes(plaintext), aes.IV, PaddingMode.None);
    Console.WriteLine($"iv = {BytesToString(aes.IV)}");
    Console.WriteLine($"enc({HalveString(plaintext)})");
    Console.WriteLine($"=   {HalveString(BytesToString(encryptedCbc2))}");
}

//////////////////////////////

static string BytesToString(byte[] arr) => BitConverter.ToString(arr).Replace("-", "").ToLower();

static byte[] StringToBytes(string str) => Enumerable.Range(0, str.Length / 2)
    .Select(i => Convert.ToByte(str.Substring(i * 2, 2), 16)).ToArray();

static string HalveString(string str) => $"{str[..(str.Length / 2)]} {str[(str.Length / 2)..]}";
