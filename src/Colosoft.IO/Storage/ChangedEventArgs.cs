using System;

namespace Colosoft.IO.Storage
{
    public class ChangedEventArgs : EventArgs
    {
        public ChangedEventArgs(string key, object oldValue, object newValue)
        {
            this.Key = key;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public string Key { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }
    }
}