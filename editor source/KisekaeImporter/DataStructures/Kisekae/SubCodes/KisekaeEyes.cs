namespace KisekaeImporter.SubCodes
{
	public class KisekaeEyes : KisekaeSubCode
	{
		public KisekaeEyes() : base("fa") { }

		public int Eye
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int EyeX
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int EyeY
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int EyeScaleX
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int EyeScaleY
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int EyeRotation
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public KisekaeColor EyeColor
		{
			get { return new KisekaeColor(GetString(6)); }
			set { Set(6, value.ToString()); }
		}

		public int EyeDepth
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

	}
}
