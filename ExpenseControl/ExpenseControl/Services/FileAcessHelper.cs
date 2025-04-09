namespace ExpenseControl.Services
{
    public class FileAcessHelper
    {
        public static string GetLocalFilePath(string fileName)
        {
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
        }
    }
}
