using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ExternalSorting.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ExternalSorting.Generator
{
    public class FilesGenerator
    {
        private readonly ILogger<FilesGenerator> _logger;
        private readonly ISentencesSource _sentences;
        private readonly Random _random;
        private const int MaxRecordNumber = 32768;

        public FilesGenerator(ISentencesSource sentences, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FilesGenerator>();
            _random = new Random(DateTime.UtcNow.Millisecond);
            _sentences = sentences;
        }

        public void Generate(long fileSizeInBytes, string path)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Start generating the test file");

            var currentSize = 0L;
            using var sw = new StreamWriter(path);

            while (currentSize < fileSizeInBytes)
            {
                var sentence = _sentences.GetSentence();
                var number = _random.Next(MaxRecordNumber);
                var record = new Record(number, sentence);
                var line = RecordSerializer.Serialize(in record);

                sw.WriteLine(line);

                currentSize += SizeHelper.GetLineBytes(line);
            }

            _logger.LogInformation($"File is generated. Elapsed {stopwatch.Elapsed}");
        }
    }
}