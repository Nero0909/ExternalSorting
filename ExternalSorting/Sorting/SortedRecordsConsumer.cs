using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ExternalSorting.Sorting
{
    public class SortedRecordsConsumer
    {
        private readonly BufferBlock<Record> _sortedRecords;

        public SortedRecordsConsumer(BufferBlock<Record> sortedRecords)
        {
            _sortedRecords = sortedRecords;
        }

        public async Task WriteSortedRecord(string outputFile)
        {
            await Task.Yield();

            await using var sw = new StreamWriter(outputFile); 
            while (await _sortedRecords.OutputAvailableAsync())
            {
                var record = await _sortedRecords.ReceiveAsync();

                sw.WriteLine(RecordSerializer.Serialize(record));
            }
        }
    }
}