using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Forms
{
	public partial class ChangeLogReview : SkinnedForm
	{
		public ChangeLogReview()
		{
			InitializeComponent();
		}

		private void ChangeLogReview_Load(object sender, EventArgs e)
		{
			wb.Navigate(Path.Combine(Config.ExecutableDirectory, "VersionHistory", "whatsnew.html"));
		}

		private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			string[] files = Directory.GetFiles(Config.ExecutableDirectory, "VersionHistory/v*.html", SearchOption.TopDirectoryOnly);
			List<string> versions = new List<string>();
			foreach (string file in files)
			{
				Match match = Regex.Match(file, @"v(\d\.)+html");
				if (!match.Success)
				{
					continue;
				}
				string str = match.Value.Substring(1);
				versions.Add(str.Remove(str.Length - 5));
			}
			versions.Sort(delegate (string x, string y) { return new Version(x).CompareTo(new Version(y)); });
			foreach (string version in versions)
			{
				lstVersions.Items.Add("v" + version);
			}
		}

		private void lstVersions_SelectedIndexChanged(object sender, EventArgs e)
		{
			ShowVersion(lstVersions.SelectedItem?.ToString());
		}

		private void ShowVersion(string version)
		{
			string file = Path.Combine(Config.ExecutableDirectory, $"VersionHistory/{version}.html");
			StringBuilder sb = new StringBuilder();
			sb.Append("<section class='card'>");
			if (File.Exists(file))
			{
				sb.Append(File.ReadAllText(file));
			}
			else
			{
				sb.Append("No change log for this version.");
			}
			sb.Append("</section>");
			wb.Document.Body.InnerHtml = sb.ToString();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
