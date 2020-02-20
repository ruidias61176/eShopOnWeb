using System.Threading;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Infrastructure.Services.CurrencyService
{
    public class CurrencyServiceStatic : ICurrencyService
    {
        public Task<decimal> Convert(decimal value, Currency source, Currency target, CancellationToken cancellationToken = default)
        {
            if(target == Currency.EUR){
            var convertedValue = value * 0.8m;
            return Task.FromResult(convertedValue);
            }
            return Task.FromResult(value);
        }
    }
} 