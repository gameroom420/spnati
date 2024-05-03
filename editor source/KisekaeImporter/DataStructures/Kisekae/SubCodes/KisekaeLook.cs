namespace KisekaeImporter.SubCodes
{
	public class KisekaeLook : KisekaeSubCode
	{
		public KisekaeLook() : base("hb") { }

		public int EyeballXMove
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public bool Reversal
		{
			get { return GetInt(1) == 1; }
			set { Set(1, value ? "1" : "0"); }
		}

		public int EyeballYMove
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int EyeballWidth
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int EyeballWidthRight
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int EyeballXMoveRight
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int EyeballYMoveRight
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}
	}
}
