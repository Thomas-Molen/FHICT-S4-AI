using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDBScraper.Helpers;
using TMDBScraper.Models;

namespace TMDBScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Create paths if not created yet
            Console.WriteLine("Initializing...");
            FileHelper fileHelper = new FileHelper();
            fileHelper.CreateDirectories();
            fileHelper.CreateIdSheet();

            //read json
            string content;
            using (StreamReader r = new StreamReader(fileHelper.tmdbIdsLocation))
            {
                content = r.ReadToEnd();
            }

            var jsonReader = new JsonTextReader(new StringReader(content))
            {
                SupportMultipleContent = true
            };
            MovieHelper movieHelper = new MovieHelper(jsonReader);

            Console.WriteLine("Initialization complete! Starting Scraping process\n This can take a while.");
            try
            {
                fileHelper.CreateCSV();

                while (jsonReader.Read())
                {
                    try
                    {
                        if (jsonReader.LineNumber % 100 == 0)
                        {
                            Console.WriteLine("Read " + jsonReader.LineNumber + " Lines");
                        }
                        var movieResponse = await movieHelper.GetNextMovie();

                        fileHelper.WriteMovie(movieResponse.Movie);

                        foreach (var actor in movieResponse.Actors)
                        {
                            fileHelper.WriteActor(actor);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Skipping Entry: {jsonReader.LineNumber}");
                        continue;
                    }
                }
            fileHelper.CloseCSV();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error occured:\n" + ex);
                fileHelper.DeleteIdSheet();
            }

            fileHelper.DeleteIdSheet();
            Console.WriteLine("Done!");
            Console.WriteLine("Press any key to close");
            Console.ReadLine();
        }
    }
}
