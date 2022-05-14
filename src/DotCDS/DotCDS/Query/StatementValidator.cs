using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using a = Antlr4.Runtime.Misc;

namespace DotCDS.Query
{
    /// <summary>
    /// Extends the <see cref="TSqlParserBaseListener"/> class and overloads any method calls of interest for parsing
    /// </summary>
    /// <remarks>This is part of the Antlr generated code. See IQueryManager.md for more information.</remarks>
    internal class StatementValidator : TSqlParserBaseListener
    {
        #region Private Fields
        ICharStream _charStream;
        private string _tableName = string.Empty;
        #endregion

        #region Public Fields
        #endregion

        #region Constructors
        internal StatementValidator()
        {

        }
        #endregion

        #region Public Properties
        public CommonTokenStream TokenStream { get; set; }
        #endregion

        #region Public Methods
        public override void EnterDrop_table([NotNull] TSqlParser.Drop_tableContext context)
        {
            base.EnterDrop_table(context);
            DebugContext(context);

            throw new NotImplementedException();
        }

        public override void EnterColumn_name_list(TSqlParser.Column_name_listContext context)
        {
            base.EnterColumn_name_list(context);
            DebugContext(context);

            throw new NotImplementedException();
        }

        public override void EnterFull_table_name([NotNull] TSqlParser.Full_table_nameContext context)
        {
            base.EnterFull_table_name(context);
            DebugContext(context);

            throw new NotImplementedException();

        }

        public override void EnterSearch_condition([NotNull] TSqlParser.Search_conditionContext context)
        {
            base.EnterSearch_condition(context);
            DebugContext(context);

        }

        public override void EnterSelect_list([NotNull] TSqlParser.Select_listContext context)
        {
            base.EnterSelect_list(context);
            DebugContext(context);

            throw new NotImplementedException();
        }

        public override void EnterSelect_list_elem([NotNull] TSqlParser.Select_list_elemContext context)
        {
            base.EnterSelect_list_elem(context);
            DebugContext(context);

            throw new NotImplementedException();
        }

        public override void EnterSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            base.EnterSelect_statement(context);
            DebugContext(context);

            throw new NotImplementedException();
        }

        public override void EnterSimple_name([NotNull] TSqlParser.Simple_nameContext context)
        {
            base.EnterSimple_name(context);
            DebugContext(context);
        }

        public override void EnterSql_clauses([NotNull] TSqlParser.Sql_clausesContext context)
        {
            base.EnterSql_clauses(context);
            DebugContext(context);
        }

        public override void EnterTable_name([NotNull] TSqlParser.Table_nameContext context)
        {
            base.EnterTable_name(context);
            DebugContext(context);

            throw new NotImplementedException();
        }

        public override void ExitSearch_condition([NotNull] TSqlParser.Search_conditionContext context)
        {
            base.ExitSearch_condition(context);
            DebugContext(context);
        }

        public override void ExitSelect_list([NotNull] TSqlParser.Select_listContext context)
        {
            base.ExitSelect_list(context);
            DebugContext(context);
        }

        public override void ExitSelect_list_elem([NotNull] TSqlParser.Select_list_elemContext context)
        {
            base.ExitSelect_list_elem(context);
            DebugContext(context);
        }

        public override void ExitSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            base.ExitSelect_statement(context);
            DebugContext(context);
        }
        public override void ExitSql_clauses([NotNull] TSqlParser.Sql_clausesContext context)
        {
            base.ExitSql_clauses(context);
            DebugContext(context);
        }
        public override void ExitTable_name([NotNull] TSqlParser.Table_nameContext context)
        {
            base.ExitTable_name(context);
            DebugContext(context);

            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        [Conditional("DEBUG")]
        private void DebugContext(ParserRuleContext context)
        {
            string debug = context.GetText();
            string fullText = GetWhiteSpaceFromCurrentContext(context);
            string callingMethod = new StackFrame(1, true).GetMethod().Name;

            Debug.WriteLine(callingMethod);
            Debug.WriteLine(debug);
            Debug.WriteLine(fullText);
        }

        private string GetWhiteSpaceFromCurrentContext(ParserRuleContext context)
        {
            int a = context.Start.StartIndex;
            int b = context.Stop.StopIndex;
            a.Interval interval = new a.Interval(a, b);
            _charStream = context.Start.InputStream;
            return _charStream.GetText(interval);
        }
        #endregion
    }
}
