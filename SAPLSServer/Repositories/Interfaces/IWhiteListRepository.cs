using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IWhiteListRepository : IRepository<WhiteList, string[]>
    {
        /// <summary>
        /// Retrieves a whitelist entity, including associated user information, based on the specified identifiers, in a read-only context.
        /// </summary>
        /// <param name="ids">An array of unique identifiers used to locate the whitelist entities. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="WhiteList"/>
        /// entity  with associated user information, or <see langword="null"/> if no matching entity is found.</returns>
        Task<WhiteList?> FindIncludingClientReadOnly(string[] ids);
        /// <summary>
        /// Retrieves a whitelist entity, including associated user information, based on the specified identifiers.
        /// </summary>
        /// <param name="ids">An array of unique identifiers used to locate the whitelist entities. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="WhiteList"/>
        /// entity  with associated user information, or <see langword="null"/> if no matching entity is found.</returns>
        Task<WhiteList?> FindIncludingClient(string[] ids);
    }
}
