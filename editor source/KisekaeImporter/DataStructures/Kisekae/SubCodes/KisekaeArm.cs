namespace KisekaeImporter.SubCodes
{
	public class KisekaeArm : KisekaeSubCode
	{
		public KisekaeArm() : base("aa") { }

		public int LeftArm
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftArm2
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftArm2Depth
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftHand
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int LeftHand2
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightArm
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightArm2
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int RightArm2Depth
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int RightHand
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int RightHand2
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public bool LeftArmFreeRotation
		{
			get { return GetBool(10); }
			set { Set(11, value); }
		}

		public bool RightArmFreeRotation
		{
			get { return GetBool(11); }
			set { Set(11, value); }
		}
	}
}
