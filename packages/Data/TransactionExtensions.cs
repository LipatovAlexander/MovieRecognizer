using System.Diagnostics.CodeAnalysis;
using Ydb.Sdk.Services.Table;

namespace Data;

public static class TransactionExtensions
{
    public static void EnsureNotNull([NotNull] this Transaction? transaction)
    {
        if (transaction is null)
        {
            throw new InvalidOperationException("Transaction is null");
        }
    }
}