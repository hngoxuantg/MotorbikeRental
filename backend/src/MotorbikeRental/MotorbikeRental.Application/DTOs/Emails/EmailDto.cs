using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MotorbikeRental.Application.DTOs.Emails
{
    public class EmailDto
    {
        public string To { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}