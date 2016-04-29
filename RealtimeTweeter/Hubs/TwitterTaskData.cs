using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Tweetinvi.Core.Interfaces.Models;

namespace RealtimeTweeter.Hubs
{
    public class TwitterTaskData
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public IOEmbedTweet Tweet { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
    }
}