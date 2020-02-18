using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;

namespace ExternalSorting.Sorting
{
    public class ExternalSortingOrchestrator
    {
        private readonly ILogger<ExternalSortingOrchestrator> _logger;

        public ExternalSortingOrchestrator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExternalSortingOrchestrator>();
        }

        public async Task Sort(string path, long chunkSize)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("Begin sorting");

            var sortedChunks = await GetSortedChunks(path, chunkSize);

            _logger.LogInformation($"Chunks sorted. Elapsed {sw.Elapsed}");

            await MergeChunks(sortedChunks, path);

            _logger.LogInformation($"Chunks merged. Elapsed {sw.Elapsed}");

            Clean(sortedChunks);
        }

        private async Task<List<string>> GetSortedChunks(string path, long chunkSize)
        {
            var chunksBlock = new BufferBlock<(int idx, List<Record> chunk)>(
                new DataflowBlockOptions { BoundedCapacity = Environment.ProcessorCount + 2 });
            var sortedChunksBlock = new BufferBlock<string>();

            var chunksProducer = new ChunksProducer(chunksBlock);
            var sortedChunksProducer = new SortedChunksProducer(sortedChunksBlock);
            var sortedChunksConsumer = new SortedChunksConsumer(sortedChunksBlock);

            var consumerOptions = new ExecutionDataflowBlockOptions { BoundedCapacity = 1 };
            var consumers = new List<ActionBlock<(int idx, List<Record> chunk)>>();
            for (var i = 0; i < Environment.ProcessorCount; i++)
            {
                consumers.Add(new ActionBlock<(int idx, List<Record> chunk)>(
                    chunk => sortedChunksProducer.SortChunk(chunk, path), consumerOptions));
            }

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            foreach (var consumer in consumers)
            {
                chunksBlock.LinkTo(consumer, linkOptions);
            }

            var produceChunksTask = chunksProducer.Produce(path, chunkSize);
            var sortingCompleteTask = Task.WhenAll(consumers.Select(x => x.Completion))
                .ContinueWith(x => sortedChunksBlock.Complete());

            var sortedChunksTask = sortedChunksConsumer.GetSortedChunkPaths();

            await Task.WhenAll(produceChunksTask, sortingCompleteTask, chunksBlock.Completion,
                sortedChunksBlock.Completion, sortedChunksTask);

            return await sortedChunksTask;
        }

        private async Task MergeChunks(List<string> sortedChunks, string outputFilePath)
        {
            var sortedRecords = new BufferBlock<Record>();
            var merger = new Merger(sortedRecords);
            var writer = new SortedRecordsConsumer(sortedRecords);

            var mergeTask = merger.Merge(sortedChunks);
            var writeTask = writer.WriteSortedRecord(outputFilePath);

            await Task.WhenAll(mergeTask, writeTask, sortedRecords.Completion);
        }

        private void Clean(IEnumerable<string> filesToDelete)
        {
            foreach (var path in filesToDelete)
            {
                File.Delete(path);
            }
        }
    }
}