namespace KisekaeImporter.SubCodes
{
	//This isn't really supported yet since the array is in the format u# instead of u## but everything assumes the latter
	public class KisekaeTextBox : KisekaeSubCode
	{
		public KisekaeColor Color
		{
			get { return new KisekaeColor(GetString(0)); }
			set { Set(0, value.ToString()); }
		}

		public int Scale
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int Leading
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int Letter
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int TateYoko
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int FormatAlign
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int Alpha
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int BoxScaleX
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int BoxScaleY
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int Width
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public int ScaleB
		{
			get { return GetInt(12); }
			set { Set(12, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(13); }
			set { Set(13, value.ToString()); }
		}

		public int AttachPoint
		{
			get { return GetInt(14); }
			set { Set(14, value.ToString()); }
		}

		public int Depth
		{
			get { return GetInt(15); }
			set { Set(15, value.ToString()); }
		}
	}
}
