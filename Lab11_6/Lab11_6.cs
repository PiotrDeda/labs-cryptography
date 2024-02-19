long p = 12987461;
long g = 3606738;
ulong a = 357;
ulong b = 199;

long A = ModPow(g, a, p);
long B = ModPow(g, b, p);

Console.WriteLine($"A = {g} ^ {a} mod {p} = {A}");
Console.WriteLine($"B = {g} ^ {b} mod {p} = {B}");
Console.WriteLine($"[Alice] s = {B}  ^ {a} mod {p} = {ModPow(B, a, p)}");
Console.WriteLine($"[Bob]   s = {A} ^ {b} mod {p} = {ModPow(A, b, p)}");

//////////////////////////////

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
