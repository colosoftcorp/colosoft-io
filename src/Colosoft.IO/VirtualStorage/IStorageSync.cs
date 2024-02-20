using System;

namespace Colosoft.IO.VirtualStorage
{
    public interface IStorageSync
    {
        IAsyncResult BeginSync(AsyncCallback callback, object state);

        VirtualStorageSyncResult EndSync(IAsyncResult ar);

        VirtualStorageSyncResult Sync();
    }
}
