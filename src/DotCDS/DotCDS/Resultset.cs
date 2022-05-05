using DotCDS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace DotCDS
{
    internal static class Resultset
    {
        /// <summary>
        /// Converts a data table to a CDS StatementResultset
        /// </summary>
        /// <param name="data">A data table to convert</param>
        /// <returns>The A corresponding CDS StatementResultset</returns>
        public static StatementResultset ToStatementResultset(DataTable data)
        {
            var resultset = new StatementResultset();

            // return if the data table has errors
            if (data.HasErrors)
            {
                resultset.IsError = true;
                return resultset;
            }

            List<ColumnSchema> columns = GetColumnSchemas(data.Columns);

            foreach (DataRow row in data.Rows)
            {
                var resultRow = new Row();

                foreach (var col in columns)
                {
                    var value = new RowValue();
                    value.Column = col;

                    var dataObject = row[col.ColumnName];

                    if (dataObject is not null)
                    {
                        value.Value = ByteString.CopyFrom(DataTypeToByteConvert.Convert((SQLColumnType)col.ColumnType, dataObject));
                    }
                    else
                    {
                        value.IsNullValue = true;
                    }

                    resultRow.Values.Add(value);
                }

                resultset.Rows.Add(resultRow);
            }

            return resultset;
        }

        /// <summary>
        /// Takes a list of data columns from a data table and returns a converted list of column schemas
        /// </summary>
        /// <param name="dataColumnCollection">The column collection of a data table</param>
        /// <returns>A corresponding list of ColumnSchemas</returns>
        public static List<ColumnSchema> GetColumnSchemas(DataColumnCollection dataColumnCollection)
        {
            var columns = new List<ColumnSchema>();

            foreach (DataColumn column in dataColumnCollection)
            {
                var colSchema = new ColumnSchema();
                colSchema.ColumnName = column.ColumnName;
                colSchema.ColumnType = (uint)SQLColumnTypeHelper.GetType(column.DataType.ToString());
                colSchema.ColumnLength = (uint)column.MaxLength;
                colSchema.Ordinal = (uint)column.Ordinal;
                colSchema.IsNullable = column.AllowDBNull;

                if (column.Table is not null)
                {
                    colSchema.TableId = column.Table.TableName ?? string.Empty;
                }

                columns.Add(colSchema);
            }

            return columns;
        }
    }
}
