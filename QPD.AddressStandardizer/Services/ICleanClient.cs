namespace QPD.AddressStandardizer.Services
{
    public interface ICleanClient
    {
        Task<string> CleanAddress(string address);
    }
}
