namespace KisekaeImporter.SubCodes
{
	public class KisekaeGlobalBelt : KisekaeSubCode, IPoseable, IMoveable
	{
		public void Pose(IPoseable pose)
		{
			KisekaeGlobalBelt other = pose as KisekaeGlobalBelt;
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
			Extra = other.Extra;
			ScaleB = other.ScaleB;
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
			set { Set(5, value.ToString()); }
		}

		public int Scale
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int ScaleY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int Line
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public KisekaeColor LineColor
		{
			get { return new KisekaeColor(GetString(9)); }
			set { Set(9, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(12); }
			set { Set(12, value.ToString()); }
		}

		public int DepthZ
		{
			get { return GetInt(13); }
			set { Set(13, value.ToString()); }
		}

		public int Extra
		{
			get { return GetInt(14); }
			set { Set(14, value.ToString()); }
		}

		public int ScaleB
		{
			get { return GetInt(15); }
			set { Set(15, value.ToString()); }
		}

		public int Alpha
		{
			get { return GetInt(16); }
			set { Set(16, value.ToString()); }
		}

		public int FineX
		{
			get { return GetInt(17); }
			set { Set(17, value.ToString()); }
		}

		public int FineY
		{
			get { return GetInt(18); }
			set { Set(18, value.ToString()); }
		}

		public void ShiftX(int offset)
		{
			//for some inexplicable reason these operate on a different unit of measurement than everything else, so try to account for that empirically
			offset = (int)(offset * 0.827f);
			X += offset;
		}
	}
}
