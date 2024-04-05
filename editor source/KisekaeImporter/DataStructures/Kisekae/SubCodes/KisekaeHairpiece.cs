namespace KisekaeImporter.SubCodes
{
	public class KisekaeHairpiece : KisekaeSubCode, IColorable, IPoseable
	{
		public KisekaeHairpiece() : base("r") { }

		/// <summary>
		/// Copies position and rotation pieces from one to another
		/// </summary>
		/// <param name="other"></param>
		public void CopyPositioningFrom(KisekaeHairpiece other)
		{
			Rotation = other.Rotation;
			Layer = other.Layer;
			X = other.X;
			Y = other.Y;
			Width = other.Width;
			Height = other.Height;
			Gravity = other.Gravity;
			Line = other.Line;
			RotationZ = other.RotationZ;
			RotationPreScale = other.RotationPreScale;
			AttachPoint = other.AttachPoint;
			Alpha = other.Alpha;
			FineX = other.FineX;
			FineY = other.FineY;
		}

		public void Pose(IPoseable pose)
		{
			if (pose is KisekaeHairpiece other)
			{
				CopyPositioningFrom(other);
			}
		}

		public void SetColors(KisekaeColor color1, KisekaeColor color2, KisekaeColor color3)
		{
			Color1 = color1;
			Color2 = color2;
			Color3 = color3;
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

		public int Width
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int Height
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int Rotation
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int X
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public int Y
		{
			get { return GetInt(10); }
			set { Set(10, value.ToString()); }
		}

		public bool Gravity
		{
			get { return GetBool(11); }
			set { Set(11, value); }
		}

		public int Line
		{
			get { return GetInt(12); }
			set { Set(12, value); }
		}

		public int RotationZ
		{
			get { return GetInt(13); }
			set { Set(13, value); }
		}

		public bool Shaded
		{
			get { return GetBool(14); }
			set { Set(14, value); }
		}

		public int RotationPreScale
		{
			get { return GetInt(15); }
			set { Set(15, value); }
		}

		public int AttachPoint
		{
			get { return GetInt(16); }
			set { Set(16, value); }
		}

		public int Alpha
		{
			get { return GetInt(17); }
			set { Set(17, value.ToString()); }
		}

		public int FineX
		{
			get { return GetInt(18); }
			set { Set(18, value.ToString()); }
		}

		public int FineY
		{
			get { return GetInt(19); }
			set { Set(19, value.ToString()); }
		}
	}
}
