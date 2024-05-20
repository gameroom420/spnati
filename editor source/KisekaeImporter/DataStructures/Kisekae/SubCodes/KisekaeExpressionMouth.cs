namespace KisekaeImporter.SubCodes
{
	public class KisekaeExpressionMouth : KisekaeSubCode
	{
		public KisekaeExpressionMouth() : base("hd") { }

		public int EmotionMouth
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public bool Reversal
		{
			get { return GetInt(1) == 1; }
			set { Set(1, value ? "1" : "0"); }
		}

		public int MouthWidth
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int MouthHeight
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int MouthSen
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int MouthYMove
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int MouthXMove
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int MouthRotation
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public bool MouthVisible
		{
			get { return GetBool(8); }
			set { Set(8, value); }
		}

		public KisekaeColor MouthSenColor
		{
			get { return new KisekaeColor(GetString(9)); }
			set { Set(9, value.ToString()); }
		}
	}
}
