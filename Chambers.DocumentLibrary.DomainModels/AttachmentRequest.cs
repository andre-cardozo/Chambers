using System;

namespace Chambers.DocumentLibrary.DomainModels
{
    public class AttachmentRequest
    {
        public string FileName { get; set; }
        public Byte[] Data { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
    }
}
