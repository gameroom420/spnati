namespace KisekaeImporter.SubCodes
{
	public class KisekaePenis : KisekaeSubCode, IPoseable
	{
		public KisekaePenis() : base("qa") { }

		public void Pose(IPoseable pose)
		{
			KisekaePenis other = pose as KisekaePenis;
			if (other == null)
			{
				return;
			}
			Size = other.Size;
			Ex = other.Ex;
			Depth = other.Depth;
			Bokki = other.Bokki;
			Swing = other.Swing;
		}

		public int Type
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public KisekaeColor ShaftColor
		{
			get { return new KisekaeColor(GetString(1)); }
			set { Set(1, value.ToString()); }
		}

		public KisekaeColor HeadColor
		{
			get { return new KisekaeColor(GetString(2)); }
			set { Set(2, value.ToString()); }
		}

		public int Size
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int Ex
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public KisekaeColor TipColor
		{
			get { return new KisekaeColor(GetString(5)); }
			set { Set(5, value.ToString()); }
		}

		public int Depth
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public bool ManualAuto
		{
			get { return GetBool(7); }
			set { Set(7, value); }
		}

		public bool Bokki
		{
			get { return GetBool(8); }
			set { Set(8, value); }
		}

		public int Swing
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int Kawa
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int TClick
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public bool SizeAuto
		{
			get { return GetBool(12); }
			set { Set(12, value); }
		}

		public int ScaleX
		{
			get { return GetInt(13); }
			set { Set(13, value.ToString()); }
		}

		public int ScaleY
		{
			get { return GetInt(14); }
			set { Set(14, value.ToString()); }
		}

		public int OffsetX
		{
			get { return GetInt(15); }
			set { Set(15, value.ToString()); }
		}

		public int OffsetY
		{
			get { return GetInt(16); }
			set { Set(16, value.ToString()); }
		}
	}
}
