using BritInsurance.Calculator.Common.Infrastructure;
using BritInsurance.Calculator.Handler;
using BritInsurance.Calculator.Processor.Interfaces;
using Moq;
using System;
using Xunit;

namespace BritInsurance.Calculator.Test.Handler
{
    public class HandlerTest
    {
        private Mock<IUtility> _mockUtility;

        private Mock<IInstructionProcessor> _mockInstructionProcessor;

        public HandlerTest()
        {
            this._mockUtility = new Mock<IUtility>();
            this._mockInstructionProcessor = new Mock<IInstructionProcessor>();
        }

        [Fact]
        public void should_handle_file_successfully()
        {
            // Arrange
            var setUpResult = 10;

            this._mockUtility.Setup(x => x.ReadFileContentAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);
            this._mockInstructionProcessor.Setup(x => x.Process(It.IsAny<string>())).Returns(setUpResult);

            var _sut = new FileHandler(this._mockUtility.Object, this._mockInstructionProcessor.Object);

            // Act
            var result = _sut.Handle("Dummy-Path").Result;

            // Assert
            Assert.Equal(setUpResult, result);
        }

        [Fact]
        public void should_throw_argument_null_exception_when_file_path_is_empty()
        {
            // Arrange
            var errorMessage = "File Path cannot be empty. (Parameter 'filePath')";
            var _sut = new FileHandler(this._mockUtility.Object, this._mockInstructionProcessor.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Handle(""));

            Assert.Equal("filePath", ex.Result.ParamName);
            Assert.Equal(errorMessage, ex.Result.Message);
        }

        [Fact]
        public void should_throw_generic_exception_code_execution_fails()
        {
            // Arrange
            var errorMessage = "Something unusual occur.";

            this._mockUtility.Setup(x => x.ReadFileContentAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);
            this._mockInstructionProcessor.Setup(x => x.Process(It.IsAny<string>())).Throws(new Exception(errorMessage));

            var _sut = new FileHandler(this._mockUtility.Object, this._mockInstructionProcessor.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _sut.Handle("Dummy-Path"));

            Assert.Equal(errorMessage, ex.Result.Message);
        }
    }
}
