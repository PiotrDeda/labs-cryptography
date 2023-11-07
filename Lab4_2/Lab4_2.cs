List<double> P = new() {
	1.0 / 2,
	1.0 / 3,
	1.0 / 6
};

List<double> K = new() {
	1.0 / 3,
	1.0 / 3,
	1.0 / 3
};

int[,] cipherMatrix = {
	{ 1, 2, 3 },
	{ 2, 3, 4 },
	{ 3, 4, 1 }
};

List<double> C = new() {
	CipherProbability(cipherMatrix, P, K, 1),
	CipherProbability(cipherMatrix, P, K, 2),
	CipherProbability(cipherMatrix, P, K, 3),
	CipherProbability(cipherMatrix, P, K, 4)
};

Console.WriteLine($"H(P) = {H(P):N3}");
Console.WriteLine($"H(K) = {H(K):N3}");
Console.WriteLine($"H(C) = {H(C):N3}");
Console.WriteLine();
Console.WriteLine($"Pr[1] = {C[0]:N3}");
Console.WriteLine($"Pr[2] = {C[1]:N3}");
Console.WriteLine($"Pr[3] = {C[2]:N3}");
Console.WriteLine($"Pr[4] = {C[3]:N3}");
Console.WriteLine();

double[,] pcProbabilities = new double[4, 3];

for (int c = 0; c < 4; c++)
	for (int p = 0; p < 3; p++)
		for (int k = 0; k < 3; k++)
			if (cipherMatrix[p, k] == c + 1)
				pcProbabilities[c, p] += P[p] * K[k] / C[c];

Console.WriteLine("\ta\tb\tc");
for (int i = 0; i < 4; i++)
{
	Console.Write($"{i + 1}\t");
	for (int j = 0; j < 3; j++)
		Console.Write($"{pcProbabilities[i, j]:N3}\t");
	Console.WriteLine();
}

Console.WriteLine();

for (int i = 0; i < 4; i++)
{
	List<double> P_i = new() {
		pcProbabilities[i, 0],
		pcProbabilities[i, 1],
		pcProbabilities[i, 2]
	};
	Console.WriteLine($"H(P|{i + 1}) = {H(P_i):N3}");
}

Console.WriteLine();

double H_P_C = 0;
for (int p = 0; p < 3; p++)
	for (int c = 0; c < 4; c++)
		if (pcProbabilities[c, p] != 0)
			H_P_C -= C[c] * pcProbabilities[c, p] * Math.Log2(pcProbabilities[c, p]);

Console.WriteLine($"H(P|C) = {H_P_C:N3}");
Console.WriteLine($"H(K|C) = {H(K) + H(P) - H(C):N3}");

//////////////////////////////

static double H(List<double> probabilities) => -probabilities.Sum(p => p == 0 ? 0 : p * Math.Log2(p));

static double CipherProbability(int[,] cipherMatrix, List<double> P, List<double> K, int c)
{
	double sum = 0;
	for (int i = 0; i < P.Count; i++)
		for (int j = 0; j < K.Count; j++)
			if (cipherMatrix[i, j] == c)
				sum += P[i] * K[j];
	return sum;
}
