using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ExternalSorting.Infrastructure;

namespace ExternalSorting.Sorting
{
    public class ChunksProducer
    {
        private readonly BufferBlock<(int idx, List<Record> chunk)> _chunks;

        public ChunksProducer(BufferBlock<(int idx, List<Record> chunk)> chunks)
        {
            _chunks = chunks;
        }

        public async Task Produce(string filePath, long chunkSize)
        {
            await Task.Yield();

            var idx = 0;
            using var sr = new StreamReader(filePath);
            while (sr.Peek() >= 0)
            {
                var chunks = new List<Record>();
                var remaining = chunkSize;
                while (remaining > 0 && sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();

                    var record = RecordSerializer.Deserialize(line);
                    chunks.Add(record);

                    var bytesRead = SizeHelper.GetLineBytes(line);
                    remaining -= bytesRead;
                }

                await _chunks.SendAsync((idx, chunks));
                idx++;
            }

            _chunks.Complete();
        }
    }
}