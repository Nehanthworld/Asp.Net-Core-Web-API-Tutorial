namespace CollegeApp.MyLogging
{
    public class LogToServerMemory : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Logto Server memory");
            //write your own logic to save the logs to Server memory
        }
    }
}
