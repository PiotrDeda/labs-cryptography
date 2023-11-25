double M = 365;

Console.WriteLine("|  Q | eps_exact | eps_approx |");
Console.WriteLine("-------------------------------");
for (double Q = 15; Q <= 30; Q++)
	Console.WriteLine($"| {Q} | {EpsExact(M, Q),9:0.#####} | {EpsApprox(M, Q),9:0.#####} |");

//////////////////////////////

static double EpsExact(double M, double Q)
{
	double product = 1;
	for (int i = 1; i <= Q - 1; i++)
		product *= (M - i) / M;
	return 1 - product;
}

static double EpsApprox(double M, double Q) => 1 - Math.Pow(Math.E, -Q * (Q - 1) / (2 * M));
