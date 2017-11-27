using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacingGame.Model
{
    public class Asteroid : SpaceObject
    {
        public Asteroid(int xkoordinate) : base(new Koordinate(xkoordinate, 0))
        {
        }
    }
}
