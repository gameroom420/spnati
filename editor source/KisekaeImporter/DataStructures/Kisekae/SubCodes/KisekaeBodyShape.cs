namespace KisekaeImporter.SubCodes
{
	public class KisekaeBodyShape : KisekaeSubCode
	{
		public KisekaeBodyShape() : base("ca") { }

		public int BodyHeight
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int Waist
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int DouHeight
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int AshiHeight
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int BodyWidth
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int HipWidth
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int HandWidth
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int AshiWidth
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int ShoulderWidth
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int Heso
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int WaistAlpha
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int BodySize
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public bool UpperBodyVisible
		{
			get { return GetBool(12); }
			set { Set(12, value); }
		}

		public bool LowerBodyVisible
		{
			get { return GetBool(13); }
			set { Set(13, value); }
		}
	}
}
