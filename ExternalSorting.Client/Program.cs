using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ExternalSorting.Generator;
using ExternalSorting.Sorting;
using Microsoft.Extensions.Logging;
using Environment = System.Environment;

namespace ExternalSorting.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

            var path = CreateTestFileDirectory();

            var generator = CreateGenerator(loggerFactory);
            var sorter = GetSorter(loggerFactory);

            var size = 10L * 1024 * 1024 * 1024;
            var chunkSize = 80 * 1024 * 1024;

            generator.Generate(size, path);
            await sorter.Sort(path, chunkSize);

            Console.ReadKey();
        }

        private static string CreateTestFileDirectory()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var testDirectory = $"{appData}\\Test";
            Directory.CreateDirectory(testDirectory);

            return @$"{testDirectory}\Test.txt";
        }

        private static FilesGenerator CreateGenerator(ILoggerFactory loggerFactory)
        {
            var settings = new SentenceSourceSettings { MaxWordsNumber = 5, SentencesPoolSize = 100 };

            var wordsSource = new LoremIpsumSource();
            var sentencesSource = new DefaultSentencesSource(wordsSource, settings);
            return new FilesGenerator(sentencesSource, loggerFactory);
        }

        private static ExternalSortingOrchestrator GetSorter(ILoggerFactory loggerFactory)
        {
            return new ExternalSortingOrchestrator(loggerFactory);
        }
    }
}
