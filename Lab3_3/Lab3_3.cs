BlumBlumShubGenerator generatorA = new(7, 19, 100);
BlumBlumShubGenerator generatorB = new(67, 71, 100);
BlumBlumShubGenerator generatorC = new(163, 167, 100);

Console.WriteLine("a)");
for (int i = 0; i < 3; i++)
{
	for (int j = 0; j < 18; j++)
		Console.Write(generatorA.Next());
	Console.WriteLine();
}

Console.WriteLine();

Console.WriteLine("b)");
for (int i = 0; i < 3; i++)
{
	for (int j = 0; j < 60; j++)
		Console.Write(generatorB.Next());
	Console.WriteLine();
}

Console.WriteLine();

Console.WriteLine("c)");
for (int i = 0; i < 4; i++)
{
	for (int j = 0; j < 60; j++)
		Console.Write(generatorC.Next());
	Console.WriteLine();
}

//////////////////////////////

class BlumBlumShubGenerator
{
	readonly int n;
	int y;

	public BlumBlumShubGenerator(int p, int q, int s)
	{
		n = p * q;
		y = s * s % n;
	}

	public int Next()
	{
		y = y * y % n;
		return Parity(y);
	}

	static int Parity(int number)
	{
		string numberString = Convert.ToString(number, 2);
		int sum = 0;
		foreach (char c in numberString)
			sum += c - '0';
		return sum % 2;
	}
}
