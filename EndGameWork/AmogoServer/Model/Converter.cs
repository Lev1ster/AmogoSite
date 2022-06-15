using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmogoWebSite.Model
{
    public static class Converter
    {
        static Dictionary<string, Type> valueFromSql =
            new Dictionary<string, Type>()
            {
                {"nvarchar", typeof(string)},
                {"int", typeof(int)},
                {"text", typeof(string)},
                {"decimal", typeof(double)}
            };

        static Dictionary<string, string> valueToSql =
            new Dictionary<string, string>()
            {
                {"heigth", "int"},
                {"width", "int"},
                {"color", "string"}
            };

        public static Type ConvertFromSQL(string sql)
        {
            if (valueFromSql.TryGetValue(sql, out Type type))
                return type;

            throw new ArgumentException("Type sql do not exists");
        }

        public static string ConvertToSQL(string name)
        {
            if (valueToSql.TryGetValue(name, out var str))
                return str;

            throw new ArgumentException("This name do not exists");
        }
    }
}