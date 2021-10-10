namespace InvertedIndex
{
    public record Document(long DocId, LexedDocumentMetadata[] TermArray);
}