using System;

namespace Colosoft.IO
{
    public static class FileChecker
    {
        public static bool CheckSignature(string filePath, int signatureSize, string expectedSignature)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Must specify a filepath", nameof(filePath));
            }

            if (string.IsNullOrEmpty(expectedSignature))
            {
                throw new ArgumentException("Must specify a value for the expected file signature", nameof(expectedSignature));
            }

            using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                if (fs.Length < signatureSize)
                {
                    return false;
                }

                var signature = new byte[signatureSize];
                var bytesRequired = signatureSize;
                var index = 0;

                while (bytesRequired > 0)
                {
                    int bytesRead = fs.Read(signature, index, bytesRequired);
                    bytesRequired -= bytesRead;
                    index += bytesRead;
                }

                var actualSignature = BitConverter.ToString(signature);
                if (actualSignature == expectedSignature)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsPKZip(string filePath)
        {
            return CheckSignature(filePath, 4, "50-4B-03-04");
        }

        public static bool IsGZip(string filePath)
        {
            return CheckSignature(filePath, 3, "1F-8B-08");
        }
    }
}
