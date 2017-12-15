using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace LSXUtility
{
    public static class Utility
    {
        /* Get the path from child to root 
         * params :
         *      child : The child GameObject
         * return : 
         *      string : The path from child to root (child => parent => parent => .... => root)
         */
        public static string GetChildToRootPath(GameObject child)
        {
            StringBuilder path = new StringBuilder();
            path.Append(child.name);
            var parent = child.transform.parent;
            while (parent)
            {
                path.Append("=>" + parent.name);
                parent = parent.transform.parent;
            }
            return path.ToString();
        }

        /* Load textures from specified path 
         * params :
         *      path : The specified path
         * return : 
         *      Texture2D[] : The array of texture in specified path
         */
        public static Texture2D[] GetTexturesInFolder(string path)
        {
            if (0 == path.Length)
            {
                Debug.LogError("The path is null");
                return null;
            }
            var texturePaths = SystemUtility.GetTexturesPaths(path);

            if (texturePaths.Length != 0)
            {
                Texture2D[] result = new Texture2D[texturePaths.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = AssetDatabase.LoadAssetAtPath(texturePaths[i], typeof(Texture2D)) as Texture2D;
                }
                return result;
            }

            return null;
        }

        /* Rotate a given texture 90 in degree
         * params :
         *      src : The source texture
         *      isCW : Whether the rotating is clockwise
         * return : 
         *      Texture2D : The rotated texture
         */
        public static Texture2D RotateTexture(Texture2D src, bool isCW = true)
        {
            if (null == src)
            {
                Debug.LogError("The Texture you want to deal with is null");
                return null;
            }
            int width = src.width; ;
            int height = src.height; ;
            Debug.Log(string.Format("{0} {1}", width, height));
            Texture2D result = new Texture2D(width, height, src.format, false);
            result.wrapMode = TextureWrapMode.Clamp;
            Color[] pixels = src.GetPixels();
            Color[] newPixels = new Color[width * height];

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    int _x, _z;
                    if (isCW)
                    {
                        _x = width - z - 1;
                        _z = x;
                    }
                    else
                    {
                        _x = z;
                        _z = height - x - 1;
                    }
                    newPixels[x + z * width] = pixels[_x + _z * width];
                }
            }
            result.SetPixels(newPixels);
            result.Apply();

            return result;
        }

        /* Scale a given texture
         * params :
         *      src : The source texture
         *      width : The scaled width
         *      height : The scaled height
         * return : 
         *      Texture2D : The scaled texture
         */
        public static Texture2D ScaleTexture(Texture2D src, int width, int height)
        {
            if (null == src)
            {
                Debug.LogError("The Texture you want to deal with is null");
                return null;
            }
            Texture2D result = new Texture2D(width, height, src.format, false);
            Color[] colors = new Color[width * height];

            float pU = 1f / (width - 1);
            float pV = 1f / (height - 1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors[x + y * width] = GetInterpolatedColor(src.GetPixels(), src.width, src.height, x * pU, y * pV);
                }
            }
            result.SetPixels(colors);
            result.Apply();
            return result;
        }

        public static Color GetInterpolatedColor(Color[] colors, int width, int height, float u, float v)
        {


            // 0-15 --> 0-31; 0 == 0, 31 = 15
            u = Mathf.Clamp01(u);
            v = Mathf.Clamp01(v);
            float x = u * (width - 1);
            float y = v * (height - 1);
            int iX0 = (int)x;
            int iY0 = (int)y;
            int iX1 = Mathf.CeilToInt(x);
            int iY1 = Mathf.CeilToInt(y);
            float wX = x - iX0;
            float wY = y - iY0;
            Color color00, color01, color10, color11;
            int i00, i01, i10, i11;
            i00 = iX0 + width * iY0;
            i01 = iX1 + width * iY0;
            i10 = iX0 + width * iY1;
            i11 = iX1 + width * iY1;
            color00 = colors[i00];
            color01 = colors[i01];
            color10 = colors[i10];
            color11 = colors[i11];

            Color xColor_0 = Color.Lerp(color00, color01, wX);
            Color xColor_1 = Color.Lerp(color10, color11, wX);
            return Color.Lerp(xColor_0, xColor_1, wY);
        }

        /* Remove a given texture's alpha channel
         * params :
         *      src : The source texture
         * return : 
         *      Texture2D : The output texture without alpha channel
         */
        public static Texture2D RemoveAlpha(Texture2D src)
        {
            if (null == src)
            {
                Debug.LogError("The Texture you want to deal with is null");
                return null;
            }
            Texture2D result = new Texture2D(src.width, src.height, TextureFormat.RGB24, false);
            result.SetPixels(src.GetPixels());
            result.Apply();
            return result;
        }
    }


}
