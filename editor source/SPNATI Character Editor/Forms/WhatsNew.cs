using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Forms
{
	public partial class WhatsNew : SkinnedForm
	{
		public WhatsNew()
		{
			InitializeComponent();

			lblVersion.Text = Config.Version;
		}

		private void WhatsNew_Load(object sender, EventArgs e)
		{
			wb.Navigate(Path.Combine(Config.ExecutableDirectory, "VersionHistory", "whatsnew.html"));
		}

		private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			DisplayVersionHistory();
		}

		private void DisplayVersionHistory()
		{
			//Acquire updates from all versions skipped
			Version lastVersion = new Version(Config.GetString(Settings.LastVersionRun).Substring(1));
			StringBuilder updates = new StringBuilder();
			string[] files = Directory.GetFiles(Config.ExecutableDirectory, "VersionHistory/v*.html", SearchOption.TopDirectoryOnly);
			List<KeyValuePair<string, string>> versionUpdates = new List<KeyValuePair<string, string>>();
			foreach (string file in files.Reverse())
			{
				string version;
				Match match = Regex.Match(file, @"v(\d\.)+html");
				if (!match.Success)
				{
					continue;
				}
				else
				{
					string str = match.Value;
					version = str.Remove(str.Length - 5);
					if (new Version(version.Substring(1)).CompareTo(lastVersion) <= 0) { continue; }
					versionUpdates.Add(new KeyValuePair<string, string>(version, File.ReadAllText(file)));
				}
			}
			versionUpdates.Sort((pair1, pair2) => - new Version(pair1.Key.Substring(1)).CompareTo(new Version(pair2.Key.Substring(1))));
			foreach (KeyValuePair<string, string> pair in versionUpdates)
			{
				updates.Append("<section class='card'>");
				updates.Append("<h1>");
				updates.Append(pair.Key);
				updates.Append("</h1>");
				updates.Append(pair.Value);
				updates.Append("</section>");
			}
			wb.Document.Body.InnerHtml = updates.ToString();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void WhatsNew_FormClosing(object sender, FormClosingEventArgs e)
		{
			Config.Set(Settings.LastVersionRun, Config.Version);
			Config.Save();
		}
	}
}
