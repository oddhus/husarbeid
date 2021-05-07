using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Users
{
    public class AddUserPayload : UserPayloadBase
    {
        public AddUserPayload(User user)
            : base(user)
        {
        }

        public AddUserPayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }
    }
}