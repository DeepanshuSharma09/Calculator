using System;
using System.IO;
using System.Threading.Tasks;

namespace BritInsurance.Calculator.Common.Infrastructure
{
    public class Utility : IUtility
    {
        public Task<string> ReadFileContentAsync(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(paramName: "path", message:"The file path cannot be empty.");

                if (!File.Exists(path))
                    throw new FileNotFoundException($"The specified file at location: {path} cannot be found.");

                return File.ReadAllTextAsync(path);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (FileNotFoundException ex)
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
