using System.Threading.Tasks;

namespace Light.Abp.Captcha
{
    public interface ISmsCaptchaSender
    {
        Task<bool> SendAsync(Captcha captcha);
    }
}