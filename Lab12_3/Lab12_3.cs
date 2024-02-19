int n = 13;

List<int> remaindersDef = new();
for (int i = 0; i < n; i++)
    remaindersDef.Add(i * i % n);

Console.WriteLine($"Definicja 6.2: {{ {string.Join(", ", remaindersDef.Distinct().OrderBy(x => x))} }}");

List<int> remaindersEuler = new();
for (int i = 0; i < n; i++)
    if ((long)Math.Pow(i, (n - 1) / 2.0) % n == 1)
        remaindersEuler.Add(i);

Console.WriteLine($"Kryterium Eulera: {{ {string.Join(", ", remaindersEuler.OrderBy(x => x))} }}");
