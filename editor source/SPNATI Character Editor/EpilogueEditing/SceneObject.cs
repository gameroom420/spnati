﻿using SPNATI_Character_Editor.Controls;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace SPNATI_Character_Editor.EpilogueEditing
{
	public class SceneObject : IDisposable
	{
		private Character _character;
		public float X;
		public float Y;
		public float WidthPct;
		public float HeightPct;
		public float Width;
		public float Height;
		public float Scale = 1;
		public float Rotation = 0;
		public float Alpha = 100;
		public SolidBrush Color = new SolidBrush(System.Drawing.Color.Black);
		public string Id;
		public string Text;
		public string Arrow;
		public float Time;
		public string Tween;
		public string Ease;
		public float Start;
		public float End;
		public int Index;

		/// <summary>
		/// Previous directive or keyframe in the scene that affects this object
		/// </summary>
		public Keyframe LastFrame;
		/// <summary>
		/// Directive or keyframe that this is linked to (i.e. updates to one should reflect on the other)
		/// </summary>
		public Keyframe LinkedFrame;
		/// <summary>
		/// Animation that this belongs to
		/// </summary>
		public SceneAnimation LinkedAnimation;

		public SceneObjectType ObjectType = SceneObjectType.Other;

		public Image Image;

		public object Max { get; private set; }

		public SceneObject() { }

		public SceneObject(SceneObject source)
		{
			CopyValuesFrom(source);
		}

		public SceneObject(ScenePreview scene, Character character, Directive directive) : this(scene, character, directive.Id, directive.Src, directive.Color)
		{
			LinkedFrame = directive;
			if (directive.DirectiveType == "sprite")
			{
				ObjectType = SceneObjectType.Sprite;
			}
			else if (directive.DirectiveType == "text")
			{
				ObjectType = SceneObjectType.Text;
			}

			string width = directive.Width;
			int widthValue = 0;
			var regex = new Regex(@"^(-?\d+)(px|%)?$");

			string height = directive.Height;
			int heightValue = 0;

			int imgWidth = (Image == null ? 1 : Image.Width);
			int imgHeight = (Image == null ? 1 : Image.Height);

			if (!string.IsNullOrEmpty(width))
			{
				Match match = regex.Match(width);
				if (match.Success)
				{
					int.TryParse(match.Groups[1].Value, out widthValue);
				}
				if (width.EndsWith("%"))
				{
					WidthPct = widthValue / 100.0f;
				}
				else
				{
					WidthPct = widthValue / (float)scene.Width;
				}
				if (string.IsNullOrEmpty(height))
				{
					HeightPct = imgHeight / imgWidth * WidthPct * scene.AspectRatio;
				}
			}
			else
			{
				WidthPct = imgWidth / (float)scene.Width;
				if (ObjectType == SceneObjectType.Text)
				{
					WidthPct = 0.2f;
				}
			}
			if (!string.IsNullOrEmpty(height))
			{
				Match match = regex.Match(height);
				if (match.Success)
				{
					int.TryParse(match.Groups[1].Value, out heightValue);
				}

				if (height.EndsWith("%"))
				{
					HeightPct = heightValue / 100.0f;
				}
				else
				{
					HeightPct = heightValue / (float)scene.Height;
				}
				if (string.IsNullOrEmpty(width))
				{
					WidthPct = imgWidth / imgHeight * HeightPct / scene.AspectRatio;
				}
			}
			else
			{
				HeightPct = imgHeight / (float)scene.Height;
			}

			Width = WidthPct * scene.Width;
			Height = HeightPct * scene.Height;

			Update(directive, scene);

			Arrow = directive.Arrow;
			Text = directive.Text;
		}

		public void ResyncAnimation()
		{
			LinkedAnimation?.Rebuild();
		}

		private void CopyValuesFrom(SceneObject source)
		{
			_character = source._character;
			X = source.X;
			Y = source.Y;
			Width = source.Width;
			Height = source.Height;
			WidthPct = source.WidthPct;
			HeightPct = source.HeightPct;
			Scale = source.Scale;
			Rotation = source.Rotation;
			Alpha = source.Alpha;
			Color.Color = source.Color.Color;
			Id = source.Id;
			Text = source.Text;
			Arrow = source.Arrow;
			Tween = source.Tween;
			Ease = source.Ease;
			Image = source.Image;
		}

		public SceneObject(ScenePreview scene, Character character, string id, string imagePath, string color)
		{
			_character = character;
			Id = id;
			if (!string.IsNullOrEmpty(color))
			{
				try
				{
					Color.Color = ColorTranslator.FromHtml(color);
				}
				catch { }
			}
			if (!string.IsNullOrEmpty(imagePath))
			{
				string path = GetImagePath(imagePath);
				try
				{
					Image = new Bitmap(path);
				}
				catch { }
			}

			if (Image == null)
			{
				WidthPct = scene.Width;
				HeightPct = scene.Height;
			}
			else
			{
				WidthPct = Image.Width / (float)scene.Width;
				HeightPct = Image.Height / (float)scene.Height;
			}

			Width = WidthPct * scene.Width;
			Height = HeightPct * scene.Height;
		}

		public static float Parse(string str, float sceneSize)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			var regex = new Regex(@"^(-?\d+)(px|%)?$");
			float value = 0;
			Match match = regex.Match(str);
			if (match.Success)
			{
				float.TryParse(match.Groups[1].Value, out value);
			}
			if (str.EndsWith("%"))
			{
				value = value / 100.0f * sceneSize;
			}
			return value;
		}

		/// <summary>
		/// Sets current values to those of a keyframe
		/// </summary>
		/// <param name="frame"></param>
		public void Update(Keyframe frame, ScenePreview scene)
		{
			if (!string.IsNullOrEmpty(frame.Time))
			{
				float.TryParse(frame.Time, out Time);
			}
			if (!string.IsNullOrEmpty(frame.Scale))
			{
				float.TryParse(frame.Scale, out Scale);
			}
			if (!string.IsNullOrEmpty(frame.Rotation))
			{
				float.TryParse(frame.Rotation, out Rotation);
			}
			if (!string.IsNullOrEmpty(frame.Opacity))
			{
				float.TryParse(frame.Opacity, out Alpha);
			}
			if (!string.IsNullOrEmpty(frame.Zoom))
			{
				float.TryParse(frame.Zoom, out Scale);
			}
			SetColor(frame);

			Directive directive = frame as Directive;
			if (directive == null || directive.Keyframes.Count == 0)
			{
				//only update X and Y is this is either a keyframe or the directive has no keyframes
				if (!string.IsNullOrEmpty(frame.X) || X == 0)
				{
					X = Parse(frame.X, scene.Width);
				}
				if (!string.IsNullOrEmpty(frame.Y) || Y == 0)
				{
					Y = Parse(frame.Y, scene.Height);
				}
				if (frame.X == "centered")
				{
					X = scene.Width / 2 - Width / 2;
				}
			}
			if (directive != null)
			{
				Tween = directive.InterpolationMethod;
				Ease = directive.EasingMethod;
			}

			SetColor(frame);
		}

		public override string ToString()
		{
			return Id;
		}

		private string GetImagePath(string path)
		{
			if (!path.StartsWith("/"))
			{
				return Path.Combine(Config.GetRootDirectory(_character), path);
			}
			return Path.Combine(Config.SpnatiDirectory, path.Substring(1));
		}

		public void SetColor(Keyframe frame)
		{
			if (!string.IsNullOrEmpty(frame.Opacity))
			{
				float.TryParse(frame.Opacity, out Alpha);
			}
			if (!string.IsNullOrEmpty(frame.Color))
			{
				try
				{
					Color.Color = ColorTranslator.FromHtml(frame.Color);
				}
				catch { }
			}

			Color.Color = System.Drawing.Color.FromArgb((int)(Alpha / 100 * 255), Color.Color);
		}

		public virtual void Dispose()
		{
			Image?.Dispose();
			Color?.Dispose();
		}

		/// <summary>
		/// Updates the X and Y values of the object and applies those to the provided directive
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="directive"></param>
		/// <returns>Whether the values were actually changed</returns>
		public virtual bool AdjustPosition(int x, int y, ScenePreview scene)
		{
			if (X == x && Y == y)
			{
				return false;
			}
			X = x;
			Y = y;

			LinkedFrame.X = ApplyPosition(x, LinkedFrame.X, (int)scene.Width);
			LinkedFrame.Y = ApplyPosition(y, LinkedFrame.Y, (int)scene.Height);
			ResyncAnimation();
			return true;
		}

		/// <summary>
		/// Gets an updated measurement, preserving whether it was in px or %
		/// </summary>
		/// <param name="value"></param>
		/// <param name="sourceValue"></param>
		/// <param name="sceneValue"></param>
		protected string ApplyPosition(int value, string sourceValue, int sceneValue)
		{
			if ((sourceValue != null && sourceValue.EndsWith("%")) || sourceValue == "centered" || ObjectType == SceneObjectType.Text)
			{
				float pct = value / (float)sceneValue;
				int percent = (int)(pct * 100);
				return percent + "%";
			}
			else
			{
				//pixels can just go straight in
				return value.ToString();
			}
		}

		public virtual bool AdjustSize(Point pt, HoverContext context, ScenePreview scene)
		{
			Directive directive = LinkedFrame as Directive;
			if (directive == null) { return false; }

			switch (context)
			{
				case HoverContext.SizeRight:
					int rt = Math.Max((int)X + 10, pt.X);

					int width = (int)((-2 * rt + 2 * X) / (-Scale - 1));
					if (Width == width)
					{
						return false;
					}
					Width = width;
					directive.Width = ApplyPosition((int)Width, directive.Width, (int)scene.Width);
					ResyncAnimation();
					return true;
				case HoverContext.SizeLeft:
					//tricker than adjusting the right side since it involves translating X too

					//first get the target width, which is the same as moving the right side by the amount the left side moved
					int lt = Math.Min((int)(X + Width - 10), pt.X); //new left position
					int dx = (int)(X - lt);
					rt = (int)(X + Width + dx);

					//use formula for SizeRight to get the width now
					width = (int)((-2 * rt + 2 * X) / (-Scale - 1));
					if (Width == width)
					{
						return false;
					}

					//now solve for X given the width, scale, and new left where l = X - (w * s) / 2
					int x = (int)((width * Scale - width) / 2 + lt);
					Width = width;
					X = x;
					directive.X = ApplyPosition((int)X, directive.X, (int)scene.Width);
					directive.Width = ApplyPosition((int)Width, directive.Width, (int)scene.Width);
					ResyncAnimation();
					return true;
				case HoverContext.SizeBottom:
					int b = Math.Max((int)Y + 10, pt.Y);

					int height = (int)((-2 * b + 2 * Y) / (-Scale - 1));
					if (Height == height)
					{
						return false;
					}
					Height = height;
					directive.Height = ApplyPosition((int)Height, directive.Height, (int)scene.Height);
					ResyncAnimation();
					return true;
				case HoverContext.SizeTop:
					//works the same as left

					int t = Math.Min((int)(Y + Height - 10), pt.Y); //new left position
					int dy = (int)(Y - t);
					b = (int)(Y + Height + dy);

					height = (int)((-2 * b + 2 * Y) / (-Scale - 1));
					if (Height == height)
					{
						return false;
					}

					int y = (int)((height * Scale - height) / 2 + t);
					Height = height;
					Y = y;
					directive.Y = ApplyPosition((int)Y, directive.Y, (int)scene.Height);
					directive.Height = ApplyPosition((int)Height, directive.Height, (int)scene.Height);
					ResyncAnimation();
					return true;
			}
			return false;
		}

		public virtual bool AdjustScale(Point point, ScenePreview scene)
		{
			//get the object's center
			int cx = (int)(X + Width / 2);
			int cy = (int)(Y + Height / 2);

			int tx = point.X;
			int ty = point.Y;

			//work with whichever of X/Y is furthest from the center
			int dx = Math.Abs(tx - cx);
			int dy = Math.Abs(ty - cy);

			int offset = Math.Max(dx, dy);

			//transform this offset to left side of the scaled box
			int t = cx - offset;

			//now we have the top-left scaled world-space point. Use the formula to convert from object space to world space and solve for scale (worldX = objX - (objWidth * Scale - objWidth) / 2)
			float scale = ((t - X) / -0.5f + Width) / Width;

			if (Scale == scale)
			{
				return false;
			}
			Scale = scale;
			LinkedFrame.Scale = scale.ToString();
			ResyncAnimation();

			return true;
		}

		public bool AdjustRotation(Point point, ScenePreview scene)
		{
			//quick and dirty - just use the angle to look from the point to the center

			float cx = X + Width / 2;
			float cy = Y + Height / 2;

			double angle = Math.Atan2(cy - point.Y, cx - point.X);
			angle = angle * (180 / Math.PI);

			if (Rotation == angle)
			{
				return false;
			}

			Rotation = (float)angle;
			LinkedFrame.Rotation = angle.ToString();
			ResyncAnimation();

			return true;
		}
	}

	public class ScenePreview : SceneObject
	{
		public float AspectRatio { get { return Width / (float)Height; } }
		public SolidBrush BackgroundColor;
		public Color OverlayColor = System.Drawing.Color.Black;
		public Scene LinkedScene;

		public ScenePreview(Scene scene)
		{
			LinkedScene = scene;
			try
			{
				BackgroundColor = new SolidBrush(ColorTranslator.FromHtml(scene.BackgroundColor));
			}
			catch { }

			string w = scene.Width.Split(new string[] { "px" }, StringSplitOptions.None)[0];
			string h = scene.Height.Split(new string[] { "px" }, StringSplitOptions.None)[0];
			float.TryParse(w, out Width);
			float.TryParse(h, out Height);


			X = (int)Parse(scene.X, Width);
			Y = (int)Parse(scene.Y, Height);

			if (!string.IsNullOrEmpty(scene.Zoom))
			{
				float.TryParse(scene.Zoom, out Scale);
			}
			if (!string.IsNullOrEmpty(scene.FadeColor))
			{
				try
				{
					OverlayColor = ColorTranslator.FromHtml(scene.FadeColor);
				}
				catch { }
			}
			if (!string.IsNullOrEmpty(scene.FadeOpacity))
			{
				float alpha;
				float.TryParse(scene.FadeOpacity, out alpha);
				int a = (int)(alpha / 100 * 255);
				OverlayColor = System.Drawing.Color.FromArgb(a, OverlayColor);
			}
			else
			{
				OverlayColor = System.Drawing.Color.FromArgb(0, OverlayColor);
			}
		}

		public override void Dispose()
		{
			BackgroundColor?.Dispose();
			base.Dispose();
		}

		public override bool AdjustPosition(int x, int y, ScenePreview scene)
		{
			if (X == x && Y == y)
			{
				return false;
			}
			X = x;
			Y = y;

			if (LinkedFrame != null)
			{
				LinkedFrame.X = ApplyPosition(x, LinkedFrame.X, (int)scene.Width);
				LinkedFrame.Y = ApplyPosition(y, LinkedFrame.Y, (int)scene.Height);
				ResyncAnimation();
			}
			else
			{
				LinkedScene.X = ApplyPosition(x, LinkedScene.X, (int)scene.Width);
				LinkedScene.Y = ApplyPosition(y, LinkedScene.Y, (int)scene.Height);
			}
			return true;
		}

		public override bool AdjustSize(Point pt, HoverContext context, ScenePreview scene)
		{
			float zoom = 1 / Scale;
			switch (context)
			{
				case HoverContext.CameraSizeRight:
					int rt = Math.Max((int)X + 10, pt.X);

					int width = (int)((-2 * rt + 2 * X) / (-zoom - 1));
					if (Width == width)
					{
						return false;
					}
					Width = width;
					LinkedScene.Width = ApplyPosition((int)Width, LinkedScene.Width, (int)scene.Width);
					return true;
				case HoverContext.CameraSizeLeft:
					//tricker than adjusting the right side since it involves translating X too

					//first get the target width, which is the same as moving the right side by the amount the left side moved
					int lt = Math.Min((int)(X + Width - 10), pt.X); //new left position
					int dx = (int)(X - lt);
					rt = (int)(X + Width + dx);

					//use formula for SizeRight to get the width now
					width = (int)((-2 * rt + 2 * X) / (-zoom - 1));
					if (Width == width)
					{
						return false;
					}

					//now solve for X given the width, scale, and new left where l = X - (w * s) / 2
					int x = (int)((width * zoom - width) / 2 + lt);
					Width = width;
					X = x;
					LinkedScene.X = ApplyPosition((int)X, LinkedScene.X, (int)scene.Width);
					LinkedScene.Width = ApplyPosition((int)Width, LinkedScene.Width, (int)scene.Width);
					return true;
				case HoverContext.CameraSizeBottom:
					int b = Math.Max((int)Y + 10, pt.Y);

					int height = (int)((-2 * b + 2 * Y) / (-zoom - 1));
					if (Height == height)
					{
						return false;
					}
					Height = height;
					LinkedScene.Height = ApplyPosition((int)Height, LinkedScene.Height, (int)scene.Height);
					return true;
				case HoverContext.CameraSizeTop:
					//works the same as left

					int t = Math.Min((int)(Y + Height - 10), pt.Y); //new left position
					int dy = (int)(Y - t);
					b = (int)(Y + Height + dy);

					height = (int)((-2 * b + 2 * Y) / (-zoom - 1));
					if (Height == height)
					{
						return false;
					}

					int y = (int)((height * zoom - height) / 2 + t);
					Height = height;
					Y = y;
					LinkedScene.Y = ApplyPosition((int)Y, LinkedScene.Y, (int)scene.Height);
					LinkedScene.Height = ApplyPosition((int)Height, LinkedScene.Height, (int)scene.Height);
					return true;
			}
			return false;
		}

		public override bool AdjustScale(Point point, ScenePreview scene)
		{
			//get the object's center
			int cx = (int)(X + Width / 2);
			int cy = (int)(Y + Height / 2);

			int tx = point.X;
			int ty = point.Y;

			//work with whichever of X/Y is furthest from the center
			int dx = Math.Abs(tx - cx);
			int dy = Math.Abs(ty - cy);

			int offset = Math.Max(dx, dy);

			//transform this offset to left side of the scaled box
			int t = cx - offset;

			//now we have the top-left scaled world-space point. Use the formula to convert from object space to world space and solve for scale (worldX = objX - (objWidth * Scale - objWidth) / 2)
			float scale = ((t - X) / -0.5f + Width) / Width;

			if (Scale == scale)
			{
				return false;
			}
			scale = 1 / scale;
			Scale = scale;
			if (LinkedFrame != null)
			{
				LinkedFrame.Zoom = scale.ToString();
				ResyncAnimation();
			}
			else
			{
				LinkedScene.Zoom = scale.ToString();
			}

			return true;
		}
	}

	public enum SceneObjectType
	{
		Other,
		Sprite,
		Text,
		Keyframe,
		Camera,
	}
}
