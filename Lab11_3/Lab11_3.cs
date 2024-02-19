List<int> values = new() { 2, 2, 3 };
(var parameters, int modulus) = GetInvChi(new List<int> { 3, 5, 7 });
Console.Write($"chi-1({string.Join(", ", Enumerable.Range(1, parameters.Count()).Select(i => $"a{i}"))}) = (");
Console.Write(string.Join(" + ", parameters.Zip(Enumerable.Range(1, parameters.Count()), (p, i) => $"{p} * a{i}")));
Console.WriteLine($") mod {modulus}");
Console.Write(string.Join(" + ", values.Zip(parameters, (v, p) => $"{p} * {v}")));
Console.Write($" mod {modulus} = {ExecuteInvChi(values, parameters, modulus)}");

//////////////////////////////

static int ExecuteInvChi(IEnumerable<int> values, IEnumerable<int> parameters, int modulus) =>
    values.Zip(parameters, (v, p) => v * p).Sum() % modulus;

static (ICollection<int> parameters, int modulus) GetInvChi(ICollection<int> moduli)
{
    int modulus = moduli.Aggregate((prod, m) => prod * m);
    var partialProducts = moduli.Select(m => modulus / m).ToList();
    var inverses = partialProducts.Zip(moduli, ModInverse);
    var parameters = partialProducts.Zip(inverses, (pp, inv) => pp * inv).ToList();
    return (parameters, modulus);
}

static int ModInverse(int a, int n) => (ExtGCD(a, n).x + n) % n;

static (int d, int x, int y) ExtGCD(int j, int k)
{
    if (j == 0)
        return (k, 0, 1);
    (int d, int xp, int yp) = ExtGCD(k % j, j);
    return (d, yp - (int)Math.Floor((double)k / j) * xp, xp);
}
