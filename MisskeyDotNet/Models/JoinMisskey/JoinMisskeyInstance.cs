using System;
using MisskeyDotNet.Models.HttpApi;

namespace MisskeyDotNet.Models.JoinMisskey
{
    public class JoinMisskeyInstance
    {
        public string Url { get; set; } = "";
        public string[] Langs { get; set; } = Array.Empty<string>();
        public double Value { get; set; }
        public Meta Meta { get; set; } = new Meta();
    }
}