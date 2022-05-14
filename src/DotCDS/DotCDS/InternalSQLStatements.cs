using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal static partial class InternalSQLStatements
    {
        internal static class SQLKeywords
        {
            internal static class CRUD
            {
                internal const string SELECT = "SELECT";
                internal const string INSERT = "INSERT";
                internal const string UPDATE = "UPDATE";
                internal const string DELETE = "DELETE";
            }
        }

        internal static class TableNames
        {
            /// <summary>
            /// anything in CDS is in the cooperative data store
            /// </summary>
            internal static class CDS
            {
                /// <summary>
                /// users in this CDS 
                /// </summary>
                internal const string USER = "CDS_USER";

                /// <summary>
                /// roles in this CDS
                /// </summary>
                internal const string ROLE = "CDS_ROLE";

                /// <summary>
                /// xref users to roles
                /// </summary>
                internal const string USER_ROLE = "CDS_USER_ROLE";

                // ----------
                // the tables below are for holding our identifier information to other participants
                // ----------

                /// <summary>
                /// holds our unique identifiers to participants
                /// </summary>
                internal const string HOST_INFO = "CDS_HOST_INFO";

                // ----------
                // the tables below are for partial databases and their contracts
                // ----------

                /// <summary>
                /// hosts that this CDS is cooperating with
                /// </summary>
                internal const string HOSTS = "CDS_HOSTS";

                /// <summary>
                /// holds schema information for partial databases participating with a remote host
                /// </summary>
                internal const string CONTRACTS = "CDS_CONTRACTS";

                /// <summary>
                /// holds the tables information for the partial databases 
                /// </summary>
                internal const string CONTRACTS_TABLES = "CDS_CONTRACTS_TABLES";

                /// <summary>
                /// holds the schema information for the tables in partial databases
                /// </summary>
                internal const string CONTRACTS_TABLE_SCHEMAS = "CDS_CONTRACTS_TABLES_SCHEMAS";
            }

            /// <summary>
            /// anything in COOP are tables stored in the user database used to enable cooperative functions with participants
            /// </summary>
            internal static class COOP
            {
                // ----------
                // the tables below are for data hosts
                // ----------

                // a list of participants for this user database
                internal const string PARTICIPANT = "COOP_PARTICIPANT";

                // a list of all the contracts that we have generated for this database
                internal const string DATABASE_CONTRACT = "COOP_DATABASE_CONTRACT";

                // the naming convention prefix for any table that is holding remote row references
                // e.g. if the table is named 'EMPLOYEE_ADDRESS' and is remotable, the actual table holding the participants references would be named
                // 'COOP_SHADOWS_EMPLOYEE_ADDRESS' and would just contain all the participants that would needed to be queried
                // and the 'EMPLOYEE_ADDRESS' would just be an empty table
                internal const string SHADOWS = "COOP_SHADOWS";

                // contains a list of all the tables that have remote data turned on for them
                // this tells CDS that rather than actually sending the request to Sqlite to query that table, 
                // to instead look at the corresponding shadow table and request data from the participant(s)
                internal const string REMOTES = "COOP_REMOTES";

                // contains the database id generated when we first enable cooperative features
                internal const string DATA_HOST = "COOP_DATA_HOST";

                // contains the table ids generated when we start setting logical storage policies on tables
                // this should be aligned with COOP_REMOTES
                internal const string DATA_HOST_TABLES = "COOP_DATA_TABLES";

                // contains the column ids generated when we start setting logical storage policies on tables
                // this needs to be aligned with the actual schema of the table in the database
                internal const string DATA_HOST_TABLE_COLUMNS = "COOP_DATA_COLUMNS";


                // ----------
                // the tables below are for partial databases (data participants)
                // ----------

                // the naming convention prefix for any table that is in a partial database
                // this table holds references back to the host for the rows
                // e.g. a table in a partial database named "EMPLOYEE_ADDRESS" would also have a table 
                // named "COOP_PARENT_EMPLOYEE_ADDRESS" which would point back to the host that had a reference to this table
                internal const string PARENT = "COOP_PARENT";
            }
        }

        internal static class RoleNames
        {
            public const string SYS_ADMIN = "SysAdmin";
        }
    }
}
