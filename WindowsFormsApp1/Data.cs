using System.Collections.Generic;

namespace LAB2_RiPOD
{
    public static class Data
    {
        private static List<Ellipse> _ellipseList = new List<Ellipse>();

        public static List<Ellipse> EllipseList 
        { 
            get => _ellipseList; 
            set => _ellipseList = value; 
        }

        public static Ellipse FindEllipse(int id)
        {
            for (int i = 0; i < _ellipseList.Count; i++)
            {
                if (_ellipseList[i].Id == id)
                {
                    return _ellipseList[i];
                }
            }

            return null;
        }
    }
}
