using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace RealtimeTweeter.Hubs
{
    public static class TwitterStream
    {
        private static IUserStream _stream;
        private static readonly IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<TwitterHub>();

        public static async Task StartStream(CancellationToken token)
        {
            // Go to https://apps.twitter.com to get your own tokens.
            Auth.SetUserCredentials("sTHGrHny1oDSAoCIJ1HOMorLr",
                "bFeBKjOBFiedReKXGuzJfeMrF5e6F4kePftKBzTEYxIiVTQf1D",
                "3250802522-hatYQjNv6aBHtZon9inaDzpWi3dNUTAYxNsWZb3",
                "hpztKrdaO3BkElPeUdcdK5M1hRv5k28Xqwh3Q2Z9RPPdN");

            if (_stream == null)
            {
                _stream = Stream.CreateUserStream();

                // Other events can be used. This is just on YOUR twitter feed.
                _stream.TweetCreatedByAnyone += async (sender, args) =>
                {
                    if (token.IsCancellationRequested)
                    {
                        _stream.StopStream();
                        token.ThrowIfCancellationRequested();
                    }

                    // let's use the embed tweet from tweetinvi
                    var embedTweet = Tweet.GenerateOEmbedTweet(args.Tweet);
                    await _context.Clients.All.updateTweet(embedTweet);
                };

                // If anything changes the state, update the UI.
                _stream.StreamPaused += async (sender, args) => { await _context.Clients.All.updateStatus("Paused."); };
                _stream.StreamResumed += async (sender, args) => { await _context.Clients.All.updateStatus("Streaming..."); };
                _stream.StreamStarted += async (sender, args) => { await _context.Clients.All.updateStatus("Started."); };
                _stream.StreamStopped += async (sender, args) => { await _context.Clients.All.updateStatus("Stopped (event)"); };

                await _stream.StartStreamAsync();
            }
            else
            {
                _stream.ResumeStream();
            }

            await _context.Clients.All.updateStatus("Started.");
        }
    }
}