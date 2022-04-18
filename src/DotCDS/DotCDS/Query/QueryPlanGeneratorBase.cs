using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Diagnostics;
using System.Linq;
using a = Antlr4.Runtime.Misc;

namespace DotCDS.Query
{
    /// <summary>
    /// Extends the <see cref="TSqlParserBaseListener"/> class and overloads any method calls of interest for query plan generation
    /// </summary>
    /// <remarks>This is part of the Antlr generated code. See IQueryManager.md for more information.</remarks>
    internal class QueryPlanGeneratorBase : TSqlParserBaseListener
    {
        #region Private Fields
        ICharStream _charStream;
        #endregion

        #region Public Fields

        #endregion

        #region Public Properties
        public CommonTokenStream TokenStream { get; set; }
        public SqliteUserDatabaseManager UserDatabaseManager { get; set; }
        public CooperativeReferenceCollection CooperativeReferenceCollection { get; set; }
        public CooperativeReferenceCheckResult HasCooperativeReferencesCheck { get; set; }
        public string DatabaseName { get; set; }
        #endregion

        #region Constructors
        public QueryPlanGeneratorBase()
        {

        }
        #endregion

        #region Public Methods
        public override void EnterDrop_table([NotNull] TSqlParser.Drop_tableContext context)
        {
            base.EnterDrop_table(context);
            DebugContext(context);

        }

        public override void ExitDrop_table([NotNull] TSqlParser.Drop_tableContext context)
        {
            base.ExitDrop_table(context);

            
        }

        public override void EnterColumn_definition([NotNull] TSqlParser.Column_definitionContext context)
        {
            base.EnterColumn_definition(context);

            
        }

        public override void EnterColumn_name_list(TSqlParser.Column_name_listContext context)
        {
            base.EnterColumn_name_list(context);
            DebugContext(context);

            
        }

        public override void EnterCreate_database([NotNull] TSqlParser.Create_databaseContext context)
        {
            base.EnterCreate_database(context);
            DebugContext(context);
        }

        public override void EnterCreate_schema([NotNull] TSqlParser.Create_schemaContext context)
        {
            base.EnterCreate_schema(context);
            DebugContext(context);

        }

        public override void EnterCreate_table([NotNull] TSqlParser.Create_tableContext context)
        {
            base.EnterCreate_table(context);
            DebugContext(context);


        }

        public override void EnterData_type([NotNull] TSqlParser.Data_typeContext context)
        {
            base.EnterData_type(context);
            DebugContext(context);

        }

        public override void EnterDelete_statement([NotNull] TSqlParser.Delete_statementContext context)
        {
            base.EnterDelete_statement(context);
            DebugContext(context);


        }

        public override void EnterDrop_database([NotNull] TSqlParser.Drop_databaseContext context)
        {
            base.EnterDrop_database(context);
            DebugContext(context);
        }

        public override void EnterExpression([NotNull] TSqlParser.ExpressionContext context)
        {
            base.EnterExpression(context);
            DebugContext(context);

            
        }

        public override void EnterExpression_list([NotNull] TSqlParser.Expression_listContext context)
        {
            base.EnterExpression_list(context);
            DebugContext(context);
        }

        public override void EnterFull_column_name([NotNull] TSqlParser.Full_column_nameContext context)
        {
            base.EnterFull_column_name(context);
            DebugContext(context);
        }

        public override void EnterFull_table_name([NotNull] TSqlParser.Full_table_nameContext context)
        {
            base.EnterFull_table_name(context);
            DebugContext(context);

            var db = GetCurrentDatabase();
            if (db is not null)
            {
                var tableName = new ContextWrapper(context, _charStream).FullText.Trim();
                bool isCooperating = db.IsTableCooperative(tableName);

                if (HasCooperativeReferencesCheck is not null)
                {
                    HasCooperativeReferencesCheck.References.Add
                    (new CooperativeReferenceCheck
                    {
                        DatabaseName = db.DatabaseName,
                        TableName = tableName,
                        IsCooperating = isCooperating
                    });
                }

                if (CooperativeReferenceCollection is not null)
                {
                    if (CooperativeReferenceCollection.CurrentReference is null)
                    {
                        var reference = new CooperativeReference();
                        reference.DatabaseName = DatabaseName;
                        reference.TableName = tableName;
                        CooperativeReferenceCollection.CurrentReference = reference;
                    }
                    else
                    {
                        var reference = CooperativeReferenceCollection.CurrentReference;
                        if (!reference.IsTableNameSet())
                        {
                            reference.TableName = tableName;
                        }
                    }
                }
            }
        }

        public override void EnterId_([NotNull] TSqlParser.Id_Context context)
        {
            base.EnterId_(context);
            DebugContext(context);
        }

        public override void EnterInsert_column_id([NotNull] TSqlParser.Insert_column_idContext context)
        {
            base.EnterInsert_column_id(context);

            string debug = context.GetText();
            DebugContext(context);

        }

        public override void EnterInsert_statement([NotNull] TSqlParser.Insert_statementContext context)
        {
            base.EnterInsert_statement(context);
            DebugContext(context);

        }

        public override void EnterNull_notnull([NotNull] TSqlParser.Null_notnullContext context)
        {
            base.EnterNull_notnull(context);
            DebugContext(context);

        }

        // will need to adjust this code to handle multiple predicates
        // and also booleans
        public override void EnterPredicate([NotNull] TSqlParser.PredicateContext context)
        {
            base.EnterPredicate(context);
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

        }

        public override void EnterSelect_list_elem([NotNull] TSqlParser.Select_list_elemContext context)
        {
            base.EnterSelect_list_elem(context);
            DebugContext(context);

            var conWrapper = new ContextWrapper(context, _charStream);

            if (CooperativeReferenceCollection is not null)
            {
                if (CooperativeReferenceCollection.CurrentReference is not null)
                {
                    var reference = CooperativeReferenceCollection.CurrentReference;
                    if (!reference.IsDatabaseNameSet())
                    {
                        reference.DatabaseName = DatabaseName;
                    }

                    if (reference.Columns is null)
                    {
                        reference.Columns = new List<string>();
                    }

                    reference.Columns.Add(conWrapper.Debug.Trim());
                }
                else
                {
                    var reference = new CooperativeReference();
                    reference.DatabaseName = DatabaseName;

                    if (reference.Columns is null)
                    {
                        reference.Columns = new List<string>();
                    }

                    reference.Columns.Add(conWrapper.Debug.Trim());

                    CooperativeReferenceCollection.CurrentReference = reference;
                }
            }
        }

        public override void EnterSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            base.EnterSelect_statement(context);
            DebugContext(context);

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

        public override void EnterTable_alias([NotNull] TSqlParser.Table_aliasContext context)
        {
            base.EnterTable_alias(context);
            DebugContext(context);

        }

        public override void EnterTable_name([NotNull] TSqlParser.Table_nameContext context)
        {
            base.EnterTable_name(context);
            DebugContext(context);

        }

        public override void EnterUpdate_elem([NotNull] TSqlParser.Update_elemContext context)
        {
            base.EnterUpdate_elem(context);
            DebugContext(context);

        }

        public override void EnterUpdate_statement([NotNull] TSqlParser.Update_statementContext context)
        {
            base.EnterUpdate_statement(context);
            DebugContext(context);

        }

        public override void ExitColumn_definition([NotNull] TSqlParser.Column_definitionContext context)
        {
            base.ExitColumn_definition(context);
            DebugContext(context);

        }

        public override void ExitCreate_database([NotNull] TSqlParser.Create_databaseContext context)
        {
            base.ExitCreate_database(context);
            DebugContext(context);
        }

        public override void ExitCreate_schema([NotNull] TSqlParser.Create_schemaContext context)
        {
            base.ExitCreate_schema(context);
            DebugContext(context);
        }

        public override void ExitCreate_table([NotNull] TSqlParser.Create_tableContext context)
        {
            base.ExitCreate_table(context);
            DebugContext(context);
        }

        public override void ExitDelete_statement([NotNull] TSqlParser.Delete_statementContext context)
        {
            base.ExitDelete_statement(context);
            DebugContext(context);

        }

        public override void ExitDrop_database([NotNull] TSqlParser.Drop_databaseContext context)
        {
            base.ExitDrop_database(context);
            DebugContext(context);
        }

        public override void ExitExpression([NotNull] TSqlParser.ExpressionContext context)
        {
            base.ExitExpression(context);
            DebugContext(context);
        }

        public override void ExitExpression_list([NotNull] TSqlParser.Expression_listContext context)
        {
            base.ExitExpression_list(context);
            DebugContext(context);
        }

        public override void ExitInsert_statement([NotNull] TSqlParser.Insert_statementContext context)
        {
            base.ExitInsert_statement(context);
            DebugContext(context);
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

            if (CooperativeReferenceCollection is not null)
            {
                var reference = CooperativeReferenceCollection.CurrentReference;
                if (reference is not null)
                {
                    if (reference.IsSet())
                    {
                        CooperativeReferenceCollection.Add(reference);
                        CooperativeReferenceCollection.CurrentReference = null;
                    }
                }
            }
        }

        public override void ExitUpdate_elem([NotNull] TSqlParser.Update_elemContext context)
        {
            base.ExitUpdate_elem(context);
            DebugContext(context);
        }

        public override void ExitUpdate_statement([NotNull] TSqlParser.Update_statementContext context)
        {
            base.ExitUpdate_statement(context);
            DebugContext(context);
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

        private SqliteUserDatabase? GetCurrentDatabase()
        {
            if (DatabaseName is null)
            {
                return null;
            }

            return UserDatabaseManager.GetSqliteUserDatabase(DatabaseName);
        }

        #endregion
    }
}
