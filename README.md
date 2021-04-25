# Abp.Captcha

ABP 验证码模块

## 1. 介绍

**Light.Abp.Captcha** 库定义了验证码的发送、校验、存储等基本接口，并提供默认实现。（发送接口需集成后自行实现）验证码默认存储为缓存。

## 2. 如何使用?

### 2.1 安装

开发人员可以通过 NuGet 搜索 `Light.Abp.Captcha` 安装组件

### 2.2 配置

支持配置验证码过期时间

配置示例:

```json
  "Settings": {
    "AbpCaptchas:EmailCaptcha.ExpireSeconds": 600,//邮箱验证码过期时间
    "AbpCaptchas:SmsCaptcha.ExpireSeconds": 300,//短信验证码过期时间
    "AbpCaptchas:Captcha.FrequencyLimitSeconds": 60//请求频率限制
  },
```

### 2.3 实现发送接口
需要在项目中实现验证码发送（短信或邮件）的接口
```csharp
    public class AliyunSmsCaptchaSender: ISmsCaptchaSender
    {
        public virtual Task<bool> SendAsync(Captcha captcha)
        {
            var templateCode = "";
            var signName = "";
            switch (captcha.Type)
            {
                case "login":
                    templateCode = "SMS_123456";
                    break;
                case "retrievePassword":
                case "changePassword":
                    templateCode = "SMS_1234567";
                    break;
            }
            var code = captcha.Code;
            var smsRequest = new SendSmsRequest()
            {
                PhoneNumbers = captcha.Receiver,
                TemplateCode = templateCode,
                TemplateParam = JsonConvert.SerializeObject(new { code }),
                SignName = signName,
            };

            var response = _smsSender.Send(smsRequest);
            if (response.Code != "OK")
            {
                throw new BusinessException(response.Message);
            }
        }
    }

    public class EmailCaptchaSender: IEmailCaptchaSender
    {
          public virtual Task<bool> SendAsync(Captcha captcha)
          {
            //todo 发送验证码
          }
    }
```
### 2.4 调用示例
在AppService中，注入ICaptchaManager即可使用
```csharp
    public class CaptchaAppService : ApplicationService
    {
        private readonly IdentityUserManager _userManager;
        private readonly ICaptchaManager _captchaManager;

        public CaptchaAppService(IdentityUserManager userManager,
            ICaptchaManager captchaManager)
        {
            _userManager = userManager;
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="type">类型：(登录:login/修改密码: changePassword/找回密码：retrievePassword)</param>
        [AllowAnonymous]
        public async Task GetAsync(string phoneNumber, string type)
        {
            await _captchaManager.SendAsync(type, phoneNumber);
        }

        public async Task VerifyAsync(string phoneNumber, string type)
        {
           await _captchaManager.VerifyAsync("login", phoneNumber, captcha);
        }
        
    }
```
