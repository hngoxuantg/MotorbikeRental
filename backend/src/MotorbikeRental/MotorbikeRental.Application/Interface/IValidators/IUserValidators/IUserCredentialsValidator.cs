using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Interface.IValidators.IUserValidators
{
    public interface IUserCredentialsValidator
    {
        Task<bool> ValidatorForCreate(UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default);
        Task<bool> ValidatorForUpdate(UserCredentialsUpdateDto userCredentialsUpdateDto, CancellationToken cancellationToken = default);
        Task<bool> ValidatorForDelete(int? id, UserCredentials userCredentials, CancellationToken cancellationToken = default);
    }
}