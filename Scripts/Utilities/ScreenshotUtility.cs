using System;
using GameTemplate.Promises;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace GameTemplate
{
    public static class ScreenshotUtility
    {
        private static RenderTexture screenshotTexture = null;
        private static Vector2Int previousScreenSize;
        private static NativeArray<byte> buffer;

        public static void SaveScreenshot(Action<byte[]> dataWriteCallback)
        {
            Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

            CoroutineExtensions.WaitForEndOfFrame()
                .ThenDo(() =>
                {
                    if (screenSize != previousScreenSize)
                    {
                        if (screenshotTexture != null)
                        {
                            Destroy();
                        }

                        screenshotTexture = new RenderTexture(screenSize.x, screenSize.y, 0, RenderTextureFormat.ARGB32, 0);
                        screenshotTexture.name = "ScreenshotWriteTex";

                        buffer = new NativeArray<byte>(screenSize.x * screenSize.y * 3, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                    }
                    
                    var flipped = RenderTexture.GetTemporary(screenshotTexture.descriptor);
                    ScreenCapture.CaptureScreenshotIntoRenderTexture(flipped);
                    Graphics.Blit(flipped, screenshotTexture, new Vector2(1, -1), new Vector2(0, 1));
                    RenderTexture.ReleaseTemporary(flipped);
                    
                    AsyncGPUReadback.RequestIntoNativeArray(ref buffer, screenshotTexture, 0, TextureFormat.RGB24, (AsyncGPUReadbackRequest request) =>
                    {
                        if (request.hasError)
                        {
                            Debug.LogError("AsyncGPUReadback encountered an error");
                            dataWriteCallback(null);
                            return;
                        }

                        using (NativeArray<byte> encodedBytes = ImageConversion.EncodeNativeArrayToPNG(buffer, GraphicsFormat.R8G8B8_SRGB, (uint)screenSize.x, (uint)screenSize.y))
                        {
                            dataWriteCallback(encodedBytes.ToArray());
                        }
                    }); 
                });
        }

        public static void Destroy()
        {
            if (screenshotTexture != null)
            {
                screenshotTexture.Release();
                UnityEngine.Object.DestroyImmediate(screenshotTexture);
                screenshotTexture = null;
            }
            if (buffer != null && buffer.IsCreated)
            {
                buffer.Dispose();
            }
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
