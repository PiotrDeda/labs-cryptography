Console.WriteLine($"x = {ChineseRemainder(new[] { 12, 9, 23 }, new[] { 25, 26, 27 })}");

//////////////////////////////

static int ChineseRemainder(int[] residues, int[] moduli)
{
    int modulus = moduli.Aggregate((prod, m) => prod * m);
    return (residues.Select((r, i) => r * modulus / moduli[i] * ModInverse(modulus / moduli[i], moduli[i])).Sum() +
            modulus) % modulus;
}

static int ModInverse(int a, int n) => (ExtGCD(a, n).x + n) % n;

static (int d, int x, int y) ExtGCD(int j, int k)
{
    if (j == 0)
        return (k, 0, 1);
    (int d, int xp, int yp) = ExtGCD(k % j, j);
    return (d, yp - (int)Math.Floor((double)k / j) * xp, xp);
}
