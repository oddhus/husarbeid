using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Users
{
    public class GetUserPayload : UserPayloadBase
    {
        public GetUserPayload(User user)
            : base(user)
        {
        }

        public GetUserPayload(UserError error)
            : base(new[] { error })
        {
        }

    }
}