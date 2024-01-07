namespace KisekaeImporter.SubCodes
{
	public class KisekaeRibbon : KisekaeClothes, IPoseable
	{
		public void Pose(IPoseable pose)
		{
			if (pose is KisekaeRibbon other)
			{
				Side = other.Side;
				Layer = other.Layer;
				ScaleX = other.ScaleX;
				Rotation = other.Rotation;
				X = other.X;
				Y = other.Y;
				Outline = other.Outline;
				ScaleY = other.ScaleY;
				Skew = other.Skew;
				Flipped = other.Flipped;
				AttachPoint = other.AttachPoint;
				FineX = other.FineX;
				FineY = other.FineY;
			}
		}

		public int Side
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int Layer
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int ScaleX
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int Outline
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public KisekaeColor OutlineColor
		{
			get { return new KisekaeColor(GetString(11)); }
			set { Set(11, value.ToString()); }
		}

		public int ScaleY
		{
			get { return GetInt(12); }
			set { Set(12, value.ToString()); }
		}

		public int Skew
		{
			get { return GetInt(13); }
			set { Set(13, value.ToString()); }
		}

		public bool Flipped
		{
			get { return GetBool(14); }
			set { Set(14, value); }
		}

		public int AttachPoint
		{
			get { return GetInt(15); }
			set { Set(15, value); }
		}

		public bool Shaded
		{
			get { return GetBool(16); }
			set { Set(16, value); }
		}

		public int Alpha
		{
			get { return GetInt(17); }
			set { Set(17, value); }
		}

		public int FineX
		{
			get { return GetInt(18); }
			set { Set(18, value); }
		}

		public int FineY
		{
			get { return GetInt(19); }
			set { Set(19, value); }
		}
	}
}
