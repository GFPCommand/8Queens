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

			// ����� �� �������������� ���������
			OnDraw(true);
		}

		private void reset_Click(object sender, EventArgs e)
		{
			// ����� ��������� � �����������
			OnDraw(true);
		}

		private void queensOnboard_Click(object sender, EventArgs e)
		{
			Random rnd = new();

			int pos = rnd.Next(0, 8);

			// �� 4 �������� � ������ ����, ����� � ��������
			isForward = pos < 4;

			// TODO:
			// �� ������������� ������ � ������ �� ������� ������� ��������
			// ����� �� �������� �� �������� ���������
			if (pos == 6 || pos == 7) pos = 5;
			if (pos == 0 || pos == 1) pos = 2;

			// �������� �����������
			setQueen(pos);

			// ������ ��� ��� ����������
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

				// �������� ��� ��� ���� � �������, ������� ���� ���
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						matrix[i, j] = 0; // 0 - ������ �� ������
					}
				}

				queensNum = 0;

				for (int i = 0; i < 8; i++)
				{
					cell[i].Image = i % 2 == 0 ? black_img : white_img; // ���� ������� ������ (��� �������), �� ������ ����������� � ������ ������, ����� ������ ������, ����� ���� ��� �����
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (matrix[i, j] == 1) // ���� ����� ����� �� ������ ���
						{
							int cellPos = i * 8 + j; // �������� ����������: ��� ������ ������ �� ����������� ������� ���� ����� ������ �������� �� ���������� �������� � ��������� ����� �������, ������� �����

							cell[cellPos].Image = cell[cellPos].BackColor.Name.Equals("Control") ? black_img : white_img; // ���� �� ����� ����, �� ������ ������ ������, ����� �����
						}
                    }
				}
			}
		}

		// ����������� ������� �����������
		void setQueen(int pos)
		{
			// ���� ����� �� ������� ������� ������, �� �� ���� ����������
			if (queensNum < 8 && pos >= 8)
			{
				pos = 0;
			}
			// ���� ����� �� ������� ������� �����, �� �� ���� ����������
			if (queensNum < 8 && pos < 0)
			{
				pos = 7;
			}

			// ���� ��� �� ������, �� �������
			if (queensNum == 8) return;

			// ���� ����� ���������
			if (tryQueen(pos))
			{
				matrix[queensNum, pos] = 1; // ������
				queensNum++; // ��������� �� ��������� ������
				if (isForward) setQueen(pos + 2); // ��� ������ ���� ��������� ������ �� 2
				else setQueen(pos - 2); // ����� �� 2 �����
			} 
			else // ���� ��������� ������
			{
				isForward = !isForward; // ������ �����������

				if (isForward) pos--; // � ��������� ���� ����� �� 1
				else pos++; // ���� ������ �� 1

				setQueen(pos); // ��������� ������
			}

			return; // ������������ �� ������ ��������
		}

		// ������� �������� �� ������������
		bool tryQueen(int idx)
		{
			// �������� �� ��������
			for (int i = 0; i < 8; i++)
			{
				if (matrix[i, idx] == 1) return false; // �������� ��� ������������� ����� ������
			}

			// �������� �� ��������� ����� ������� ���������
			for (int i = 1; i <= queensNum && idx - i >= 0; ++i)
			{
				if (matrix[queensNum - i, idx - i] == 1) return false; // �������� ��� ������������� ����� ������
			}

			// �������� ����� ��������� ������������ ��������
			for (int i = 1; i <= queensNum && idx + i < 8; i++)
			{
				if (matrix[queensNum - i, idx + i] == 1) return false; // �������� ��� ������������� ����� ������
			}

			// ���� ������ ��� ��������, �������� ��� ������������� ����� ��������
			return true;
		}
	}
}
