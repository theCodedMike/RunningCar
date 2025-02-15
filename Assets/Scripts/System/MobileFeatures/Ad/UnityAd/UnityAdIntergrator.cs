using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using File = System.IO.File;

namespace System.MobileFeatures.Ad.UnityAd
{
    public static class UnityAdIntegrator
    {
        private const string FilePathUnityAdModel = "Assets/Scripts/System/MobileFeatures/Ad/UnityAd/UnityAdModel.cs";
        private const string FilePathUnityAdController = "Assets/Scripts/System/MobileFeatures/Ad/UnityAd/UnityAdController.cs";

        private const string TargetUndef = "#undef POPPOP_UNITYADS_INTEGRATED";
        private const string ReplacementDefine = "#define POPPOP_UNITYADS_INTEGRATED";

        public static void Integrate()
        {
            Replace(FilePathUnityAdModel, TargetUndef, ReplacementDefine);
            Replace(FilePathUnityAdController, TargetUndef, ReplacementDefine);
        }

        public static void Disintegrate()
        {
            Replace(FilePathUnityAdModel, ReplacementDefine, TargetUndef);
            Replace(FilePathUnityAdController, ReplacementDefine, TargetUndef);
        }

        public static bool Replace(string filePath, string target, string replacement)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("UnityAdIntegrator: Files not found");
                return false;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(target, replacement);
            }
            File.WriteAllLines(filePath, lines);
            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
            
            Debug.Log("UnityAdIntegrator: finished");
            return true;
        }
    }
}
