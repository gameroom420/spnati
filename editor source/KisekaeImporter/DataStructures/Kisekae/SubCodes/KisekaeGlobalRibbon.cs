namespace KisekaeImporter.SubCodes
{
	public class KisekaeGlobalRibbon : KisekaeSubCode, IPoseable, IMoveable
	{
		public void Pose(IPoseable pose)
		{
			if (pose is KisekaeGlobalRibbon other)
			{
				Scale = other.Scale;
				Rotation = other.Rotation;
				X = other.X;
				Y = other.Y;
				DepthZ = other.DepthZ;
				ScaleB = other.ScaleB;
				FineX = other.FineX;
				FineY = other.FineY;
			}
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

		public int Line
		{
			get { return GetInt(7); }
			set { Set(7, value); }
		}

		public KisekaeColor LineColor
		{
			get { return new KisekaeColor(GetString(8)); }
			set { Set(8, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(9); }
			set { Set(9, value); }
		}

		public int X
		{
			get { return GetInt(10); }
			set { Set(10, value); }
		}

		public int Y
		{
			get { return GetInt(11); }
			set { Set(11, value); }
		}

		public int DepthZ
		{
			get { return GetInt(12); }
			set { Set(12, value); }
		}

		public int ScaleY
		{
			get { return GetInt(13); }
			set { Set(13, value); }
		}

		public int ScaleB
		{
			get { return GetInt(14); }
			set { Set(14, value); }
		}

		public bool Shadow
		{
			get { return GetBool(15); }
			set { Set(15, value); }
		}

		public int Alpha
		{
			get { return GetInt(16); }
			set { Set(16, value); }
		}

		public int FineX
		{
			get { return GetInt(17); }
			set { Set(17, value); }
		}

		public int FineY
		{
			get { return GetInt(17); }
			set { Set(17, value); }
		}

		public void ShiftX(int offset)
		{
			//for some inexplicable reason these operate on a different unit of measurement than everything else, so try to account for that empirically
			offset = (int)(offset * 0.907f);

			X += offset;
		}
	}
}
