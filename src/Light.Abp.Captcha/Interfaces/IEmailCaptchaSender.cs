using System.Threading.Tasks;

namespace Light.Abp.Captcha
{
    public interface IEmailCaptchaSender
    {
        Task<bool> SendAsync(Captcha captcha);
    }
}