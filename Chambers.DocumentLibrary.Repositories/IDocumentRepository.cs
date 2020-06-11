using System.Collections.Generic;
using System.Net.Http;
using Chambers.DocumentLibrary.DomainModels;

namespace Chambers.DocumentLibrary.Repositories
{
    public interface IDocumentRepository    
    {
        AttachmentResponse Get(string location);
        void Delete(string location);
        List<AttachmentResponse> Get();
        HttpContent GetStream(string location);
    }
}
