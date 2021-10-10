using System.Collections.Generic;
using System.Linq;

namespace InvertedIndex
{
    public class Index : ITermIndex
    {
        private readonly Dictionary<string, List<MetadataEntry>> _index;

        public Index()
        {
            _index = new Dictionary<string, List<MetadataEntry>>();
        }

        public bool TryAddTerm(long docId, LexedDocumentMetadata lexedDocument)
        {
            var metadata = new MetadataEntry(docId, lexedDocument.Frequency);

            if (_index.TryGetValue(lexedDocument.Term, out var currentMetadata))
            {
                currentMetadata.Add(metadata);
                _index[lexedDocument.Term] = currentMetadata;
            }
            else
            {
                if (!_index.TryAdd(lexedDocument.Term, new List<MetadataEntry>()
                {
                    metadata,
                }))
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryGetMetadata(string key, out IEnumerable<MetadataEntry> metadata)
        {
            if (_index.TryGetValue(key, out var idxMetadata))
            {
                metadata = idxMetadata;
                return true;
            }
            
            metadata = null;
            return false;
        }

        public long IndexCount => _index.Count;
    }
}