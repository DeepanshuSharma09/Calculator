using BritInsurance.Calculator.Handler.Interfaces;
using BritInsurance.Calculator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BritInsurance.Calculator.Api
{
    /// <summary>
    /// Calculator class for Brit Insurance
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Handler instance.
        /// </summary>
        private IHandler handler;

        /// <summary>
        /// Create an instance of Calculator
        /// </summary>
        /// <param name="processor"></param>
        public Calculator(IHandler handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Rest Api /api/Calculate
        /// </summary>
        /// <param name="req">Request params</param>
        /// <param name="instructionSet">instruction set name</param>
        /// <param name="log">Logger instance</param>
        /// <param name="context">Execution Context</param>
        /// <returns>Result object containing CalculationResult</returns>
        [FunctionName("calculate")]
        public async Task<IActionResult> Calculate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "calculate")]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            CalculationResult result = new CalculationResult();
            string instructionSet = string.Empty;

            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                instructionSet = req.Query["instructionSet"];

                if (string.IsNullOrEmpty(instructionSet))
                    throw new ArgumentNullException(paramName: "InstructionSet", message: "Please provide an InstructionSet.");

                var filePath = Path.Combine(context.FunctionAppDirectory, "InstructionSet", $"{instructionSet}.txt");

                result.Result = await this.handler.Handle(filePath);

                return new OkObjectResult(result);
            }
            catch (ArgumentNullException ex)
            {
                var errorMessage = $"Error while calculating based on InstructionSet: { instructionSet}. Error Details: { ex.Message}";
                log.LogError(errorMessage);
                result.Error = new ErrorResponse() { ErrorMessage = errorMessage, ErrorCode = StatusCodes.Status400BadRequest };

                return new BadRequestObjectResult(result);
            }
            catch (DivideByZeroException ex)
            {
                var errorMessage = $"Error while calculating based on InstructionSet: {instructionSet}. Error Details: {ex.Message}";
                log.LogError(errorMessage);
                result.Error = new ErrorResponse() { ErrorMessage = errorMessage, ErrorCode = StatusCodes.Status422UnprocessableEntity };

                return new UnprocessableEntityObjectResult(result);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error while calculating based on InstructionSet: {instructionSet}. Error Details: {ex.Message}";
                log.LogError(errorMessage);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
