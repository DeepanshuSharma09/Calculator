using System.Threading.Tasks;

namespace BritInsurance.Calculator.Processor.Interfaces
{
    public interface IInstructionProcessor
    {
        int Process(string filePath);
    }
}
