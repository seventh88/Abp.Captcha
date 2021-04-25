namespace Light.Abp.Captcha
{
    public class AbpCaptchaOptions
    {
        public AbpCaptchaOptions()
        {
            EmailCaptchaExpireSeconds = 600;
            SmsCaptchaExpireSeconds = 300;
            CaptchaFrequencyLimitSeconds = 60;
        }

        public int EmailCaptchaExpireSeconds { get; set; }

        public int SmsCaptchaExpireSeconds { get; set; }

        public int CaptchaFrequencyLimitSeconds { get; set; }
    }
}
