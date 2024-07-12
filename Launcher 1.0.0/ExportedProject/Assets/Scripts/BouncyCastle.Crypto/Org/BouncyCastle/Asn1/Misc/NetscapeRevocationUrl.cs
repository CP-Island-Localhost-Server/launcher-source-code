namespace Org.BouncyCastle.Asn1.Misc
{
	public class NetscapeRevocationUrl : DerIA5String
	{
		public NetscapeRevocationUrl(DerIA5String str)
			: base(str.GetString())
		{
		}

		public override string ToString()
		{
			return "NetscapeRevocationUrl: " + GetString();
		}
	}
}
