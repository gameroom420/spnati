using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Helper functions for setting and reading from input controls
	/// </summary>
	public static class GUIHelper
	{
		public static string ListToString(List<int> list)
		{
			string result;
			if (list.Count == 0)
			{
				result = "";
			}
			else
			{
				list.Sort();
				StringBuilder sb = new StringBuilder();
				int last = list[0];
				int startRange = last;
				for (int i = 1; i < list.Count; i++)
				{
					int stage = list[i];
					if (stage - 1 > last)
					{
						if (startRange == last)
						{
							sb.Append(startRange.ToString() + " ");
						}
						else
						{
							sb.Append(string.Format("{0}-{1} ", startRange, last));
						}
						startRange = stage;
					}
					last = stage;
				}
				if (startRange == last)
				{
					sb.Append(startRange.ToString());
				}
				else
				{
					sb.Append(string.Format("{0}-{1}", startRange, last));
				}
				result = sb.ToString();
			}
			return result;
		}

		public static List<int> StringToList(string input)
		{
			List<int> list = new List<int>();
			string[] ranges = input.Split(' ');
			foreach (string range in ranges)
			{
				string[] bounds = range.Split('-');
				int min;
				if (int.TryParse(bounds[0], out min))
				{
					int max;
					if (bounds.Length < 2 || !int.TryParse(bounds[1], out max))
					{
						max = min;
					}
					for (int i = min; i <= max; i++)
					{
						list.Add(i);
					}
				}
			}
			list.Sort();
			return list;
		}

		public static List<Word> ParseWords(string sentence)
		{
			Tuple<string, FormatMarker>[] markers = new Tuple<string, FormatMarker>[] {
				new Tuple<string, FormatMarker>("<i>", FormatMarker.ItalicOn),
				new Tuple<string, FormatMarker>("</i>", FormatMarker.ItalicOff),
				new Tuple<string, FormatMarker>("<br>", FormatMarker.LineBreak),
				new Tuple<string, FormatMarker>("<br/>", FormatMarker.LineBreak),
			};
			List<Word> words = new List<Word>();
			string[] rawWords = sentence.Split(' ');
			for (int i = 0; i < rawWords.Length; i++)
			{
				string word = rawWords[i];
				ParseForMarker(word, 0, markers, words);
			}
			return words;
		}

		private static void ParseForMarker(string text, int markerIndex, Tuple<string, FormatMarker>[] markers, List<Word> words)
		{
			string tag = markers[markerIndex].Item1;
			FormatMarker marker = markers[markerIndex].Item2;
			string[] segments = text.Split(new string[] { tag }, StringSplitOptions.None);
			Regex regex = new Regex(@"^[!?,\.]+$");
			for (int i = 0; i < segments.Length; i++)
			{
				string segment = segments[i];
				if (i > 0)
				{
					words.Add(new Word(marker));
				}
				if (markerIndex < markers.Length - 1)
				{
					ParseForMarker(segment, markerIndex + 1, markers, words);
				}
				else if(!string.IsNullOrEmpty(segment))
				{
					//if punctuation only, add it to the previous word
					if (words.Count > 0 && regex.IsMatch(segment))
					{
						bool found = false;
						for (int j = words.Count - 1; j >= 0; j--)
						{
							if (words[j].Formatter == FormatMarker.None)
							{
								words[j].Text += segment;
								found = true;
								break;
							}
						}
						if (found)
						{
							continue;
						}
					}
					
					words.Add(new Word(segment));
				}
			}
		}
	}

	public class Word
	{
		public string Text;
		public float Width;
		public FormatMarker Formatter;

		public Word(string text)
		{
			Text = text;
		}

		public Word(FormatMarker formatter)
		{
			Formatter = formatter;
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(Text))
			{
				return Text;
			}
			return Formatter.ToString();
		}
	}

	public enum FormatMarker
	{
		None = 0,
		ItalicOn = 1,
		ItalicOff = 2,
		LineBreak = 3,
	}
}
