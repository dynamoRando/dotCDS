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

        /// <summary>
        /// Walks the SQL Statement and determines if any of the objects in the statement reference a cooperative object
        /// </summary>
        /// <param name="statement">The SQL Statement to parse</param>
        /// <returns><c>TRUE</c> if any objects are cooperative, otherwise <c>FALSE</c></returns>
        public ActionResult<bool> HasCooperativeObjects(string statement)
        {
            var actionResult = new ActionResult<bool>();
            actionResult.IsSuccessful = true;

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

            _generator.TokenStream = tokens;
            _generator.UserDatabaseManager = _databaseManger;

            _walker.Walk(_generator, tree);

            // always remove the error handler
            parser.RemoveErrorListener(errorHandler);

            if (errorHandler.Errors.Count > 0)
            {
                actionResult.IsSuccessful = false;

                var error = errorHandler.Errors.First();
                string errorMesage = $"Syntax Error: {error.Message} at {error.Line}:{error.CharPositionInLine}";
                actionResult.Message = errorMesage;
            }

            actionResult.Result = _generator.HasCooperativeReferences;

            return actionResult;
        }

        /// <summary>
        /// Walks the SQL Statement and returns a collection of objects that are cooperating and the columns needed
        /// </summary>
        /// <param name="statement">The SQL Statement to parse</param>
        /// <returns>A collection of cooperative objects and their columns</returns>
        public ActionResult<CooperativeReferenceCollection> GetCooperativeReferences(string statement)
        {
            var actionResult = new ActionResult<CooperativeReferenceCollection>();
            actionResult.IsSuccessful = true;

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

            _generator.TokenStream = tokens;
            _generator.UserDatabaseManager = _databaseManger;
            _generator.CooperativeReferenceCollection = new CooperativeReferenceCollection();

            _walker.Walk(_generator, tree);

            // always remove the error handler
            parser.RemoveErrorListener(errorHandler);

            if (errorHandler.Errors.Count > 0)
            {
                actionResult.IsSuccessful = false;

                var error = errorHandler.Errors.First();
                string errorMesage = $"Syntax Error: {error.Message} at {error.Line.ToString()}:{error.CharPositionInLine.ToString()}";
                actionResult.Message = errorMesage;
            }

            actionResult.Result = _generator.CooperativeReferenceCollection;
            return actionResult;
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
