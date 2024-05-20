namespace KisekaeImporter.SubCodes
{
	public class KisekaeImage : KisekaeSubCode, IPoseable
	{
		public void Pose(IPoseable pose)
		{
			KisekaeImage other = pose as KisekaeImage;
			if (other == null)
			{
				return;
			}
			AnchorPoint = other.AnchorPoint;
			Scale = other.Scale;
			ScaleY = other.ScaleY;
			Depth = other.Depth;
			Reversal = other.Reversal;
			X = other.X;
			Y = other.Y;
			Rotation = other.Rotation;
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

		public int ScaleY
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		//Unknown: 6
		//Unknown: 7
		//Unknown: 8

		public int ScaleB
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int Alpha
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public int AnchorPoint
		{
			get { return GetInt(11); }
			set { Set(11, value.ToString()); }
		}

		public int Reversal
		{
			get { return GetInt(12); }
			set { Set(12, value.ToString()); }
		}

		public int FineX
		{
			get { return GetInt(13); }
			set { Set(13, value.ToString()); }
		}

		public int FineY
		{
			get { return GetInt(14); }
			set { Set(14, value.ToString()); }
		}
	}
}
