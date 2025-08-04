using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class RoleService : IRoleService
    {
        private readonly IMapper mapper;
        private readonly RoleManager<Roles> roleManager;
        public RoleService(IMapper mapper, RoleManager<Roles> roleManager)
        {
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        public async Task<IEnumerable<RoleDto>> GetAllRoles(CancellationToken cancellationToken = default)
        {
            return mapper.Map<IEnumerable<RoleDto>>(await roleManager.Roles.ToListAsync());
        }
        public async Task<RoleDto> GetRoleById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<RoleDto>(await roleManager.FindByIdAsync(id.ToString()));
        }
    }
}