using System;

namespace ExternalSorting
{
    public struct Record : IComparable<Record>
    {
        public Record(int number, string sentence)
        {
            Number = number;
            Sentence = sentence;
        }

        public int Number { get; }

        public string Sentence { get; }

        public int CompareTo(Record other)
        {
            var sentenceComparison = string.Compare(Sentence, other.Sentence, StringComparison.Ordinal);
            return sentenceComparison != 0 ? sentenceComparison : Number.CompareTo(other.Number);
        }
    }
}