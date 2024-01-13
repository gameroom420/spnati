using System;

namespace SPNATI_Character_Editor.Analyzers
{
	public class GenderAnalyzer : IDataAnalyzer
	{
		public string Key
		{
			get { return "Gender"; }
		}

		public string Name
		{
			get { return "Gender"; }
		}

		public string FullName
		{
			get { return "Gender"; }
		}

		public string ParentKey
		{
			get { return ""; }
		}

		public string[] GetValues()
		{
			return new string[] { "female", "male" };
		}

		public Type GetValueType()
		{
			return typeof(string);
		}

		public bool MeetsCriteria(Character character, string op, string value)
		{
			return StringOperations.Matches(character.Gender, op, value);
		}
	}

	public class PenisSizeAnalyzer : IDataAnalyzer
	{
		public string Key
		{
			get { return "Penis Size"; }
		}

		public string Name
		{
			get { return "Penis Size"; }
		}

		public string FullName
		{
			get { return "Penis Size"; }
		}

		public string ParentKey
		{
			get { return ""; }
		}

		public string[] GetValues()
		{
			return new string[] { "large", "medium", "small" };
		}

		public Type GetValueType()
		{
			return typeof(string);
		}

		public bool MeetsCriteria(Character character, string op, string value)
		{
			return StringOperations.Matches((!string.IsNullOrEmpty(character.LegacySize) && character.Gender == "male")? character.LegacySize : character.Penis, op, value);
		}
	}

	public class BreastsSizeAnalyzer : IDataAnalyzer
	{
		public string Key
		{
			get { return "Breasts Size"; }
		}

		public string Name
		{
			get { return "Breasts Size"; }
		}

		public string FullName
		{
			get { return "Breasts Size"; }
		}

		public string ParentKey
		{
			get { return ""; }
		}

		public string[] GetValues()
		{
			return new string[] { "large", "medium", "small" };
		}

		public Type GetValueType()
		{
			return typeof(string);
		}

		public bool MeetsCriteria(Character character, string op, string value)
		{
			return StringOperations.Matches((!string.IsNullOrEmpty(character.LegacySize) && character.Gender == "female") ? character.LegacySize : character.Breasts, op, value);
		}
	}

	public class SkinAnalyzer : NumericAnalyzer
	{
		public override string Key
		{
			get { return "AlternateSkins"; }
		}

		public override string Name
		{
			get { return "Alternate Skins"; }
		}

		public override string FullName
		{
			get { return "Alternate Skin Count"; }
		}

		public override string ParentKey
		{
			get { return ""; }
		}

		public override string[] GetValues()
		{
			return null;
		}

		public override int GetValue(Character character)
		{
			return character.Metadata.AlternateSkins.Count;
		}
	}

	public class IntelligenceAnalyzer : IDataAnalyzer
	{
		public string Key
		{
			get { return "Intelligence"; }
		}

		public string Name
		{
			get { return "Intelligence"; }
		}

		public string FullName
		{
			get { return "Intelligence"; }
		}

		public string ParentKey
		{
			get { return ""; }
		}

		public string[] GetValues()
		{
			return new string[] { "best", "good", "average", "bad", "throw", "no-swap" };
		}

		public Type GetValueType()
		{
			return typeof(string);
		}

		public bool MeetsCriteria(Character character, string op, string value)
		{
			foreach (StageSpecificValue val in character.Intelligence)
			{
				if (StringOperations.Matches(val.Value, op, value))
				{
					return true;
				}
			}
			return false;
		}
	}

	public class WriterAnalyzer : IDataAnalyzer
	{
		public string Key
		{
			get { return "Writer"; }
		}

		public string Name
		{
			get { return "Writer"; }
		}

		public string FullName
		{
			get { return "Writer"; }
		}

		public string ParentKey
		{
			get { return ""; }
		}

		public string[] GetValues()
		{
			return new string[] { };
		}

		public Type GetValueType()
		{
			return typeof(string);
		}

		public bool MeetsCriteria(Character character, string op, string value)
		{
			string[] writers = (character.Metadata.Writer ?? "").Split(',');
			foreach (string writer in writers)
			{
				if (StringOperations.Matches(writer, op, value))
				{
					return true;
				}
			}
			return false;
		}
	}

	public class ArtistAnalyzer : IDataAnalyzer
	{
		public string Key
		{
			get { return "Artist"; }
		}

		public string Name
		{
			get { return "Artist"; }
		}

		public string FullName
		{
			get { return "Artist"; }
		}

		public string ParentKey
		{
			get { return ""; }
		}

		public string[] GetValues()
		{
			return new string[] { };
		}

		public Type GetValueType()
		{
			return typeof(string);
		}

		public bool MeetsCriteria(Character character, string op, string value)
		{
			string[] artists = (character.Metadata.Artist ?? "").Split(',');
			foreach (string artist in artists)
			{
				if (StringOperations.Matches(artist, op, value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
