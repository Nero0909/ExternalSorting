using System;
using System.Linq;

namespace ExternalSorting.Generator
{
    public class DefaultSentencesSource : ISentencesSource
    {
        private readonly IWordsSource _wordsSource;
        private readonly string[] _sentencesPool;
        private readonly Random _random;

        public DefaultSentencesSource(IWordsSource wordsSource, SentenceSourceSettings settings)
        {
            _random = new Random(DateTime.UtcNow.Millisecond);
            _wordsSource = wordsSource;
            _sentencesPool = InitializePool(settings);
        }

        public string GetSentence()
        {
            var idx = _random.Next(_sentencesPool.Length);
            return _sentencesPool[idx];
        }

        private string[] InitializePool(SentenceSourceSettings settings)
        {
            var sentences = new string[settings.SentencesPoolSize];

            var currentSize = 0;
            while (currentSize < settings.SentencesPoolSize)
            {
                var words = _wordsSource.GetWords(GetRandom(1, settings.MaxWordsNumber)).ToList();
                words[0] = FirstCharToUpper(words[0]);

                sentences[currentSize] = string.Join(' ', words);
                currentSize++;
            }

            return sentences;
        }

        private string FirstCharToUpper(string input) => input.First().ToString().ToUpper() + input.Substring(1);

        // Returns a random integer uniformly in [from, to)
        private int GetRandom(int from, int to) => @from + _random.Next(to - @from);
    }
}