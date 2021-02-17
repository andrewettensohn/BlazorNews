using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNews.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string By { get; set; }

        public List<int> Kids { get; set; }

        public int Parent { get; set; }

        public string Text { get; set; }

        public string Time { get; set; }

        public string Type { get; set; }
    }
}
