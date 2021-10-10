using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using InvertedIndex;

namespace InvertedIndex.Tests {
    public class IndexTest
    {
        private Index _idx;

        private readonly List<int> _defaultPositions = new List<int>()
        {
            0,
        };

        public IndexTest()
        {
            _idx = new Index();
        }

        [Fact]
        public void IndexIsAbleToReturnOk()
        {
            _idx = new Index();
            Assert.True(_idx.TryAddTerm(1, new LexedDocumentMetadata("hello", 1)));
        }
        
        [Fact]
        public void IndexHandlesSeveralInsertionsOfDifferentKey()
        {
            _idx = new Index();
            Assert.True(_idx.TryAddTerm(1, new LexedDocumentMetadata("hello", 1)));
            Assert.True(_idx.TryAddTerm(1, new LexedDocumentMetadata("world", 1)));
            Assert.True(_idx.TryAddTerm(1, new LexedDocumentMetadata("well", 3)));
            Assert.True(_idx.IndexCount == 3);
        }
        
        [Fact]
        public void IndexHandlesSeveralInsertionsOfSameKey()
        {
            _idx = new Index();
            Assert.True(_idx.TryAddTerm(1, new LexedDocumentMetadata("hello", 1)));
            Assert.True(_idx.TryAddTerm(2, new LexedDocumentMetadata("hello", 2)));
            Assert.True(_idx.TryAddTerm(3, new LexedDocumentMetadata("hello", 1)));
            Assert.True(_idx.IndexCount == 1);
            Assert.True(_idx.TryGetMetadata("hello", out var metadata));
            Assert.True(metadata.Count() == 3);
        }
    }
}