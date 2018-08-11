namespace MonoForms
{
    class MonoValueEventArg : System.EventArgs
    {
        public object Value { get; private set; }

        public MonoValueEventArg(object value)
        {
            Value = value;
        }
    }
}
