using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacingGame.Model
{
    public class IntPair
    {
        public int first;
        public int second;

        public IntPair(int first, int second)
        {
            this.first = first;
            this.second = second;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            IntPair p = obj as IntPair;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (first == p.first) && (second == p.second);
        }

        public override int GetHashCode()
        {
            return first ^ second;
        }
    }

    public class Koordinate : IntPair
    {
        public Koordinate(int xkor, int ykor) : base(xkor, ykor)
        {
        }
    }

    public class Tablesize : IntPair
    {
        public Tablesize(int width, int height) : base(width, height)
        {
        }
    }
}
