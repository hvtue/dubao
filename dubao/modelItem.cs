using System;
using System.Collections.Generic;
using System.Text;

namespace dubao
{
    public class modelItem
    {
              public modelDetail item { get; set; }
        public modelItem() { }
    }
    public class modelDetail {
        public string description { get; set; }
        public string pubDate { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public modelDetail() { }
    }

    
}
