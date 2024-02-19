using System.Text;

Console.WriteLine("Library implementation:");
// Copied from Python implementation as SHA3 was implemented in a later version of C# than the one used during the labs
Console.WriteLine("6b4e03423667dbb73b6e15454f0eb1abd4597f9a1b078e3f5b5a6bc7");
Console.WriteLine();
Console.WriteLine("Custom implementation:");
Console.WriteLine(BitConverter.ToString(Keccak(1152, 448, Encoding.UTF8.GetBytes(""), 0x06, 224 / 8)).Replace("-", "")
    .ToLower());

//////////////////////////////

// Original implementation copyright
/*
# -*- coding: utf-8 -*-
# Implementation by Gilles Van Assche, hereby denoted as "the implementer".
#
# For more information, feedback or questions, please refer to our website:
# https://keccak.team/
#
# To the extent possible under law, the implementer has waived all copyright
# and related or neighboring rights to the source code in this file.
# http://creativecommons.org/publicdomain/zero/1.0/
*/

static ulong ROL64(ulong x, int y) => (x << y) | (x >> (64 - y));

static ulong[,] KeccakF1600onLanes(ulong[,] lanes)
{
    ulong R = 1;
    for (int round = 0; round < 24; round++)
    {
        // θ
        ulong[] C = new ulong[5];
        for (int x = 0; x < 5; x++)
            C[x] = lanes[x, 0] ^ lanes[x, 1] ^ lanes[x, 2] ^ lanes[x, 3] ^ lanes[x, 4];

        ulong[] D = new ulong[5];
        for (int x = 0; x < 5; x++)
            D[x] = C[(x + 4) % 5] ^ ROL64(C[(x + 1) % 5], 1);

        for (int x = 0; x < 5; x++)
            for (int y = 0; y < 5; y++)
                lanes[x, y] ^= D[x];

        // ρ and π
        {
            (int x, int y) = (1, 0);
            ulong current = lanes[x, y];
            for (int t = 0; t < 24; t++)
            {
                (x, y) = (y, (2 * x + 3 * y) % 5);
                (current, lanes[x, y]) = (lanes[x, y], ROL64(current, (t + 1) * (t + 2) / 2));
            }
        }

        // χ
        for (int y = 0; y < 5; y++)
        {
            ulong[] T = new ulong[5];
            for (int x = 0; x < 5; x++)
                T[x] = lanes[x, y];

            for (int x = 0; x < 5; x++)
                lanes[x, y] = T[x] ^ (~T[(x + 1) % 5] & T[(x + 2) % 5]);
        }

        // ι
        for (int j = 0; j < 7; j++)
        {
            R = ((R << 1) ^ ((R >> 7) * 0x71)) % 256;
            if ((R & 2) != 0)
                lanes[0, 0] ^= 1UL << ((1 << j) - 1);
        }
    }

    return lanes;
}

static ulong Load64(byte[] b) => BitConverter.ToUInt64(b, 0);

static byte[] Store64(ulong a) => BitConverter.GetBytes(a);

static byte[] KeccakF1600(byte[] state)
{
    ulong[,] lanes = new ulong[5, 5];

    for (int x = 0; x < 5; x++)
    {
        for (int y = 0; y < 5; y++)
        {
            byte[] temp = new byte[8];
            Buffer.BlockCopy(state, 8 * (x + 5 * y), temp, 0, 8);
            lanes[x, y] = Load64(temp);
        }
    }

    lanes = KeccakF1600onLanes(lanes);

    byte[] newState = new byte[200];
    for (int x = 0; x < 5; x++)
        for (int y = 0; y < 5; y++)
            Buffer.BlockCopy(Store64(lanes[x, y]), 0, newState, 8 * (x + 5 * y), 8);

    return newState;
}

static byte[] Keccak(int rate, int capacity, byte[] inputBytes, byte delimitedSuffix, int outputByteLen)
{
    List<byte> outputBytes = new();
    byte[] state = new byte[200];
    int rateInBytes = rate / 8;
    int blockSize = 0;

    if (rate + capacity != 1600 || rate % 8 != 0)
        throw new();

    int inputOffset = 0;

    // === Absorb all the input blocks ===
    while (inputOffset < inputBytes.Length)
    {
        blockSize = Math.Min(inputBytes.Length - inputOffset, rateInBytes);

        for (int i = 0; i < blockSize; i++)
            state[i] ^= inputBytes[i + inputOffset];

        inputOffset += blockSize;

        if (blockSize == rateInBytes)
        {
            state = KeccakF1600(state);
            blockSize = 0;
        }
    }

    // === Do the padding and switch to the squeezing phase ===
    state[blockSize] ^= delimitedSuffix;

    if ((delimitedSuffix & 0x80) != 0 && blockSize == rateInBytes - 1)
        state = KeccakF1600(state);

    state[rateInBytes - 1] ^= 0x80;
    state = KeccakF1600(state);

    // === Squeeze out all the output blocks ===
    while (outputByteLen > 0)
    {
        blockSize = Math.Min(outputByteLen, rateInBytes);
        outputBytes.AddRange(state.Take(blockSize));
        outputByteLen -= blockSize;

        if (outputByteLen > 0)
            state = KeccakF1600(state);
    }

    return outputBytes.ToArray();
}
