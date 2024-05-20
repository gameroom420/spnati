namespace KisekaeImporter.SubCodes
{
	public class KisekaeExpressionBrow : KisekaeSubCode
	{
		public KisekaeExpressionBrow() : base("hc") { }

		public int EmotionEyebrowLeft
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int EyebrowRotationLeft
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int EyebrowMoveLeft
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int EmotionEyebrowRight
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int EyebrowRotationRight
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int EyebrowMoveRight
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int EyebrowMoveHorizontalLeft
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int EyebrowMoveHorizontalRight
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}
	}
}
