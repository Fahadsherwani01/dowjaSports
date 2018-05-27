using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dowjaSports.Models
{
    public class cart
    {
        public string quantity { get; set; }
        public int id { get; set; }
        public string key { get; set; }
        public string  size { get; set; }
        public string payment { get; set; }
        public string imgUrl { get; set; }
        public string productName { get; set; }
    }
}