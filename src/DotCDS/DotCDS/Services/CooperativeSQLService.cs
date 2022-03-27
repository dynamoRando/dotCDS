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

        public CooperativeSQLService(SQLServiceHandler handler)
        {
            _handler = handler;
        }

        public override Task<TestReply> IsOnline(TestRequest request, ServerCallContext context)
        {
            var reply = new TestReply();
            reply.ReplyTimeUTC = DateTime.UtcNow.ToString();
            reply.ReplyEchoMessage = request.RequestEchoMessage;
            return Task.FromResult(reply);
        }

        public override Task<CreateUserDatabaseReply> CreateUserDatabase(CreateUserDatabaseRequest request, ServerCallContext context)
        {
            var result = new CreateDatabaseResult();
            var authResult = new AuthResult();
            authResult.IsAuthenticated = _handler.IsValidLogin(request.Authentication.UserName, request.Authentication.Pw);

            result.AuthenticationResult = authResult;
            result.IsSuccessful = _handler.HandleCreateDatabase(request.Authentication.UserName, request.Authentication.Pw, request.DatabaseName);

            throw new NotImplementedException();
        }


    }
}
