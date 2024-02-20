using Colosoft.IO.Storage.Blazor.StorageOptions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Colosoft.IO.Storage.Blazor.Serialization
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions options;

        public SystemTextJsonSerializer(IOptions<LocalStorageOptions> options)
        {
            this.options = options.Value.JsonSerializerOptions;
        }

        public SystemTextJsonSerializer(LocalStorageOptions localStorageOptions)
        {
            this.options = localStorageOptions.JsonSerializerOptions;
        }

        public T Deserialize<T>(string text) => JsonSerializer.Deserialize<T>(text, this.options) !;

        public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, this.options);
    }
}
