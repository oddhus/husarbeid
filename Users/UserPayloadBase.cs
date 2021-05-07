using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Users
{
    public class UserPayloadBase : Payload
    {
        protected UserPayloadBase(User user)
        {
            User = user;
        }

        protected UserPayloadBase(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }

        public User? User { get; }
    }
}