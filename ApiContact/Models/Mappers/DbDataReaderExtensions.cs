using ApiContact.Models;
using System.Data.Common;

namespace ApiContact.Models.Mappers
{
    internal static class DbDataReaderExtensions
    {
        internal static Contact ToContact(this DbDataReader reader)
        {
            return new Contact()
            {
                Id = (int)reader["Id"],
                LastName = (string)reader["LastName"],
                FirstName = (string)reader["FirstName"],
                Email = (string)reader["Email"],
                BirthDate = (DateTime)reader["BirthDate"]
            };
        }
    }
}
