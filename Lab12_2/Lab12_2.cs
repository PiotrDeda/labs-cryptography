long p = 1511;
long q = 2003;
long d = 1234577;
long y = 152702;

ulong dp = (ulong)(d % (p - 1));
ulong dq = (ulong)(d % (q - 1));
long Mp = (long)ModInverse(q, p);
long Mq = (long)ModInverse(p, q);

Console.WriteLine($"dp = {dp}");
Console.WriteLine($"dq = {dq}");
Console.WriteLine($"Mp = {Mp}");
Console.WriteLine($"Mq = {Mq}");
Console.WriteLine($"x = {DecodeRSA(p, q, dp, dq, Mp, Mq, y)}");

//////////////////////////////

static long DecodeRSA(long p, long q, ulong dp, ulong dq, long Mp, long Mq, long y)
{
    long xp = ModPow(y, dp, p);
    long xq = ModPow(y, dq, q);
    return (Mp * q * xp + Mq * p * xq) % (p * q);
}

static ulong ModInverse(long a, long n) => (ulong)((ExtGCD(a, n).x + n) % n);

static (int d, int x, int y) ExtGCD(long j, long k)
{
    if (j == 0)
        return ((int)k, 0, 1);
    (int d, int xp, int yp) = ExtGCD(k % j, j);
    return (d, yp - (int)Math.Floor((double)k / j) * xp, xp);
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
