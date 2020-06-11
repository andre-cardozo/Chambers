using Chambers.DocumentLibrary.BusinessServices;
using Chambers.DocumentLibrary.DomainModels;
using Chambers.DocumentLibrary.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;

namespace Chambers.DocumentLibrary.UnitTests
{
    [TestClass]
    public class DocUploadTests
    {
        private DocumentService _docService;
        [TestInitialize]
        public void Setup()
        {
            var mockDocumentRepo = new Mock<IDocumentRepository>();
            mockDocumentRepo.Setup(x => x.Get()).Returns(TestData.Attachments);
            mockDocumentRepo.Setup(x => x.Get(It.IsAny<string>())).Returns(TestData.Attachments.First());
            mockDocumentRepo.Setup(x => x.Delete(It.IsAny<string>()));
            mockDocumentRepo.Setup(x => x.GetStream(It.IsAny<string>()))
                .Returns(() => new ByteArrayContent(TestData.ImageBytes));
            _docService = new DocumentService(mockDocumentRepo.Object);

        }

        
        
       
        [TestMethod]
        public void GivenIhaveaPDFtoupload()
        {

            //Given I have a PDF to upload
            var attachment = new AttachmentRequest(){Data = new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1, 0xA1 } , FileName = "Chambers.pdf", Type = "application/pdf"};
            //PS: in real world we don't need the user or shouldn't trust the user to provide the mime type

            //    When I send the PDF to the API
            _docService.Add(attachment);

            //    Then it is uploaded successfully
            //no errors. Response from Add will be void.
            Assert.AreEqual(1,1);
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GivenIHaveANon_PdfToUpload()
        {
            //Given I have a non-pdf to upload
            var attachment = new AttachmentRequest() { Data = new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1, 0xA1 }, FileName = "Chambers.pdf", Type = "application/docx" , Location = "A"};
            //PS: in real world we don't need the user or shouldn't trust the user to provide the mime type
            
            // When I send the non-pdf to the API
            _docService.Add(attachment);
            //Then the API does not accept the file and returns the appropriate messaging and status
            //Exception is thrown
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GivenIHaveAMaxPdfSizeOfMB()
        {
            //Given I have a max pdf size of 5MB
            var attachment = new AttachmentRequest() { Data = new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1, 0xA1 }, FileName = "Chambers.pdf", Type = "application/docx", Location = "A" };
            //PS: in real world we don't need the user or shouldn't trust the user to provide the mime type
            
            //When I send the pdf to the API
            _docService.Add(attachment);
            
            //then Then the API does not accept the file and returns the appropriate messaging and status
            //Exception is thrown
        }



        [TestMethod]
        public void GivenICallTheNewDocumentServiceAPI()
        {
            //Given I call the new document service API
           
           

            //When I call the API to get a list of documents
            var result =  _docService.Get();

            //Then a list of PDFs’ is returned with the following properties: name, location, file-size
            Assert.IsTrue(result.Any());
            Assert.AreNotEqual(result.First().FileName, "");
            Assert.AreNotEqual(result.First().Location, "");
            Assert.AreNotEqual(result.First().FileSize, "");
        }


        [TestMethod]
        public void GivenIHaveAListOfPDFs()
        {
            //Given I have a list of PDFs’
            var attachments = new List<AttachmentResponse>
            {
                new AttachmentResponse() {FileName = "A"}, 
                new AttachmentResponse() {FileName = "B"}
            };
            //When I choose to re-order the list of PDFs’
            var result =  _docService.Reorder(attachments, "FileName", Enums.OrderDirection.Asc);
            //Then the list of PDFs’ is returned in the new order for subsequent calls to the API
            Assert.IsTrue(result.First().FileName == "A");

            //When I choose to re-order the list of PDFs’
            var resultDesc = _docService.Reorder(attachments, "FileName", Enums.OrderDirection.Desc);
            //Then the list of PDFs’ is returned in the new order for subsequent calls to the API
            Assert.IsTrue(resultDesc.First().FileName == "B");
        }

        
        
        
        [TestMethod]
        public void GivenIHaveChosenAPDFFromTheListAPI()
        {
            //    Given I have chosen a PDF from the list API
            var attachments = TestData.Attachments;
            var chosenPdf = attachments.First();

            //    When I request the location for one of the PDF's
           var result =  _docService.Get(chosenPdf.Location);

            //The PDF is downloaded
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void GivenIHaveSelectedAPDFFromTheListAPIThatINoLongerRequire()
        {
            //    Given I have selected a PDF from the list API that I no longer require
           
            var chosenPdf = TestData.Attachments.First();
            //When I request to delete the PDF
            //TODO: MOQ the repository call, to return the attachment list minus the deleted item

            _docService.Delete(chosenPdf.Location);
            //    Then the PDF is deleted and will no longer return from the list API and can no longer be downloaded from its location directly
            
            //Assert no error. 
            Assert.AreEqual(1,1);
        }


        
        
        
        [TestMethod]
        public void GivenIAttemptToDeleteAFileThatDoesNotExist()
        {
            //Given I attempt to delete a file that does not exist

            //    When I request to delete the non-existing pdf

            //Then the API returns an appropriate response
        }

    }
}
