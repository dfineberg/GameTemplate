using UnityEngine;

public static class GradientExtensions
{
    public static Texture2D GetTexture(this Gradient gradient, int resolution)
    {
        TextureFormat format; // prefer more bits per channel to avoid banding
        if (SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
        {
            format = TextureFormat.RGBAFloat;
        }
        else if (SystemInfo.SupportsTextureFormat(TextureFormat.RGBAHalf))
        {
            format = TextureFormat.RGBAHalf;
        }
        else
        {
            format = TextureFormat.ARGB32;
        }

        var tex = new Texture2D(resolution, 1, format, false)
        {
            wrapMode = TextureWrapMode.Clamp
        };

        var colours = new Color[resolution];

        for (var i = 0; i < resolution; i++) 
            colours[i] = gradient.Evaluate((float) i / resolution);
        
        tex.SetPixels(colours);
        tex.Apply();

        return tex;
    }
}
