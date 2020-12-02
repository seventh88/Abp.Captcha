using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Light.Abp.Captcha
{
    public class CacheCaptchaStore : ICaptchaStore, ITransientDependency
    {
        public IDistributedCache<Captcha> Cache { get; }

        public CacheCaptchaStore(IDistributedCache<Captcha> cache)
        {
            Cache = cache;
        }

        public virtual async Task<Captcha> CreateAsync(Captcha captcha)
        {
            var key = GetKey(captcha.Type, captcha.Receiver, captcha.TenantId);
            await Cache.SetAsync(key, captcha, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(captcha.ExpireSeconds)
            });
            return captcha;
        }

        public virtual async Task<Captcha> FindAsync(string type, string receiver, string code, Guid? tenantId)
        {
            var key = GetKey(type, receiver, tenantId);
            var captcha = await Cache.GetAsync(key);
            if (captcha != null && captcha.Code != code)
            {
                return null;
            }
            return captcha;
        }

        public virtual async Task<bool> ExistValidAsync(string type, string receiver, Guid? tenantId)
        {
            var key = GetKey(type, receiver, tenantId);
            var captcha = await Cache.GetAsync(key);
            return captcha != null && !captcha.IsUsed;
        }

        public virtual async Task<Captcha> UsedAsync(Captcha captcha)
        {
            var key = GetKey(captcha.Type, captcha.Receiver, captcha.TenantId);
            await Cache.RemoveAsync(key);
            return captcha;
        }

        public virtual string GetKey(string type, string receiver, Guid? tenantId)
        {
            var key = $"{type}_{receiver}_";
            if (tenantId != null)
            {
                key += tenantId.ToString();
            }

            return key;
        }
    }
}