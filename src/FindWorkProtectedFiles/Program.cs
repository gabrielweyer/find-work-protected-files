using System;
using System.IO;

namespace FindWorkProtectedFiles
{
    class Program
    {
        static int Main(string[] args)
        {
            var cipherStandardOutputFilePath = Path.GetTempFileName();
            var cipherStandardErrorFilePath = Path.GetTempFileName();

            var deleteStandardErrorFile = true;

            try
            {
                Console.WriteLine("===============");
                Console.WriteLine("=  Arguments  =");
                Console.WriteLine("===============");

                if (args.Length != 1)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(args),
                        $"\"{string.Join(", ", args)}\"",
                        "You should provide the path of the directory to search as the single argument: `dotnet fwpf C:\\`");
                }

                var searchPath = args[0];

                Console.WriteLine($"Searching path: '{searchPath}'");

                CipherInvoker.RunCipher(searchPath, cipherStandardOutputFilePath, cipherStandardErrorFilePath);

                var lines = File.ReadLines(cipherStandardOutputFilePath);

                CipherOutputParser.GetEnterpriseProtectedFiles(lines, Console.WriteLine);

                Console.WriteLine();
                Console.WriteLine("We're done, have a good one");
                return 0;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                deleteStandardErrorFile = false;

                return -1;
            }
            finally
            {
                try
                {
                    File.Delete(cipherStandardOutputFilePath);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Couldn't delete cipher standard output file '{cipherStandardOutputFilePath}'");
                }

                if (deleteStandardErrorFile)
                {
                    try
                    {
                        File.Delete(cipherStandardErrorFilePath);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Couldn't delete cipher standard error file '{cipherStandardOutputFilePath}'");
                    }
                }
            }
        }
    }
}