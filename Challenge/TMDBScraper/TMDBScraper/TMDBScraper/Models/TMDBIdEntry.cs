using System;
using System.Collections.Generic;
using System.Text;

namespace TMDBScraper.Models
{
    class TMDBIdEntry
    {
        public bool adult { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public float popularity { get; set; }
        public bool video { get; set; }
    }
}
