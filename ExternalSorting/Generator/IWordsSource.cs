using System.Collections.Generic;

namespace ExternalSorting.Generator
{
    public interface IWordsSource
    {
        IEnumerable<string> GetWords(int numberOfWords);
    }
}