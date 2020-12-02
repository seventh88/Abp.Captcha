using System;
using System.Threading.Tasks;
using Light.Abp.Captcha.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace Light.Abp.Captcha
{
    public class DefaultCaptchaManager : ICaptchaManager, ITransientDependency
    {
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
            ISettingProvider settingProvider,
            ISmsCaptchaSender smsCaptchaSender,
            IEmailCaptchaSender emailCaptchaSender,
            ICaptchaStore captchaStore)
        {
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
            var exist = await CaptchaStore.ExistValidAsync(type, receiver, CurrentTenant.Id);
            if (exist)
            {
                throw new BusinessException(CaptchaErrorCodes.FrequencyLimit);
            }
        }

        public virtual async Task SendAsync(string type, string receiver)
        {
            await BeforeSendAsync(type, receiver);

            var code = await CaptchaGenerator.CreateAsync();

            var receiverType = receiver.Contains("@") ? EnumReceiverType.Email : EnumReceiverType.PhoneNumber;

            string expireSecondSettingName = receiverType == EnumReceiverType.Email ? AbpCaptchaSettings.EmailCaptchaExpireSeconds : AbpCaptchaSettings.SmsCaptchaExpireSeconds;
            int expireSeconds = await SettingProvider.GetAsync(expireSecondSettingName, 60);

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
            var captcha = await CaptchaStore.FindAsync(type, receiver, code, CurrentTenant.Id);
            if (captcha == null)
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