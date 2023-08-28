﻿using SPNATI_Character_Editor.DataStructures;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls.Dashboards
{
	public partial class SponsorshipWidget : UserControl, IDashboardWidget
	{
		private Character _character;

		public SponsorshipWidget()
		{
			InitializeComponent();
		}

		public void Initialize(Character character)
		{
			_character = character;
			grpRequirements.Shield();
		}

		public bool IsVisible()
		{
			return TestRequirements.Instance.Lines > 0;
		}

		public IEnumerator DoWork()
		{
			CharacterHistory history = CharacterHistory.Get(_character, false);
			TestRequirements requirements = TestRequirements.Instance;
			//StringBuilder sb = new StringBuilder();
			float fileSize = history.GetTotalFileSize(true);
			LineWork current = history.Current;
			barLines.Maximum = requirements.Lines;
			barLines.Value = current.TotalLines;
			barFilters.Maximum = requirements.Filtered;
			barFilters.Value = current.FilterCount;
			barTargets.Maximum = requirements.Targeted;
			barTargets.Value = current.TargetedCount;
			barUnique.Maximum = requirements.UniqueTargets;
			barUnique.Value = current.Targets.Count;

			if (Listing.Instance.IsCharacterReleased(_character.FolderName))
			{
				barSize.Maximum = requirements.BiggerSizeLimit;
			}
			else
			{
				barSize.Maximum = requirements.SizeLimit;
			}

			barSize.Value = (decimal)fileSize;
			barCollectibles.Maximum = TestRequirements.Instance.GetAllowedCollectibles(current.TotalLines);
			barCollectibles.Value = _character.Collectibles.Count;
			int maxSettings = TestRequirements.Instance.GetAllowedSettings(current.TotalLines);
			if (maxSettings < 1 || !Listing.Instance.IsCharacterReleased(_character.FolderName))
			{
				barSettings.Visible = false;
			}
			else
			{
				int currentSettings = 0;
				foreach (CharacterSettingsGroup group in _character.Behavior.CharacterSettingsGroups)
				{
					currentSettings += group.CharacterSettings.Count - 1;
				}
				barSettings.Visible = true;
				barSettings.Maximum = maxSettings;
				barSettings.Value = currentSettings;
			}
			grpRequirements.Unshield();
			yield break;
		}
	}
}
