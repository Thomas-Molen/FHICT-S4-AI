using System;
using System.Collections.Generic;
using System.Text;

namespace TMDBScraper.Models
{
    public class Actor
    {
        public int? MovieId { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public float? Popularity { get; set; }
    }
}
