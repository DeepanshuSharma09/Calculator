using BritInsurance.Calculator.Handler.Interfaces;
using BritInsurance.Calculator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace BritInsurance.Calculator.Test.Api
{
    public class CalculatorApiTest
    {
        private Mock<IHandler> _mockHandler;

        private Mock<ILogger> _mockLogger;

        public CalculatorApiTest()
        {
            this._mockHandler = new Mock<IHandler>();
            this._mockLogger = new Mock<ILogger>();
        }

        [Fact]
        public void should_successfully_generate_200_ok_result()
        {
            // Arrange
            var expectedResult = 5;
            this._mockHandler.Setup(x => x.Handle(It.IsAny<string>())).ReturnsAsync(expectedResult);
            var _sut = new Calculator.Api.Calculator(this._mockHandler.Object);

            // Act
            var result = _sut.Calculate(CreateHttpRequest("instructionSet", "Sample"),
                this._mockLogger.Object,
                new ExecutionContext() { FunctionAppDirectory = Directory.GetCurrentDirectory() }).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, ((OkObjectResult)result).StatusCode);
            Assert.Equal(expectedResult, ((CalculationResult)((OkObjectResult)result).Value).Result);
        }

        [Fact]
        public void should_generate_400_bad_request_result()
        {
            // Arrange
            var _sut = new Calculator.Api.Calculator(this._mockHandler.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Calculate(CreateHttpRequest("instructionSet", ""),
                this._mockLogger.Object,
                new ExecutionContext() { FunctionAppDirectory = Directory.GetCurrentDirectory() }));

            var result = _sut.Calculate(CreateHttpRequest("instructionSet", ""),
                this._mockLogger.Object,
                new ExecutionContext() { FunctionAppDirectory = Directory.GetCurrentDirectory() }).Result;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
        }

        [Fact]
        public void should_result_error_response_for_400_bad_request()
        {
            // Arrange
            var _sut = new Calculator.Api.Calculator(this._mockHandler.Object);
            var errorMessage = "Error while calculating based on InstructionSet: . Error Details: Please provide an InstructionSet. (Parameter 'InstructionSet')";

            // Act
            var result = _sut.Calculate(CreateHttpRequest("instructionSet", ""),
                this._mockLogger.Object,
                new ExecutionContext() { FunctionAppDirectory = Directory.GetCurrentDirectory() }).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, ((CalculationResult)((BadRequestObjectResult)result).Value).Error.ErrorCode);
            Assert.Equal(errorMessage, ((CalculationResult)((BadRequestObjectResult)result).Value).Error.ErrorMessage);
        }

        [Fact]
        public void should_generate_422_unprocessable_entity_result()
        {
            // Arrange
            var errorMessage = "Error while calculating based on InstructionSet: Sample. Error Details: Invalid operation";

            this._mockHandler.Setup(x => x.Handle(It.IsAny<string>())).ThrowsAsync(new DivideByZeroException("Invalid operation"));
            var _sut = new Calculator.Api.Calculator(this._mockHandler.Object);

            // Act
            var result = _sut.Calculate(CreateHttpRequest("instructionSet", "Sample"),
                this._mockLogger.Object,
                new ExecutionContext() { FunctionAppDirectory = Directory.GetCurrentDirectory() }).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, ((UnprocessableEntityObjectResult)result).StatusCode);
            Assert.Equal(errorMessage, ((CalculationResult)((UnprocessableEntityObjectResult)result).Value).Error.ErrorMessage);
        }

        [Fact]
        public void should_generate_500_internal_server_error_result()
        {
            // Arrange
            this._mockHandler.Setup(x => x.Handle(It.IsAny<string>())).ThrowsAsync(new Exception("Something unexpected occur"));
            var _sut = new Calculator.Api.Calculator(this._mockHandler.Object);

            // Act
            var result = _sut.Calculate(CreateHttpRequest("instructionSet", "Sample"),
                this._mockLogger.Object,
                new ExecutionContext() { FunctionAppDirectory = Directory.GetCurrentDirectory() }).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        private static HttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
        {
            var ctx = new DefaultHttpContext();
            var request = ctx.Request;
            request.Query = new QueryCollection(new Dictionary<string, StringValues> { { queryStringKey, queryStringValue } });
            return request;
        }
    }
}
