using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EcommerceDev.Infrastructure.Geolocation
{
    public class GoogleGeolocationService : IGeolocationService
    {
        private readonly GeolocationSettings _geolocationSettings;

        public GoogleGeolocationService(IOptions<GeolocationSettings> options)
        {
            _geolocationSettings = options.Value;
        }

        public async Task<int> GetDistance(string origin, string destination)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_geolocationSettings.ApiBaseUrl}/json?destinations={destination}&origins={origin}&key={_geolocationSettings.GeolocationApiKey}");

            var response = await client.SendAsync(request);

            var responseModel = await response.Content.ReadFromJsonAsync<RootGoogleDistanceResponseModel>();

            if (responseModel == null)
            {
                return -1;
            }

            var distanceInMeters = responseModel.rows.First().elements.First().distance.value;

            var distanceInKm = distanceInMeters / 1000;

            return distanceInKm;
        }
    }
}
