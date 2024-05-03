namespace KisekaeImporter.SubCodes
{
	public class KisekaeGlobalArm : KisekaeSubCode, IMoveable, IPoseable
	{
		public void Pose(IPoseable pose)
		{
			KisekaeGlobalArm other = pose as KisekaeGlobalArm;
			if (other == null)
			{
				return;
			}
			Scale = other.Scale;
			Rotation = other.Rotation;
			X = other.X;
			Y = other.Y;
			DepthZ = other.DepthZ;
			Wrist = other.Wrist;
			WristRotation = other.WristRotation;
			Width = other.Width;
		}

		public int Type
		{
			get { return GetInt(0); }
			set { Set(0, value); }
		}

		public bool Reversal
		{
			get { return GetBool(1); }
			set { Set(1, value); }
		}

		public KisekaeColor Color
		{
			get { return new KisekaeColor(GetString(2)); }
			set { Set(2, value.ToString()); }
		}

		public int Depth
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int Alpha
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int Scale
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int DepthZ
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int Wrist
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int WristRotation
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public int Width
		{
			get { return GetInt(12); }
			set { Set(12, value); }
		}

		public int FineX
		{
			get { return GetInt(13); }
			set { Set(13, value); }
		}

		public int FineY
		{
			get { return GetInt(14); }
			set { Set(14, value); }
		}

		public void ShiftX(int offset)
		{
			X += offset;
		}
	}
}
