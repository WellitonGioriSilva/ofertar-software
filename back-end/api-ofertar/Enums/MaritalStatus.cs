using System.Text.Json.Serialization;

namespace api_ofertar.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MaritalStatus
    {
        Single = 'S',
        Married = 'M',
        Divorced = 'D',
        Widowed = 'W'
    }
}