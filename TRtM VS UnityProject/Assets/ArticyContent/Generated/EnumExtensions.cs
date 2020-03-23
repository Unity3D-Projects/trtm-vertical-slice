namespace Articy.The_Road_To_Moscow
{
	public static class EnumExtensionMethods
	{
		public static string GetDisplayName(this Image aImage)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("Image").GetEnumValue(((int)(aImage))).DisplayName;
		}

		public static string GetDisplayName(this SFX aSFX)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("SFX").GetEnumValue(((int)(aSFX))).DisplayName;
		}

		public static string GetDisplayName(this Music aMusic)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("Music").GetEnumValue(((int)(aMusic))).DisplayName;
		}

		public static string GetDisplayName(this Ambience aAmbience)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("Ambience").GetEnumValue(((int)(aAmbience))).DisplayName;
		}

		public static string GetDisplayName(this TextPosdition aTextPosdition)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("TextPosdition").GetEnumValue(((int)(aTextPosdition))).DisplayName;
		}

		public static string GetDisplayName(this ShapeType aShapeType)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("ShapeType").GetEnumValue(((int)(aShapeType))).DisplayName;
		}

		public static string GetDisplayName(this SelectabilityModes aSelectabilityModes)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("SelectabilityModes").GetEnumValue(((int)(aSelectabilityModes))).DisplayName;
		}

		public static string GetDisplayName(this VisibilityModes aVisibilityModes)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("VisibilityModes").GetEnumValue(((int)(aVisibilityModes))).DisplayName;
		}

		public static string GetDisplayName(this OutlineStyle aOutlineStyle)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("OutlineStyle").GetEnumValue(((int)(aOutlineStyle))).DisplayName;
		}

		public static string GetDisplayName(this PathCaps aPathCaps)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("PathCaps").GetEnumValue(((int)(aPathCaps))).DisplayName;
		}

		public static string GetDisplayName(this LocationAnchorSize aLocationAnchorSize)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("LocationAnchorSize").GetEnumValue(((int)(aLocationAnchorSize))).DisplayName;
		}

	}
}

