using System.Collections.Generic;
using husarbeid.Common;
using husarbeid.Data;

namespace husarbeid.Users
{
    public class ClaimTaskPayload : UserPayloadBase
    {
        public ClaimTaskPayload(User user)
            : base(user)
        {
        }

        public ClaimTaskPayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}