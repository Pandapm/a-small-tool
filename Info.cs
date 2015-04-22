using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 用户名助记
{
    class Info
    {
        public  string infoSite
        {
            get;
            set;
         }
        public string infoName
        {
            get;
            set;
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}", infoSite, infoName);
        }
    }
}
