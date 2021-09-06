using Domain.Models;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IConfigurationsRepository
    {
        public Task<Configuration> GetHotelConfiguration();
    }
}
