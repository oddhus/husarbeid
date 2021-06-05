using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using husarbeid.Common;
using husarbeid.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace husarbeid.Users
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseApplicationDbContext]
        public async Task<AddUserPayload> CreateUserAsync(
            AddUserInput input,
            [ScopedService] ApplicationDbContext context,
            [Service] ITokenService tokenService,
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = input.Name,
                HashedPassword = BC.HashPassword(input.Password),
                BirthDate = input.BirthDate
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return new AddUserPayload(user, tokenService.Create(user));
        }

        [UseApplicationDbContext]
        public LoginUserPayload LoginUser(
            LoginUserInput input,
            [ScopedService] ApplicationDbContext context,
            [Service] ITokenService tokenService)
        {
            var user = context.Users.Where(u => u.Username == input.Name).FirstOrDefault();
            if (user == null || !BC.Verify(input.Password, user.HashedPassword))
            {
                return new LoginUserPayload(
                    new[] { new UserError("Wrong username or password.", "WRONG_USERNAME_OR_PW") });
            }

            return new LoginUserPayload(user, tokenService.Create(user));
        }

        [UseApplicationDbContext]
        public async Task<ClaimTaskPayload> ClaimTask(
            ClaimTaskInput input,
            [GlobalStateAttribute("currentUserId")] int? currentUserId,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (currentUserId == null)
            {
                return new ClaimTaskPayload(
                    new UserError("Please sign in and try again", "NOT_AUTHORIZED"));
            }

            User user = await context.Users.FirstOrDefaultAsync(
                u => u.Id == currentUserId, cancellationToken);

            if (user == null)
            {
                return new ClaimTaskPayload(
                    new UserError("No valid user was found", "USER_NOT_VALID"));
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

            await context.SaveChangesAsync(cancellationToken);

            return new ClaimTaskPayload(user);
        }
    }
}