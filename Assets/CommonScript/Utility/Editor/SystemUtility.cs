using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace LSXUtility 
{
    public static class SystemUtility
    {
        /* Remove the ReadOnly attribute of given file
         * params :
         *      filePath : The file path
         * return : 
         *      void
         */
        public static void RemoveFileReadOnly(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return;
            var attribute = System.IO.File.GetAttributes(filePath);
            if ((attribute & System.IO.FileAttributes.ReadOnly) > 0)
                System.IO.File.SetAttributes(filePath, attribute & (~System.IO.FileAttributes.ReadOnly));
        }

        /* Remove the ReadOnly attribute of given directory
         * params :
         *      filePath : The directory path
         * return : 
         *      void
         */
        public static void RemoveFilesReadOnly(string filePath)
        {
            if (!System.IO.Directory.Exists(filePath))
                return;
            var all = System.IO.Directory.GetFiles(filePath, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (var f in all)
                RemoveFileReadOnly(f);
            Debug.Log(string.Format("{0} file(s)", all.Length));
        }

        public static string[] GetTexturesPaths(string path)
        {
            List<string> allTextures = new List<string>();
            string[] allFiles = System.IO.Directory.GetFiles(path, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            for (int i = 0; i < allFiles.Length; i++)
            {
                string p = allFiles[i].ToLower();
                if (p.EndsWith(".tga") || p.EndsWith(".png") || p.EndsWith(".bmp") || p.EndsWith(".jpg") || p.EndsWith(".dds"))
                {
                    allTextures.Add(p.Replace('\\', '/'));
                }
            }
            return allTextures.ToArray();
        }
    }

    
}
