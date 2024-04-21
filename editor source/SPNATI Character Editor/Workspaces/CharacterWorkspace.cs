﻿using Desktop;
using SPNATI_Character_Editor.Activities;
using SPNATI_Character_Editor.Services;
using System;
using System.Collections.Generic;

namespace SPNATI_Character_Editor.Workspaces
{
	[Workspace(typeof(Character))]
	public class CharacterWorkspace : Workspace
	{
		public const string SpellCheckerService = "SpellCheck";

		private static HashSet<Character> _sessionActivations = new HashSet<Character>();
		private Character _character;

		protected override void OnInitialize()
		{
			_character = Record as Character;
			_character.PrepareForEdit();

			SpellCheckerService spellChecker = new SpellCheckerService(_character);
			SetData(SpellCheckerService, spellChecker);

			Config.Set(Settings.LastCharacter, _character.FolderName);
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			if (!_sessionActivations.Contains(_character))
			{
				_sessionActivations.Add(_character);
				if (Config.EnableDashboard && (Config.DevMode || Config.IncludesUserName(_character.Metadata.Writer)) && GlobalCache.HasChanges(_character.FolderName))
				{
					Shell.Instance.ShowToast("New Incoming Dialogue!", $"Some characters have had new lines written that target {_character}. Check out the Dashboard for a summary.");
				}
			}

			if (new Version(_character.Version.Substring(1)).CompareTo(Config.version) > 0)
			{
				ShowBanner($"This character was saved in a later version of the editor ({_character.Version}). Some features may not work properly.", Desktop.Skinning.SkinnedHighlight.Bad);
			}

			string status = Listing.Instance.GetCharacterStatus(_character.FolderName);

			if (Config.WarnAboutIncompleteStatus)
			{
				if (status == OpponentStatus.Duplicate)
				{
					ShowBanner("This character has been replaced by a newer version.", Desktop.Skinning.SkinnedHighlight.Bad);
				}
				else if (status == OpponentStatus.Event)
				{
					ShowBanner("This is a joke character, only available during the April Fool's Day event.", Desktop.Skinning.SkinnedHighlight.Bad);
				}
				else if (status == OpponentStatus.Incomplete || status == OpponentStatus.Broken)
				{
					ShowBanner("This character is incomplete, meaning they have likely been abandoned.", Desktop.Skinning.SkinnedHighlight.Bad);
				}
			}
		}

		public override bool AllowAutoStart(Type activityType)
		{
			if (activityType == typeof(Dashboard) && (!Config.EnableDashboard || _character.IsNew))
			{
				return false;
			}
			if ((activityType == typeof(PoseListEditor) || activityType == typeof(TemplateEditor)) && !Config.ShowLegacyPoseTabs)
			{
				return false;
			}
			return base.AllowAutoStart(activityType);
		}

		public override IActivity GetDefaultActivity()
		{
			if (!Config.StartOnDashboard)
			{
				List<IActivity> list = Activities[WorkspacePane.Main];
				return list[1];
			}
			return base.GetDefaultActivity();
		}
	}
}
