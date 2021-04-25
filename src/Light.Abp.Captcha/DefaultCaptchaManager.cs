using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace Light.Abp.Captcha
{
    public class DefaultCaptchaManager : ICaptchaManager, ITransientDependency
    {
        private readonly AbpCaptchaOptions _options;
        public IClock Clock { get; }

        public ICaptchaGenerator CaptchaGenerator { get; }

        public ICurrentTenant CurrentTenant { get; }

        public ISettingProvider SettingProvider { get; }

        public ISmsCaptchaSender SmsCaptchaSender { get; }

        public IEmailCaptchaSender EmailCaptchaSender { get; }

        public ICaptchaStore CaptchaStore { get; }

        public ILogger<DefaultCaptchaManager> Logger { get; set; }

        public DefaultCaptchaManager(IClock clock,
            ICaptchaGenerator captchaGenerator,
            ICurrentTenant currentTenant,
            IOptions<AbpCaptchaOptions> options,
            ISettingProvider settingProvider,
            ISmsCaptchaSender smsCaptchaSender,
            IEmailCaptchaSender emailCaptchaSender,
            ICaptchaStore captchaStore)
        {
            _options = options.Value;
            Clock = clock;
            CaptchaGenerator = captchaGenerator;
            CurrentTenant = currentTenant;
            SettingProvider = settingProvider;
            SmsCaptchaSender = smsCaptchaSender;
            EmailCaptchaSender = emailCaptchaSender;
            CaptchaStore = captchaStore;

            Logger = NullLogger<DefaultCaptchaManager>.Instance;
        }

        public virtual async Task BeforeSendAsync(string type, string receiver)
        {
            var exist = await CaptchaStore.FindAsync(type, receiver, CurrentTenant.Id);
            if (exist == null)
            {
                return;
            }
            int expireSeconds = _options.CaptchaFrequencyLimitSeconds;
            if (exist.CreationTime.AddSeconds(expireSeconds) > Clock.Now)//检查是否超过请求频率限制
            {
                throw new BusinessException(CaptchaErrorCodes.FrequencyLimit);
            }
        }

        public virtual async Task SendAsync(string type, string receiver)
        {
            await BeforeSendAsync(type, receiver);

            var code = await CaptchaGenerator.CreateAsync();

            var receiverType = receiver.Contains("@") ? EnumReceiverType.Email : EnumReceiverType.PhoneNumber;

            int expireSeconds = receiverType == EnumReceiverType.Email ? _options.EmailCaptchaExpireSeconds : _options.SmsCaptchaExpireSeconds;
            var captcha = new Captcha(type, code, receiver, receiverType, Clock.Now, expireSeconds, CurrentTenant.Id);

            bool sendResult = receiverType == EnumReceiverType.Email
                ? await EmailCaptchaSender.SendAsync(captcha)
                : await SmsCaptchaSender.SendAsync(captcha);
            if (!sendResult)
            {
                throw new BusinessException(CaptchaErrorCodes.SendFailed);
            }
            await CaptchaStore.CreateAsync(captcha);
        }


        public virtual async Task VerifyAsync(string type, string receiver, string code)
        {
            var captcha = await CaptchaStore.FindAsync(type, receiver, CurrentTenant.Id);
            if (captcha == null)
            {
                throw new BusinessException(CaptchaErrorCodes.Error);
            }
            if (captcha.Code != code)
            {
                throw new BusinessException(CaptchaErrorCodes.Error);
            }
            if (captcha.IsUsed)
            {
                throw new BusinessException(CaptchaErrorCodes.Used);
            }

            if (captcha.IsExpire(Clock.Now))
            {
                throw new BusinessException(CaptchaErrorCodes.Expired);
            }
            captcha.Use(Clock.Now);
            await CaptchaStore.UsedAsync(captcha);
        }
    }
}