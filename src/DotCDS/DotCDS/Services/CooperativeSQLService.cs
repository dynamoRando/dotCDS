using DotCDS.Common;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Services
{
    internal class CooperativeSQLService : Common.CooperativeSQLService.CooperativeSQLServiceBase
    {
        private SQLServiceHandler _handler;

        public override Task<TestReply> IsOnline(TestRequest request, ServerCallContext context)
        {
            var reply = new TestReply();
            reply.ReplyTimeUTC = DateTime.UtcNow.ToString();
            reply.ReplyEchoMessage = request.RequestEchoMessage;
            return Task.FromResult(reply);
        }

        public override Task<CreateUserDatabaseReply> CreateUserDatabase(CreateUserDatabaseRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
            return base.CreateUserDatabase(request, context);
        }

        public override Task<StatementReply> ExecuteStatement(StatementRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
            return base.ExecuteStatement(request, context);
        }
    }
}
