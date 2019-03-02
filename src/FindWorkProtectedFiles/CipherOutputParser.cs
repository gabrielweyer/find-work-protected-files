using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FindWorkProtectedFiles
{
    public static class CipherOutputParser
    {
        public static void GetEnterpriseProtectedFiles(IEnumerable<string> lines, Action<string> consoleWriter)
        {
            Console.WriteLine();
            Console.WriteLine("===========================");
            Console.WriteLine("=  Parsing cipher output  =");
            Console.WriteLine("===========================");
            Console.WriteLine();

            var watch = Stopwatch.StartNew();

            try
            {
                using (var lineIterator = lines.GetEnumerator())
                {
                    var currentDirectory = string.Empty;
                    var currentEncryptedFile = string.Empty;

                    var skipNextLine = false;
                    var parseEnterpriseName = false;

                    do
                    {
                        if (skipNextLine)
                        {
                            lineIterator.MoveNext();
                            skipNextLine = false;
                        }

                        if (IsUselessLine(lineIterator))
                        {
                            continue;
                        }

                        if (IsLineWithDirectoryName(lineIterator))
                        {
                            currentDirectory = lineIterator.Current.Substring(9);
                            skipNextLine = true;

                            continue;
                        }

                        if (IsLineWithUnencryptedFilename(lineIterator))
                        {
                            continue;
                        }

                        if (IsLineWithEncryptedFilename(lineIterator))
                        {
                            currentEncryptedFile = lineIterator.Current.Substring(2);
                            skipNextLine = true;

                            continue;
                        }

                        if (IsFileEnterpriseProtected(lineIterator))
                        {
                            parseEnterpriseName = true;
                            skipNextLine = true;

                            continue;
                        }

                        if (!parseEnterpriseName)
                        {
                            continue;
                        }

                        var currentEnterpriseOwnership = lineIterator.Current.Substring(4);
                        consoleWriter(
                            $"'{currentDirectory}{currentEncryptedFile}' is protected by company '{currentEnterpriseOwnership}'");

                        parseEnterpriseName = false;
                    } while (lineIterator.MoveNext());
                }
            }
            finally
            {
                Console.WriteLine($"cipher output parsing took: {watch.Elapsed}");
            }

            bool IsUselessLine(IEnumerator<string> lineIterator)
            {
                return string.IsNullOrEmpty(lineIterator.Current);
            }

            bool IsLineWithDirectoryName(IEnumerator<string> lineIterator)
            {
                return lineIterator.Current.StartsWith(" Listing ");
            }

            bool IsLineWithUnencryptedFilename(IEnumerator<string> lineIterator)
            {
                return lineIterator.Current.StartsWith("U ");
            }

            bool IsLineWithEncryptedFilename(IEnumerator<string> lineIterator)
            {
                return lineIterator.Current.StartsWith("E ");
            }

            bool IsFileEnterpriseProtected(IEnumerator<string> lineIterator)
            {
                return lineIterator.Current.StartsWith("    Enterprise Protected");
            }
        }
    }
}