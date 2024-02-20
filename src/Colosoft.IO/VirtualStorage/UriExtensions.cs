using System;

namespace Colosoft.IO.VirtualStorage
{
    public static class UriExtensions
    {
        private const string UriSchemePack = "pack";
        private const string VirtualStorageUriEscaped = "virtualstorage:///";

        public static bool IsPackVirtualStorage(this Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return uri.IsAbsoluteUri
                && string.Compare(uri.Scheme, UriSchemePack, StringComparison.OrdinalIgnoreCase) == 0
                && string.Compare(
                    GetPackageUri(uri).GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped),
                    VirtualStorageUriEscaped,
                    StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static Uri GetPackageUri(Uri packUri)
        {
            string text = packUri.GetComponents(UriComponents.HostAndPort, UriFormat.UriEscaped);
            text = text.Replace(',', '/');
            Uri uri = new Uri(Uri.UnescapeDataString(text));
            return uri;
        }
    }
}
