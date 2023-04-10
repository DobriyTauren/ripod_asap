using System.Drawing;
using System.Windows.Forms;

namespace LAB2_RiPOD
{
    public static class Drawer
    {
        private static int _lastY = 10;
        public static void DrawEllipses(Graphics g, int radius, ASAP asap, Color color)
        {
            Data.EllipseList.Clear(); // очистка перед рисованием, добавление новых в список;

            int x = 0, y = 0;
            Font font = new Font("Arial", 10);
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(brush, 2);

            for (int i = 0; i < asap.steps.Count; i++)
            {
                for (int j = 0; j < asap.steps[i].Count; j++)
                {
                    int id = asap.steps[i][j] + 1;
                    x = 80 * (j + 1);
                    y = 80 * (i + 1);

                    Ellipse ellipse = new Ellipse(id, x, y, radius, pen);
                    Data.EllipseList.Add(ellipse);

                    g.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);
                    g.DrawString($"{id} | {asap.FindTypes(id) + 1}", font, new SolidBrush(Color.White), x - 15, y - 10);
                }

                g.DrawString($"{i + 1}", new Font("Arial", 22), new SolidBrush(color), 12, y - 16);
            }
        }

        public static void DrawLines(Graphics g, ASAP Asap, Color color)
        {
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(brush, 2);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            for (int i = 0; i < Asap.List_chains.Count; i++)
            {
                for (int j = 0; j < Asap.List_chains[i].Count; j++)
                {
                    Ellipse firstEllipse;
                    Ellipse secondEllipse;
                    try
                    {
                        firstEllipse = Data.FindEllipse(Asap.List_chains[i][j] + 1);
                        secondEllipse = Data.FindEllipse(Asap.List_chains[i][j + 1] + 1);
                    }
                    catch
                    {
                        break;
                    }

                    g.DrawLine(pen, firstEllipse.X, firstEllipse.Y, secondEllipse.X, secondEllipse.Y);
                    //g.DrawString($"{steps[i]}", font, brush, (x1 + x2) / 2, (y1 + y2) / 2);
                }
            }
        }

        public static void AddLabel(Panel panel, string text)
        {
            Label label = new Label(); // создаем Label
            label.Text = text; // задаем текст Label

            // задаем свойства Label
            label.AutoSize = true;
            label.Location = new Point(10, _lastY); // задаем координаты
            label.Font = new Font("Arial", 12); // задаем шрифт

            _lastY += 25;

            panel.Controls.Add(label); // добавляем Label в Panel

        }
        
        public static void Clear (Panel panel)
        {
            panel.Controls.Clear();

            _lastY = 10;
        }
    }
}
