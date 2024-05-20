namespace KisekaeImporter.SubCodes
{
	public class KisekaeHead : KisekaeSubCode
	{
		public KisekaeHead() : base("dd") { }

		public int Hoho
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int Contour
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int ContourWidth
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int ContourHeight
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int HeadScale
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int NeckHeight
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public bool NeckVisible
		{
			get { return GetBool(6); }
			set { Set(6, value); }
		}

		public bool HeadVisible
		{
			get { return GetBool(7); }
			set { Set(7, value); }
		}
	}
}
