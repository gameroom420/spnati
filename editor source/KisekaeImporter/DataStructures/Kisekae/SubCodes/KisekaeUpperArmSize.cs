namespace KisekaeImporter.SubCodes
{
	public class KisekaeUpperArmSize : KisekaeSubCode
	{
		public KisekaeUpperArmSize() : base("ce") { }

		public int LeftUpperArmScaleX
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftUpperArmScaleY
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftUpperArmOffsetX
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftUpperArmOffsetY
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int RightUpperArmScaleX
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightUpperArmScaleY
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightUpperArmOffsetX
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int RightUpperArmOffsetY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public bool LeftShoulderVisible
		{
			get { return GetBool(8); }
			set { Set(8, value); }
		}

		public bool RightShoulderVisible
		{
			get { return GetBool(9); }
			set { Set(9, value); }
		}

		public bool LeftUpperArmVisible
		{
			get { return GetBool(10); }
			set { Set(10, value); }
		}

		public bool RightUpperArmVisible
		{
			get { return GetBool(11); }
			set { Set(11, value); }
		}
	}
}
