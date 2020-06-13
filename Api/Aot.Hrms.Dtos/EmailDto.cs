using System;
using System.Collections.Generic;

namespace Aot.Hrms.Dtos
{
    public class EmailDto
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string IsHtml { get; set; }
        public List<string> Attachments { get; set; }
    }
}
