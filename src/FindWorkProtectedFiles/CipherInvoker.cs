using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FindWorkProtectedFiles
{
    internal static class CipherInvoker
    {
        private static readonly int TimeoutInMs = (int) TimeSpan.FromMinutes(10).TotalMilliseconds;

        /// <summary>
        /// Somehow overly complicated implementation due to: http://stackoverflow.com/q/139593/57369
        /// </summary>
        /// <param name="searchPath"></param>
        /// <param name="standardOutputFilePath"></param>
        /// <param name="standardErrorFilePath"></param>
        public static void RunCipher(
            string searchPath,
            string standardOutputFilePath,
            string standardErrorFilePath)
        {
            Console.WriteLine();
            Console.WriteLine("=====================");
            Console.WriteLine("=  Invoking cipher  =");
            Console.WriteLine("=====================");

            var cipherArguments = $"/c /s:{searchPath}";

            Console.WriteLine($"Invoking cipher with arguments: '{cipherArguments}' (this might take a few minutes)");

            var cipherProcessStartInfo = new ProcessStartInfo
            {
                FileName = "cipher",
                Arguments = cipherArguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            using (var outputWriter = File.CreateText(standardOutputFilePath))
            using (var errorWriter = File.CreateText(standardErrorFilePath))
            using (var outputWaitHandle = new AutoResetEvent(false))
            using (var errorWaitHandle = new AutoResetEvent(false))
            {
                using (var process = new Process())
                {
                    process.StartInfo = cipherProcessStartInfo;

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (String.IsNullOrWhiteSpace(e.Data))
                        {
                            // ReSharper disable AccessToDisposedClosure
                            // `Process` scope is shorter than the AutoResentEvent(s)
                            outputWaitHandle.Set();
                            // ReSharper restore AccessToDisposedClosure
                        }
                        else
                        {
                            // ReSharper disable AccessToDisposedClosure
                            // `Process` scope is shorter than the file writer
                            outputWriter.WriteLine(e.Data);
                            // ReSharper restore AccessToDisposedClosure
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (String.IsNullOrWhiteSpace(e.Data))
                        {
                            // ReSharper disable AccessToDisposedClosure
                            // `Process` scope is shorter than the AutoResentEvent(s)
                            errorWaitHandle.Set();
                            // ReSharper restore AccessToDisposedClosure
                        }
                        else
                        {
                            // ReSharper disable AccessToDisposedClosure
                            // `Process` scope is shorter than the file writer
                            errorWriter.WriteLine(e.Data);
                            // ReSharper restore AccessToDisposedClosure
                        }
                    };

                    var isTimeout = false;

                    var watch = Stopwatch.StartNew();

                    try
                    {
                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        if (process.WaitForExit(TimeoutInMs))
                        {
                            process.WaitForExit();

                            if (process.ExitCode != 0)
                            {
                                throw new InvalidOperationException(
                                    $"cipher exited with the exit code '{process.ExitCode}', log is available at '{standardErrorFilePath}'.");
                            }
                        }
                        else
                        {
                            isTimeout = true;

                            throw new TimeoutException(
                                $"cipher did not complete after waiting for {TimeoutInMs} ms, log is available at '{standardErrorFilePath}'.");
                        }
                    }
                    finally
                    {
                        Console.WriteLine($"cipher took: {watch.Elapsed}");

                        if (!isTimeout)
                        {
                            outputWaitHandle.WaitOne(1);
                            errorWaitHandle.WaitOne(1);
                        }
                    }
                }
            }
        }
    }
}