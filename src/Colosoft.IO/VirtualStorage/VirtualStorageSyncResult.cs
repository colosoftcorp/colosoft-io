using System;

namespace Colosoft.IO.VirtualStorage
{
    public class VirtualStorageSyncResult
    {
        private VirtualStorageSyncFailure[] failures = Array.Empty<VirtualStorageSyncFailure>();

        public bool Success
        {
            get { return this.Failures.Length == 0; }
        }

#pragma warning disable CA1819 // Properties should not return arrays
        public VirtualStorageSyncFailure[] Failures
#pragma warning restore CA1819 // Properties should not return arrays
        {
            get { return this.failures; }
            set
            {
                this.failures = value ?? Array.Empty<VirtualStorageSyncFailure>();
            }
        }
    }
}
