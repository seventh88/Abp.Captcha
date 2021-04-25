using Light.Abp.Captcha.Localization;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Light.Abp.Captcha
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpLocalizationModule),
        typeof(AbpCachingModule))]
    public class AbpCaptchaModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpCaptchaModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpCaptchaResource>("en")
                    .AddVirtualJson("/Localization");
            });
        }
    }
}
