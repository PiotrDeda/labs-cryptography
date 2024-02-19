Func<long, long> f = x => x * x + 1;
List<(long n, int i1, int i2)> values = new() {
    (262063, 35, 70),
    (9420457, 50, 100),
    (181937053, 165, 330)
};

foreach ((long value, int i1, int i2) in values)
{
    (long p, long x, long xp) = RhoPollard(value, 1, f);
    Console.WriteLine($"{value} = {p} * {value / p}");
    Console.WriteLine($"x[{i1}] = {x}");
    Console.WriteLine($"x[{i2}] = {xp}");
    Console.WriteLine();
}

//////////////////////////////

static (long p, long x, long xp) RhoPollard(long n, long x1, Func<long, long> f)
{
    long x = x1;
    long xp = f(x) % n;
    long p = GCD(Math.Abs(x - xp), n);
    while (p == 1)
    {
        x = f(x) % n;
        xp = f(xp) % n;
        xp = f(xp) % n;
        p = GCD(Math.Abs(x - xp), n);
    }
    return (p, x, xp);
}

static long GCD(long j, long k) => j == 0 ? k : GCD(k % j, j);
