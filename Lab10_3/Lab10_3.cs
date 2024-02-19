byte[][] rcon = {
    StringToBytes("01000000"),
    StringToBytes("02000000"),
    StringToBytes("04000000"),
    StringToBytes("08000000"),
    StringToBytes("10000000"),
    StringToBytes("20000000"),
    StringToBytes("40000000"),
    StringToBytes("80000000"),
    StringToBytes("1b000000"),
    StringToBytes("36000000")
};

byte[][] sbox = {
    StringToBytes("637c777bf26b6fc53001672bfed7ab76"),
    StringToBytes("ca82c97dfa5947f0add4a2af9ca472c0"),
    StringToBytes("b7fd9326363ff7cc34a5e5f171d83115"),
    StringToBytes("04c723c31896059a071280e2eb27b275"),
    StringToBytes("09832c1a1b6e5aa0523bd6b329e32f84"),
    StringToBytes("53d100ed20fcb15b6acbbe394a4c58cf"),
    StringToBytes("d0efaafb434d338545f9027f503c9fa8"),
    StringToBytes("51a3408f929d38f5bcb6da2110fff3d2"),
    StringToBytes("cd0c13ec5f974417c4a77e3d645d1973"),
    StringToBytes("60814fdc222a908846eeb814de5e0bdb"),
    StringToBytes("e0323a0a4906245cc2d3ac629195e479"),
    StringToBytes("e7c8376d8dd54ea96c56f4ea657aae08"),
    StringToBytes("ba78252e1ca6b4c6e8dd741f4bbd8b8a"),
    StringToBytes("703eb5664803f60e613557b986c11d9e"),
    StringToBytes("e1f8981169d98e949b1e87e9ce5528df"),
    StringToBytes("8ca1890dbfe6426841992d0fb054bb16")
};

Console.WriteLine(string.Join('\n', KeyExpansion("2b7e151628aed2a6abf7158809cf4f3c", rcon, sbox)
    .Select((word, i) => $"w[{i}] = {BytesToString(word)}")));

//////////////////////////////

static string BytesToString(byte[] arr) => BitConverter.ToString(arr).Replace("-", "").ToLower();

static byte[] StringToBytes(string str) => Enumerable.Range(0, str.Length / 2)
    .Select(i => Convert.ToByte(str.Substring(i * 2, 2), 16)).ToArray();

static byte[] Xor(byte[] a, byte[] b) => a.Zip(b, (a, b) => (byte)(a ^ b)).ToArray();

static byte SubBytes(byte a, byte[][] sbox) => sbox[a / 16][a % 16];

static byte[] SubWord(byte[] word, byte[][] sbox) => new[]
    { SubBytes(word[0], sbox), SubBytes(word[1], sbox), SubBytes(word[2], sbox), SubBytes(word[3], sbox) };

static byte[] RotWord(byte[] word) => new[] { word[1], word[2], word[3], word[0] };

static List<byte[]> KeyExpansion(string keyString, byte[][] rcon, byte[][] sbox)
{
    byte[] key = StringToBytes(keyString);
    List<byte[]> w = new();

    for (int i = 0; i <= 3; i++)
        w.Add(new[] { key[4 * i], key[4 * i + 1], key[4 * i + 2], key[4 * i + 3] });

    for (int i = 4; i < 44; i++)
    {
        byte[] temp = w.Last();
        if (i % 4 == 0)
            temp = Xor(SubWord(RotWord(temp), sbox), rcon[i / 4 - 1]);
        w.Add(Xor(w[i - 4], temp));
    }

    return w;
}
