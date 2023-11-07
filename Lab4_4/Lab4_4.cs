List<int> bits = new() { 2, 4, 8, 16, 32, 64, 128, 256, 512 };

List<double> P = new() {
	1.0 / 2,
	1.0 / 2
};

foreach (int n in bits)
{
	double sum = 0;
	for (int i = 0; i < n; i++)
		sum += H(P);
	Console.WriteLine($"[{n}] H(P) = {sum:N3}");
}

//////////////////////////////

static double H(List<double> probabilities) => -probabilities.Sum(p => p == 0 ? 0 : p * Math.Log2(p));
