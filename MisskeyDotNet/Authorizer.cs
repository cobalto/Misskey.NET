using System;
using System.Threading.Tasks;
using MisskeyDotNet.Helpers;
using MisskeyDotNet.Models.Authorization;

namespace MisskeyDotNet
{
    public sealed class Authorizer
    {
        public InstanceSettings InstanceSettings
        {
            get { return instanceSettings; }
        }

        public Authorizer(InstanceSettings instanceSettings)
        {
            this.instanceSettings = instanceSettings;
        }

        public bool TryOpenBrowser()
        {
            try
            {
                ProcessHelper.OpenUrl(this.instanceSettings.Url);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async ValueTask<Misskey> CheckAsync()
        {
            var res = await new Misskey(this.instanceSettings).ApiAsync(string.Format(Constants.AUTH_CHECK_ENDPOINT, this.instanceSettings.Uuid));

            if (res["token"] is string token)
                return new Misskey(this.instanceSettings, token);

            throw new InvalidOperationException("Failed to check MiAuth session");
        }

        private readonly InstanceSettings instanceSettings;
    }
}