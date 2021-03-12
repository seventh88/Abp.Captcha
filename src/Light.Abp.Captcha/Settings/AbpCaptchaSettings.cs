namespace Light.Abp.Captcha.Settings
{
    public static class AbpCaptchaSettings
    {
        private const string Prefix = "AbpCaptchas";

        public const string EmailCaptchaExpireSeconds = Prefix + ".EmailCaptcha.ExpireSeconds";

        public const string SmsCaptchaExpireSeconds = Prefix + ".SmsCaptcha.ExpireSeconds";

        public const string CaptchaFrequencyLimitSeconds = Prefix + ".Captcha.FrequencyLimitSeconds";
    }
}