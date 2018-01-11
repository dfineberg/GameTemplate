using UnityEngine;

namespace GameTemplate
{
    public static class ScreenshotUtility {

        public static Texture2D ScreenshotToTexture(Rect screenRect)
        {
            var tex = new Texture2D((int)screenRect.width, (int)screenRect.height, TextureFormat.RGB24, false);

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
