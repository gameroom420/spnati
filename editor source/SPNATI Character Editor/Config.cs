﻿using System;
using System.Collections.Generic;
using System.IO;

namespace SPNATI_Character_Editor
{
	public static class Config
	{
		/// <summary>
		/// List of released versions since update tracking was added, used for determining which updates a user skipped and providing info about those
		/// </summary>
		public static readonly string[] VersionHistory = new string[] { "v3.0", "v3.0.1", "v3.1", "v3.2" };

		/// <summary>
		/// Current Version
		/// </summary>
		public static string Version { get { return VersionHistory[VersionHistory.Length - 1]; } }

		private static Dictionary<string, string> _settings = new Dictionary<string, string>();

		/// <summary>
		/// Gets a string configuration setting
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetString(string key)
		{
			return _settings.Get(key.ToLower()) ?? "";
		}

		/// <summary>
		/// Gets a boolean configuration setting
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool GetBoolean(string key)
		{
			string setting = _settings.Get(key.ToLower()) ?? "";
			return !string.IsNullOrEmpty(setting) && setting != "0";
		}

		public static int GetInt(string key)
		{
			string setting = _settings.Get(key.ToLower()) ?? "0";
			int value;
			int.TryParse(setting, out value);
			return value;
		}

		/// <summary>
		/// Sets a configuration setting
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set(string key, string value)
		{
			_settings[key.ToLower()] = value;
		}

		/// <summary>
		/// Sets a boolean configuration setting
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set(string key, bool value)
		{
			_settings[key.ToLower()] = (value ? "1" : "0");
		}

		/// <summary>
		/// Sets a numeric configuration setting
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void Set(string key, int value)
		{
			_settings[key.ToLower()] = value.ToString();
		}

		static Config()
		{
			//3.0 and up use config.ini. Older versions use settings.ini. Using different filenames to allow side-by-side installs since the structure was changed

			string filename = Path.Combine(AppDataDirectory, "config.ini");
			if (File.Exists(filename))
			{
				ReadSettings(filename);
			}
			else
			{
				filename = Path.Combine(AppDataDirectory, "settings.ini");
				if (File.Exists(filename))
				{
					ReadLegacySettings(filename);
				}
			}
		}

		private static void ReadSettings(string file)
		{
			string[] lines = File.ReadAllLines(file);
			try
			{
				for (int i = 0; i < lines.Length; i++)
				{
					string line = lines[i];
					string[] kvp = line.Split('=');
					string key = kvp[0].ToLower();
					string value = kvp[1];
					Set(key, value);
				}
			}
			catch
			{
			}
		}

		private static void ReadLegacySettings(string file)
		{
			string[] lines = File.ReadAllLines(file);
			try
			{
				Set(Settings.GameDirectory, lines[0]);
				Set(Settings.LastCharacter, lines[1]);
				Set(Settings.LastVersionRun, lines[5]);
			}
			catch { }
		}

		public static void Save()
		{
			string dataDir = AppDataDirectory;
			string filename = Path.Combine(dataDir, "config.ini");
			if (!Directory.Exists(dataDir))
			{
				Directory.CreateDirectory(dataDir);
			}

			List<string> lines = new List<string>();
			foreach (KeyValuePair<string, string> kvp in _settings)
			{
				lines.Add($"{kvp.Key.ToLower()}={kvp.Value}");
			}
			File.WriteAllLines(filename, lines);
		}

		/// <summary>
		/// Gets where SPNATI is located
		/// </summary>
		public static string SpnatiDirectory
		{
			get { return GetString(Settings.GameDirectory); }
		}

		/// <summary>
		/// Gets the programs %appdata% path
		/// </summary>
		public static string AppDataDirectory
		{
			get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SPNATI"); }
		}

		/// <summary>
		/// Retrieves the root directory for a character
		/// </summary>
		public static string GetRootDirectory(Character character)
		{
			if (character == null || string.IsNullOrEmpty(character.FolderName))
				return "";
			return GetRootDirectory(character.FolderName);
		}

		/// <summary>
		/// Retrieves the full directory name for a folder
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static string GetRootDirectory(string folder)
		{
			if (GetString(Settings.GameDirectory) == null || folder == null)
				return "";
			return Path.Combine(GetString(Settings.GameDirectory), "opponents", folder);
		}
	}

	public static class Settings
	{
		public static readonly string GameDirectory = "game";
		public static readonly string LastCharacter = "last";
		public static readonly string LastVersionRun = "version";

		#region Settings that probably only make sense for debugging
		public static readonly string LoadOnlyLastCharacter = "loadlast";
		public static readonly string HideImages = "safemode";
		#endregion
	}
}
