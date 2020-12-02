using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Light.Abp.Captcha
{
    public class DefaultCaptchaGenerator : ICaptchaGenerator, ITransientDependency
    {
        public async Task<string> CreateAsync(int length = 6)
        {
            string buffer = "0123456789";
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            int range = buffer.Length;
            for (int i = 0; i < length; i++)
            {
                sb.Append(buffer.Substring(r.Next(range), 1));
            }
            return await Task.FromResult(sb.ToString());
        }
    }
}