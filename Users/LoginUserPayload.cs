using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Users
{
    public class LoginUserPayload : UserPayloadBase
    {
        public LoginUserPayload(User user, string token)
            : base(user)
        {
            Token = token;
        }

        public LoginUserPayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public string? Token { get; }
    }
}