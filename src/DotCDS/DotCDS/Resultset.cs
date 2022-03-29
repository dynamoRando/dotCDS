using DotCDS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal static class Resultset
    {
        public static StatementResultset ToStatementResultset(DataTable data)
        {
            var resultset = new StatementResultset();
            int width = data.Columns.Count;

            foreach (DataColumn column in data.Columns)
            {
                string name = column.ColumnName;
                string dataType = column.DataType.ToString();
                
            }

            throw new NotImplementedException();
        }
    }
}
