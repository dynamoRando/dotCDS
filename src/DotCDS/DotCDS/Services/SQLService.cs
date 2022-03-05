using DotCDS.Common;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Services
{
    internal class SQLService : Common.SQLService.SQLServiceBase
    {
        private SQLServiceHandler _handler;

        public override Task<TestReply> IsOnline(TestRequest request, ServerCallContext context)
        {
            var reply = new TestReply();
            reply.ReplyTimeUTC = DateTime.UtcNow.ToString();
            reply.ReplyEchoMessage = request.RequestEchoMessage;
            return Task.FromResult(reply);
        }

        public override Task<SQLQueryReply> ExecuteSQLQuery(SQLQueryRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
            return base.ExecuteSQLQuery(request, context);
        }
    }
}
