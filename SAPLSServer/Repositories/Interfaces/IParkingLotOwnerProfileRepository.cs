using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IParkingLotOwnerProfileRepository : IRepository<ParkingLotOwnerProfile, string>
    {
        /// <summary>
        /// Retrieves a parking lot owner profile associated with the specified user ID, including user-related data, in a
        /// read-only context.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="userId">The unique identifier of the user whose parking lot owner profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingLotOwnerProfile"/>
        /// object with associated user data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingLotOwnerProfile?> FindIncludingUserReadOnly(string userId);
        /// <summary>
        /// Retrieves a parking lot owner profile associated with the specified user ID, including user-related data.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="userId">The unique identifier of the user whose parking lot owner profile is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingLotOwnerProfile"/>
        /// object with associated user data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingLotOwnerProfile?> FindIncludingUser(string userId);
    }
}
