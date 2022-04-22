using System;
using System.Collections.Generic;
using System.Text;

namespace TMDBScraper.Models
{
    public class Movie
    {
        public int? Id { get; set; }
        public string ImdbId { get; set; }
        public bool? Collection { get; set; }
        public long? Budget { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public double? Popularity { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public DateTime? Release { get; set; }
        public long? Revenue { get; set; }
        public int? Runtime { get; set; }
        public string Title { get; set; }
        public int? CastSize { get; set; }
        public int? CrewSize { get; set; }
        public string Director { get; set; }
        public float? DirectorPopularity { get; set; }
        public float? CastPopularity { get; set; }
    }
}
