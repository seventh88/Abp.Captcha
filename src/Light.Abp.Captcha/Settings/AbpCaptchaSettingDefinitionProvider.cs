using Volo.Abp.Settings;

namespace Light.Abp.Captcha.Settings
{
    public class AbpCaptchaSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            context.Add(new SettingDefinition(AbpCaptchaSettings.EmailCaptchaExpireSeconds));
            context.Add(new SettingDefinition(AbpCaptchaSettings.SmsCaptchaExpireSeconds));
            context.Add(new SettingDefinition(AbpCaptchaSettings.CaptchaFrequencyLimitSeconds));
        }
    }
}
