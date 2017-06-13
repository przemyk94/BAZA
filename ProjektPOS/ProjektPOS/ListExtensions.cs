using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPOS
{
    public static class ListExtensions
    {
        public static int MaxId(this List<Postac> postacie)
        {
            int res = 0;
            foreach(var p in postacie)
            {
                if (p.Id > res)
                    res = p.Id;
            }
            return res;
        }
    }
}
