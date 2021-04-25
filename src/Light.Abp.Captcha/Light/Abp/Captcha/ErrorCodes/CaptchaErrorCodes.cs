namespace Light.Abp.Captcha
{
    public static class CaptchaErrorCodes
    {
        public const string Prefix = "Light.Abp.Captcha:";
        public const string FrequencyLimit = Prefix + "FrequencyLimit";
        public const string SendFailed = Prefix + "SendFailed";
        public const string Error = Prefix + "Error";
        public const string Used = Prefix + "Used";
        public const string Expired = Prefix + "Expired";
    }
}