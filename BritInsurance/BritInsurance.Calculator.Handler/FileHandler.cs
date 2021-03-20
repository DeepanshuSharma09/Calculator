using BritInsurance.Calculator.Common.Infrastructure;
using BritInsurance.Calculator.Handler.Interfaces;
using BritInsurance.Calculator.Processor.Interfaces;
using System;
using System.Threading.Tasks;

namespace BritInsurance.Calculator.Handler
{
    public class FileHandler : IHandler
    {
        private IUtility utility;

        private IInstructionProcessor instructionProcessor;

        public FileHandler(IUtility utility, IInstructionProcessor processor)
        {
            this.utility = utility;
            this.instructionProcessor = processor;
        }

        public async Task<int> Handle(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentNullException(paramName: "filePath", message: "File Path cannot be empty.");

                var content = await this.utility.ReadFileContentAsync(filePath);

                return this.instructionProcessor.Process(content);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
