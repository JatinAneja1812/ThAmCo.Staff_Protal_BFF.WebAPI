using DTOs;
using Microsoft.Extensions.Logging;
using Service.Interfaces.Company;

namespace Service.Classes.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly ILogger<CompanyService> _logger;
        private readonly CompanyDetailsDTO _details;
        public CompanyService(ILogger<CompanyService> Logger)
        {
            _logger = Logger;
            _details = new CompanyDetailsDTO
            {
                CompanyAddressId = "AY821Y",
                CompanyName = "Three Amigos Cooperation",
                ShopNumber = "433",
                Street = "Corporate Avenue",
                City = "MiddlesBrough",
                State = "United Kingdoms",
                PostalCode = "TS12RU"
            };
        }
        public CompanyDetailsDTO GetCompanyDetails()
        {
            try
            {
                return _details;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while retrieving company details: {ex.Message}");
                // You might want to throw an exception or handle the error in some way
                throw;
            }
        }
    }
}
