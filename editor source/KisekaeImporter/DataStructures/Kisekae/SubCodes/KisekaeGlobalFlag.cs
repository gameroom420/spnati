namespace KisekaeImporter.SubCodes
{
	public class KisekaeGlobalFlag : KisekaeSubCode, IPoseable, IMoveable
	{
		public void Pose(IPoseable pose)
		{
			KisekaeGlobalFlag other = pose as KisekaeGlobalFlag;
			if (other == null)
			{
				return;
			}
			Scale = other.Scale;
			ScaleY = other.ScaleY;
			Rotation = other.Rotation;
			X = other.X;
			Y = other.Y;
			DepthZ = other.DepthZ;
			ScaleB = other.ScaleB;
		}

		public int Type
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int Depth
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int Scale
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int ScaleY
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int DepthZ
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int ExtraShape
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int ScaleB
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public void ShiftX(int offset)
		{
			X += offset;
		}
	}
}
