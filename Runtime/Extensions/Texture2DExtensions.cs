/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="Texture2D"/> class.
    /// </summary>
    public static class Texture2DExtensions {
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a watermark to another texture in the bottom-right-hand corner.
        /// </summary>
        /// <param name="mainTexture">The texture that you want to add the watermark to.</param>
        /// <param name="watermark">The watermark that you want to add to the main texture.</param>
        /// <param name="offset">The offset from the bottom-right of the main texture.</param>
        /// <returns>A texture with the watermark applied.</returns>
        public static Texture2D AddWatermark(this Texture2D mainTexture, Texture2D watermark, int offset = 0) {
            if(watermark == null) return mainTexture;
            if(!watermark.isReadable) {
                Debug.LogError("The watermark must be readable!");
                return mainTexture;
            }
            // Create a new writable texture.
            var result = new Texture2D(mainTexture.width, mainTexture.height);

            // Draw watermark at bottom right corner.
            var startX = mainTexture.width - watermark.width - offset;
            var endY = watermark.height + offset;

            for (var x = 0; x < mainTexture.width; x++) {
                for (var y = 0; y < mainTexture.height; y++) {
                    var bgColor = mainTexture.GetPixel(x, y);
                    var wmColor = new Color(0, 0, 0, 0);

                    // Change this test if no longer drawing at the bottom right corner.
                    if (x >= startX && y <= endY &&y>=offset)
                        wmColor = watermark.GetPixel(x-startX, y-offset);

                    switch(wmColor.a) {
                        case 0:
                            result.SetPixel(x, y, bgColor);
                            break;
                        case 1:
                            result.SetPixel(x, y, wmColor);
                            break;
                        default:
                            var blended = bgColor * (1.0f - wmColor.a) + wmColor;
                            blended.a = 1.0f;
                            result.SetPixel(x, y, blended);
                            break;
                    }
                }
            }
            result.Apply();
            return result;
        }
        
        // Scale a Texture2D using a single scale value while maintaining the aspect ratio
        public static Texture2D ScaleTexture(this Texture2D sourceTexture, float scale) {
            int newWidth = Mathf.RoundToInt(sourceTexture.width * scale);
            int newHeight = Mathf.RoundToInt(sourceTexture.height * scale);

            return ScaleTexture(sourceTexture, newWidth, newHeight);
        }

        // Scale a Texture2D using bilinear filtering
        public static Texture2D ScaleTexture(this Texture2D sourceTexture, int newWidth, int newHeight) {
            // Create a new texture with the desired width and height
            Texture2D scaledTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
            // Calculate the ratio of the source texture's width and height to the new width and height
            float ratioX = (float)sourceTexture.width / newWidth;
            float ratioY = (float)sourceTexture.height / newHeight;
            // Loop through each pixel in the new texture and sample the source texture using bilinear filtering
            for (int y = 0; y < newHeight; y++) {
                for (int x = 0; x < newWidth; x++) {
                    // Calculate the coordinates in the source texture based on the current pixel in the new texture
                    float u = x * ratioX;
                    float v = y * ratioY;
                    // Perform the bilinear filtering by sampling the four nearest pixels in the source texture
                    Color color = BilinearSample(sourceTexture, u, v);
                    scaledTexture.SetPixel(x, y, color);
                }
            }
            // Apply the changes to the texture
            scaledTexture.Apply();
            return scaledTexture;
        }

        public static Texture2D ReplaceColor(this Texture2D texture, Color targetColor, Color replacementColor, float threshold) {
            var result = new Texture2D(texture.width, texture.height);

            for (int x = 0; x < texture.width; x++) {
                for (int y = 0; y < texture.height; y++) {
                    Color pixelColor = texture.GetPixel(x, y);

                    // Compare the pixel color to the target color with a threshold
                    if (pixelColor.ColorSimilar(targetColor, threshold)) {
                        Color modifiedColor = pixelColor.ModifyColor(replacementColor);
                        result.SetPixel(x, y, modifiedColor);
                    } else {
                        result.SetPixel(x, y, pixelColor);
                    }
                }
            }

            result.Apply();
            return result;
        }
        
        // Perform bilinear filtering by sampling the four nearest pixels in the source texture
        public static Color BilinearSample(Texture2D texture, float u, float v) {
            // Calculate the pixel coordinates of the four nearest pixels
            int x1 = Mathf.FloorToInt(u);
            int y1 = Mathf.FloorToInt(v);
            int x2 = Mathf.CeilToInt(u);
            int y2 = Mathf.CeilToInt(v);
            // Calculate the blend factors based on the decimal parts of u and v
            float blendX = u - x1;
            float blendY = v - y1;
            // Sample the four nearest pixels in the source texture using GetPixelBilinear
            Color color1 = texture.GetPixelBilinear((float)x1 / texture.width, (float)y1 / texture.height);
            Color color2 = texture.GetPixelBilinear((float)x2 / texture.width, (float)y1 / texture.height);
            Color color3 = texture.GetPixelBilinear((float)x1 / texture.width, (float)y2 / texture.height);
            Color color4 = texture.GetPixelBilinear((float)x2 / texture.width, (float)y2 / texture.height);
            // Perform the bilinear blending
            Color finalColor = color1 * (1 - blendX) * (1 - blendY) +
                               color2 * blendX * (1 - blendY) +
                               color3 * (1 - blendX) * blendY +
                               color4 * blendX * blendY;
            return finalColor;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}