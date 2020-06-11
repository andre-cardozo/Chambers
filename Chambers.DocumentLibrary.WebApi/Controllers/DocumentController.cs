using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Chambers.DocumentLibrary.BusinessServices;
using Chambers.DocumentLibrary.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chambers.DocumentLibrary.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private ILogger<DocumentController> _logger;
        private readonly IDocumentService _documentService;

        public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }
        /// <summary>
        /// Return all the documents
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public List<AttachmentResponse> GetAll()
        {
            return _documentService.Get();
        }

        /// <summary>
        /// Return all the documents
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        public HttpResponseMessage Get(string location)
        {
            return _documentService.Get(location);
        }


        /// <summary>
        /// Deletes  document
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public void Delete(string location)
        {
             _documentService.Delete(location);
        }
    }
}
