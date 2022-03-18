using System;
using System.Collections.Generic;
using System.Text;

namespace TMDBScraper.Models
{
    class Person
    {
        public int? Id { get; set; }
        public bool? Adult { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public double? Popularity { get; set; }
    }
}
