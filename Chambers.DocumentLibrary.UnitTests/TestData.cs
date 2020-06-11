using System;
using System.Collections.Generic;
using System.Text;
using Chambers.DocumentLibrary.DomainModels;

namespace Chambers.DocumentLibrary.UnitTests
{
  public static  class TestData
    {
         public static Byte[] ImageBytes = new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1, 0xA1 };
         public static List<AttachmentResponse>  Attachments = new List<AttachmentResponse>
        {
            new AttachmentResponse() {FileName = "A", Location = "A", FileSize = "100", Type = "application/pdf"},
            new AttachmentResponse() {FileName = "B", Location = "B", FileSize = "100", Type = "application/pdf"}
        };
    }
}
