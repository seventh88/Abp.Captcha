namespace Light.Abp.Captcha.Settings
{
    public static class AbpCaptchaSettings
    {
        private const string Prefix = "Captchas";

        public const string EmailCaptchaExpireSeconds = Prefix + ".EmailCaptcha.ExpireSeconds";

        public const string PhoneNumberCaptchaExpireSeconds = Prefix + ".PhoneNumberCaptcha.ExpireSeconds";
    }
}