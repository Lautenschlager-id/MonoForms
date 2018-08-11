namespace MonoForms
{
    class MonoIndexEventArg : System.EventArgs
    {
        public int Index { get; private set; }

        public object Data { get; private set; }

        public MonoIndexEventArg(int index, object data = null)
        {
            Index = index;

            Data = data;
        }
    }
}
