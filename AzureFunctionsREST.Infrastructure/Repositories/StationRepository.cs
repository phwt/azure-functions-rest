using AzureFunctionsREST.Domain.Interfaces;
using AzureFunctionsREST.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public class StationRepository : GenericRepository<Station>, IStationRepository
    {
        private static readonly string _collectionName = "Stations";
        public StationRepository(IConfiguration configuration)
                : base(_collectionName, configuration)
        { }

    }
}