using System.Threading.Tasks;

namespace Light.Abp.Captcha
{
    public interface ICaptchaManager
    {
        Task BeforeSendAsync(string type, string receiver);

        Task SendAsync(string type, string receiver);

        Task VerifyAsync(string type, string receiver, string code);

    }
}