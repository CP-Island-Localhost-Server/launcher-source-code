using ClubPenguin.Analytics;
using Disney.Kelowna.Common;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class CdnGetFileBIHelper : CdnGetFileHelper
	{
		public readonly string BITier1;

		protected readonly string biLabel;

		public CdnGetFileBIHelper(string biTier1, string logTitle, string biLabel, string expectedContentHash)
			: base(logTitle, expectedContentHash)
		{
			BITier1 = biTier1;
			this.biLabel = biLabel;
		}

		protected override void downloadCompleted(bool success, string filename, string errorMessage)
		{
			base.downloadCompleted(success, filename, errorMessage);
			Service.Get<ICPSwrveService>().Action(BITier1, "complete");
		}

		protected override void downloadFailed(string filename, string errorMessage)
		{
			base.downloadFailed(filename, errorMessage);
			Service.Get<ICPSwrveService>().Action(BITier1, "error", errorMessage);
			Service.Get<ICPSwrveService>().Action("error_prompt", "error_downloading_" + biLabel);
		}

		protected override void hashTestFailed(string filename, string calculatedHash)
		{
			base.hashTestFailed(filename, calculatedHash);
			Service.Get<ICPSwrveService>().Action(BITier1, "error", "content_hash_mismatch");
		}

		protected override void downloadedFileNotFound(string filename)
		{
			base.downloadedFileNotFound(filename);
			Service.Get<ICPSwrveService>().Action(BITier1, "error", "file_not_found");
		}
	}
}
