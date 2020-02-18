using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ExternalSorting.Sorting
{
    public class SortedChunksProducer
    {
        private readonly BufferBlock<string> _sortedChunks;

        public SortedChunksProducer(BufferBlock<string> sortedChunks)
        {
            _sortedChunks = sortedChunks;
        }

        public async Task SortChunk((int idx, List<Record> chunks) chunk, string filePath)
        {
            chunk.chunks.Sort();

            var chunkPath = $"{Path.GetDirectoryName(filePath)}\\" +
                            $"{Path.GetFileNameWithoutExtension(filePath)}.part{chunk.idx}";

            var records = chunk.chunks.Select(record => RecordSerializer.Serialize(record));

            await File.WriteAllLinesAsync(chunkPath, records);

            chunk.chunks = null;

            await _sortedChunks.SendAsync(chunkPath);
        }
    }
}