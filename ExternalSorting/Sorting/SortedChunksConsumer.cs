using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ExternalSorting.Sorting
{
    public class SortedChunksConsumer
    {
        private readonly BufferBlock<string> _sortedChunks;

        public SortedChunksConsumer(BufferBlock<string> sortedChunks)
        {
            _sortedChunks = sortedChunks;
        }

        public async Task<List<string>> GetSortedChunkPaths()
        {
            await Task.Yield();

            var paths = new List<string>();
            while (await _sortedChunks.OutputAvailableAsync())
            {
                paths.Add(await _sortedChunks.ReceiveAsync());
            }

            return paths;
        }
    }
}