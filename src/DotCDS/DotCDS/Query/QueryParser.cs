using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotCDS.Query.Enum;

namespace DotCDS.Query
{
    internal class QueryParser
    {
        #region Private Fields
        private SqliteUserDatabaseManager _databaseManger;
        ParseTreeWalker _walker;
        QueryPlanGeneratorBase _generator;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public void SetSqliteDatabaseManager(SqliteUserDatabaseManager manager)
        {
            _databaseManger = manager;
        }

        public ActionResult<bool> IsReadStatement(string statement, string databaseName)
        {
            var action = new ActionResult<bool>();
            action.IsSuccessful = true;

            if (statement.Contains(InternalSQLStatements.SQLKeywords.CRUD.SELECT, StringComparison.OrdinalIgnoreCase))
            {
                action.Result = true;
            }
            else
            {
                action.Result = false;
            }

            return action;
        }

        public ActionResult<bool> IsWriteStatement(string statement, string databaseName)
        {
            var action = new ActionResult<bool>();
            action.IsSuccessful = true;

            if (statement.Contains(InternalSQLStatements.SQLKeywords.CRUD.INSERT, StringComparison.OrdinalIgnoreCase) ||
                statement.Contains(InternalSQLStatements.SQLKeywords.CRUD.UPDATE, StringComparison.OrdinalIgnoreCase) ||
                statement.Contains(InternalSQLStatements.SQLKeywords.CRUD.DELETE, StringComparison.OrdinalIgnoreCase)
                )
            {
                action.Result = true;
            }
            else
            {
                action.Result = false;
            }

            return action;
        }

        /// <summary>
        /// Walks the SQL Statement and determines if any of the objects in the statement reference a cooperative object
        /// </summary>
        /// <param name="statement">The SQL Statement to parse</param>
        /// <param name="databaseName">The name of the database the statement is for</param>
        /// <returns><c>TRUE</c> if any objects are cooperative, otherwise <c>FALSE</c></returns>
        public ActionResult<bool> HasCooperativeObjects(string statement, string databaseName)
        {
            var action = new ActionResult<bool>();
            action.IsSuccessful = true;

            if (_walker is null)
            {
                _walker = new ParseTreeWalker();
            }

            // not sure if there's a way to not have to allocate new objects each time we evaluate a SQL statement
            AntlrInputStream inputStream = new AntlrInputStream(statement);
            var caseStream = new CaseChangingCharStream(inputStream, true);
            TSqlLexer lexer = new TSqlLexer(caseStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            TSqlParser parser = new TSqlParser(tokens);

            var errorHandler = new SyntaxErrorListener();
            parser.AddErrorListener(errorHandler);

            IParseTree tree = null;
            var type = GetStatementType(statement);

            switch (type)
            {
                case StatementType.DML:
                    tree = parser.dml_clause();
                    break;
                case StatementType.DDL:
                    tree = parser.ddl_clause();
                    break;
                case StatementType.Unknown:
                default:
                    action.IsSuccessful = false;
                    action.Message = "Unable to determine type of statement";
                    action.Result = false;
                    return action;
            }

            if (_generator is null)
            {
                _generator = new QueryPlanGeneratorBase();
            }

            _generator.TokenStream = tokens;
            _generator.UserDatabaseManager = _databaseManger;
            _generator.DatabaseName = databaseName;
            _generator.CooperativeReferenceCollection = null;
            _generator.HasCooperativeReferencesCheck = new CooperativeReferenceCheckResult();
            _generator.HasCooperativeReferencesCheck.DatabaseName = databaseName;

            _walker.Walk(_generator, tree);

            // always remove the error handler
            parser.RemoveErrorListener(errorHandler);

            if (errorHandler.Errors.Count > 0)
            {
                action.IsSuccessful = false;

                var error = errorHandler.Errors.First();
                string errorMesage = $"Syntax Error: {error.Message} at {error.Line}:{error.CharPositionInLine}";
                action.Message = errorMesage;
            }

            var checkResults = _generator.HasCooperativeReferencesCheck;
            if (checkResults.References.Any(reference => reference.IsCooperating))
            {
                action.Result = true;
            }
            return action;
        }

        /// <summary>
        /// Walks the SQL Statement and returns a collection of objects that are cooperating and the columns needed
        /// </summary>
        /// <param name="statement">The SQL Statement to parse</param>
        /// <param name="databaseName">The name of the database the statement is for</param>
        /// <returns>A collection of cooperative objects and their columns</returns>
        public ActionResult<CooperativeReferenceCollection> GetCooperativeReferences(string statement, string databaseName)
        {
            var action = new ActionResult<CooperativeReferenceCollection>();
            action.IsSuccessful = true;

            if (_walker is null)
            {
                _walker = new ParseTreeWalker();
            }

            // not sure if there's a way to not have to allocate new objects each time we evaluate a SQL statement
            AntlrInputStream inputStream = new AntlrInputStream(statement);
            var caseStream = new CaseChangingCharStream(inputStream, true);
            TSqlLexer lexer = new TSqlLexer(caseStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            TSqlParser parser = new TSqlParser(tokens);

            var errorHandler = new SyntaxErrorListener();
            parser.AddErrorListener(errorHandler);

            IParseTree tree = null;
            var type = GetStatementType(statement);

            if (type == StatementType.DML)
            {
                tree = parser.dml_clause();
            }
            else if (type == StatementType.DDL)
            {
                tree = parser.ddl_clause();
            }

            if (_generator is null)
            {
                _generator = new QueryPlanGeneratorBase();
            }

            _generator.TokenStream = tokens;
            _generator.UserDatabaseManager = _databaseManger;
            _generator.CooperativeReferenceCollection = new CooperativeReferenceCollection(databaseName);
            _generator.HasCooperativeReferencesCheck = null;
            _generator.DatabaseName = databaseName;

            _walker.Walk(_generator, tree);

            // always remove the error handler
            parser.RemoveErrorListener(errorHandler);

            if (errorHandler.Errors.Count > 0)
            {
                action.IsSuccessful = false;

                var error = errorHandler.Errors.First();
                string errorMesage = $"Syntax Error: {error.Message} at {error.Line.ToString()}:{error.CharPositionInLine.ToString()}";
                action.Message = errorMesage;
            }

            action.Result = _generator.CooperativeReferenceCollection;
            return action;
        }
        #endregion

        #region Private Methods
        private StatementType GetStatementType(string statement)
        {
            var ddlKeywords = DDLKeywords.Get();
            var statementItems = statement.Split(" ");

            foreach (var word in statementItems)
            {
                if (ddlKeywords.Any(keyword => string.Equals(keyword, word, StringComparison.OrdinalIgnoreCase)))
                {
                    return StatementType.DDL;
                }
            }

            return StatementType.DML;
        }

        #endregion
    }
}
