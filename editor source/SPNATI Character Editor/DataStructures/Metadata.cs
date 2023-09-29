using Desktop.DataStructures;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Data representation of meta.xml
	/// </summary>
	/// <remarks>
	/// PROPERTY ORDER IS IMPORTANT - Order determines attribute order in generated XML files
	/// </remarks>
	[XmlRoot("opponent")]
	public class Metadata : BindableObject, IHookSerialization
	{
		[XmlElement("lastupdate")]
		public long LastUpdate { get; set; }

		[XmlElement("enabled")]
		public bool Enabled
		{
			get { return Get<bool>(); }
			set { Set(value); }
		}

		[XmlElement("first")]
		public string FirstName
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("last")]
		public string LastName
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("label")]
		public string Label
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("pic")]
		public SelectPortrait Portrait { get; set; }

		[XmlElement("gender")]
		public string Gender
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[DefaultValue("")]
		[XmlElement("height")]
		public string LegacyHeight
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("from")]
		public string Source
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("writer")]
		public string Writer
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("artist")]
		public string Artist
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlElement("description")]
		public string Description
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[DefaultValue(false)]
		[XmlElement("crossGender")]
		public bool CrossGender
		{
			get { return Get<bool>(); }
			set { Set(value); }
		}

		[DefaultValue(100.0f)]
		[XmlElement("scale")]
		public float Scale
		{
			get { return Get<float>(); }
			set { Set(value); }
		}

		[XmlElement("epilogue")]
		public List<EpilogueMeta> Endings { get; set; }

		[XmlElement("layers")]
		public int Layers
		{
			get { return Get<int>(); }
			set { Set(value); }
		}

		[DefaultValue("")]
		[XmlElement("default-costume-name")]
		public string DefaultCostumeName
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

		[XmlArray("tags")]
		[XmlArrayItem("tag")]
		public List<CharacterTag> LegacyTags { get; set; }

		[XmlElement("alternates")]
		public List<AlternateSkin> AlternateSkins { get; set; }

		[DefaultValue(false)]
		[XmlElement("has_collectibles")]
		public bool HasCollectibles
		{
			get { return Get<bool>(); }
			set { Set(value); }
		}

		/// <summary>
		/// Count of unique text across all lines
		/// </summary>
		[XmlElement("lines")]
		public int Lines { get; set; }

		/// <summary>
		/// Count of unique poses used across all lines
		/// </summary>
		[XmlElement("poses")]
		public int Poses { get; set; }

		/// <summary>
		/// Custom z-ordering
		/// </summary>
		[DefaultValue(0)]
		[XmlElement("z-index")]
		public int Z
		{
			get { return Get<int>(); }
			set { Set(value); }
		}

		/// <summary>
		/// Speech bubble position relative to image
		/// </summary>
		[DefaultValue(DialogueLayer.under)]
		[XmlElement("dialogue-layer")]
		public DialogueLayer BubblePosition
		{
			get { return Get<DialogueLayer>(); }
			set { Set(value); }
		}

		/// <summary>
		/// Size of speech bubble text
		/// </summary>
		[DefaultValue(FontSize.normal)]
		[XmlElement("font-size")]
		public FontSize TextSize
		{
			get { return Get<FontSize>(); }
			set { Set(value); }
		}

		[DefaultValue(0)]
		[XmlElement("release")]
		public int LegacyRelease
		{
			get { return Get<int>(); }
			set { Set(0); }
		}

		[XmlAnyElement]
		public List<System.Xml.XmlElement> ExtraXml { get; set; }

		public Metadata()
		{
			Scale = 100.0f;
			AlternateSkins = new List<AlternateSkin>();
			Portrait = new SelectPortrait();
		}

		public Metadata(Character c) : this()
		{
			PopulateFromCharacter(c);
		}

		/// <summary>
		/// Builds the meta data from a character instance
		/// </summary>
		/// <param name="c"></param>
		public void PopulateFromCharacter(Character c)
		{
			FirstName = c.FirstName;
			LastName = c.LastName;
			if (string.IsNullOrEmpty(Label))
			{
				Label = c.Label;
			}
			if (string.IsNullOrEmpty(Gender))
			{
				Gender = c.Gender;
			}
			Layers = c.Layers;
			DefaultCostumeName = c.Metadata.DefaultCostumeName;
			Endings = c.Endings.ConvertAll(e => new EpilogueMeta
			{
				Status = e.Status,
				Title = e.Title,
				Gender = e.Gender,
				GalleryImage = e.GalleryImage ?? (e.Scenes.Count > 0 ? e.Scenes[0].Background : null),
				AlsoPlaying = e.AlsoPlaying,
				PlayerStartingLayers = e.PlayerStartingLayers,
				Hint = e.Hint,
				EpilogueDescription = e.EpilogueDescription,
				HasMarkerConditions = !string.IsNullOrWhiteSpace(e.AllMarkers)
					|| !string.IsNullOrWhiteSpace(e.AnyMarkers)
					|| !string.IsNullOrWhiteSpace(e.NotMarkers)
					|| !string.IsNullOrWhiteSpace(e.AlsoPlayingAllMarkers)
					|| !string.IsNullOrWhiteSpace(e.AlsoPlayingAnyMarkers)
					|| !string.IsNullOrWhiteSpace(e.AlsoPlayingNotMarkers)
			});
			LegacyTags = c.Tags;
			HasCollectibles = c.Collectibles.Count > 0;
			int lines, poses;
			c.GetUniqueLineAndPoseCount(out lines, out poses);
			Lines = lines;
			Poses = poses;
			if (c.Metadata.AlternateSkins != null)
			{
				foreach (AlternateSkin skin in c.Metadata.AlternateSkins)
					foreach (SkinLink link in skin.Skins)
						if (link.LayersNonSkip == c.Layers)
							link.LayersNonSkip = 0;
			}
		}

		public void OnBeforeSerialize()
		{

		}

		public void OnAfterDeserialize(string source)
		{
			//Encoding these doesn't need to be done in OnBeforeSerialize because the serializer does it automatically
			Description = XMLHelper.DecodeEntityReferences(Description);
		}
	}

	public class SelectPortrait
	{
		[DefaultValue(0)]
		[XmlAttribute("x")]
		public int X;

		[DefaultValue(0)]
		[XmlAttribute("y")]
		public int Y;

		[DefaultValue(100.0f)]
		[XmlAttribute("scale")]
		public float Scale;

		[XmlText]
		public string Image;

		public SelectPortrait()
		{
			Scale = 100.0f;
		}
	}

	public class EpilogueMeta
	{
		[DefaultValue("online")]
		[XmlAttribute("status")]
		public string Status;

		[XmlAttribute("gender")]
		public string Gender;

		[XmlAttribute("playerStartingLayers")]
		public string PlayerStartingLayers;

		[DefaultValue(false)]
		[XmlAttribute("markers")]
		public bool HasMarkerConditions;

		[XmlAttribute("img")]
		public string GalleryImage;

		[XmlAttribute("alsoPlaying")]
		public string AlsoPlaying;

		[XmlAttribute("hint")]
		public string Hint;

		[XmlAttribute("description")]
		public string EpilogueDescription;

		[XmlText]
		public string Title;
	}

	public enum DialogueLayer
	{
		under,
		over
	}

	public enum FontSize
	{
		normal,
		small,
		smaller
	}
}
