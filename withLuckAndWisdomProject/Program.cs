using System;

namespace withLuckAndWisdomProject
{
    //main method
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Main())
                game.Run();
        }
    }
}
