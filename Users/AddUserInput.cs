using System;

namespace husarbeid.Users
{
    public record AddUserInput(
        string Name,
        string Password,
        DateTime? BirthDate);
}