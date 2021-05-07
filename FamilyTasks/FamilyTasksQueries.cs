using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using husarbeid.Data;
using husarbeid.DataLoader;

namespace husarbeid.FamilyTasks
{
    [ExtendObjectType(name: "Query")]
    public class FamilyTaskQueries
    {
        [UseApplicationDbContext]
        public Task<List<FamilyTask>> GetFamilyTasks([ScopedService] ApplicationDbContext context) =>
            context.FamilyTasks.ToListAsync();

        [UseApplicationDbContext]
        public Task<FamilyTask> GetFamilyTaskAsync(
            [ID(nameof(FamilyTask))] int id,
            FamilyTaskByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
                dataLoader.LoadAsync(id, cancellationToken);
    }
}