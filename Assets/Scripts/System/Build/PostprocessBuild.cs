#if UNITY_EDITOR
#if UNITY_IOS

using System.IO;
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace System.Build
{
    public class PostprocessBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                SettingsCommon settingsCommon = AssetDatabase.LoadAssetAtPath<SettingsCommon>("Assets/ScriptableObjects/Settings/SettingsCommon.asset");
                string plistPath = report.summary.outputPath + "Info.plist";
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
                
                var rootDict = plist.root;
                rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", settingsCommon.gameUseEncryption);

                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }
    }
}

#endif
#endif