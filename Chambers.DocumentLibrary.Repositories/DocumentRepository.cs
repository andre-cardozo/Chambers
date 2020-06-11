using System;
using System.Collections.Generic;
using System.Net.Http;
using Chambers.DocumentLibrary.DomainModels;

namespace Chambers.DocumentLibrary.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        public AttachmentResponse Get(string location)
        {
            //TODO: Add reference to EF and read from DB
            throw new NotImplementedException();
        }

        public void Delete(string location)
        {
            throw new NotImplementedException();
        }

        public List<AttachmentResponse> Get()
        {
            throw new NotImplementedException();
        }

        public HttpContent GetStream(string location)
        {

            throw new NotImplementedException();
        }

        public void Add()
        {
            throw new NotImplementedException();
        }
    }
}