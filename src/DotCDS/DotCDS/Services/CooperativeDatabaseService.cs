using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common;
using DotCDS.DatabaseClient;
using DotCDS.Query;
using Grpc.Core;

namespace DotCDS.Services
{
    internal class CooperativeDatabaseService : CooperativeDataService.CooperativeDataServiceBase
    {
        #region Private Fields
        private DatabaseServiceHandler _handler;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public CooperativeDatabaseService(DatabaseServiceHandler handler)
        {
            _handler = handler;
        }
        #endregion

        #region Public Methods
        public void SetHandler(DatabaseServiceHandler handler)
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


        public override Task<CreateDatabaseResult> CreatePartialDatabase(CreateDatabaseRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<ParticipantAcceptsContractResult> AcceptContract(ParticipantAcceptsContractRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<CreateTableResult> CreateTableInDatabase(CreateTableRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<GetRowFromPartialDatabaseResult> GetRowFromPartialDatabase(GetRowFromPartialDatabaseRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<GetRowsFromTableResult> GetRowsFromTable(GetRowsFromTableRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<InsertRowResult> InsertRowIntoTable(InsertRowRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<NotifyHostOfRemovedRowResponse> NotifyHostOfRemovedRow(NotifyHostOfRemovedRowRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<RemoveRowFromPartialDatabaseResult> RemoveRowFromPartialDatabase(RemoveRowFromPartialDatabaseRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<SaveContractResult> SaveContract(SaveContractRequest request, ServerCallContext context)
        {
            var result = new SaveContractResult();
            result.IsSaved = _handler.HandleSaveContract(request.Contract);

            return Task.FromResult(result);
        }

        public override Task<UpdateRowDataHashForHostResponse> UpdateRowDataHashForHost(UpdateRowDataHashForHostRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<UpdateRowInTableResult> UpdateRowInTable(UpdateRowInTableRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private AuthResult GetAuthResult(AuthRequest request)
        {
            var authResult = new AuthResult();
            authResult.IsAuthenticated = _handler.IsValidLogin(request.UserName, request.Pw);

            return authResult;
        }
        #endregion



    }
}
