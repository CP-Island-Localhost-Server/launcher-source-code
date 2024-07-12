using System.Collections;
using System.Runtime.Remoting.Channels;

namespace Mono.Remoting.Channels.Unix
{
	internal class UnixBinaryServerFormatterSinkProvider : IServerChannelSinkProvider, IServerFormatterSinkProvider
	{
		private IServerChannelSinkProvider next;

		private UnixBinaryCore _binaryCore;

		internal static string[] AllowedProperties = new string[2] { "includeVersions", "strictBinding" };

		public IServerChannelSinkProvider Next
		{
			get
			{
				return next;
			}
			set
			{
				next = value;
			}
		}

		public UnixBinaryServerFormatterSinkProvider()
		{
			_binaryCore = UnixBinaryCore.DefaultInstance;
		}

		public UnixBinaryServerFormatterSinkProvider(IDictionary properties, ICollection providerData)
		{
			_binaryCore = new UnixBinaryCore(this, properties, AllowedProperties);
		}

		public IServerChannelSink CreateSink(IChannelReceiver channel)
		{
			IServerChannelSink nextSink = null;
			if (next != null)
			{
				nextSink = next.CreateSink(channel);
			}
			UnixBinaryServerFormatterSink unixBinaryServerFormatterSink = new UnixBinaryServerFormatterSink(nextSink, channel);
			unixBinaryServerFormatterSink.BinaryCore = _binaryCore;
			return unixBinaryServerFormatterSink;
		}

		public void GetChannelData(IChannelDataStore channelData)
		{
		}
	}
}
