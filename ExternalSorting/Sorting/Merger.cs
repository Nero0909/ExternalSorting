using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ExternalSorting.Infrastructure;

namespace ExternalSorting.Sorting
{
    public class Merger
    {
        private readonly MinHeap<FileRecord> _queue;
        private readonly BufferBlock<Record> _sortedRecords;

        public Merger(BufferBlock<Record> sortedRecords)
        {
            _sortedRecords = sortedRecords;
            _queue = new MinHeap<FileRecord>();
        }

        public async Task Merge(List<string> sortedChunks)
        {
            await Task.Yield();

            var readers = new List<StreamReader>();
            try
            {
                readers.AddRange(sortedChunks.Select(partition => new StreamReader(partition)));
                foreach (var reader in readers)
                {
                    var record = RecordSerializer.Deserialize(reader.ReadLine());
                    _queue.Enqueue(new FileRecord(reader, record));
                }

                while (_queue.Any())
                {
                    var smallest = _queue.Dequeue();
                    await _sortedRecords.SendAsync(smallest.Record);

                    var newRecord = smallest.File.ReadLine();
                    if (newRecord == null)
                    {
                        continue;
                    }

                    var next = RecordSerializer.Deserialize(newRecord);
                    _queue.Enqueue(new FileRecord(smallest.File, next));
                }
            }
            finally
            {
                foreach (var reader in readers)
                {
                    reader.Dispose();
                }

                _sortedRecords.Complete();
            }
        }
    }
}