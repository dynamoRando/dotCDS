﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Query
{
    internal static class DDLKeywords
    {
        private static string[] _keywords = new string[4];

        public const string CREATE = "CREATE";
        public const string ALTER = "ALTER";
        public const string DROP = "DROP";
        public const string TRUNCATE = "TRUNCATE";
        public const string TABLE = "TABLE";
        public const string IF_EXISTS = "IF EXISTS";

        public static string[] Get()
        {
            _keywords[0] = CREATE;
            _keywords[1] = ALTER;
            _keywords[2] = DROP;
            _keywords[3] = TRUNCATE;
            return _keywords;
        }
    }
}
