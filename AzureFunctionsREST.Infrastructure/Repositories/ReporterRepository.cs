using AzureFunctionsREST.Domain.Interfaces;
using AzureFunctionsREST.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public class ReporterRepository : GenericRepository<Reporter>, IReporterRepository
    {
        private static readonly string _collectionName = "Reporters";
        public ReporterRepository(IConfiguration configuration)
                : base(_collectionName, configuration)
        { }

    }
}