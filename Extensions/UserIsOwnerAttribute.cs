using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using husarbeid.Data;
using Microsoft.AspNetCore.Mvc;

namespace husarbeid
{
    public class UserIsOwnerAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.Use(next => context =>
            {
                if (!context.ContextData.ContainsKey("currentUserId"))
                {
                    context.ReportError(
                        ErrorBuilder.New()
                        .SetMessage("Must be authorized to perform this action")
                        .SetCode("NOT_AUTHORIZED")
                        .Build());
                    context.Result = new ForbidResult();
                }
                return next(context);
            });
        }
    }
}