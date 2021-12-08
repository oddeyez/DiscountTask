using System;
namespace DiscountCodeAPI.Services
{
    public class CodeGenerator : ICodeGenerator
    {
        public CodeGenerator()
        {
        }

        public string GenerateCode()
        {
            Random r = new Random();
            long code = r.Next(10000, 99999);
            return code.ToString();

        }
    }
}
