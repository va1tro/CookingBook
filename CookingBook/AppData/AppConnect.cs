using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.AppData
{
    internal class AppConnect
    {
        public static Entities model01;
        public static Authors CurrentUser { get; set; }
        public static Entities GetContext()
        {
            if (model01 == null)
                model01 = new Entities();
            return model01;
        }
    }
}
