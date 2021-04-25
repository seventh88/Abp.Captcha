using System;
using Volo.Abp.MultiTenancy;

namespace Light.Abp.Captcha
{
    public class Captcha : IMultiTenant
    {
        public Captcha(string type, string code, string receiver, EnumReceiverType receiverType, DateTime creationTime, int expireSeconds, Guid? tenantId)
        {
            Type = type;
            Code = code;
            Receiver = receiver;
            ReceiverType = receiverType;
            CreationTime = creationTime;
            ExpireSeconds = expireSeconds;
            TenantId = tenantId;
            ExpireTime = creationTime.AddSeconds(expireSeconds);
        }

        /// <summary>
        /// 验证码类型(注册/找回密码/登陆/...)
        /// </summary>
        public virtual string Type { get; protected set; }

        public virtual string Code { get; protected set; }

        /// <summary>
        /// 接收人（电话/Email）
        /// </summary>
        public virtual string Receiver { get; protected set; }

        public virtual EnumReceiverType ReceiverType { get; protected set; }

        public virtual DateTime CreationTime { get; protected set; }

        public virtual int ExpireSeconds { get; protected set; }

        public virtual DateTime ExpireTime { get; protected set; }

        public virtual DateTime? UsedTime { get; protected set; }

        public virtual bool IsUsed { get; protected set; }
        public Guid? TenantId { get; protected set; }

        public void Use(DateTime usedTime)
        {
            if (!IsUsed)
            {
                this.UsedTime = usedTime;
                this.IsUsed = true;
            }
        }

        public bool IsExpire(DateTime now)
        {
            return now > ExpireTime;
        }

    }
}