using DotCDS.Common;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common.Enum;

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
            var actionResult = _handler.HandleCreateDatabase(request.Authentication.UserName, request.Authentication.Pw, request.DatabaseName);
            result.IsCreated = actionResult.IsSuccessful;
            result.Message = actionResult.Message;

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

        /// <summary>
        /// This may be deprecated - in theory shouldn't we only do reads, and CDS figures out if it's cooperative or not?
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
            var result = new AddParticipantReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.IsSuccessful = _handler.HandleAddParticipant(request.DatabaseName, request.Alias, request.Ip4Address, request.Port);

            return Task.FromResult(result);
        }

        public override Task<ExecuteCooperativeWriteReply> ExecuteCooperativeWrite(ExecuteCooperativeWriteRequest request, ServerCallContext context)
        {
            var result = new ExecuteCooperativeWriteReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;

            result.TotalRowsAffected = _handler.HandleCooperativeWrite(request.Authentication.UserName,
                request.Authentication.Pw, request.Alias, Guid.Parse(request.ParticipantId),
                request.DatabaseName, request.SqlStatement);

            return Task.FromResult(result);
        }

        public override Task<RejectPendingContractReply> RejectPendingContract(RejectPendingContractRequest request, ServerCallContext context)
        {
            var result = new RejectPendingContractReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.IsSuccessful = _handler.HandleRejectPendingContract(request.HostAlias);

            return Task.FromResult(result);
        }

        public override Task<ViewPendingContractsReply> ReviewPendingContracts(ViewPendingContractsRequest request, ServerCallContext context)
        {
            var result = new ViewPendingContractsReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.Contracts.AddRange(_handler.HandleViewPendingContracts());

            return Task.FromResult(result);
        }

        public override Task<SetLogicalStoragePolicyReply> SetLogicalStoragePolicy(SetLogicalStoragePolicyRequest request, ServerCallContext context)
        {
            var result = new SetLogicalStoragePolicyReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;

            LogicalStoragePolicy policy;
            switch (request.PolicyMode)
            {
                case 0:
                    policy = LogicalStoragePolicy.None;
                    break;
                case 1:
                    policy = LogicalStoragePolicy.HostOnly;
                    break;
                case 2:
                    policy = LogicalStoragePolicy.ParticipantOwned;
                    break;
                case 3:
                    policy = LogicalStoragePolicy.Shared;
                    break;
                case 4:
                    policy = LogicalStoragePolicy.Mirror;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            result.IsSuccessful = _handler.HandleSetLogicalStoragePolicy(request.Authentication.UserName,
                request.Authentication.Pw, request.DatabaseName, request.TableName, policy);

            return Task.FromResult(result);
        }

        public override Task<EnableCoooperativeFeaturesReply> EnableCoooperativeFeatures(EnableCoooperativeFeaturesRequest request, ServerCallContext context)
        {
            var result = new EnableCoooperativeFeaturesReply();
            AuthResult authResult = GetAuthResult(request.Authentication);
            result.AuthenticationResult = authResult;
            result.IsSuccessful = _handler.HandleEnableCooperativeFeatures(request.Authentication.UserName,
                request.Authentication.Pw, request.DatabaseName);

            return Task.FromResult(result);
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
