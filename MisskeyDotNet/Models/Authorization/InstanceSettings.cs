using System;
using MisskeyDotNet.Extensions;
using MisskeyDotNet.Helpers;

namespace MisskeyDotNet.Models.Authorization
{
	public class InstanceSettings
    {
        public string Host { get; }

        public string? Name { get; }

        public string? IconUrl { get; }

        public string? CallbackUrl { get; }

        public string Uuid { get; }

        public Permission[] Permissions { get; } = new Permission[0];

        public string Url { get; }

        public InstanceSettings(string host, string? name, string? iconUrl = null, string? callbackUrl = null, params Permission[] permissions)
        {
            Host = host;
            Name = name;
            IconUrl = iconUrl;
            CallbackUrl = callbackUrl;
            Uuid = Guid.NewGuid().ToString();
            Permissions = permissions;

            var query = new
            {
                name = Name,
                icon = IconUrl,
                callback = CallbackUrl,
                permission = string.Join(',', Permissions.ToStringArray()),
            }.ToQueryString();

            Url = string.Format(Constants.AUTH_URL, Host, Uuid, query ?? string.Empty);
        }
    }
}