﻿using Desktop;
using Newtonsoft.Json;
using SPNATI_Character_Editor.Providers;
using System;
using System.IO;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Recipe for creating a case geared to target a particular game situation
	/// </summary>
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class Recipe : Definition
	{
		public bool Core;

		[JsonProperty("case")]
		public Case Case = new Case();

		[JsonProperty("label")]
		public string Label;

		public string FileName;

		public string GetFilePath()
		{
			string dir = "";
			if (Core)
			{
				dir = Path.Combine("Resources", "Recipes");
			}
			else
			{
				dir = Path.Combine(Config.AppDataDirectory, "Recipes");
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
			}
			string filename = FileName;
			if (string.IsNullOrEmpty(filename))
			{
				filename = Key;
			}
			if (!filename.EndsWith(".txt"))
			{
				filename += ".txt";
			}
			return Path.Combine(dir, filename);
		}

		/// <summary>
		/// Turn this recipe into a case for a character
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public Case AddToCharacter(Character character)
		{
			Case workingCase = Case.Copy();
			//apply to all applicable stages by default
			for (int stage = 0; stage < character.Layers + Clothing.ExtraStages; stage++)
			{
				if (TriggerDatabase.UsedInStage(workingCase.Tag, character, stage))
				{
					workingCase.Stages.Add(stage);
				}
				if (workingCase.TotalRounds == "1")
				{
					break;
				}
			}
			if (workingCase.OneShotId > 0)
			{
				//need to assign a unique ID
				workingCase.OneShotId = ++character.Behavior.MaxCaseId;
			}
			if (!string.IsNullOrEmpty(Label))
			{
				CharacterEditorData editorData = CharacterDatabase.GetEditorData(character);
				if (editorData != null)
				{
					editorData.SetLabel(workingCase, Label, null, null);
				}
			}
			DialogueLine line = new DialogueLine("happy", Description);
			workingCase.Lines.Add(line);
			character.Behavior.AddWorkingCase(workingCase);
			return workingCase;
		}
	}

	public class RecipeProvider : DefinitionProvider<Recipe>
	{
		protected override Recipe CreateRecord(string key)
		{
			string guid = Guid.NewGuid().ToString();
			Recipe recipe = base.CreateRecord(guid);
			recipe.Name = key;
			return recipe;
		}

		public override string GetLookupCaption()
		{
			return "Choose a Recipe";
		}

		public override void ApplyDefaults(Recipe definition)
		{
			Recipe recipe = definition as Recipe;
			recipe.Case.Tag = "hand";
		}

		public static void Load()
		{
			string dir = Path.Combine("Resources", "Recipes");
			if (Directory.Exists(dir))
			{
				foreach (string file in Directory.EnumerateFiles(dir, "*.txt"))
				{
					try
					{
						string json = File.ReadAllText(file);
						Recipe recipe = Json.Deserialize<Recipe>(json);
						recipe.FileName = Path.GetFileName(file);
						recipe.Core = true;
						Definitions.Instance.Add(recipe);
					}
					catch { }
				}
			}

			dir = Path.Combine(Config.AppDataDirectory, "Recipes");
			if (Directory.Exists(dir))
			{
				foreach (string file in Directory.EnumerateFiles(dir, "*.txt"))
				{
					try
					{
						string json = File.ReadAllText(file);
						Recipe recipe = Json.Deserialize<Recipe>(json);
						recipe.FileName = Path.GetFileName(file);
						Definitions.Instance.Add(recipe);
					}
					catch { }
				}
			}

		}
	}
}
