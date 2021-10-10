using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace InvertedIndex.Tests
{
    public class IndexerTest
    {
        private readonly Indexer _indexer;

        public IndexerTest()
        {
            var logFactory = new LoggerFactory();
            Index index = new Index();
            _indexer = new Indexer(logFactory.CreateLogger("IndexerTest"), index);
        }

        [Fact]
        public void TestIndexDocument()
        {
            var docMetadata = CreateSampleProcessedDoc();
            var doc = new Document(100, docMetadata);
            
            _indexer.IndexDocument(doc);

            var search = _indexer.Search(new[] { "brown" });
            Assert.Single(search);
        }

        [Fact]
        public void TestIndexingOfTwoEqualDocs()
        {
            var docMetadata1 = CreateSampleProcessedDoc();
            var doc1 = new Document(100, docMetadata1);
            var doc2 = new Document(101, docMetadata1);
            
            _indexer.IndexDocument(doc1);
            _indexer.IndexDocument(doc2);

            var search = _indexer.Search(new[] { "brown" });
            Assert.True(search.Count == 2);
            Assert.True(search.All(s => s.Frequency == 1));
        }

        private LexedDocumentMetadata[] CreateSampleProcessedDoc(string sampleText = "The quick brown fox jumps over the lazy dog")
        {
            var afterStemming = sampleText.ToLowerInvariant().Replace("the", "");

            var wordFrequency = new Dictionary<string, int>();
            foreach (var term in afterStemming.Split(' '))
            {
                if (wordFrequency.TryGetValue(term, out var frequency))
                {
                    wordFrequency[term] = frequency + 1;
                }
                else
                {
                    wordFrequency.Add(term, 1);
                }
            }
            
            var docMetadata = new LexedDocumentMetadata[wordFrequency.Count];

            var idx = 0;
            foreach (var term in wordFrequency)
            {
                docMetadata[idx] = new LexedDocumentMetadata(term.Key, term.Value);
                idx++;
            }

            return docMetadata;
        }
    }
}