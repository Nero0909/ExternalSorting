﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ExternalSorting.Generator
{
    public class LoremIpsumSource : IWordsSource
    {
        private static string _originalString = "lorem ipsum amet pellentesque mattis accumsan maximus " +
                                                "etiam mollis ligula non iaculis ornare mauris efficitur " +
                                                "ex eu rhoncus aliquam in hac habitasse platea dictumst " +
                                                "maecenas ultrices purus at venenatis auctor sem nulla " +
                                                "urna molestie nisi mi a ut euismod nibh id libero lacinia " +
                                                "sit amet lacinia lectus viverra donec scelerisque dictum " +
                                                "enim dignissim dolor cursus morbi rhoncus elementum magna " +
                                                "sed sed velit consectetur adipiscing elit curabitur nulla " +
                                                "eleifend vel tempor metus phasellus vel pulvinar lobortis " +
                                                "quis nullam felis orci congue vitae augue nisi tincidunt " +
                                                "id posuere fermentum facilisis ultricies mi nisl fusce " +
                                                "neque vulputate integer tortor tempus praesent proin quis " +
                                                "nunc massa congue quam auctor eros placerat eros leo nec " +
                                                "sapien egestas duis feugiat vestibulum porttitor odio " +
                                                "sollicitudin arcu et aenean sagittis ante urna fringilla " +
                                                "risus et vivamus semper nibh eget finibus est laoreet justo " +
                                                "commodo sagittis vitae nunc diam ac tellus posuere condimentum " +
                                                "enim tellus faucibus suscipit ac nec turpis interdum malesuada " +
                                                "fames primis quisque pretium ex feugiat porttitor massa " +
                                                "vehicula dapibus blandit hendrerit elit aliquet nam orci " +
                                                "fringilla blandit ullamcorper mauris ultrices consequat tempor " +
                                                "convallis gravida sodales volutpat finibus neque pulvinar " +
                                                "varius porta laoreet eu ligula porta placerat lacus pharetra " +
                                                "erat bibendum leo tristique cras rutrum at dui tortor in varius " +
                                                "arcu interdum vestibulum magna ante imperdiet erat luctus odio " +
                                                "non dui volutpat bibendum quam euismod mattis class aptent " +
                                                "taciti sociosqu ad litora torquent per conubia nostra inceptos " +
                                                "himenaeos suspendisse lorem a sem eleifend commodo dolor cursus " +
                                                "luctus lectus";

        private readonly string[] _words;
        private readonly Random _random;

        public LoremIpsumSource()
        {
            _words = _originalString.Split(" ");
            _random = new Random(DateTime.UtcNow.Millisecond);
        }

        public IEnumerable<string> GetWords(int numberOfWords)
        {
            return _words.OrderBy(x => _random.Next(20)).Take(numberOfWords);
        }
    }
}