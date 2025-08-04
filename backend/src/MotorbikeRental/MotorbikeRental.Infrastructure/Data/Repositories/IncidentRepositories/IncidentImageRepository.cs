using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.IncidentRepositories
{
    public class IncidentImageRepository : BaseRepository<IncidentImage>, IIncidentImageRepository
    {
        public IncidentImageRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
        public async Task DeleteImages(IEnumerable<IncidentImage> images, CancellationToken cancellationToken)
        {
            dbContext.IncidentImages.RemoveRange(images);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}