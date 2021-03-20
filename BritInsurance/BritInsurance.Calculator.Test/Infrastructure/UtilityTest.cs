using BritInsurance.Calculator.Common.Infrastructure;
using System;
using System.IO;
using Xunit;

namespace BritInsurance.Calculator.Test.Infrastructure
{
    public class UtilityTest
    {
        [Fact]
        public void should_read_file_successfully()
        {
            // Arrange
            var filePath = $"{Directory.GetCurrentDirectory()}\\InstructionSet\\Sample.txt";
            var _sut = new Utility();

            //Act
            var fileContent = _sut.ReadFileContentAsync(filePath).Result;

            //Assert
            Assert.NotNull(fileContent);
            Assert.True(fileContent.Length > 0);
        }

        [Fact]
        public void should_throw_argument_null_exception_when_filepath_not_sent()
        {
            // Arrange
            var filePath = string.Empty;
            var _sut = new Utility();

            //Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.ReadFileContentAsync(filePath));
        }

        [Fact]
        public void read_file_throw_file_not_found_exception_when_file_dont_exist()
        {
            // Arrange
            var filePath = $"{Directory.GetCurrentDirectory()}\\InstructionSet\\Dummy.txt";
            var _sut = new Utility();

            //Act & Assert
            Assert.ThrowsAsync<FileNotFoundException>(() => _sut.ReadFileContentAsync(filePath));
        }
    }
}
