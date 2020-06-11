using System;

namespace Chambers.DocumentLibrary.DomainModels
{
    public class AttachmentResponse
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string FileSize { get; set; }
    }
}
