using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LogMonitor
{
    class Program
    {
        private static string? targetFilePath;  // Mark targetFilePath as nullable
        private static long lastMaxOffset = 0;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the path of the target file (txt only):");
            targetFilePath = Console.ReadLine();

            if (string.IsNullOrEmpty(targetFilePath) || !File.Exists(targetFilePath))
            {
                Console.WriteLine("Invalid file path. Exiting program.");
                return;
            }

            Console.WriteLine($"Monitoring changes in file: {targetFilePath}");

            while (true)
            {
                await Task.Delay(15000); // 15 seconds delay
                CheckFileForChanges();
            }
        }

        private static void CheckFileForChanges()
        {
            try
            {
                if (targetFilePath == null) return; // Add a null check for targetFilePath

                using (var fileStream = new FileStream(targetFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream))
                {
                    fileStream.Seek(lastMaxOffset, SeekOrigin.Begin);

                    string newChanges = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(newChanges))
                    {
                        lastMaxOffset = fileStream.Position;
                        ReportChanges(newChanges);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        private static void ReportChanges(string changes)
        {
            Console.WriteLine($"Detected changes at {DateTime.Now}:");
            Console.WriteLine(changes);
        }
    }
}
