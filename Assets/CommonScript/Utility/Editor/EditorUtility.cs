using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LSXUtility
{

    public static class EitorUtility
    {
        static private string GetRelativeAssetsPath(string path)
        {
            return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
        }

    

        static private void FindReferenceAsset(string path, List<string> containExtensions, Action<string> onFind)
        {
            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                    .Where(s => containExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            UInt32 count = 0;
            string guid = AssetDatabase.AssetPathToGUID(path);
            foreach (var file in files)
            {
                if (Regex.IsMatch(File.ReadAllText(file), guid))
                {
                    count++;
                    onFind(file);
                
                }
            }
            Debug.LogWarning(string.Format("There are {0} assets that contain the selected asset", count));
        }

        //引用资源映射表
        static readonly Dictionary<Type, List<string>> ASSET_REFERENCE_DIC = new Dictionary<Type, List<string>> 
        { 
            {typeof(Shader), new List<string>() {".mat"}},
            //{typeof(Material), new List<string>() {".prefab"}},
        };

        [MenuItem("LSX/Find Asset Reference In Certain Type Assets", false, 1)]
        static private void FindAssetInCertainAssets()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(path))
            {
                var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                List<string> containExtensions = new List<string>();
                if (!ASSET_REFERENCE_DIC.TryGetValue(obj.GetType(), out containExtensions))
                {
                    Debug.LogWarning("The type of the selected asset has not reference Asset");
                    return;
                }

                FindReferenceAsset(path, containExtensions, (file) =>
                {
                    Debug.Log(GetRelativeAssetsPath(file));
                });

            }
        }

        [MenuItem("LSX/Find Asset Reference In Prefab", false, 1)]
        static private void FindAssetInPrefab()
        {
             EditorSettings.serializationMode = SerializationMode.ForceText;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(path))
            {
                var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                List<string> containExtensions = new List<string> {".prefab"};
                FindReferenceAsset(path, containExtensions, (file) =>
                {
                    var relativePath = GetRelativeAssetsPath(file);
                    var go = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(relativePath) as GameObject;
                    if (obj as Material)
                    {
                        var renders = go.GetComponentsInChildren<Renderer>();
                        foreach(var render in renders)
                        {
                            var materials = render.sharedMaterials.ToList();
                            if (materials.Contains(obj as Material))
                            {
                                var namePath = Utility.GetChildToRootPath(render.gameObject);
                                Debug.Log(string.Format("In Prefab {0}  <{1}> contain the select asset", relativePath, namePath));
                            }
                            
                        }
                    }
                
                });
            }
        }


        [MenuItem("LSX/Test", false, 1)]
        public static void Test()
        {
            var tex1 = LoadPngJpgAt("/CommandBuffer/Textures/DecalSign.png");
            //var tex2 = AssetDatabase.LoadAssetAtPath("Assets/CommandBuffer/Textures/12.exr", typeof(Texture2D)) as Texture2D;
            var cavas = Selection.activeGameObject;
            if (cavas)
            {
                Debug.LogWarning("!!!!!!!!!!" + cavas.name);
                var rawIamge = cavas.transform.Find("1").GetComponentInChildren<RawImage>();
                if (rawIamge)
                {
                    rawIamge.texture = tex1;
                }

//                 rawIamge = cavas.transform.Find("2").GetComponentInChildren<RawImage>();
//                 if (rawIamge)
//                 {
//                     rawIamge.texture = tex2;
//                 }
            }
            
        }
        public static Texture2D LoadPngJpgAt(string path)
        {

            string fullPath = Application.dataPath + path;
            if (System.IO.File.Exists(fullPath))
            {
                Texture2D result = new Texture2D(2, 2);
                var data = System.IO.File.ReadAllBytes(fullPath);
                result.LoadImage(data);
                return result;
            }
            Debug.Log("Can't Load @" + fullPath);
            return null;
        }

    }
}

