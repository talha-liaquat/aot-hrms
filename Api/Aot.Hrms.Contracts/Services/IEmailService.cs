using Aot.Hrms.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aot.Hrms.Contracts.Services
{
    public interface IEmailService
    {
        public bool SendEmail(EmailDto email);
    }
}
