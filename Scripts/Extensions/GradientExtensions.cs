using UnityEngine;

public static class GradientExtensions
{
    public static Texture2D GetTexture(this Gradient gradient, int resolution)
    {
        var tex = new Texture2D(resolution, 1, TextureFormat.ARGB32, false)
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
