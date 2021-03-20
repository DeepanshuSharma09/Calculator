using BritInsurance.Calculator.Processor;
using System;
using System.IO;
using Xunit;

namespace BritInsurance.Calculator.Test.Processor
{
    public class ProcessorTest
    {
        [Fact]
        public void should_successfully_process_instructions()
        {
            // Arrange
            int expectedResult = 16;
            var filePath = $"{Directory.GetCurrentDirectory()}\\InstructionSet\\Sample.txt";
            var instructions = File.ReadAllText(filePath);

            var _sut = new InstructionProcessor();

            // Act
            var actualResult = _sut.Process(instructions);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void should_result_zero_when_instructions_are_empty()
        {
            // Arrange
            int expectedResult = 0;
            var instructions = string.Empty;

            var _sut = new InstructionProcessor();

            // Act
            var actualResult = _sut.Process(instructions);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void should_result_divide_by_zero_exception()
        {
            // Arrange
            var filePath = $"{Directory.GetCurrentDirectory()}\\InstructionSet\\DivideByZeroException.txt";
            var instructions = File.ReadAllText(filePath);

            var _sut = new InstructionProcessor();

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => _sut.Process(instructions));
        }

        [Fact]
        public void should_throw_missing_field_exception_when_apply_instruction_not_found()
        {
            // Arrange
            var filePath = $"{Directory.GetCurrentDirectory()}\\InstructionSet\\MissingApplyInstruction.txt";
            var instructions = File.ReadAllText(filePath);

            var _sut = new InstructionProcessor();

            // Act & Assert
            Assert.Throws<MissingFieldException>(() => _sut.Process(instructions));
        }
    }
}
