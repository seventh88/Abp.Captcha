﻿using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Light.Abp.Captcha
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpCachingModule))]
    public class AbpCaptchaModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
