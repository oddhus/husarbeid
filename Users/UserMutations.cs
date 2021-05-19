using System.Linq;
using System.Threading.Tasks;
using Authentication;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using husarbeid.Common;
using husarbeid.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            [Service] ITokenService tokenService)
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

        //[UserIsOwner]
        [UseApplicationDbContext]
        public async Task<ClaimTaskPayload> ClaimTask(
            [GlobalStateAttribute("currentUserId")] int? currentUserId,
            ClaimTaskInput input,
            [ScopedService] ApplicationDbContext context
            )
        {
            if (currentUserId == null)
            {
                return new ClaimTaskPayload(
                    new UserError("Not owner", "NOT_ALLOWED"));
            }

            User user = await context.Users.FindAsync(currentUserId);
            if (user == null)
            {
                return new ClaimTaskPayload(
                    new UserError("User not valid", "USER_NOT_VALID"));
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