Console.WriteLine($"({610} / {987}) = {Jacobi(610, 987)}");
Console.WriteLine($"({20964} / {1987}) = {Jacobi(20964, 1987)}");
Console.WriteLine($"({1234567} / {11111111}) = {Jacobi(1234567, 11111111)}");

//////////////////////////////

static int Jacobi(int a, int n)
{
    int result = 1;

    while (a != 0)
    {
        int k = 0;
        while (a % 2 == 0 && a != 0)
        {
            a /= 2;
            k++;
        }
        if (k % 2 == 1 && (n % 8 == 3 || n % 8 == 5))
            result = -result;

        (a, n) = (n, a);
        if (a % 4 == 3 && n % 4 == 3)
            result = -result;

        a %= n;
    }

    return result;
}
