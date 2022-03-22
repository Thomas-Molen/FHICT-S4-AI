using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDBScraper.Models;

namespace TMDBScraper.Helpers
{
    public class MovieHelper
    {
        JsonTextReader jsonReader;
        JsonSerializer jsonSerializer;
        TMDbClient TMDBclient;
        public MovieHelper(JsonTextReader _jsonReader)
        {
            jsonReader = _jsonReader;
            jsonSerializer = new JsonSerializer();

            //Setup API connection
            TMDBclient = new TMDbClient("22ccb4ae1a1568e8f54ba7191600477c");
        }

        public async Task<GetMovieResponse> GetNextMovie()
        {
            TMDBIdEntry movieIdEntry = jsonSerializer.Deserialize<TMDBIdEntry>(jsonReader);
            var movie = await TMDBclient.GetMovieAsync(movieIdEntry.id, MovieMethods.Credits);
            if (movie.Status != "Released" || movie.Adult)
            {
                throw new ArgumentException("Given movie is not valid");
            }

            List<Actor> actors = new List<Actor>();
            foreach (var newActor in movie.Credits.Cast)
            {
                Actor actor = new Actor
                {
                    MovieId = movie.Id,
                    Gender = newActor.Gender.ToString(),
                    Name = newActor.Name,
                    Popularity = newActor.Popularity
                };
                actors.Add(actor);
            }

            // Set the cast from the credits in a list of persons and somehow store this in csv

            Models.Movie movieToAdd = new Models.Movie
            {
                Id = movie.Id,
                ImdbId = movie.ImdbId,
                Title = movie.Title,
                Runtime = movie.Runtime,
                Budget = movie.Budget,
                Popularity = movie.Popularity,
                Release = movie.ReleaseDate,
                Revenue = movie.Revenue,
                Company = movie.ProductionCompanies?.FirstOrDefault()?.Name ?? null,
                Country = movie.ProductionCountries?.FirstOrDefault()?.Iso_3166_1 ?? null,
                Language = movie.OriginalLanguage,
                Cast = movie.Credits?.Cast?.Count,
                Crew = movie.Credits?.Crew?.Count,
                Genre = movie.Genres?.FirstOrDefault()?.Name ?? null
            };

            // Disgusting check for collection
            if (movie.BelongsToCollection != null)
            {
                movieToAdd.Collection = true;
            }
            else
            {
                movieToAdd.Collection = false;
            }

            GetMovieResponse result = new GetMovieResponse
            {
                Movie = movieToAdd,
                Actors = actors
            };

            return result;
        }
    }
}
