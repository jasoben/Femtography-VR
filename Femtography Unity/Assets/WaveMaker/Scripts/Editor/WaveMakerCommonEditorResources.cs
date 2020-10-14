using UnityEngine;

namespace WaveMaker
{
    /// <summary>
    /// Common settings for the custom editors
    /// </summary>
    public static class WaveMakerCommonEditorResources
    {
        static GUIStyle AssetNameStyle;
        static GUIStyle TitleStyle;
        static GUIStyle CenteredStyle;
        static GUIStyle WarningStyle;
        static GUIStyle BoxStyle;
        static GUIStyle NoteStyle;

        public static GUIStyle GetAssetNameStyle()
        {
            if (AssetNameStyle == null)
            {
                AssetNameStyle = new GUIStyle();
                AssetNameStyle.alignment = TextAnchor.MiddleCenter;
                AssetNameStyle.fontSize = 15;
                AssetNameStyle.fontStyle = FontStyle.Bold;
            }
            return AssetNameStyle;
        }

        public static GUIStyle GetTitleStyle()
        {
            if (TitleStyle == null)
            {
                TitleStyle = new GUIStyle();
                TitleStyle.alignment = TextAnchor.MiddleLeft;
                TitleStyle.fontSize = 12;
                TitleStyle.fontStyle = FontStyle.Bold;
            }
            return TitleStyle;
        }

        public static GUIStyle GetCenteredStyle()
        {
            if (CenteredStyle == null)
            {
                CenteredStyle = new GUIStyle();
                CenteredStyle.alignment = TextAnchor.MiddleCenter;
            }
            return CenteredStyle;
        }

        public static GUIStyle GetBoxStyle()
        {
            if (BoxStyle == null)
            {
                BoxStyle = new GUIStyle();
                BoxStyle.border = new RectOffset(2, 2, 2, 2);
            }
            return BoxStyle;
        }
              

    }
}
