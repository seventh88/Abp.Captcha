using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Light.Abp.Captcha
{
    public class NullSmsCaptchaSender : ISmsCaptchaSender, ISingletonDependency
    {
        public ILogger<DefaultCaptchaManager> Logger { get; set; }
        public NullSmsCaptchaSender()
        {
            Logger = NullLogger<DefaultCaptchaManager>.Instance;
        }
        public virtual Task<bool> SendAsync(Captcha captcha)
        {
            Logger.LogWarning($"SMS Captcha Sending was not implemented! Using {nameof(DefaultCaptchaManager)}:");
            Logger.LogWarning("Captcha :{@captcha} ", captcha);
            return Task.FromResult(true);
        }
    }
}