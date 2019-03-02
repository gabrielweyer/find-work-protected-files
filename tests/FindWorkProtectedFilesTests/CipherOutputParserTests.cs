using System.Collections.Generic;
using System.IO;
using FindWorkProtectedFiles;
using Xunit;

namespace FindWorkProtectedFilesTests
{
    public class CipherOutputParserTests
    {
        private readonly List<string> _console = new List<string>();

        [Fact]
        public void GivenOutputContainsEnterpriseProtectedFiles_ThenWriteFilePathAndEnterpriseNameToConsole()
        {
            // Arrange

            var input = File.ReadLines("./input/compatibility-level-enterprise-protected.log");

            // Act

            CipherOutputParser.GetEnterpriseProtectedFiles(input, WriteLineToConsole);

            // Assert

            Assert.Equal(3, _console.Count);
            Assert.Equal("'C:\\00000557.bin' is protected by company 'company-name-7.com'", _console[0]);
            Assert.Equal("'C:\\00000558.bin' is protected by company 'company-name-8.com'", _console[1]);
            Assert.Equal("'C:\\00000559.bin' is protected by company 'company-name-9.com'", _console[2]);
        }

        [Fact]
        public void GivenOutputContainsNonEnterpriseProtectedFiles_ThenIgnoreThem()
        {
            // Arrange

            var input = File.ReadLines("./input/compatibility-level-non-enterprise.log");

            // Act

            CipherOutputParser.GetEnterpriseProtectedFiles(input, WriteLineToConsole);

            // Assert

            Assert.Empty(_console);
        }

        private void WriteLineToConsole(string line)
        {
            _console.Add(line);
        }
    }
}
