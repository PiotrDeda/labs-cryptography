Random rand = new();
List<int> values = new() { 561, 1729, 6601, 881, 9677, 17321, 37579 };
int s = 50;
values.ForEach(n => Console.WriteLine($"{n} - {(MillerRabin(n, s, rand) ? "P" : "Z")}"));

//////////////////////////////

static bool Witness(int a, int n)
{
    int t = 0, u = n - 1;
    while (u % 2 == 0)
    {
        u /= 2;
        t++;
    }

    int[] x = new int[t + 1];
    x[0] = (int)ModPow(a, (ulong)u, n);
    for (int i = 1; i <= t; ++i)
    {
        x[i] = x[i - 1] * x[i - 1] % n;
        if (x[i] == 1 && x[i - 1] != 1 && x[i - 1] != n - 1)
            return true;
    }
    return x[t] != 1;
}

static bool MillerRabin(int n, int s, Random rand)
{
    for (int j = 1; j <= s; ++j)
        if (Witness(rand.Next(1, n - 1), n))
            return false;
    return true;
}

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
