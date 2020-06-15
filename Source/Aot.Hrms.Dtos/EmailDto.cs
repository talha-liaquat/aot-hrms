using System;
using System.Collections.Generic;

namespace Aot.Hrms.Dtos
{
    public class EmailDto
    {
        public EmailDto()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
        }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public List<string> Attachments { get; set; }
    }
}
