namespace _8Queens
{
	public partial class Form1 : Form
	{
		private int[,] matrix = new int[8, 8];

		private int queensNum = 0;

		private bool isForward = false;

		private PictureBox[] cell = new PictureBox[64];

		private readonly Bitmap black_img = new("chess-queen-black.png");
		private readonly Bitmap white_img = new("chess-queen-white.png");

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			for (int i = 1; i <= 64; i++)
			{
				string name = $"pictureBox{i}";
				cell[i - 1] = Controls.Find(name, true).FirstOrDefault() as PictureBox;
			}

			// выход на первоначальную отрисовку
			OnDraw(true);
		}

		private void reset_Click(object sender, EventArgs e)
		{
			// сброс состояний и перерисовка
			OnDraw(true);
		}

		private void queensOnboard_Click(object sender, EventArgs e)
		{
			Random rnd = new();

			int pos = rnd.Next(0, 8);

			// до 4 стартуем в прямом ходе, иначе в обратном
			isForward = pos < 4;

			// TODO:
			// на предпоследнюю ячейку и вторую не хватает глубины рекурсии
			// старт на границах не проходит корректно
			if (pos == 6 || pos == 7) pos = 5;
			if (pos == 0 || pos == 1) pos = 2;

			// начинаем расстановку
			setQueen(pos);

			// рисуем что где получилось
			OnDraw(false);
		}

		private void OnDraw(bool isReset)
		{
			for (int i = 0; i < 64; i++)
			{
				cell[i].Image = null;
			}

			if (isReset)
			{

				// обнуляем все что есть в массиве, решения пока нет
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						matrix[i, j] = 0; // 0 - ячейка не занята
					}
				}

				queensNum = 0;

				for (int i = 0; i < 8; i++)
				{
					cell[i].Image = i % 2 == 0 ? black_img : white_img; // если позиция четная (для массива), то ставим изображение с черным ферзем, иначе ставим белого, чтобы было все видно
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (matrix[i, j] == 1) // если стоит ферзь то рисуем его
						{
							int cellPos = i * 8 + j; // адресная арифметика: для поиска ячейки из двухмерного массива надо номер строки умножить на количество столбцов и прибавить номер столбца, который нужен

							cell[cellPos].Image = cell[cellPos].BackColor.Name.Equals("Control") ? black_img : white_img; // если на белом фоне, то рисуем черную фигуру, иначе белую
						}
                    }
				}
			}
		}

		// рекурсивная функция расстановки
		void setQueen(int pos)
		{
			// если дошли до крайней границы справа, но не всех расставили
			if (queensNum < 8 && pos >= 8)
			{
				pos = 0;
			}
			// если дошли до крайней границы слева, но не всех расставили
			if (queensNum < 8 && pos < 0)
			{
				pos = 7;
			}

			// если все на местах, то выходим
			if (queensNum == 8) return;

			// если можно поставить
			if (tryQueen(pos))
			{
				matrix[queensNum, pos] = 1; // ставим
				queensNum++; // снижаемся на следующую строку
				if (isForward) setQueen(pos + 2); // при прямом ходе смещаемся вправо на 2
				else setQueen(pos - 2); // иначе на 2 влево
			} 
			else // если поставить нельзя
			{
				isForward = !isForward; // меняем направление

				if (isForward) pos--; // и смещаемся либо влево на 1
				else pos++; // либо вправо на 1

				setQueen(pos); // запускаем заново
			}

			return; // возвращаемся из глубин рекурсии
		}

		// функция проверки на допустимость
		bool tryQueen(int idx)
		{
			// проверка по столбцам
			for (int i = 0; i < 8; i++)
			{
				if (matrix[i, idx] == 1) return false; // сообщаем что запрашиваемое место занято
			}

			// проверка по диагонали вдоль главной диагонали
			for (int i = 1; i <= queensNum && idx - i >= 0; ++i)
			{
				if (matrix[queensNum - i, idx - i] == 1) return false; // сообщаем что запрашиваемое место занято
			}

			// проверка вдоль диагонали параллельной побочной
			for (int i = 1; i <= queensNum && idx + i < 8; i++)
			{
				if (matrix[queensNum - i, idx + i] == 1) return false; // сообщаем что запрашиваемое место занято
			}

			// если прошли все проверки, сообщаем что запрашиваемое место свободно
			return true;
		}
	}
}
