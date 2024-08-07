using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	internal class SignerInputBuffer : MemoryStream
	{
		private class SigStream : BaseOutputStream
		{
			private readonly ISigner s;

			internal SigStream(ISigner s)
			{
				this.s = s;
			}

			public override void WriteByte(byte b)
			{
				s.Update(b);
			}

			public override void Write(byte[] buf, int off, int len)
			{
				s.BlockUpdate(buf, off, len);
			}
		}

		internal void UpdateSigner(ISigner s)
		{
			WriteTo(new SigStream(s));
		}
	}
}
