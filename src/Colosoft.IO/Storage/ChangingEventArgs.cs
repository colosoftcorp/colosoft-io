namespace Colosoft.IO.Storage
{
    public class ChangingEventArgs : ChangedEventArgs
    {
        public ChangingEventArgs(string key, object oldValue, object newValue)
             : base(key, oldValue, newValue)
        {
        }

        public bool Cancel { get; set; } = false;
    }
}