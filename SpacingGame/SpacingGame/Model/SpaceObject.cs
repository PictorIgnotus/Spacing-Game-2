using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacingGame.Model
{
    public class SpaceObject
    {
        public enum Direction { Right, Left, Down, Up };

        protected Koordinate koordinate;

        public Koordinate KOORDINATE
        {
            get { return koordinate; }
        }

        public SpaceObject(Koordinate koordinate)
        {
            this.koordinate = koordinate;
        }

        public void MoveTo(Direction dir)
        {
            switch (dir)
            {
                case Direction.Right:
                    ++koordinate.first;
                    break;
                case Direction.Left:
                    --koordinate.first;
                    break;
                case Direction.Down:
                    ++koordinate.second;
                    break;
                case Direction.Up:
                    --koordinate.second;
                    break;
                default:
                    break;
            }
        }
    }
}
