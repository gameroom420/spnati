using KisekaeImporter.DataStructures.Kisekae;
using KisekaeImporter.SubCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace KisekaeImporter
{
	/// <summary>
	/// Data representation of a kisekae scene, which contains 1 or more characters + scene data
	/// For a single character without scene data, the format is VersionNumber**Code1_Code2_..._CodeN/#]Asset1/#]Asset2/#].../#]AssetN, where an Asset is a URL or font
	/// For a scene, the format is VersionNumber***Character1*Character2*...*Character9#/]SceneCode1_SceneCode2_..._SceneCodeN/#]SceneAsset1/#]SceneAsset2/#].../#]SceneAssetN, where Character is the format for a single character (minus VersionNumber**)
	/// </summary>
	public class KisekaeCode
	{
		private const string DefaultVersion = "68";

		/// <summary>
		/// Kisekae version this code was generated for
		/// </summary>
		public string Version { get; set; }

		public int MinorVersion { get; set; }

		public int AlphaVersion { get; set; }

		public KisekaeModel[] Models = new KisekaeModel[9];
		
		public KisekaeChunk Scene = null;

		public KisekaeCode()
		{
			Version = DefaultVersion;
			MinorVersion = 0;
			AlphaVersion = 0;
		}
		//public KisekaeCode(KisekaeCode original)
		//{
		//	string code = original.Serialize();
		//	Deserialize(code);
		//}
		//public KisekaeCode(KisekaeCode original, bool resetAll)
		//{
		//	if (resetAll)
		//	{
		//		Reset(original.Version);
		//	}
		//	Deserialize(original.Serialize());
		//}
		public KisekaeCode(string data) : this(data, false)
		{
		}
		public KisekaeCode(string data, bool resetAll)
		{
			if (resetAll)
			{
				Reset(data);
			}
			Deserialize(data);
		}

		//private static string DefaultCode54;
		private static string DefaultCode68_;
		static KisekaeCode()
		{
			//DefaultCode54 = "54**ia_if_ib_id_ic_jc_ie_ja_jb_jd_je_jf_jg_ka_kb_kc_kd_ke_kf_la_lb_oa_os_ob_oc_od_oe_of_lc_og_oh_oo_op_oq_or_om_on_ok_ol_oi_oj_r00_s00_m00_n00_t00";
			//DefaultCode68 = "68**ia_if_ib_id_ic_jc_ie_ja_jb_jd_je_jf_jg_ka_kb_kc_kd_ke_kf_la_lb_oa_os_ob_oc_od_oe_of_lc_og_oh_oo_op_oq_or_om_on_ok_ol_oi_oj_r00_s00_m00_n00_t00_f00";
			DefaultCode68_ = "68**ia_ib_id_if_ic_jc_ie_ja_jb_jf_jg_jd_je_ka_kb_kf_kc_kd_ke_la_lb_lc_oa_os_ob_oc_od_oe_of_og_oh_oo_op_oq_or_om_on_ok_ol_oi_oj_r00_s00_m00_n00_t00_f00"; //changed order; did not add kg_
		}

		public override int GetHashCode()
		{
			return Serialize().GetHashCode();
		}

		public void Reset(string data)
		{
			//Fill in empty subcodes to get a blank slate
			//Deserialize(data.StartsWith("54") ? DefaultCode54 : DefaultCode68);
			Deserialize(DefaultCode68_);
		}

		/// <summary>
		/// Merges another code into this one. Any existing subcodes will be replaced
		/// </summary>
		/// <param name="code">Code to merge into this one</param>
		public void MergeIn(KisekaeCode code, bool applyEmpties, bool poseOnly)
		{
			if (code == null)
			{
				return;
			}
			int mergingVersion;
			int version;
			if (int.TryParse(code.Version, out mergingVersion) && int.TryParse(Version, out version))
			{
				if (mergingVersion > version)
				{
					StepUpTransform(this, version, mergingVersion);
					Version = code.Version;
				}
				else if (mergingVersion < version)
				{
					StepUpTransform(code, mergingVersion, version);
				}
			}

			if (code.Scene != null)
			{
				if (Scene == null)
				{
					Scene = new KisekaeChunk("");
				}
				Scene.MergeIn(code.Scene, applyEmpties, poseOnly);
			}

			for (int i = 0; i < Models.Length; i++)
			{
				if (code.Models[i] != null)
				{
					if (Models[i] == null)
					{
						Models[i] = new KisekaeModel("");
					}
					Models[i].MergeIn(code.Models[i], applyEmpties, poseOnly);
				}
			}
		}

		public override string ToString()
		{
			return Serialize();
		}

		/// <summary>
		/// Converts a code into its string representation that Kisekae can import
		/// </summary>
		/// <returns></returns>
		public string Serialize()
		{
			if (string.IsNullOrEmpty(Version))
			{
				return "";
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(Version);
			sb.Append("**");
			if (Scene != null)
			{
				for (int i = 0; i < Models.Length; i++)
				{
					sb.Append("*");
					KisekaeModel model = Models[i];
					if (model != null)
					{
						sb.Append(model.Serialize());
					}
					else
					{
						sb.Append("0");
					}
				}

				if (Scene != null)
				{
					sb.Append("#/]");
				}
				sb.Append(Scene.Serialize());
			}
			else
			{
				if (Models[0] != null)
				{
					sb.Append(Models[0].Serialize());
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Gets whether a subcode with the given prefix exists in either the scene or any model
		/// </summary>
		/// <param name="prefix"></param>
		/// <returns></returns>
		public bool HasSubCode(string id, int index)
		{
			Type componentType = KisekaeSubCodeMap.GetComponentType(id);

			for (int i = 0; i < Models.Length; i++)
			{
				KisekaeModel model = Models[i];
				if (model != null)
				{
					KisekaeComponent component = model.GetComponent(componentType);
					if (component != null && component.HasSubCode(id, index))
					{
						return true;
					}
				}
			}
			if (Scene != null)
			{
				KisekaeComponent component = Scene.GetComponent(componentType);
				if (component != null && component.HasSubCode(id, index))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets all existing subcodes of the given type across all models
		/// </summary>
		/// <returns></returns>
		public IEnumerable<KisekaeSubCode> GetSubCodesOfType<T>()
		{
			for (int i = 0; i < Models.Length; i++)
			{
				KisekaeChunk model = Models[i];
				if (model != null)
				{
					foreach (KisekaeSubCode code in model.GetSubCodesOfType<T>())
					{
						yield return code;
					}
				}
			}

			if (Scene != null)
			{
				foreach (KisekaeSubCode code in Scene.GetSubCodesOfType<T>())
				{
					yield return code;
				}
			}
		}

		public T GetComponent<T>() where T : KisekaeComponent
		{
			if (Models[0] != null)
			{
				return Models[0].GetComponent(typeof(T)) as T;
			}
			return null;
		}

		public T GetOrAddComponent<T>() where T : KisekaeComponent
		{
			if (Models[0] == null)
			{
				Models[0] = new KisekaeModel("");
			}
			return Models[0].GetOrAddComponent<T>();
		}

		public void ReplaceComponent<T>(T component) where T : KisekaeComponent
		{
			if (Models[0] != null)
			{
				Models[0].ReplaceComponent(component);
			}
		}

		/// <summary>
		/// Deserializes a code into its parts
		/// </summary>
		/// <param name="data"></param>
		public void Deserialize(string data)
		{
			if (string.IsNullOrEmpty(data))
				return;
			data = data.Trim();
			if (string.IsNullOrEmpty(data))
				return;

			//Extract the version and whether this is a scene or not
			string[] versionSplit = data.Split(new string[] { "**" }, StringSplitOptions.None);
			string subdata = "";
			if (versionSplit.Length == 1)
			{
				Version = DefaultVersion;
				subdata = data;
			}
			else
			{
				Version = versionSplit[0];
				subdata = versionSplit[1];
			}
			if (subdata.StartsWith("*"))
			{
				subdata = subdata.Substring(1);
				Scene = new KisekaeChunk("");
			}

			//split out the scene data
			string[] modelSceneSplit = subdata.Split(new string[] { "#/]" }, StringSplitOptions.None);

			//process models
			string[] models = modelSceneSplit[0].Split('*');
			for (int i = 0; i < models.Length; i++)
			{
				if (i >= Models.Length)
				{
					continue; //too many characters. This can't be valid.
				}
				string modelData = models[i];
				if (modelData == "0")
				{
					continue;
				}
				
				Models[i] = new KisekaeModel(models[i]);
			}

			KisekaeModInfo modInfo = Models[0].GetComponent<KisekaeModInfo>();
			if (modInfo != null)
			{
				MinorVersion = modInfo.Version.MinorVersion;
				AlphaVersion = modInfo.Version.AlphaVersion;
			}
			else
			{
				MinorVersion = 0;
				AlphaVersion = 0;
			}

			//process scene
			if (modelSceneSplit.Length > 1)
			{
				Scene.Deserialize(modelSceneSplit[1]);
			}
		}

		public int TotalAssets
		{
			get
			{
				int count = 0;
				for (int i = 0; i < Models.Length; i++)
				{
					if (Models[i] != null)
					{
						count += Models[i].Assets.Count;
					}
				}
				if (Scene != null)
				{
					count += Scene.Assets.Count;
				}
				return count;
			}
		}

		/// <summary>
		/// Performs code transformations so merging separate versions doesn't screw things up
		/// </summary>
		/// <param name="code"></param>
		/// <param name="oldVersion"></param>
		/// <param name="newVersion"></param>
		private static void StepUpTransform(KisekaeCode code, int oldVersion, int newVersion)
		{
			/*
			for (int i = 0; i < code.Models.Length; i++)
			{
				KisekaeModel model = code.Models[i];
				if (model != null)
				{
					if (oldVersion < 83 && newVersion >= 83)
					{
						//mouth shapes
						KisekaeExpression expression = model.GetComponent<KisekaeExpression>();
						if (expression != null)
						{
							expression.Mouth.MouthYMove = 50;
							expression.Mouth.MouthXMove = 50;
							expression.Mouth.MouthRotation = 50;
						}
						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							ConvertShoe84(clothing.LeftShoe);
							ConvertShoe84(clothing.RightShoe);
						}
					}
				}
			}
			*/
		}

		/*
		private static void ConvertShoe84(KisekaeShoe shoe)
		{
			switch (shoe.Type)
			{
				case 0:
					shoe.Top = 1;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color1;
					break;
				case 1:
					shoe.Top = 2;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 10:
					shoe.Type = 1;
					shoe.Top = 3;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 11:
					shoe.Type = 1;
					shoe.Top = 4;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 15:
					shoe.Top = 6;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color1;
					break;
				case 16:
					shoe.Type = 1;
					shoe.Top = 7;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 17:
					shoe.Type = 14;
					shoe.Top = 8;
					shoe.TopColor1 = shoe.Color1;
					shoe.TopColor2 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 18:
					shoe.Type = 14;
					shoe.Top = 9;
					shoe.TopColor1 = shoe.Color1;
					shoe.TopColor2 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 19:
					shoe.Type = 15;
					shoe.Top = 10;
					shoe.TopColor1 = shoe.Color2;
					shoe.Color2 = shoe.Color3;
					break;
				case 20:
					shoe.Type = 16;
					shoe.Top = 11;
					shoe.TopColor1 = shoe.Color2;
					break;
			}
		}
		*/

		/// <summary>
		/// Returns a positive integer if this code was exported by a version of KKL older than the one specified by the parameters,
		/// a negative integer if this code was exported by a newer version, 
		/// and zero if the versions were the same.
		/// </summary>
		public int BeforeKKLVersion(int majorVersion, int minorVersion, int alphaVersion)
		{
			int intVersion;
			if (int.TryParse(Version, out intVersion))
			{
				if (intVersion == majorVersion)
				{
					if (MinorVersion == minorVersion)
					{
						return alphaVersion - AlphaVersion;
					}
					else return minorVersion - MinorVersion;
				}
				else return majorVersion - intVersion;
			}
			else return 1;
		}

		private int BeforeKKLVersion(int majorVersion, int minorVersion, int alphaVersion, int majorCompared, int minorCompared, int alphaCompared)
		{
			if (majorCompared == majorVersion)
			{
				if (minorCompared == minorVersion)
				{
					return alphaVersion - alphaCompared;
				}
				else return minorVersion - minorCompared;
			}
			else return majorVersion - majorCompared;
		}

		public void UpdateCode(int majorVersion, int minorVersion, int alphaVersion)
		{

			// model code

			for (int i = 0; i < Models.Length; i++)
			{
				KisekaeModel model = Models[i];
				if (model != null)
				{

					if (BeforeKKLVersion(106, 0, 0) > 0 && BeforeKKLVersion(106, 0, 0, majorVersion, minorVersion, alphaVersion) <= 0)
					{
						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeRibbon ribbon = (KisekaeRibbon)clothing.GetSubCode("m", j);
								if (ribbon != null)
								{
									ribbon.FineX = 50;
									ribbon.FineY = 50;
								}
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeBelt belt = (KisekaeBelt)clothing.GetSubCode("s", j);
								if (belt != null)
								{
									belt.FineX = 50;
									belt.FineY = 50;
								}
							}
						}

						KisekaeExternalParts externalParts = model.GetComponent<KisekaeExternalParts>();
						if (externalParts != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeImage image = (KisekaeImage)externalParts.GetSubCode("f", j);
								if (image != null)
								{
									image.FineX = 50;
									image.FineY = 50;
								}
							}
						}

						KisekaeFace face = model.GetComponent<KisekaeFace>();
						if (face != null)
						{
							KisekaeEyebrows eyebrows = face.Eyebrows;
							if (eyebrows != null)
							{
								eyebrows.X = 50;
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeMark mark = (KisekaeMark)face.GetSubCode("t", j);
								if (mark != null)
								{
									mark.Vary = 0;
									mark.AttachPoint = 0;
								}
							}
						}

						KisekaeAppearance appearance = model.GetComponent<KisekaeAppearance>();
						if (appearance != null)
						{
							KisekaePenis penis = appearance.Penis;
							if (penis != null)
							{
								penis.SizeAuto = true;
								penis.ScaleX = 50;
								penis.ScaleY = 50;
								penis.OffsetX = 50;
								penis.OffsetY = 50;
							}
						}

						KisekaeExpression expression = model.GetComponent<KisekaeExpression>();
						if (expression != null)
						{
							KisekaeExpressionBrow expressionBrow = expression.Brows;
							if (expressionBrow != null)
							{
								expressionBrow.EyebrowMoveHorizontalLeft = 50;
								expressionBrow.EyebrowMoveHorizontalRight = 50;
							}
						}

						KisekaePose pose = model.GetComponent<KisekaePose>();
						if (pose != null)
						{
							KisekaeArm arm = pose.Arms;
							if (arm != null)
							{
								arm.LeftArmFreeRotation = false;
								arm.RightArmFreeRotation = false;
							}
						}
					}
					if (BeforeKKLVersion(107, 0, 2) > 0 && BeforeKKLVersion(107, 0, 2, majorVersion, minorVersion, alphaVersion) <= 0)
					{
						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeRibbon ribbon = (KisekaeRibbon)clothing.GetSubCode("m", j);
								if (ribbon != null && ribbon.Type >= 143 && ribbon.Type <= 150)
								{
									ribbon.Outline = 0;
								}
							}
						}
					}
					if (BeforeKKLVersion(107, 0, 5) > 0 && BeforeKKLVersion(107, 0, 5, majorVersion, minorVersion, alphaVersion) <= 0)
					{
						KisekaeAppearance appearance = model.GetComponent<KisekaeAppearance>();
						if (appearance != null)
						{
							KisekaeThighSize thighSize = appearance.ThighSize;
							if (thighSize != null)
							{
								thighSize.LeftShiriVisible = thighSize.LeftThighVisible;
								thighSize.RightShiriVisible = thighSize.RightThighVisible;
							}
						}

						KisekaeHair hair = model.GetComponent<KisekaeHair>();
						if (hair != null)
						{
							for (int j = 0; j < 999; j++)
							{
								KisekaeHairpiece hairpiece = (KisekaeHairpiece)hair.GetSubCode("r", j);
								if (hairpiece != null)
								{
									hairpiece.FineX = 50;
									hairpiece.FineY = 50;
								}
							}
						}

						KisekaeFace face = model.GetComponent<KisekaeFace>();
						if (face != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeMark mark = (KisekaeMark)face.GetSubCode("t", j);
								if (mark != null)
								{
									mark.FineX = 50;
									mark.FineY = 50;
								}
							}
						}
					}
					if (BeforeKKLVersion(107, 0, 6) > 0 && BeforeKKLVersion(107, 0, 6, majorVersion, minorVersion, alphaVersion) <= 0)
					{
						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							for (int j = 0; j < 999; j++)
							{
								KisekaeRibbon ribbon = (KisekaeRibbon)clothing.GetSubCode("m", j);
								if (ribbon != null)
								{
									ribbon.FineX *= 5;
									ribbon.FineX -= 200;
									ribbon.FineY *= 5;
									ribbon.FineY -= 200;
								}
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeBelt belt = (KisekaeBelt)clothing.GetSubCode("s", j);
								if (belt != null)
								{
									belt.FineX *= 5;
									belt.FineX -= 200;
									belt.FineY *= 5;
									belt.FineY -= 200;
								}
							}
						}

						KisekaeExternalParts externalParts = model.GetComponent<KisekaeExternalParts>();
						if (externalParts != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeImage image = (KisekaeImage)externalParts.GetSubCode("f", j);
								if (image != null)
								{
									image.FineX *= 5;
									image.FineX -= 200;
									image.FineY *= 5;
									image.FineY -= 200;
								}
							}
						}
					}
					if (BeforeKKLVersion(107, 1, 1) > 0 && BeforeKKLVersion(107, 1, 1, majorVersion, minorVersion, alphaVersion) <= 0)
					{
						KisekaePose pose = model.GetComponent<KisekaePose>();
						if (pose != null)
						{
							KisekaeBodyRotation bodyRotation = pose.Body;
							if (bodyRotation != null)
								bodyRotation.Rotation *= 10;

							KisekaePlacement placement = pose.Placement;
							if (placement != null)
								placement.Jump *= 10;
						}

						KisekaeShader shader = model.GetComponent<KisekaeShader>();
						if (shader != null)
						{
							KisekaeShadow shadow = shader.Shadow;
							if (shadow != null)
								shadow.Rotation *= 10;
						}

						KisekaeAppearance appearance = model.GetComponent<KisekaeAppearance>();
						if (appearance != null)
						{
							KisekaeThighSize thighSize = appearance.ThighSize;
							if (thighSize != null)
							{
								thighSize.LeftThighRotation *= 10;
								thighSize.RightThighRotation *= 10;
								thighSize.LeftThighScaleX *= 10;
								thighSize.LeftThighScaleY *= 10;
								thighSize.LeftThighOffsetX *= 10;
								thighSize.LeftThighOffsetY *= 10;
								thighSize.RightThighScaleX *= 10;
								thighSize.RightThighScaleY *= 10;
								thighSize.RightThighOffsetX *= 10;
								thighSize.RightThighOffsetY *= 10;
							}

							KisekaeLegSize legSize = appearance.LegSize;
							if (legSize != null)
							{
								legSize.LeftLegRotation *= 10;
								legSize.RightLegRotation *= 10;
								legSize.LeftLegScaleX *= 10;
								legSize.LeftLegScaleY *= 10;
								legSize.LeftLegOffsetX *= 10;
								legSize.LeftLegOffsetY *= 10;
								legSize.RightLegScaleX *= 10;
								legSize.RightLegScaleY *= 10;
								legSize.RightLegOffsetX *= 10;
								legSize.RightLegOffsetY *= 10;
							}

							KisekaeFootSize footSize = appearance.FootSize;
							if (footSize != null)
							{
								footSize.LeftFootRotation *= 10;
								footSize.RightFootRotation *= 10;
								footSize.LeftFootScaleX *= 10;
								footSize.LeftFootScaleY *= 10;
								footSize.LeftFootOffsetX *= 10;
								footSize.LeftFootOffsetY *= 10;
								footSize.RightFootScaleX *= 10;
								footSize.RightFootScaleY *= 10;
								footSize.RightFootOffsetX *= 10;
								footSize.RightFootOffsetY *= 10;
							}

							KisekaeBodyShape bodyShape = appearance.BodyShape;
							if (bodyShape != null)
							{
								bodyShape.BodyHeight *= 10;
								bodyShape.AshiHeight *= 10;
								bodyShape.AshiWidth *= 10;
								bodyShape.BodyWidth *= 10;
								bodyShape.ShoulderWidth *= 10;
								bodyShape.HipWidth *= 10;
								bodyShape.DouHeight *= 10;
							}

							KisekaeHead head = appearance.Head;
							if (head != null)
							{
								head.HeadScale *= 10;
								head.NeckHeight *= 10;
								head.ContourWidth *= 10;
								head.ContourHeight *= 10;
							}

							KisekaeUpperArmSize upperArmSize = appearance.UpperArmSize;
							if (upperArmSize != null)
							{
								upperArmSize.LeftUpperArmScaleX *= 10;
								upperArmSize.LeftUpperArmScaleY *= 10;
								upperArmSize.LeftUpperArmOffsetX *= 10;
								upperArmSize.LeftUpperArmOffsetY *= 10;
								upperArmSize.RightUpperArmScaleX *= 10;
								upperArmSize.RightUpperArmScaleY *= 10;
								upperArmSize.RightUpperArmOffsetX *= 10;
								upperArmSize.RightUpperArmOffsetY *= 10;
							}

							KisekaeArmSize armSize = appearance.ArmSize;
							if (armSize != null)
							{
								armSize.LeftArmScaleX *= 10;
								armSize.LeftArmScaleY *= 10;
								armSize.LeftArmOffsetX *= 10;
								armSize.LeftArmOffsetY *= 10;
								armSize.RightArmScaleX *= 10;
								armSize.RightArmScaleY *= 10;
								armSize.RightArmOffsetX *= 10;
								armSize.RightArmOffsetY *= 10;
							}

							KisekaeHandSize handSize = appearance.HandSize;
							if (handSize != null)
							{
								handSize.LeftHandScaleX *= 10;
								handSize.LeftHandScaleY *= 10;
								handSize.LeftHandOffsetX *= 10;
								handSize.LeftHandOffsetY *= 10;
								handSize.RightHandScaleX *= 10;
								handSize.RightHandScaleY *= 10;
								handSize.RightHandOffsetX *= 10;
								handSize.RightHandOffsetY *= 10;
							}
						}

						KisekaeFace face = model.GetComponent<KisekaeFace>();
						if (face != null)
						{
							KisekaeEyes eyes = face.Eyes;
							if (eyes != null)
							{
								eyes.EyeX *= 10;
								eyes.EyeY *= 10;
								eyes.EyeScaleX *= 10;
								eyes.EyeScaleY *= 10;
							}

							KisekaeIris iris = face.Iris;
							if (iris != null)
							{
								iris.Scale *= 10;
								iris.ScaleY *= 10;
								iris.XOffset *= 10;
								iris.YOffset *= 10;
							}

							KisekaePupil pupil = face.Pupils;
							if (pupil != null)
							{
								pupil.LightRotation *= 10;
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeMark mark = (KisekaeMark)face.GetSubCode("t", j);
								if (mark != null)
								{
									mark.Rotation *= 10;
									mark.ScaleX *= 10;
									mark.ScaleY *= 10;
								}
							}
						}

						KisekaeExpression expression = model.GetComponent<KisekaeExpression>();
						if (expression != null)
						{
							KisekaeLook look = expression.Look;
							if (look != null)
							{
								look.EyeballWidth *= 10;
								look.EyeballWidthRight *= 10;
								look.EyeballXMove *= 10;
								look.EyeballXMoveRight *= 10;
								look.EyeballYMove *= 10;
								look.EyeballYMoveRight *= 10;
							}

							KisekaeExpressionBrow expressionBrow = expression.Brows;
							if (expressionBrow != null)
							{
								expressionBrow.EyebrowMoveLeft *= 10;
								expressionBrow.EyebrowMoveRight *= 10;
								expressionBrow.EyebrowMoveHorizontalLeft *= 10;
								expressionBrow.EyebrowMoveHorizontalRight *= 10;
							}

							KisekaeExpressionMouth expressionMouth = expression.Mouth;
							if (expressionMouth != null)
							{
								expressionMouth.MouthWidth *= 10;
								expressionMouth.MouthHeight *= 10;
								expressionMouth.MouthXMove *= 10;
								expressionMouth.MouthYMove *= 10;
							}
						}

						KisekaeHair hair = model.GetComponent<KisekaeHair>();
						if (hair != null)
						{
							KisekaeBangs bangs = hair.Bangs;
							if (bangs != null)
							{
								bangs.BangsHeight *= 10;
							}

							KisekaeBackHair backHair = hair.Back;
							if (backHair != null)
							{
								backHair.Height *= 10;
								backHair.Width *= 10;
								backHair.Y *= 10;
							}

							KisekaeHairSide leftSide = hair.LeftSide;
							if (leftSide != null)
							{
								leftSide.Height *= 10;
								leftSide.X *= 10;
							}

							KisekaeHairSide rightSide = hair.RightSide;
							if (rightSide != null)
							{
								rightSide.Height *= 10;
								rightSide.X *= 10;
							}

							for (int j = 0; j < 999; j++)
							{
								KisekaeHairpiece hairpiece = (KisekaeHairpiece)hair.GetSubCode("r", j);
								if (hairpiece != null)
								{
									hairpiece.Rotation *= 10;
									hairpiece.RotationPlus *= 10;
									hairpiece.ScaleX *= 10;
									hairpiece.ScaleY *= 10;
								}
							}
						}

						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							for (int j = 0; j < 999; j++)
							{
								KisekaeRibbon ribbon = (KisekaeRibbon)clothing.GetSubCode("m", j);
								if (ribbon != null)
								{
									ribbon.Rotation *= 10;
									ribbon.ScaleX *= 10;
									ribbon.ScaleY *= 10;
								}
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeBelt belt = (KisekaeBelt)clothing.GetSubCode("s", j);
								if (belt != null)
								{
									belt.Rotation *= 10;
									belt.Scale *= 10;
									belt.ScaleY *= 10;
								}
							}

						}

						KisekaeExternalParts externalParts = model.GetComponent<KisekaeExternalParts>();
						if (externalParts != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeImage image = (KisekaeImage)externalParts.GetSubCode("f", j);
								if (image != null)
								{
									image.Rotation *= 10;
									image.Scale *= 10;
									image.ScaleY *= 10;
								}
							}
						}



						KisekaeModInfo modInfo = model.GetComponent<KisekaeModInfo>();
						if (modInfo != null)
						{
							modInfo.Version.MinorVersion = minorVersion;
							modInfo.Version.AlphaVersion = alphaVersion;
						}
						else
						{
							model.MergeIn(new KisekaeChunk("fv" + minorVersion + "." + alphaVersion), false, false);
						}

					}
				}
			}


			// global/system/scene code

			KisekaeChunk scene = Scene;
			if (scene != null)
			{
				if (BeforeKKLVersion(106,0,0) > 0 && BeforeKKLVersion(106, 0, 0, majorVersion, minorVersion, alphaVersion) <= 0)
				{
					KisekaeGlobalParts globalParts = scene.GetComponent<KisekaeGlobalParts>();
					if (globalParts != null)
					{
						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalRibbon globalRibbon = (KisekaeGlobalRibbon)globalParts.GetSubCode("w", j);
							if (globalRibbon != null)
							{
								globalRibbon.FineX = 50;
								globalRibbon.FineY = 50;
								globalRibbon.Alpha = 100;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalBelt globalBelt = (KisekaeGlobalBelt)globalParts.GetSubCode("x", j);
							if (globalBelt != null)
							{
								globalBelt.FineX = 50;
								globalBelt.FineY = 50;
								globalBelt.Alpha = 100;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalArm globalArm = (KisekaeGlobalArm)globalParts.GetSubCode("a", j);
							if (globalArm != null)
							{
								globalArm.FineX = 50;
								globalArm.FineY = 50;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalChair globalChair = (KisekaeGlobalChair)globalParts.GetSubCode("e", j);
							if (globalChair != null)
							{
								globalChair.FineX = 50;
								globalChair.FineY = 50;
								globalChair.Alpha = 100;
							}
						}
					}

					KisekaeScene kisekaeScene = scene.GetComponent<KisekaeScene>();
					if (scene != null)
					{

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalImage globalImage = (KisekaeGlobalImage)kisekaeScene.GetSubCode("v", j);
							if (globalImage != null)
							{
								globalImage.FineX = 50;
								globalImage.FineY = 50;
							}
						}
					}
				}
				if (BeforeKKLVersion(107,1,1) > 0 && BeforeKKLVersion(107, 1, 1, majorVersion, minorVersion, alphaVersion) <= 0)
				{
					KisekaeGlobalParts globalParts = scene.GetComponent<KisekaeGlobalParts>();
					if (globalParts != null)
					{
						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalRibbon globalRibbon = (KisekaeGlobalRibbon)globalParts.GetSubCode("w", j);
							if (globalRibbon != null)
							{
								globalRibbon.Rotation *= 10;
								globalRibbon.Scale *= 10;
								globalRibbon.ScaleY *= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalBelt globalBelt = (KisekaeGlobalBelt)globalParts.GetSubCode("x", j);
							if (globalBelt != null)
							{
								globalBelt.Rotation *= 10;
								globalBelt.Scale *= 10;
								globalBelt.ScaleY *= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalSpeech globalSpeech = (KisekaeGlobalSpeech)globalParts.GetSubCode("z", j);
							if (globalSpeech != null)
							{
								globalSpeech.Rotation *= 10;
								globalSpeech.ExtraArrowRotation *= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalArm globalArm = (KisekaeGlobalArm)globalParts.GetSubCode("a", j);
							if (globalArm != null)
							{
								globalArm.Rotation *= 10;
								globalArm.Scale *= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalFlag globalFlag = (KisekaeGlobalFlag)globalParts.GetSubCode("y", j);
							if (globalFlag != null)
							{
								globalFlag.Rotation *= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalChair globalChair = (KisekaeGlobalChair)globalParts.GetSubCode("e", j);
							if (globalChair != null)
							{
								globalChair.Rotation *= 10;
								globalChair.Scale *= 10;
							}
						}
					}

					KisekaeScene kisekaeScene = scene.GetComponent<KisekaeScene>();
					if (scene != null)
					{
						for (int j = 0; j < 99; j++)
						{
							KisekaeTextBox textBox = (KisekaeTextBox)kisekaeScene.GetSubCode("u", j);
							if (textBox != null)
							{
								textBox.Rotation *= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalImage globalImage = (KisekaeGlobalImage)kisekaeScene.GetSubCode("v", j);
							if (globalImage != null)
							{
								globalImage.Rotation *= 10;
								globalImage.Scale *= 10;
								globalImage.ScaleY *= 10;
							}
						}
					}
				}




				KisekaeModInfo sceneModInfo = scene.GetComponent<KisekaeModInfo>();
				if (sceneModInfo != null)
				{
					sceneModInfo.Version.MinorVersion = minorVersion;
					sceneModInfo.Version.AlphaVersion = alphaVersion;
				}
				else
				{
					scene.MergeIn(new KisekaeChunk("fv" + minorVersion + "." + alphaVersion), false, false);
				}

			}


			Version = majorVersion.ToString();
			MinorVersion = minorVersion;
			AlphaVersion = alphaVersion;
		}

		public void DowndateCode(int majorVersion, int minorVersion, int alphaVersion)
		{

			// model code

			for (int i = 0; i < Models.Length; i++)
			{
				KisekaeModel model = Models[i];
				if (model != null)
				{

					if (BeforeKKLVersion(107, 0, 6) > 0 && BeforeKKLVersion(107, 0, 6, majorVersion, minorVersion, alphaVersion) <= 0)
					{
						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							for (int j = 0; j < 999; j++)
							{
								KisekaeRibbon ribbon = (KisekaeRibbon)clothing.GetSubCode("m", j);
								if (ribbon != null)
								{
									ribbon.FineX += 200;
									ribbon.FineX /= 5;
									ribbon.FineY += 200;
									ribbon.FineY /= 5;
								}
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeBelt belt = (KisekaeBelt)clothing.GetSubCode("s", j);
								if (belt != null)
								{
									belt.FineX += 200;
									belt.FineX /= 5;
									belt.FineY += 200;
									belt.FineY /= 5;
								}
							}
						}

						KisekaeExternalParts externalParts = model.GetComponent<KisekaeExternalParts>();
						if (externalParts != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeImage image = (KisekaeImage)externalParts.GetSubCode("f", j);
								if (image != null)
								{
									image.FineX += 200;
									image.FineX /= 5;
									image.FineY += 200;
									image.FineY /= 5;
								}
							}
						}
					}
					if (BeforeKKLVersion(107, 1, 1) < 0 && BeforeKKLVersion(107, 1, 1, majorVersion, minorVersion, alphaVersion) > 0)
					{
						KisekaePose pose = model.GetComponent<KisekaePose>();
						if (pose != null)
						{
							KisekaeBodyRotation bodyRotation = pose.Body;
							if (bodyRotation != null)
								bodyRotation.Rotation /= 10;

							KisekaePlacement placement = pose.Placement;
							if (placement != null)
								placement.Jump /= 10;
						}

						KisekaeShader shader = model.GetComponent<KisekaeShader>();
						if (shader != null)
						{
							KisekaeShadow shadow = shader.Shadow;
							if (shadow != null)
								shadow.Rotation /= 10;
						}

						KisekaeAppearance appearance = model.GetComponent<KisekaeAppearance>();
						if (appearance != null)
						{
							KisekaeThighSize thighSize = appearance.ThighSize;
							if (thighSize != null)
							{
								thighSize.LeftThighRotation /= 10;
								thighSize.RightThighRotation /= 10;
								thighSize.LeftThighScaleX /= 10;
								thighSize.LeftThighScaleY /= 10;
								thighSize.LeftThighOffsetX /= 10;
								thighSize.LeftThighOffsetY /= 10;
								thighSize.RightThighScaleX /= 10;
								thighSize.RightThighScaleY /= 10;
								thighSize.RightThighOffsetX /= 10;
								thighSize.RightThighOffsetY /= 10;
							}

							KisekaeLegSize legSize = appearance.LegSize;
							if (legSize != null)
							{
								legSize.LeftLegRotation /= 10;
								legSize.RightLegRotation /= 10;
								legSize.LeftLegScaleX /= 10;
								legSize.LeftLegScaleY /= 10;
								legSize.LeftLegOffsetX /= 10;
								legSize.LeftLegOffsetY /= 10;
								legSize.RightLegScaleX /= 10;
								legSize.RightLegScaleY /= 10;
								legSize.RightLegOffsetX /= 10;
								legSize.RightLegOffsetY /= 10;
							}

							KisekaeFootSize footSize = appearance.FootSize;
							if (footSize != null)
							{
								footSize.LeftFootRotation /= 10;
								footSize.RightFootRotation /= 10;
								footSize.LeftFootScaleX /= 10;
								footSize.LeftFootScaleY /= 10;
								footSize.LeftFootOffsetX /= 10;
								footSize.LeftFootOffsetY /= 10;
								footSize.RightFootScaleX /= 10;
								footSize.RightFootScaleY /= 10;
								footSize.RightFootOffsetX /= 10;
								footSize.RightFootOffsetY /= 10;
							}

							KisekaeBodyShape bodyShape = appearance.BodyShape;
							if (bodyShape != null)
							{
								bodyShape.BodyHeight /= 10;
								bodyShape.AshiHeight /= 10;
								bodyShape.AshiWidth /= 10;
								bodyShape.BodyWidth /= 10;
								bodyShape.ShoulderWidth /= 10;
								bodyShape.HipWidth /= 10;
								bodyShape.DouHeight /= 10;
							}

							KisekaeHead head = appearance.Head;
							if (head != null)
							{
								head.HeadScale /= 10;
								head.NeckHeight /= 10;
								head.ContourWidth /= 10;
								head.ContourHeight /= 10;
							}

							KisekaeUpperArmSize upperArmSize = appearance.UpperArmSize;
							if (upperArmSize != null)
							{
								upperArmSize.LeftUpperArmScaleX /= 10;
								upperArmSize.LeftUpperArmScaleY /= 10;
								upperArmSize.LeftUpperArmOffsetX /= 10;
								upperArmSize.LeftUpperArmOffsetY /= 10;
								upperArmSize.RightUpperArmScaleX /= 10;
								upperArmSize.RightUpperArmScaleY /= 10;
								upperArmSize.RightUpperArmOffsetX /= 10;
								upperArmSize.RightUpperArmOffsetY /= 10;
							}

							KisekaeArmSize armSize = appearance.ArmSize;
							if (armSize != null)
							{
								armSize.LeftArmScaleX /= 10;
								armSize.LeftArmScaleY /= 10;
								armSize.LeftArmOffsetX /= 10;
								armSize.LeftArmOffsetY /= 10;
								armSize.RightArmScaleX /= 10;
								armSize.RightArmScaleY /= 10;
								armSize.RightArmOffsetX /= 10;
								armSize.RightArmOffsetY /= 10;
							}

							KisekaeHandSize handSize = appearance.HandSize;
							if (handSize != null)
							{
								handSize.LeftHandScaleX /= 10;
								handSize.LeftHandScaleY /= 10;
								handSize.LeftHandOffsetX /= 10;
								handSize.LeftHandOffsetY /= 10;
								handSize.RightHandScaleX /= 10;
								handSize.RightHandScaleY /= 10;
								handSize.RightHandOffsetX /= 10;
								handSize.RightHandOffsetY /= 10;
							}
						}

						KisekaeFace face = model.GetComponent<KisekaeFace>();
						if (face != null)
						{
							KisekaeEyes eyes = face.Eyes;
							if (eyes != null)
							{
								eyes.EyeX /= 10;
								eyes.EyeY /= 10;
								eyes.EyeScaleX /= 10;
								eyes.EyeScaleY /= 10;
							}

							KisekaeIris iris = face.Iris;
							if (iris != null)
							{
								iris.Scale /= 10;
								iris.ScaleY /= 10;
								iris.XOffset /= 10;
								iris.YOffset /= 10;
							}

							KisekaePupil pupil = face.Pupils;
							if (pupil != null)
							{
								pupil.LightRotation /= 10;
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeMark mark = (KisekaeMark)face.GetSubCode("t", j);
								if (mark != null)
								{
									mark.Rotation /= 10;
									mark.ScaleX /= 10;
									mark.ScaleY /= 10;
								}
							}
						}

						KisekaeExpression expression = model.GetComponent<KisekaeExpression>();
						if (expression != null)
						{
							KisekaeLook look = expression.Look;
							if (look != null)
							{
								look.EyeballWidth /= 10;
								look.EyeballWidthRight /= 10;
								look.EyeballXMove /= 10;
								look.EyeballXMoveRight /= 10;
								look.EyeballYMove /= 10;
								look.EyeballYMoveRight /= 10;
							}

							KisekaeExpressionBrow expressionBrow = expression.Brows;
							if (expressionBrow != null)
							{
								expressionBrow.EyebrowMoveLeft /= 10;
								expressionBrow.EyebrowMoveRight /= 10;
								expressionBrow.EyebrowMoveHorizontalLeft /= 10;
								expressionBrow.EyebrowMoveHorizontalRight /= 10;
							}

							KisekaeExpressionMouth expressionMouth = expression.Mouth;
							if (expressionMouth != null)
							{
								expressionMouth.MouthWidth /= 10;
								expressionMouth.MouthHeight /= 10;
								expressionMouth.MouthXMove /= 10;
								expressionMouth.MouthYMove /= 10;
							}
						}

						KisekaeHair hair = model.GetComponent<KisekaeHair>();
						if (hair != null)
						{
							KisekaeBangs bangs = hair.Bangs;
							if (bangs != null)
							{
								bangs.BangsHeight /= 10;
							}

							KisekaeBackHair backHair = hair.Back;
							if (backHair != null)
							{
								backHair.Height /= 10;
								backHair.Width /= 10;
								backHair.Y /= 10;
							}

							KisekaeHairSide leftSide = hair.LeftSide;
							if (leftSide != null)
							{
								leftSide.Height /= 10;
								leftSide.X /= 10;
							}

							KisekaeHairSide rightSide = hair.RightSide;
							if (rightSide != null)
							{
								rightSide.Height /= 10;
								rightSide.X /= 10;
							}

							for (int j = 0; j < 999; j++)
							{
								KisekaeHairpiece hairpiece = (KisekaeHairpiece)hair.GetSubCode("r", j);
								if (hairpiece != null)
								{
									hairpiece.Rotation /= 10;
									hairpiece.RotationPlus /= 10;
									hairpiece.ScaleX /= 10;
									hairpiece.ScaleY /= 10;
								}
							}
						}

						KisekaeClothing clothing = model.GetComponent<KisekaeClothing>();
						if (clothing != null)
						{
							for (int j = 0; j < 999; j++)
							{
								KisekaeRibbon ribbon = (KisekaeRibbon)clothing.GetSubCode("m", j);
								if (ribbon != null)
								{
									ribbon.Rotation /= 10;
									ribbon.ScaleX /= 10;
									ribbon.ScaleY /= 10;
								}
							}

							for (int j = 0; j < 99; j++)
							{
								KisekaeBelt belt = (KisekaeBelt)clothing.GetSubCode("s", j);
								if (belt != null)
								{
									belt.Rotation /= 10;
									belt.Scale /= 10;
									belt.ScaleY /= 10;
								}
							}

						}

						KisekaeExternalParts externalParts = model.GetComponent<KisekaeExternalParts>();
						if (externalParts != null)
						{
							for (int j = 0; j < 99; j++)
							{
								KisekaeImage image = (KisekaeImage)externalParts.GetSubCode("f", j);
								if (image != null)
								{
									image.Rotation /= 10;
									image.Scale /= 10;
									image.ScaleY /= 10;
								}
							}
						}
					}


					KisekaeModInfo modInfo = model.GetComponent<KisekaeModInfo>();
					if (modInfo != null)
					{
						modInfo.Version.MinorVersion = minorVersion;
						modInfo.Version.AlphaVersion = alphaVersion;
					}
					else
					{
						model.MergeIn(new KisekaeChunk("fv" + minorVersion + "." + alphaVersion), false, false);
					}

				}
			}

			// global/system/scene code

			KisekaeChunk scene = Scene;
			if (scene != null)
			{
				if (BeforeKKLVersion(107,1,1) < 0 && BeforeKKLVersion(107, 1, 1, majorVersion, minorVersion, alphaVersion) > 0)
				{
					KisekaeGlobalParts globalParts = scene.GetComponent<KisekaeGlobalParts>();
					if (globalParts != null)
					{
						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalRibbon globalRibbon = (KisekaeGlobalRibbon)globalParts.GetSubCode("w", j);
							if (globalRibbon != null)
							{
								globalRibbon.Rotation /= 10;
								globalRibbon.Scale /= 10;
								globalRibbon.ScaleY /= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalBelt globalBelt = (KisekaeGlobalBelt)globalParts.GetSubCode("x", j);
							if (globalBelt != null)
							{
								globalBelt.Rotation /= 10;
								globalBelt.Scale /= 10;
								globalBelt.ScaleY /= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalSpeech globalSpeech = (KisekaeGlobalSpeech)globalParts.GetSubCode("z", j);
							if (globalSpeech != null)
							{
								globalSpeech.Rotation /= 10;
								globalSpeech.ExtraArrowRotation /= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalArm globalArm = (KisekaeGlobalArm)globalParts.GetSubCode("a", j);
							if (globalArm != null)
							{
								globalArm.Rotation /= 10;
								globalArm.Scale /= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalFlag globalFlag = (KisekaeGlobalFlag)globalParts.GetSubCode("y", j);
							if (globalFlag != null)
							{
								globalFlag.Rotation /= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalChair globalChair = (KisekaeGlobalChair)globalParts.GetSubCode("e", j);
							if (globalChair != null)
							{
								globalChair.Rotation /= 10;
								globalChair.Scale /= 10;
							}
						}
					}

					KisekaeScene kisekaeScene = scene.GetComponent<KisekaeScene>();
					if (scene != null)
					{
						for (int j = 0; j < 99; j++)
						{
							KisekaeTextBox textBox = (KisekaeTextBox)kisekaeScene.GetSubCode("u", j);
							if (textBox != null)
							{
								textBox.Rotation /= 10;
							}
						}

						for (int j = 0; j < 99; j++)
						{
							KisekaeGlobalImage globalImage = (KisekaeGlobalImage)kisekaeScene.GetSubCode("v", j);
							if (globalImage != null)
							{
								globalImage.Rotation /= 10;
								globalImage.Scale /= 10;
								globalImage.ScaleY /= 10;
							}
						}
					}
				}

				KisekaeModInfo sceneModInfo = scene.GetComponent<KisekaeModInfo>();
				if (sceneModInfo != null)
				{
					sceneModInfo.Version.MinorVersion = minorVersion;
					sceneModInfo.Version.AlphaVersion = alphaVersion;
				}
				else
				{
					scene.MergeIn(new KisekaeChunk("fv" + minorVersion + "." + alphaVersion), false, false);
				}
			}


			Version = majorVersion.ToString();
			MinorVersion = minorVersion;
			AlphaVersion = alphaVersion;
		}



	}
}
