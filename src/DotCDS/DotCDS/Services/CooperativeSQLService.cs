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
        #region Private Fields
        private SQLServiceHandler _handler;

        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public CooperativeSQLService(SQLServiceHandler handler)
        {
            _handler = handler;
        }
        #endregion

        #region Public Methods
        public override Task<TestReply> IsOnline(TestRequest request, ServerCallContext context)
        {
            var reply = new TestReply();
            reply.ReplyTimeUTC = DateTime.UtcNow.ToString();
            reply.ReplyEchoMessage = request.RequestEchoMessage;
            return Task.FromResult(reply);
        }

        public override Task<CreateUserDatabaseReply> CreateUserDatabase(CreateUserDatabaseRequest request, ServerCallContext context)
        {
            var result = new CreateUserDatabaseReply();
            AuthResult authResult = GetAuthResult(request.Authentication);

            result.AuthenticationResult = authResult;
            result.IsCreated = _handler.HandleCreateDatabase(request.Authentication.UserName, request.Authentication.Pw, request.DatabaseName);

            return Task.FromResult(result);
        }

        public override Task<ExecuteReadReply> ExecuteRead(ExecuteReadRequest request, ServerCallContext context)
        {
            var result = new ExecuteReadReply();
            AuthResult authResult = GetAuthResult(request.Authentication);

            result.AuthenticationResult = authResult;
            var queryResult = _handler.ExecuteRead(request.Authentication.UserName, request.Authentication.Pw, request.DatabaseName, request.SqlStatement);
            result.Results.Add(queryResult);

            return Task.FromResult(result);
        }

        public override Task<ExecuteWriteReply> ExecuteWrite(ExecuteWriteRequest request, ServerCallContext context)
        {
            var result = new ExecuteWriteReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.TotalRowsAffected = _handler.ExecuteWrite(request.Authentication.UserName, request.Authentication.Pw, request.DatabaseName, request.SqlStatement);

            return Task.FromResult(result);
        }

        public override Task<HasTableReply> HasTable(HasTableRequest request, ServerCallContext context)
        {
            var result = new HasTableReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.HasTable = _handler.HandleHasTable(request.Authentication.UserName, request.Authentication.Pw, request.DatabaseName, request.TableName);

            return Task.FromResult(result);
        }

        public override Task<AcceptPendingContractReply> AcceptPendingContract(AcceptPendingContractRequest request, ServerCallContext context)
        {
            var result = new AcceptPendingContractReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.IsSuccessful = _handler.HandleAcceptPendingContract(request.HostAlias);

            return Task.FromResult(result);
        }

        public override Task<ExecuteCooperativeReadReply> ExecuteCooperativeRead(ExecuteCooperativeReadRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<GenerateContractReply> GenerateContract(GenerateContractRequest request, ServerCallContext context)
        {
            var result = new GenerateContractReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.IsSuccessful = _handler.HandleGenerateContract(request.DatabaseName, request.Description, request.DatabaseName);

            return Task.FromResult(result);
        }

        public override Task<GetLogicalStoragePolicyReply> GetLogicalStoragePolicy(GetLogicalStoragePolicyRequest request, ServerCallContext context)
        {
            var result = new GetLogicalStoragePolicyReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.PolicyMode = (uint)_handler.HandleGetLogicalStoragePolicy(request.DatabaseName, request.TableName);

            return Task.FromResult(result);
        }

        public override Task<AddParticipantReply> AddParticipant(AddParticipantRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<ExecuteCooperativeWriteReply> ExecuteCooperativeWrite(ExecuteCooperativeWriteRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<RejectPendingContractReply> RejectPendingContract(RejectPendingContractRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<ViewPendingContractsReply> ReviewPendingContracts(ViewPendingContractsRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<SetLogicalStoragePolicyReply> SetLogicalStoragePolicy(SetLogicalStoragePolicyRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<EnableCoooperativeFeaturesReply> EnableCoooperativeFeatures(EnableCoooperativeFeaturesRequest request, ServerCallContext context)
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
