using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Model;

namespace DotCDS
{
    internal interface ICooperativeStore
    {
        void SetSQLSettings(PortSettings settings);
        void SetDataSettings(PortSettings settings);
        bool HasLogin(string loginName);
        bool CreateLogin(string userName, string pw);
        bool AddUserToRole(string userName, string roleName);
        bool IsValidLogin(string userName, string pw);
        bool UserIsInRole(string userName, string roleName);
        bool HasTable(string tableName);
        bool HasRole(string roleName);
        bool GenerateHostInformation(string hostName);
        DatabaseHostInfo GetHostInformation();
        bool SavePendingContract(DatabaseContract contract);
        DatabaseContract[] GetPendingContracts();
    }
}
