using System;
using GameTemplate.Promises;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameTemplate
{
    public static class ScreenshotUtility
    {
        private static RenderTexture screenshotTexture = null;
        private static Vector2Int previousScreenSize;

        public static void SaveScreenshot(Action<NativeArray<byte>> dataWriteCallback)
        {
            Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

            CoroutineExtensions.WaitForEndOfFrame()
                .ThenDo(() =>
                {
                    if (screenSize != previousScreenSize)
                    {
                        if (screenshotTexture != null)
                        {
                            screenshotTexture.Release();
                            UnityEngine.Object.DestroyImmediate(screenshotTexture);
                        }

                        screenshotTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGB32, 0);
                        screenshotTexture.name = "ScreenshotWriteTex";
                    }
                    
                    ScreenCapture.CaptureScreenshotIntoRenderTexture(screenshotTexture);
                    AsyncGPUReadback.Request(screenshotTexture, 0, TextureFormat.RGBA32, (AsyncGPUReadbackRequest request) =>
                    {
                        using (NativeArray<byte> imageBytes = request.GetData<byte>())
                        {
                            dataWriteCallback(imageBytes);
                        }
                    }); 
                });
        }

        public static Texture2D ByteArrayToTexture(byte[] bytes)
        {
            var tex = new Texture2D(1, 1);
            tex.name = "ScreenshotReadTex";
            tex.LoadImage(bytes);

            return tex;
        }
    }
}
