using System;
using Microsoft.SqlServer.Management.Smo;

namespace TableTranslator.Helpers
{
    internal static class ConversionHelper
    {
        internal static Type GetClrType(this Column column)
        {
            var isNullable = column.Nullable;

            switch (column.DataType.SqlDataType)
            {
                case SqlDataType.BigInt:
                    return GetTypeWithCorrectNullability<long>(isNullable);
                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.Timestamp:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                    return typeof(byte[]);
                case SqlDataType.Bit:
                    return GetTypeWithCorrectNullability<bool>(isNullable);
                case SqlDataType.Char:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.SysName:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                    return typeof(string);
                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.Date:
                case SqlDataType.Time:
                case SqlDataType.DateTime2:
                    return GetTypeWithCorrectNullability<DateTime>(isNullable);
                case SqlDataType.Decimal:
                case SqlDataType.Money:
                case SqlDataType.SmallMoney:
                    return GetTypeWithCorrectNullability<decimal>(isNullable);
                case SqlDataType.Float:
                    return GetTypeWithCorrectNullability<double>(isNullable);
                case SqlDataType.Int:
                    return GetTypeWithCorrectNullability<int>(isNullable);
                case SqlDataType.Real:
                    return GetTypeWithCorrectNullability<float>(isNullable);
                case SqlDataType.UniqueIdentifier:
                    return GetTypeWithCorrectNullability<Guid>(isNullable);
                case SqlDataType.SmallInt:
                    return GetTypeWithCorrectNullability<short>(isNullable);
                case SqlDataType.TinyInt:
                    return GetTypeWithCorrectNullability<byte>(isNullable);
                case SqlDataType.Variant:
                case SqlDataType.DateTimeOffset:
                    return GetTypeWithCorrectNullability<DateTimeOffset>(isNullable);
                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        private static Type GetTypeWithCorrectNullability<T>(bool nullable) where T : struct 
        {
            return nullable ? typeof(T?) : typeof(T);
        }
    }
}