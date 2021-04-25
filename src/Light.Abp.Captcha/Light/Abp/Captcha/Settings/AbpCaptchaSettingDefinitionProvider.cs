using Light.Abp.Captcha.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Light.Abp.Captcha.Settings
{
    public class AbpCaptchaSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(CreateAppVersionSettings());
        }

        protected SettingDefinition[] CreateAppVersionSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(
                        name: AbpCaptchaSettings.EmailCaptchaExpireSeconds,
                        defaultValue: "600",
                        displayName: L("DisplayName:EmailCaptchaExpireSeconds"),
                        description: L("Description:EmailCaptchaExpireSeconds"),
                        isVisibleToClients: true)
                    .WithProviders(
                        GlobalSettingValueProvider.ProviderName,
                        TenantSettingValueProvider.ProviderName),

                new SettingDefinition(
                        name:AbpCaptchaSettings.SmsCaptchaExpireSeconds,
                        defaultValue: "300",
                        displayName: L("DisplayName:SmsCaptchaExpireSeconds"),
                        description: L("Description:SmsCaptchaExpireSeconds"),
                        isVisibleToClients: true)
                    .WithProviders(
                        GlobalSettingValueProvider.ProviderName,
                        TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                        name:AbpCaptchaSettings.CaptchaFrequencyLimitSeconds,
                        defaultValue: "60",
                        displayName: L("DisplayName:CaptchaFrequencyLimitSeconds"),
                        description: L("Description:CaptchaFrequencyLimitSeconds"),
                        isVisibleToClients: true)
                    .WithProviders(
                        GlobalSettingValueProvider.ProviderName,
                        TenantSettingValueProvider.ProviderName),
            };
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpCaptchaResource>(name);
        }
    }
}
