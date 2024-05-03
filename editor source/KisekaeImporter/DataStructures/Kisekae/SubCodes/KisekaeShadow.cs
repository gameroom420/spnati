namespace KisekaeImporter.SubCodes
{
	public class KisekaeShadow : KisekaeSubCode
	{
		public KisekaeShadow() : base("bf") { }

		public int Blur
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public KisekaeColor Color
		{
			get { return new KisekaeColor(GetString(1)); }
			set { Set(1, value.ToString()); }
		}

		public bool Inset
		{
			get { return GetBool(2); }
			set { Set(2, value); }
		}

		public bool Cutout
		{
			get { return GetBool(3); }
			set { Set(3, value); }
		}

		public bool Hide
		{
			get { return GetBool(4); }
			set { Set(4, value); }
		}

		public int Alpha
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int Str
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int Distance
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}
	}
}
