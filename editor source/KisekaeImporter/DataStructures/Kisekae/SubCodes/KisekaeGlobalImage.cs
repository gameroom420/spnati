namespace KisekaeImporter.SubCodes
{
	public class KisekaeGlobalImage : KisekaeSubCode, IPoseable, IMoveable
	{
		public void Pose(IPoseable pose)
		{
			KisekaeGlobalImage other = pose as KisekaeGlobalImage;
			if (other == null)
			{
				return;
			}
			Scale = other.Scale;
			Depth = other.Depth;
			X = other.X;
			Y = other.Y;
			Rotation = other.Rotation;
			ScaleY = other.ScaleY;
			ScaleB = other.ScaleB;
		}

		public int Scale
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int Depth
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public bool Size
		{
			get { return GetBool(5); }
			set { Set(5, value); }
		}

		public int ScaleY
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		/*public KisekaeColor Color1
		{
			get { return new KisekaeColor(GetString(7)); }
			set { Set(7, value.ToString()); }
		}

		public KisekaeColor Color2
		{
			get { return new KisekaeColor(GetString(8)); }
			set { Set(8, value.ToString()); }
		}

		public KisekaeColor Color3
		{
			get { return new KisekaeColor(GetString(9)); }
			set { Set(9, value.ToString()); }
		}*/

		public int ScaleB
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int Alpha
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public int AttachPoint
		{
			get { return GetInt(12); }
			set { Set(12, value.ToString()); }
		}

		public int Reversal
		{
			get { return GetInt(12); }
			set	{ Set(12, value); }
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
