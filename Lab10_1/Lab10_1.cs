using System.Security.Cryptography;
using System.Text;

string head = """
              P6
              265 314
              255

              """;

byte[] tuxBody = File.ReadAllBytes("Tux.body");

using (var aes = Aes.Create())
{
    aes.BlockSize = 128;

    // ECB
    byte[] encryptedEcb = aes.EncryptEcb(tuxBody, PaddingMode.PKCS7);

    using (StreamWriter writer = new("Tux.ecb.ppm", false, new UTF8Encoding(false)))
    {
        writer.Write(head);
    }

    using (FileStream fileStream = new("Tux.ecb.ppm", FileMode.Append, FileAccess.Write))
    {
        fileStream.Write(encryptedEcb, 0, encryptedEcb.Length);
    }

    // CBC
    byte[] encryptedCbc = aes.EncryptCbc(tuxBody, aes.IV);

    using (StreamWriter writer = new("Tux.cbc.ppm", false, new UTF8Encoding(false)))
    {
        writer.Write(head);
    }

    using (FileStream fileStream = new("Tux.cbc.ppm", FileMode.Append, FileAccess.Write))
    {
        fileStream.Write(encryptedCbc, 0, encryptedCbc.Length);
    }

    // CTR
    using (Stream inputStream = File.OpenRead("Tux.body"))
    using (MemoryStream outputStream = new())
    {
        AesCtrTransform(aes.Key, aes.IV, inputStream, outputStream);
        byte[] encryptedCtr = outputStream.ToArray();

        using (StreamWriter writer = new("Tux.ctr.ppm", false, new UTF8Encoding(false)))
        {
            writer.Write(head);
        }

        using (FileStream fileStream = new("Tux.ctr.ppm", FileMode.Append, FileAccess.Write))
        {
            fileStream.Write(encryptedCtr, 0, encryptedCtr.Length);
        }
    }
}

//////////////////////////////////////////////////////////////////////////////////
// https://stackoverflow.com/questions/6374437/can-i-use-aes-in-ctr-mode-in-net //
//////////////////////////////////////////////////////////////////////////////////
static void AesCtrTransform(
    byte[] key, byte[] salt, Stream inputStream, Stream outputStream)
{
    SymmetricAlgorithm aes = Aes.Create();
    aes.Mode = CipherMode.ECB;
    aes.Padding = PaddingMode.None;
    aes.BlockSize = 128;

    int blockSize = aes.BlockSize / 8;

    if (salt.Length != blockSize)
        throw new ArgumentException(
            "Salt size must be same as block size " +
            $"(actual: {salt.Length}, expected: {blockSize})");

    byte[] counter = (byte[])salt.Clone();

    var xorMask = new Queue<byte>();

    byte[] zeroIv = new byte[blockSize];
    ICryptoTransform counterEncryptor = aes.CreateEncryptor(key, zeroIv);

    int b;
    while ((b = inputStream.ReadByte()) != -1)
    {
        if (xorMask.Count == 0)
        {
            byte[] counterModeBlock = new byte[blockSize];

            counterEncryptor.TransformBlock(
                counter, 0, counter.Length, counterModeBlock, 0);

            for (int i2 = counter.Length - 1; i2 >= 0; i2--)
                if (++counter[i2] != 0)
                    break;

            foreach (byte b2 in counterModeBlock)
                xorMask.Enqueue(b2);
        }

        byte mask = xorMask.Dequeue();
        outputStream.WriteByte((byte)((byte)b ^ mask));
    }
}
