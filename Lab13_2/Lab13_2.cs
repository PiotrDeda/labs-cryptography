foreach (long value in new long[] { 262063, 9420457 })
    for (ulong B = 2; B <= 100; B++)
    {
        long d = Pollard(value, B);
        if (d != 0)
        {
            Console.WriteLine($"{value} = {d} * {value / d} (B = {B})");
            break;
        }
    }

//////////////////////////////

static long Pollard(long n, ulong B)
{
    long a = 2;
    for (ulong j = 2; j <= B; j++)
        a = ModPow(a, j, n);
    long d = GCD(a - 1, n);
    if (d <= 1 || d >= n)
        return 0;
    return d;
}

static long GCD(long j, long k) => j == 0 ? k : GCD(k % j, j);

static long ModPow(long a, ulong e, long n)
{
    long d = 1;
    for (int i = 63; i >= 0; i--)
    {
        d = d * d % n;
        if ((e >> i) % 2 == 1)
            d = d * a % n;
    }
    return d;
}
