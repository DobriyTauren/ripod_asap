using System;
using System.Drawing;
using System.Windows.Forms;

namespace LAB2_RiPOD
{
    public partial class MainForm : Form
    {
        private ASAP _asap = new ASAP();
        private Color _backColor = Color.CadetBlue;
        private int _radius = 30;
        public MainForm()
        {
            InitializeComponent();

            Paint += MainForm_Paint;
        }

        private void ViewResults()
        {
            // шаги
            Drawer.AddLabel(OutputPanel, $"Всего шагов: {_asap.steps.Count}");

            Drawer.AddLabel(OutputPanel, $"Всего процессоров: { _asap.MaxProcessorsCount}");
            Drawer.AddLabel(OutputPanel, $"Всего типов: {_asap.CountTypes}");

            for (int i = 0; i < _asap.CountTypes; i++)
            {
                Drawer.AddLabel(OutputPanel, "Тип " + (i + 1) + ": " + _asap.countProcessorsByTypes[i]);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _asap.ReadFile("params.txt");
                _asap.Planning();
                ViewResults();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при попытке отрисовки графа, попробуйте изменить связи!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            try
            {
                Graphics graphics = (sender as Button).Parent.CreateGraphics();
                graphics.Clear(Color.WhiteSmoke);

                Drawer.Clear(OutputPanel);

                _asap = new ASAP();
                _asap.Clear();
                _asap.ReadFile("params.txt");
                _asap.Planning();

                // рисуем вершины
                Drawer.DrawEllipses(graphics, _radius, _asap, _backColor);

                //рисуем ребра
                Drawer.DrawLines(graphics, _asap, _backColor);

                // рисуем вершины
                Drawer.DrawEllipses(graphics, _radius, _asap, _backColor);

                ViewResults();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при попытке отрисовки графа, попробуйте изменить связи!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            // рисуем вершины
            Drawer.DrawEllipses(e.Graphics, _radius, _asap, _backColor);

            //рисуем ребра
            Drawer.DrawLines(e.Graphics, _asap, _backColor);

            // рисуем вершины
            Drawer.DrawEllipses(e.Graphics, _radius, _asap, _backColor);
        }
    }
}
