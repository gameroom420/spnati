﻿using KisekaeImporter.SubCodes;

namespace KisekaeImporter
{
	public class KisekaeAppearance : KisekaeComponent
	{
		[KisekaeSubCode("ca")]
		public KisekaeBodyShape BodyShape
		{
			get { return GetSubCode<KisekaeBodyShape>("ca"); }
			set { SetSubCode("ca", value); }
		}
		[KisekaeSubCode("cc")]
		public KisekaeArmSize ArmSize
		{
			get { return GetSubCode<KisekaeArmSize>("cc"); }
			set { SetSubCode("cc", value); }
		}
		[KisekaeSubCode("cd")]
		public KisekaeHandSize HandSize
		{
			get { return GetSubCode<KisekaeHandSize>("cd"); }
			set { SetSubCode("cd", value); }
		}
		[KisekaeSubCode("ce")]
		public KisekaeUpperArmSize UpperArmSize
		{
			get { return GetSubCode<KisekaeUpperArmSize>("ce"); }
			set { SetSubCode("ce", value); }
		}
		[KisekaeSubCode("cf")]
		public KisekaeThighSize ThighSize
		{
			get { return GetSubCode<KisekaeThighSize>("cf"); }
			set { SetSubCode("cf", value); }
		}
		[KisekaeSubCode("cg")]
		public KisekaeLegSize LegSize
		{
			get { return GetSubCode<KisekaeLegSize>("cg"); }
			set { SetSubCode("cg", value); }
		}
		[KisekaeSubCode("ch")]
		public KisekaeFootSize FootSize
		{
			get { return GetSubCode<KisekaeFootSize>("ch"); }
			set { SetSubCode("ch", value); }
		}
		[KisekaeSubCode("da")]
		public KisekaeSkin Skin
		{
			get { return GetSubCode<KisekaeSkin>("da"); }
			set { SetSubCode("da", value); }
		}
		[KisekaeSubCode("db")]
		public KisekaeTan Tan
		{
			get { return GetSubCode<KisekaeTan>("db"); }
			set { SetSubCode("db", value); }
		}
		[KisekaeSubCode("dd")]
		public KisekaeHead Head
		{
			get { return GetSubCode<KisekaeHead>("dd"); }
			set { SetSubCode("dd", value); }
		}
		[KisekaeSubCode("dh")]
		public KisekaeNipples Nipples
		{
			get { return GetSubCode<KisekaeNipples>("dh"); }
			set { SetSubCode("dh", value); }
		}
		[KisekaeSubCode("di")]
		public KisekaeBreasts Breasts
		{
			get { return GetSubCode<KisekaeBreasts>("di"); }
			set { SetSubCode("di", value); }
		}
		[KisekaeSubCode("qa")]
		public KisekaePenis Penis
		{
			get { return GetSubCode<KisekaePenis>("qa"); }
			set { SetSubCode("qa", value); }
		}
		[KisekaeSubCode("qb")]
		public KisekaeBalls Balls
		{
			get { return GetSubCode<KisekaeBalls>("qb"); }
			set { SetSubCode("qb", value); }
		}
		[KisekaeSubCode("dc")]
		public KisekaeVagina Vagina
		{
			get { return GetSubCode<KisekaeVagina>("dc"); }
			set { SetSubCode("dc", value); }
		}
		[KisekaeSubCode("eh")]
		public KisekaePubicHair Pubes
		{
			get { return GetSubCode<KisekaePubicHair>("eh"); }
			set { SetSubCode("eh", value); }
		}
		[KisekaeSubCode("pb")]
		public KisekaeAnimalEars AnimalEars
		{
			get { return GetSubCode<KisekaeAnimalEars>("pb"); }
			set { SetSubCode("pb", value); }
		}
		[KisekaeSubCode("pc")]
		public KisekaeHorns Horns
		{
			get { return GetSubCode<KisekaeHorns>("pc"); }
			set { SetSubCode("pc", value); }
		}
		[KisekaeSubCode("pd")]
		public KisekaeWings Wings
		{
			get { return GetSubCode<KisekaeWings>("pd"); }
			set { SetSubCode("pd", value); }
		}
		[KisekaeSubCode("pe")]
		public KisekaeTail Tail
		{
			get { return GetSubCode<KisekaeTail>("pe"); }
			set { SetSubCode("pe", value); }
		}
	}
}
