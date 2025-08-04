using MotorbikeRental.Domain.Entities.Incidents;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents
{
    public interface IIncidentImageRepository : IBaseRepository<IncidentImage>
    {
        Task DeleteImages(IEnumerable<IncidentImage> images, CancellationToken cancellationToken);
    }
}