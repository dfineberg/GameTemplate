using UnityEngine;

namespace GameTemplate
{
    public static class Texture2DExtensions
    {
        public static Sprite ToSprite(this Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}
