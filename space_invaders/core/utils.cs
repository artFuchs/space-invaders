using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace space_invaders.core
{
    class Size
    {
        private int W;
        private int H;

        public Size(int W, int H)
        {
            this.W = W;
            this.H = H;
        }

        public Size(Size source)
        {
            if (source == null)
            {
                H = 0;
                W = 0;
            }
            else
            {
                H = source.GetH();
                W = source.GetW();
            }
        }

        public int GetW()
        {
            return W;
        }

        public int GetH()
        {
            return H;
        }
    }
}
