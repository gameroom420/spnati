namespace KisekaeImporter.SubCodes
{
	public class KisekaeBackHair : KisekaeSubCode, IColorable, IPoseable
	{
		public KisekaeBackHair() : base("ec") { }

		public void Pose(IPoseable pose)
		{
			KisekaeBackHair other = pose as KisekaeBackHair;
			if (other == null)
			{
				return;
			}
			Height = other.Height;
			Width = other.Width;
			Y = other.Y;
		}

		public int Shape
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public void SetColors(KisekaeColor color1, KisekaeColor color2, KisekaeColor color3)
		{
			Color1 = color1;
			Color2 = color2;
			Color3 = color3;
		}

		public int Height
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public KisekaeColor Color1
		{
			get { return new KisekaeColor(GetString(2)); }
			set { Set(2, value.ToString()); }
		}

		public KisekaeColor Color2
		{
			get { return new KisekaeColor(GetString(3)); }
			set { Set(3, value.ToString()); }
		}

		public KisekaeColor Color3
		{
			get { return new KisekaeColor(GetString(4)); }
			set { Set(4, value.ToString()); }
		}

		public int Width
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int Gravity
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}
	}
}
