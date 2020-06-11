using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Chambers.DocumentLibrary.DomainModels;
using Chambers.DocumentLibrary.Repositories;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;

namespace Chambers.DocumentLibrary.BusinessServices
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepo;

        public DocumentService(IDocumentRepository documentRepo)
        {
            _documentRepo = documentRepo;
        }
        public void Add(AttachmentRequest attachment)
        {
            //valid attachment type.

            if (ValidateAttachment(attachment, "application/pdf"))
            {

            }


        }

        /// <summary>
        /// Validates if an attachment is of a mime type
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="applicationPdf"></param>
        private bool ValidateAttachment(AttachmentRequest attachment, string applicationPdf)
        {
            //TODO: the file type needs to be got from the byte array and not the extension as this can be faked.
            //for the test purpose, I'm using the file extension :-)
            if (attachment.Type != applicationPdf)
                throw new NotSupportedException("Invalid file. Only PDF's are supported");

            var MaxFileSize = 5; //move to web.config
            if (GetSizeInMB(attachment.Data) > MaxFileSize)
                throw new NotSupportedException("Max file size is 5 MB");

            return true;
        }

        /// <summary>
        /// return the size of the byte array in MB
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private decimal GetSizeInMB(Byte[] input)
        {
            return (decimal)input.Length / 1048576;
        }


        /// <summary>
        /// Reorders a list of attachments on a property
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="reorderField"></param>
        public ICollection<AttachmentResponse> Reorder(List<AttachmentResponse> attachments, string reorderField, Enums.OrderDirection direction)
        {
            return direction == Enums.OrderDirection.Asc ? attachments.AsQueryable().OrderBy(reorderField).ToList() : attachments.AsQueryable().OrderByDescending(d => d.FileName).ToList();
        }

        public List<AttachmentResponse> Get()
        {
          return  _documentRepo.Get();
        }
        public HttpResponseMessage Get(string location)
        {
            //Assumption: documents are stored in S3 bucket, blob or file system and no in the DB
            var docToReturn = _documentRepo.Get(location);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = _documentRepo.GetStream(location)
            };
            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment") { FileName = docToReturn.FileName };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return response;
        }


        public void Delete(string chosenPdfLocation)
        {
           //delete the document from storage and remove the record from DB
           _documentRepo.Delete(chosenPdfLocation);
        }
    }

    public interface IDocumentService
    {
        void Add(AttachmentRequest attachment);

        ICollection<AttachmentResponse> Reorder(List<AttachmentResponse> attachments, string reorderField,
            Enums.OrderDirection direction);

        List<AttachmentResponse> Get();
        HttpResponseMessage Get(string location);
        void Delete(string location);
    }
}
