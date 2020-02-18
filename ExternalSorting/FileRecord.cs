using System;
using System.IO;

namespace ExternalSorting
{
    public struct FileRecord : IComparable<FileRecord>
    {
        public FileRecord(StreamReader file, Record record)
        {
            File = file;
            Record = record;
        }

        public StreamReader File { get; }

        public Record Record { get; }

        public int CompareTo(FileRecord other)
        {
            return Record.CompareTo(other.Record);
        }
    }
}