using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using husarbeid.Data;
using husarbeid.DataLoader;

namespace husarbeid.Families
{
    [ExtendObjectType(name: "Query")]
    public class FamilyQueries
    {
        [UseApplicationDbContext]
        public Task<List<Family>> GetFamilies([ScopedService] ApplicationDbContext context) =>
            context.Families.ToListAsync();

        [UseApplicationDbContext]
        public Task<Family> GetFamilyAsync(
            [ID(nameof(Family))] int id,
            FamilyByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
                dataLoader.LoadAsync(id, cancellationToken);


    }
}