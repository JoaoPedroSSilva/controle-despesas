namespace ExpenseControl.Models
{
    public class FileAcessHelper
    {
        public static string GetLocalFilePath(string fileName)
        {
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
        }
    }
}
