using Kp.Ms.Oxi.Models.Entity;
using Kp.Ms.Oxi.Models.Response;

namespace Kp.Ms.Oxi.Converters;

public static class TransactionConverter
{
    public static TransactionDto Convert(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            InterfaceType = transaction.InterfaceType,
            Url = transaction.Url,
            Xml = transaction.Xml
        };
    }
}