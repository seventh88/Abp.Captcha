using System;
using System.Threading.Tasks;

namespace Light.Abp.Captcha
{
    public interface ICaptchaStore
    {
        Task<Captcha> CreateAsync(Captcha captcha);

        Task<Captcha> FindAsync(string type, string receiver, string code, Guid? tenantId);

        Task<bool> ExistValidAsync(string type, string receiver, Guid? tenantId);

        Task<Captcha> UsedAsync(Captcha captcha);

    }
}
