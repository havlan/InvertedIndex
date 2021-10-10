using System.Collections.Generic;

namespace InvertedIndex
{
    public interface ITermIndex
    {
        bool TryAddTerm(long docId, LexedDocumentMetadata documentMetadata);

        bool TryGetMetadata(string key, out IEnumerable<MetadataEntry> metadata);
    }
}