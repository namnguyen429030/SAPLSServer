using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IStaffProfileRepository : IRepository<StaffProfile, string>
    {
        /// <summary>
        /// Retrieves a staff profile associated with the specified user ID, including user-related data, in a
        /// read-only context.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="userId">The unique identifier of the user whose staff profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="StaffProfile"/>
        /// object with associated user data if found; otherwise, <see langword="null"/>.</returns>
        Task<StaffProfile?> FindIncludingUserReadOnly(string userId);
        /// <summary>
        /// Retrieves a staff profile associated with the specified user ID, including user-related data.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="userId">The unique identifier of the user whose staff profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="StaffProfile"/>
        /// object with associated user data if found; otherwise, <see langword="null"/>.</returns>
        Task<StaffProfile?> FindIncludingUser(string userId);
    }
}