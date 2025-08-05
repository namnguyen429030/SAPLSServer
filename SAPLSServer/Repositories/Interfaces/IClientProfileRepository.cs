using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IClientProfileRepository : IRepository<ClientProfile, string>
    {
        /// <summary>
        /// Retrieves a client profile associated with the specified user ID, including user-related data, in a
        /// read-only context.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="userId">The unique identifier of the user whose client profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A <see cref="ClientProfile"/> object containing the client profile and associated user data,  or <see
        /// langword="null"/> if no matching profile is found.</returns>
        Task<ClientProfile?> FindIncludingUserReadOnly(string userId);
        /// <summary>
        /// Retrieves a client profile associated with the specified user ID, including user-related data.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="userId">The unique identifier of the user whose client profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A <see cref="ClientProfile"/> object containing the client profile and associated user data,  or <see
        /// langword="null"/> if no matching profile is found.</returns>
        Task<ClientProfile?> FindIncludingUser(string userId);
    }
}
