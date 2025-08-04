using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Exceptions
{
    public class ValidatorException : BaseCustomException
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
        public override string ErrorCode => "VALIDATION_ERROR";
        public ValidatorException() { }
        public ValidatorException(string? message) : base(message) { }
        public ValidatorException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}