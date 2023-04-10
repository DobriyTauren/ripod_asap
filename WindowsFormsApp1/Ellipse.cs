using System.Drawing;

namespace LAB2_RiPOD
{
    public class Ellipse
    {
        private int _id = 0, _x = 0, _y = 0, _radius = 0;
        private Pen _pen = new Pen(Brushes.AliceBlue);

        public Ellipse(int id, int x, int y, int radius, Pen pen)
        {
            _id = id;
            _x = x;
            _y = y;
            _radius = radius;

            _pen = pen;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int X
        {
            get => _x;
            set => _x = value;
        }
        public int Y
        {
            get => _y;
            set => _y = value;
        }
        public int Radius
        {
            get => _radius;
            set => _radius = value;
        }
        public Pen Pen
        {
            get => _pen;
            set => _pen = value;
        }
    }
}
