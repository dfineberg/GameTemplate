using UnityEngine;

namespace GameTemplate
{
    public static class ScreenshotUtility
    {
        private static Texture2D previousScreenshotTexture = null;
        private static Vector2Int previousScreenSize;

        private static Texture2D ScreenshotToTexture(Rect screenRect)
        {
            Texture2D tex;

            Vector2Int screenSize = new Vector2Int((int)screenRect.width, (int)screenRect.height);

            if (screenSize == previousScreenSize)
            {
                tex = previousScreenshotTexture;
            }
            else
            {
                if (previousScreenshotTexture != null)
                {
                    Object.DestroyImmediate(previousScreenshotTexture);
                }

                tex = new Texture2D(screenSize.x, screenSize.y, TextureFormat.RGB24, false);
                tex.name = "ScreenshotWriteTex";
            
                previousScreenshotTexture = tex;
                previousScreenSize = screenSize;
            }

            tex.ReadPixels(screenRect, 0, 0);
            tex.Apply();

            return tex;
        }

        public static Texture2D ScreenshotToTexture()
        {
            return ScreenshotToTexture(new Rect(0, 0, Screen.width, Screen.height));
        }

        public static byte[] ScreenshotToByteArray(Rect screenRect)
        {
            return ScreenshotToTexture(screenRect).EncodeToPNG();
        }

        public static byte[] ScreenshotToByteArray()
        {
            return ScreenshotToTexture().EncodeToPNG();
        }

        public static Texture2D ByteArrayToTexture(byte[] bytes)
        {
            var tex = new Texture2D(1, 1);
            tex.name = "ScreenshotReadTex";
            tex.LoadImage(bytes);

            return tex;
        }

        public static Sprite ByteArrayToSprite(byte[] bytes)
        {
            var tex = ByteArrayToTexture(bytes);

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1);
        }
    }
}
