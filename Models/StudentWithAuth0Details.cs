using System.Text.Json.Serialization;

namespace QuizManager.Models
{
    public class StudentWithAuth0Details
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string School { get; set; }
        public DateTime? SignUpDate { get; set; }
        public DateTime? LatestLogin { get; set; }
        public string LoginTimes { get; set; }
        public string LastIp { get; set; }
        public bool? IsEmailVerified { get; set; }
        public string LoginBrowser { get; set; }
        public bool IsMobile { get; set; }
        public LocationInfo LocationInfo { get; set; }
        public string AreasOfExpertise { get; set; }
        public string Keywords { get; set; }


    }

    public class LocationInfo
    {
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_code3")]
        public string CountryCode3 { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("city_name")]
        public string CityName { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("time_zone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("continent_code")]
        public string ContinentCode { get; set; }

        public string FormattedLocation =>
            !string.IsNullOrEmpty(CityName) && !string.IsNullOrEmpty(CountryName)
                ? $"{CityName}, {CountryName}"
                : !string.IsNullOrEmpty(CountryName)
                    ? CountryName
                    : "Location unknown";
    }

}