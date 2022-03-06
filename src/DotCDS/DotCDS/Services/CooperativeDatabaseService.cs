using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common;
using Grpc.Core;

namespace DotCDS.Services
{
    internal class CooperativeDatabaseService : CooperativeDataService.CooperativeDataServiceBase
    {
        public override Task<TestReply> IsOnline(TestRequest request, ServerCallContext context)
        {
            var reply = new TestReply();
            reply.ReplyTimeUTC = DateTime.UtcNow.ToString();
            reply.ReplyEchoMessage = request.RequestEchoMessage;
            return Task.FromResult(reply);
        }

    }
}
