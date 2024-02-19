List<(int a, int n)> values = new() { (17, 101), (357, 1234), (3125, 9987) };

foreach ((int a, int n) in values)
    Console.WriteLine($"{a}^-1 mod {n} = {ModInverse(a, n)}");

//////////////////////////////

static int ModInverse(int a, int n) => (ExtGCD(a, n).x + n) % n;

static (int d, int x, int y) ExtGCD(int j, int k)
{
    if (j == 0)
        return (k, 0, 1);
    (int d, int xp, int yp) = ExtGCD(k % j, j);
    return (d, yp - (int)Math.Floor((double)k / j) * xp, xp);
}
