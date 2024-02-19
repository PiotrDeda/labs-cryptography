Console.WriteLine($"x = {ExtendedChineseRemainder(new[] { 13, 15 }, new[] { 4, 56 }, new[] { 99, 101 })}");

//////////////////////////////

static int ExtendedChineseRemainder(int[] coefficients, int[] residues, int[] moduli) =>
    ChineseRemainder(coefficients.Zip(moduli, ModInverse).Zip(residues, (mi, r) => mi * r).ToArray(), moduli);

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
