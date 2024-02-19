Console.WriteLine($"NWD(57, 93) =  {GCD(57, 93)}");
(int d, int s, int t) = ExtGCD(57, 93);
Console.WriteLine($"s = {s}");
Console.WriteLine($"t = {t}");
Console.WriteLine($"57 * {s} + 93 * {t} = {d}");

//////////////////////////////

static int GCD(int j, int k) => j == 0 ? k : GCD(k % j, j);

static (int d, int x, int y) ExtGCD(int j, int k)
{
    if (j == 0)
        return (k, 0, 1);
    (int d, int xp, int yp) = ExtGCD(k % j, j);
    return (d, yp - (int)Math.Floor((double)k / j) * xp, xp);
}
