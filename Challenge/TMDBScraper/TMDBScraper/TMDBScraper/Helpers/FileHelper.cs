using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMDBScraper.Models;

namespace TMDBScraper.Helpers
{
    public class FileHelper
    {
        string currentDateTime;
        string csvLocation;
        string tmdbCompressedIdsLocation;
        public readonly string tmdbIdsLocation = "InputData/currentIds.json";

        StreamWriter writer;
        CsvWriter csv;
        public FileHelper()
        {
            currentDateTime = DateTime.Now.ToString("dd_MM_yyyy_HHMMss");
            csvLocation = "OutputData/movies_" + currentDateTime + ".csv";
            tmdbCompressedIdsLocation = "InputData/tmdbIds_" + currentDateTime + ".json.gz";
        }

        public void CreateDirectories()
        {   
            Console.WriteLine("Setting up file paths");
            Directory.CreateDirectory(Path.GetDirectoryName(csvLocation));
            Directory.CreateDirectory(Path.GetDirectoryName("InputData/"));
        }

        public void CreateIdSheet()
        {
            //get a zipped tmdb json file
            Console.WriteLine("Getting Id Sheet from TMDB servers");
            using (var client = new WebClient())
            {
                client.DownloadFile(
                    "http://files.tmdb.org/p/exports/movie_ids_" + DateTime.Now.AddDays(-1).ToString("MM_dd_yyyy") + ".json.gz",
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
        }

        public void DeleteIdSheet()
        {
            File.Delete(tmdbIdsLocation);
        }

        public void CreateCSV()
        {
            writer = new StreamWriter(csvLocation);
            csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteHeader<Movie>();
            csv.NextRecord();
        }

        public void WriteCSV(Movie movie)
        {
            csv.NextRecord();
            csv.WriteRecord(movie);   
        }

        public void CloseCSV()
        {
            writer.Flush();
        }
    }
}
