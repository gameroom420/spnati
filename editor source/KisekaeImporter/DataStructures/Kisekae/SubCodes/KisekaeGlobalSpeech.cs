namespace KisekaeImporter.SubCodes
{
	public class KisekaeGlobalSpeech : KisekaeSubCode, IPoseable, IMoveable
	{
		public void Pose(IPoseable pose)
		{
			KisekaeGlobalSpeech other = pose as KisekaeGlobalSpeech;
			if (other == null)
			{
				return;
			}
			Scale = other.Scale;
			ScaleY = other.ScaleY;
			ExtraArrowScale = other.ExtraArrowScale;
			ExtraArrowScaleY = other.ExtraArrowScaleY;
			Rotation = other.Rotation;
			X = other.X;
			Y = other.Y;
			DepthZ = other.DepthZ;
			ScaleB = other.ScaleB;
			ExtraArrowX = other.ExtraArrowX;
			ExtraArrowY = other.ExtraArrowY;
			ExtraArrowRotation = other.ExtraArrowRotation;
		}

		public int Type
		{
			get { return GetInt(0); }
			set { Set(0, value); }
		}

		public int Reversal
		{
			get { return GetInt(1); }
			set { Set(1, value); }
		}

		public KisekaeColor Color1
		{
			get { return new KisekaeColor(GetString(2)); }
			set { Set(2, value.ToString()); }
		}

		public KisekaeColor Color2
		{
			get { return new KisekaeColor(GetString(3)); }
			set { Set(3, value.ToString()); }
		}

		public KisekaeColor Color3
		{
			get { return new KisekaeColor(GetString(4)); }
			set { Set(4, value.ToString()); }
		}

		public int Depth
		{
			get { return GetInt(5); }
			set { Set(5, value); }
		}

		public int Scale
		{
			get { return GetInt(6); }
			set { Set(6, value); }
		}

		public int ScaleY
		{
			get { return GetInt(7); }
			set { Set(7, value); }
		}

		public int Line
		{
			get { return GetInt(8); }
			set { Set(8, value); }
		}

		public KisekaeColor LineColor
		{
			get { return new KisekaeColor(GetString(9)); }
			set { Set(9, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(10); }
			set { Set(10, value); }
		}

		public int X
		{
			get { return GetInt(11); }
			set { Set(11, value); }
		}

		public int Y
		{
			get { return GetInt(12); }
			set { Set(12, value); }
		}

		public int DepthZ
		{
			get { return GetInt(13); }
			set { Set(13, value); }
		}

		public int Alpha
		{
			get { return GetInt(14); }
			set { Set(14, value); }
		}

		public int Blend
		{
			get { return GetInt(15); }
			set { Set(15, value); }
		}

		public int ScaleB
		{
			get { return GetInt(16); }
			set { Set(16, value); }
		}

		public bool ExtraArrowVisible
		{
			get { return GetBool(17); }
			set { Set(17, value); }
		}

		public int ExtraArrowType
		{
			get { return GetInt(18); }
			set { Set(18, value); }
		}

		public int ExtraArrowSideReversal
		{
			get { return GetInt(19); }
			set { Set(19, value); }
		}

		public int ExtraArrowLine
		{
			get { return GetInt(20); }
			set { Set(20, value); }
		}

		public int ExtraArrowScale
		{
			get { return GetInt(21); }
			set { Set(21, value); }
		}

		public int ExtraArrowScaleY
		{
			get { return GetInt(22); }
			set { Set(22, value); }
		}

		public int ExtraArrowRotation
		{
			get { return GetInt(23); }
			set { Set(23, value); }
		}

		public int ExtraArrowX
		{
			get { return GetInt(24); }
			set { Set(24, value); }
		}

		public int ExtraArrowY
		{
			get { return GetInt(25); }
			set { Set(25, value); }
		}

		public void ShiftX(int offset)
		{
			X += offset;
		}
	}
}
