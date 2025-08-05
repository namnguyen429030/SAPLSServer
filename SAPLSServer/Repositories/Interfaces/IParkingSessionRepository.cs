using SAPLSServer.Models;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IParkingSessionRepository : IRepository<ParkingSession, string>
    {
        /// <summary>
        /// Retrieves a parking session associated with the specified ID, including vehicle and parking lot data, in a
        /// read-only context.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="id">The unique identifier of the parking session to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingSession"/>
        /// object with associated vehicle and parking lot data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingSession?> FindIncludingVehicleAndParkingLotReadOnly(string id);
        
        /// <summary>
        /// Retrieves a parking session associated with the specified ID, including vehicle data, in a
        /// read-only context.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="id">The unique identifier of the parking session to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingSession"/>
        /// object with associated vehicle data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingSession?> FindIncludingVehicleReadOnly(string id);
        
        /// <summary>
        /// Retrieves a parking session associated with the specified ID, including parking lot data, in a
        /// read-only context.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="id">The unique identifier of the parking session to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingSession"/>
        /// object with associated parking lot data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingSession?> FindIncludingParkingLotReadOnly(string id);
        /// <summary>
        /// Retrieves a parking session associated with the specified ID, including vehicle and parking lot data.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="id">The unique identifier of the parking session to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingSession"/>
        /// object with associated vehicle and parking lot data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingSession?> FindIncludingVehicleAndParkingLot(string id);
        
        /// <summary>
        /// Retrieves a parking session associated with the specified ID, including vehicle data.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="id">The unique identifier of the parking session to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingSession"/>
        /// object with associated vehicle data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingSession?> FindIncludingVehicle(string id);
        
        /// <summary>
        /// Retrieves a parking session associated with the specified ID, including parking lot data.
        /// </summary>
        /// <remarks>This method operates in a read-only context, ensuring that no modifications are made
        /// to the underlying data.</remarks>
        /// <param name="id">The unique identifier of the parking session to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ParkingSession"/>
        /// object with associated parking lot data if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingSession?> FindIncludingParkingLot(string id);
    }
}
