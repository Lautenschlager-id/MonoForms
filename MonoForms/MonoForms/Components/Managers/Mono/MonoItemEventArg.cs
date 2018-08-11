namespace MonoForms
{
    class MonoItemEventArg : System.EventArgs
    {
        public object Item { get; private set; }

        public MonoItemEventArg(object item)
        {
            Item = item;
        }
    }
}
