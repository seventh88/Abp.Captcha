using System.Threading.Tasks;

namespace Light.Abp.Captcha
{
    public interface ICaptchaGenerator
    {
        Task<string> CreateAsync(int length = 6);
    }
}
