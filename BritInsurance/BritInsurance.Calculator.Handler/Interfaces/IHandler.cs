using System.Threading.Tasks;

namespace BritInsurance.Calculator.Handler.Interfaces
{
    public interface IHandler
    {
        Task<int> Handle(string path);
    }
}
