// The MIT License (MIT)
// Copyright (c) 2017 David Evans @phosphoer
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#if UNITY_EDITOR

namespace BuilderTool
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    [CreateAssetMenu(fileName = "new-build-definition", menuName = "Build Definition")]
    public class BuildDefinition : ScriptableObject
    {
        public string LocationPathName = "Relative/Path/Build";
        public string ProductNameOverride;
        public string CompanyNameOverride;
        public BuildTarget BuildTarget;
        public BuildTargetGroup BuildTargetGroup;
        public string[] Defines;
        public SceneField[] Scenes;
        public bool WriteBuildInfo = true;
        public string BuildInfoFile = "BuildInfo.cs";
        public string BuildName = "Main";

        private const string kBuildVersionText = "public static class BuildInfo {{ public static string Name = \"{0}\"; public static string Date = \"{1}\"; }}";

        [ContextMenu("Build")]
        public void Build()
        {
            BuildPipeline.BuildPlayer(ApplyBuildSettings());
        }


        [ContextMenu("Copy Scenes From Build Settings")]
        public void CopySceneListFromBuild()
        {
            Scenes = new SceneField[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                var sceneBuildSetting = EditorBuildSettings.scenes[i];
                SceneField sceneField = AssetDatabase.LoadAssetAtPath(sceneBuildSetting.path, typeof(SceneField)) as object as SceneField;
                Scenes[i] = new SceneField();
                Scenes[i].sceneAsset = sceneField.sceneAsset;
            }
        }

        [ContextMenu("Apply Build Settings")]
        public BuildPlayerOptions ApplyBuildSettings()
        {
            if (WriteBuildInfo)
            {
                string pathToBuildAsset = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
                string versionFileText = string.Format(kBuildVersionText, BuildName, System.DateTime.Now.ToString());
                System.IO.File.WriteAllText(System.IO.Path.Combine(pathToBuildAsset, BuildInfoFile), versionFileText);
                AssetDatabase.Refresh();
            }

            string defineList = string.Join(";", Defines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup, defineList);

            // Parse Path
            Regex regex = new Regex(@"\..+$");
            string pathName = regex.Replace(LocationPathName, "");
            switch (BuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    pathName += ".exe";
                    break;
            }

            // Set basic options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions();
            buildOptions.targetGroup = BuildTargetGroup;
            buildOptions.target = BuildTarget;
            buildOptions.locationPathName = pathName;

            if (!string.IsNullOrEmpty(ProductNameOverride)) PlayerSettings.productName = ProductNameOverride;
            if (!string.IsNullOrEmpty(CompanyNameOverride)) PlayerSettings.companyName = CompanyNameOverride;

            // Build scene list
            List<string> sceneList = new List<string>();
            List<EditorBuildSettingsScene> editorScenes = new List<EditorBuildSettingsScene>();
            foreach (SceneField scene in Scenes)
            {
                var sceneAsset = AssetDatabase.GetAssetPath(scene.sceneAsset);
                sceneList.Add(sceneAsset);
                editorScenes.Add(new EditorBuildSettingsScene(sceneAsset, true));
            }

            buildOptions.scenes = sceneList.ToArray();
            EditorBuildSettings.scenes = editorScenes.ToArray();

            return buildOptions;
        }
    }

}

#endif