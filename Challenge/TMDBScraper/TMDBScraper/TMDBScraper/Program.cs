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
using TMDbLib.Client;
using TMDBScraper.Models;

namespace TMDBScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create paths if not created yet
            Console.WriteLine("Initializing...");
            string currentDateTime = DateTime.Now.ToString("dd_MM_yyyy_HHMMss");
            string csvLocation = "OutputData/movies_"+ currentDateTime +".csv";
            string tmdbCompressedIdsLocation = "InputData/tmdbIds_" + currentDateTime + ".json.gz";
            string tmdbIdsLocation = "InputData/currentIds.json";

            TMDbClient TMDBclient = new TMDbClient("22ccb4ae1a1568e8f54ba7191600477c");

            Console.WriteLine("Setting up file paths");
            Directory.CreateDirectory(Path.GetDirectoryName(csvLocation));
            Directory.CreateDirectory(Path.GetDirectoryName("InputData/"));

            //get a zipped tmdb json file
            Console.WriteLine("Getting Id Sheet from TMDB servers");
            using (var client = new WebClient())
            {
                client.DownloadFile(
                    "http://files.tmdb.org/p/exports/movie_ids_"+ DateTime.Now.AddDays(-1).ToString("MM_dd_yyyy") + ".json.gz",
                    tmdbCompressedIdsLocation
                    );
            }

            //unzip json
            using FileStream compressedFileStream = File.Open(tmdbCompressedIdsLocation, FileMode.Open);
            using FileStream outputFileStream = File.Create(tmdbIdsLocation);
            using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);

            decompressor.CopyTo(outputFileStream);
            compressedFileStream.Close();
            outputFileStream.Close();

            //read json
            string content;
            using (StreamReader r = new StreamReader(tmdbIdsLocation))
            {
                content = r.ReadToEnd();
            }

            var jsonReader = new JsonTextReader(new StringReader(content))
            {
                SupportMultipleContent = true
            };

            var jsonSerializer = new JsonSerializer();

            Console.WriteLine("Initialization complete! Starting Scraping process\n This can take a while.");
            using (var writer = new StreamWriter(csvLocation))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<TMDbLib.Objects.Movies.Movie>();
                csv.NextRecord();
                while (jsonReader.Read())
                {
                    if (jsonReader.LineNumber % 1000 == 0)
                    {
                        Console.WriteLine("Read " + jsonReader.LineNumber + " Lines");
                    }
                    TMDBIdEntry movieIdEntry = jsonSerializer.Deserialize<TMDBIdEntry>(jsonReader);
                    var movieToAdd = TMDBclient.GetMovieAsync(movieIdEntry.id).Result;
                    csv.WriteRecord(movieToAdd);
                    csv.NextRecord();
                }
                writer.Flush();
            }

            File.Delete(tmdbIdsLocation);
            Console.WriteLine("Done!");

            //read file
            //using (var reader = new StreamReader(csvLocation))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    var records = csv.GetRecords<Movie>();
            //    var movies = records.ToList();
            //}
        }
    }
}
