using System;
using System.Collections.Generic;
using System.Text;

namespace TMDBScraper.Models
{
    public class GetMovieResponse
    {
        public Movie Movie { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
