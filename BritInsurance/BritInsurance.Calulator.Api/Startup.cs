using BritInsurance.Calculator.Common.Infrastructure;
using BritInsurance.Calculator.Handler;
using BritInsurance.Calculator.Handler.Interfaces;
using BritInsurance.Calculator.Processor;
using BritInsurance.Calculator.Processor.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BritInsurance.Calculator.Api.Startup))]
namespace BritInsurance.Calculator.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IInstructionProcessor, InstructionProcessor>();
            builder.Services.AddTransient<IHandler, FileHandler>();
            builder.Services.AddTransient<IUtility, Utility>(); 
        }
    }
}
