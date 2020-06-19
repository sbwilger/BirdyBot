/// <summary>
/// Serana Wilger
/// 06/18/2020
/// Program.cs
/// 
/// This is the main class which runs the bot
/// </summary>

namespace BirdyBot
{
    class Program
    {
        //instantiates and runs the bot
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
