using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common;
using Grpc.Core;

namespace DotCDS.Services
{
    internal class CooperativeAdminService : AdminService.AdminServiceBase
    {
        public override Task<TestReply> IsOnline(TestRequest request, ServerCallContext context)
        {
            return base.IsOnline(request, context);
        }
    }
}
