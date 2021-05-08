using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Users
{
    public class AddUserPayload : UserPayloadBase
    {
        public AddUserPayload(User user, string token)
            : base(user)
        {
            Token = token;
        }

        public AddUserPayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public string? Token { get; }
    }
}