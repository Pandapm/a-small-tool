using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 用户名助记
{
    class Info
    {
        public  string info_site
        {
            get;
            set;
         }
        public string info_name
        {
            get;
            set;
        }
        public override string ToString()
        {
            return info_site + ": " + info_name;
        }
    }
}
