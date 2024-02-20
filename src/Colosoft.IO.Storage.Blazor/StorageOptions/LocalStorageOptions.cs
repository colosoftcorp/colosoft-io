using System.Text.Json;

namespace Colosoft.IO.Storage.Blazor.StorageOptions
{
    public class LocalStorageOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions();
    }
}
