using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace InvertedIndex
{
    public class Indexer
    {
        private readonly ITermIndex _invertedIndex;
        private readonly ILogger _logger;

        public Indexer(ILogger logger, ITermIndex invertedIndex)
        {
            this._logger = logger;
            this._invertedIndex = invertedIndex;
        }

        public void IndexDocument(Document doc)
        {
            if (doc?.TermArray == null)
            {
                throw new ArgumentNullException(nameof(doc), "Document cannot be null");
            }
            
            for (var i=0; i<doc.TermArray.Length; i++)
            {
                if (!_invertedIndex.TryAddTerm(doc.DocId, doc.TermArray[i]))
                {
                    _logger.LogWarning($"Failed to index document {doc.DocId} at position {i}.");
                }
            }
        }

        public List<MetadataEntry> Search(string[] terms)
        {
            if (terms == null || terms.Length == 0)
            {
                throw new ArgumentNullException(nameof(terms), "Search terms cannot be null");
            }

            var indexMetadata = new List<MetadataEntry>();

            for (var i = 0; i < terms.Length; i++)
            {
                if (!_invertedIndex.TryGetMetadata(terms[i], out var metadata))
                {
                    _logger.LogDebug($"Could not find any index entry for term={terms[i]}.");
                    continue;
                }

                indexMetadata.AddRange(metadata);
            }
            
            _logger.LogDebug($"Query of length {terms.Length} returned {indexMetadata.Count} metadata entries.");
            return indexMetadata;
        }
    }
}