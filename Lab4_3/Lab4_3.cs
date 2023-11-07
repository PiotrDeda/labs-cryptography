List<double> P1 = new() {
	1.0 / 2,
	1.0 / 2
};

List<double> P2 = new() {
	3.0 / 4,
	1.0 / 4
};

List<double> P3 = new() {
	99.0 / 100,
	1.0 / 100
};

Console.WriteLine($"[1/2, 1/2]      H(P) = {H(P1):N3}");
Console.WriteLine($"[3/4, 1/4]      H(P) = {H(P2):N3}");
Console.WriteLine($"[99/100, 1/100] H(P) = {H(P3):N3}");

//////////////////////////////

static double H(List<double> probabilities) => -probabilities.Sum(p => p == 0 ? 0 : p * Math.Log2(p));
