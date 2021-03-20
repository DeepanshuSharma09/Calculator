using System.Threading.Tasks;

namespace BritInsurance.Calculator.Common.Infrastructure
{
    public interface IUtility
    {
        Task<string> ReadFileContentAsync(string path);
    }
}
