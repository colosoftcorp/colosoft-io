using System;

namespace Colosoft.IO
{
    public static class Checksum
    {
        private static bool md5ProviderDisabled;

        private static System.Security.Cryptography.MD5 GetMD5Provider()
        {
            System.Security.Cryptography.MD5 md = null;
            if (!md5ProviderDisabled)
            {
                try
                {
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
                    md = new System.Security.Cryptography.MD5CryptoServiceProvider();
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
                }
                catch (InvalidOperationException)
                {
                    md5ProviderDisabled = true;
                }
            }

            return md;
        }

        public static byte[] CalculateMD5(System.IO.Stream stream)
        {
            System.Security.Cryptography.MD5 md = GetMD5Provider();
            if (md == null)
            {
                return Array.Empty<byte>();
            }

            using (md)
            {
                return md.ComputeHash(stream);
            }
        }

        public static byte[] CalculateMD5(byte[] buffer, int offset, int count)
        {
            System.Security.Cryptography.MD5 md = GetMD5Provider();
            if (md == null)
            {
                return Array.Empty<byte>();
            }

            using (md)
            {
                return md.ComputeHash(buffer, offset, count);
            }
        }

        public static byte[] CalculateMD5(string fileName)
        {
            if (md5ProviderDisabled)
            {
                return Array.Empty<byte>();
            }

            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                return CalculateMD5(stream);
            }
        }
    }
}
