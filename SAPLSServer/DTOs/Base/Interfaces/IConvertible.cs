namespace SAPLSServer.DTOs.Base
{
    public interface IConvertible<out TConverted> where TConverted : class
    {
        TConverted Convert();
    }
}
