namespace MonoForms
{
#if WINDOWS
    static class Program
    {
        [System.STAThread]
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
#endif
}
