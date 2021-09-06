using Domain.Models;

namespace Persistence.Mappers
{
    public static class ConfigurationsMapper
    {
        public static ConfigurationPersistence AsPersistence(this Configuration configuration)
        {
            if (configuration is null)
                return null;
            return new ConfigurationPersistence
            {
                MaxDaysInAdvance = configuration.MaxDaysInAdvance,
                MaximumReservationDays = configuration.MaximumReservationDays
            };
        }

        public static Configuration AsDomainModel(this ConfigurationPersistence configuration)
        {
            if (configuration is null)
                return null;
            return new Configuration(maxDaysInAdvance: configuration.MaxDaysInAdvance, maximumReservationDays: configuration.MaximumReservationDays);
        }
    }
}
