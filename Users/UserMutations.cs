using System.Threading.Tasks;
using Authentication;
using HotChocolate;
using HotChocolate.Types;
using husarbeid.Common;
using husarbeid.Data;
using BC = BCrypt.Net.BCrypt;

namespace husarbeid.Users
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseApplicationDbContext]
        public async Task<AddUserPayload> AddUserAsync(
            AddUserInput input,
            [ScopedService] ApplicationDbContext context,
            [Service] TokenService tokenService)
        {
            var user = new User
            {
                Username = input.Name,
                HashedPassword = BC.HashPassword(input.Password),
                BirthDate = input.BirthDate
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new AddUserPayload(user, tokenService.Create(user));
        }

        [UseApplicationDbContext]
        public async Task<ClaimTaskPayload> ClaimTask(
            ClaimTaskInput input,
            [ScopedService] ApplicationDbContext context)
        {
            User user = await context.Users.FindAsync(input.UserId);

            if (user is null)
            {
                return new ClaimTaskPayload(
                    new UserError("User not found.", "USER_NOT_FOUND"));
            }

            foreach (var taskId in input.TaskIds)
            {
                var newTask = new FamilyTask
                {
                    Id = taskId,
                };
                context.FamilyTasks.Attach(newTask);
                newTask.AssignedToId = user.Id;
            }


            await context.SaveChangesAsync();

            return new ClaimTaskPayload(user);
        }

    }
}