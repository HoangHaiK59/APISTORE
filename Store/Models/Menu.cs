using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Menu
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public List<Submenu> submenus { get; set; }
    }

    public class Submenu
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
    }
}
