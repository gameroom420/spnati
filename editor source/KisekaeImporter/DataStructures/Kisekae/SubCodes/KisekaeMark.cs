namespace KisekaeImporter.SubCodes
{
	public class KisekaeMark : KisekaeSubCode, IPoseable
	{
		public KisekaeMark() : base("t") { }

		public void CopyPositionFrom(KisekaeMark other)
		{
			Side = other.Side;
			ScaleX = other.ScaleX;
			ScaleY = other.ScaleY;
			Rotation = other.Rotation;
			OffsetX = other.OffsetX;
			OffsetY = other.OffsetY;
			Alpha = other.Alpha;
			AttachPoint = other.AttachPoint;
			FineX = other.FineX;
			FineY = other.FineY;
		}

		public void Pose(IPoseable pose)
		{
			if (pose is KisekaeMark other)
			{
				CopyPositionFrom(other);
			}
		}

		public int Shape
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public KisekaeColor Color1
		{
			get { return new KisekaeColor(GetString(1)); }
			set { Set(1, value.ToString()); }
		}

		public KisekaeColor Color2
		{
			get { return new KisekaeColor(GetString(2)); }
			set { Set(2, value.ToString()); }
		}

		public KisekaeColor Color3
		{
			get { return new KisekaeColor(GetString(3)); }
			set { Set(3, value.ToString()); }
		}

		public int ScaleX
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int ScaleY
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int OffsetX
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int OffsetY
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int Side
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int RotationZ
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int Layer
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public int Alpha
		{
			get { return GetInt(12); }
			set { Set(12, value.ToString()); }
		}

		public int Vary
		{
			get { return GetInt(13); }
			set { Set(13, value.ToString()); }
		}

		public int AttachPoint
		{
			get { return GetInt(14); }
			set { Set(14, value.ToString()); }
		}

		public int FineX
		{
			get { return GetInt(15); }
			set { Set(15, value.ToString()); }
		}

		public int FineY
		{
			get { return GetInt(16); }
			set { Set(16, value.ToString()); }
		}
	}
}
