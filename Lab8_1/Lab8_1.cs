using System.Text;

Matrix A = new(7, 4);

for (int col = 0; col < A.Columns; col++)
	for (int row = 0; row < A.Rows; row++)
		if (row < col + 4 && row > col - 1)
			A[row, col] = 1;

Matrix pattern = new(1, 4);
pattern[0, 0] = 0;
pattern[0, 1] = 1;
pattern[0, 2] = 0;
pattern[0, 3] = 1;

for (int i = 0; i < 128; i++)
{
	Matrix x = Matrix.FromValue(i);
	Matrix h = x * A;
	if (h.Equals(pattern))
		Console.WriteLine($"{i.ToString().PadLeft(3, '0')} - {x.GetRowString(0, true)} - {h.GetRowString(0, false)}");
}

//////////////////////////////

class Matrix
{
	readonly int[,] values;

	public Matrix(int rows, int columns)
	{
		Rows = rows;
		Columns = columns;
		values = new int[rows, columns];
	}

	public int Rows { get; }
	public int Columns { get; }

	public int this[int row, int col]
	{
		get => values[row, col];
		set => values[row, col] = value;
	}

	public static Matrix FromValue(int value)
	{
		Matrix result = new(1, 7);
		for (int i = 6; i >= 0; i--)
		{
			result[0, i] = value % 2;
			value /= 2;
		}
		return result;
	}

	public string GetRowString(int row, bool spaces)
	{
		StringBuilder sb = new();
		sb.Append('(');
		for (int j = 0; j < Columns; j++)
		{
			sb.Append($"{values[row, j]},");
			if (spaces)
				sb.Append(' ');
		}
		sb.Length--;
		if (spaces)
			sb.Length--;
		sb.Append(')');
		return sb.ToString();
	}

	public bool Equals(Matrix other)
	{
		if (Rows != other.Rows || Columns != other.Columns)
			return false;
		for (int i = 0; i < Rows; i++)
			for (int j = 0; j < Columns; j++)
				if (this[i, j] != other[i, j])
					return false;
		return true;
	}

	public static Matrix operator *(Matrix a, Matrix b)
	{
		if (a.Columns != b.Rows)
			throw new();

		Matrix result = new(a.Rows, b.Columns);

		for (int i = 0; i < a.Rows; i++)
			for (int j = 0; j < b.Columns; j++)
			{
				int sum = 0;
				for (int k = 0; k < a.Columns; k++)
					sum = (sum + a[i, k] * b[k, j]) % 2;
				result[i, j] = sum % 2;
			}

		return result;
	}
}
