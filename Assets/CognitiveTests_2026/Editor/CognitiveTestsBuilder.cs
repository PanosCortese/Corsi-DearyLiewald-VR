// =====================================================================================
//  CognitiveTestsBuilder.cs   (Editor only)
// -------------------------------------------------------------------------------------
//  Purpose   : One-click / batch-mode builds of every Corsi VR and Deary-Liewald VR variant:
//                Windows (.exe):  <Test>_Varjo, <Test>_ProEye, <Test>_OpenXR_PC
//                Android (.apk):  <Test>_OpenXR_Android (universal Quest + Focus Vision),
//                                 <Test>_FocusVision    (HTC VIVE Focus Vision ET + pupil),
//                                 <Test>_QuestPro       (Meta Quest Pro ET)
//              For each build it sets: the single scene, the XR loader for the target
//              platform (Varjo loader OR OpenXR loader, never both), the variant-specific
//              OpenXR feature set & AndroidManifest entries, product name and (for Android)
//              a distinct package id so all APKs can be installed side by side.
//
//  Output    : <UNITY_VR_PROJECTS>/Builds_2026/<Test>/<Variant>/<Test>_<Variant>.exe|.apk
//              (folder can be overridden with -ctOut <path>)
//
//  Batch mode (Editor must be closed):
//    Windows : Unity.exe -batchmode -quit -projectPath <p> -buildTarget Win64
//                        -executeMethod CognitiveTestsBuilder.BuildAllWindows
//    Android : Unity.exe -batchmode -quit -projectPath <p> -buildTarget Android
//                        -executeMethod CognitiveTestsBuilder.BuildAllAndroid
//    ET APKs : Unity.exe -batchmode -quit -projectPath <p> -buildTarget Android
//                        -executeMethod CognitiveTestsBuilder.BuildEyeTrackingAndroid
//  Menu      : PsyNet Lab > Cognitive Tests > Build ...
//  Layer     : Editor tooling. This project is dedicated to these builds, so changing
//              Player/XR settings here does not affect ACG_Final - 2025.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created (6 Windows .exe + 2 universal Android .apk).
//    2026-10-29  v1.1  Added FocusVision (VIVE OpenXR ET + pupil) and QuestPro (Meta ET) APKs.
// =====================================================================================

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEditor.XR.OpenXR.Features;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;
using UnityEngine.XR.OpenXR.Features.MetaQuestSupport;

public static class CognitiveTestsBuilder
{
    private const string Company = "PanagiotisKourtesis";
    private const string VarjoLoader = "Varjo.XR.VarjoLoader";
    private const string OpenXRLoader = "UnityEngine.XR.OpenXR.OpenXRLoader";

    /// <summary>Tracks which Android variant is currently being built so the manifest post-processor
    /// can inject the exact permissions and hardware features required for that headset.</summary>
    internal static HardwareVariant CurrentAndroidVariant = HardwareVariant.OpenXR_Android;

    private static string OutputRoot
    {
        get
        {
            string[] args = System.Environment.GetCommandLineArgs();
            int i = System.Array.IndexOf(args, "-ctOut");
            if (i >= 0 && i + 1 < args.Length) return args[i + 1];
            // <project>/../Builds_2026
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "Builds_2026"));
        }
    }

    [MenuItem("PsyNet Lab/Cognitive Tests/Build all Windows variants (6 exe)")]
    public static void BuildAllWindows()
    {
        var results = new List<string>();
        bool ok = true;
        foreach (CognitiveTest test in System.Enum.GetValues(typeof(CognitiveTest)))
            foreach (var v in new[] { HardwareVariant.Varjo, HardwareVariant.ProEye, HardwareVariant.OpenXR_PC })
                ok &= Build(test, v, results);
        Finish(ok, results);
    }

    [MenuItem("PsyNet Lab/Cognitive Tests/Build all Android variants (6 apk)")]
    public static void BuildAllAndroid()
    {
        var results = new List<string>();
        bool ok = true;
        foreach (CognitiveTest test in System.Enum.GetValues(typeof(CognitiveTest)))
            foreach (var v in new[] { HardwareVariant.OpenXR_Android, HardwareVariant.FocusVision, HardwareVariant.QuestPro })
                ok &= Build(test, v, results);
        Finish(ok, results);
    }

    [MenuItem("PsyNet Lab/Cognitive Tests/Build Eye-Tracking Android variants (4 apk: FocusVision + QuestPro)")]
    public static void BuildEyeTrackingAndroid()
    {
        var results = new List<string>();
        bool ok = true;
        foreach (CognitiveTest test in System.Enum.GetValues(typeof(CognitiveTest)))
            foreach (var v in new[] { HardwareVariant.FocusVision, HardwareVariant.QuestPro })
                ok &= Build(test, v, results);
        Finish(ok, results);
    }

    private static void Finish(bool ok, List<string> results)
    {
        Debug.Log("[Builder] ================= SUMMARY =================");
        foreach (var r in results) Debug.Log("[Builder] " + r);
        if (Application.isBatchMode) EditorApplication.Exit(ok ? 0 : 1);
    }

    public static bool Build(CognitiveTest test, HardwareVariant variant, List<string> results)
    {
        string testName = test == CognitiveTest.Corsi ? "CorsiVR" : "DearyLiewaldVR";
        string scene = CognitiveTestsSceneBuilder.ScenePath(test, variant);
        if (!File.Exists(scene))
        {
            results.Add($"FAILED {testName}_{variant}: scene missing ({scene}) - run 'Generate Variant Scenes' first");
            return false;
        }

        bool android = variant == HardwareVariant.OpenXR_Android ||
                       variant == HardwareVariant.FocusVision ||
                       variant == HardwareVariant.QuestPro;
        BuildTarget target = android ? BuildTarget.Android : BuildTarget.StandaloneWindows64;
        BuildTargetGroup group = android ? BuildTargetGroup.Android : BuildTargetGroup.Standalone;
        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
        }

        // ---- XR loader: exactly one loader per build
        ConfigureLoader(group, variant == HardwareVariant.Varjo ? VarjoLoader : OpenXRLoader);

        // ---- Player settings (dedicated project -> safe to set per build)
        PlayerSettings.companyName = Company;
        PlayerSettings.productName = $"{testName}_{variant}";
        if (android)
        {
            CurrentAndroidVariant = variant;
            ConfigureAndroid(testName, variant);
        }

        string dir = Path.Combine(OutputRoot, testName, variant.ToString());
        Directory.CreateDirectory(dir);
        string file = Path.Combine(dir, $"{testName}_{variant}" + (android ? ".apk" : ".exe"));

        var options = new BuildPlayerOptions
        {
            scenes = new[] { scene },
            locationPathName = file,
            target = target,
            targetGroup = group,
            options = BuildOptions.None
        };
        Debug.Log($"[Builder] Building {testName}_{variant} -> {file}");
        BuildReport report = BuildPipeline.BuildPlayer(options);
        bool ok = report.summary.result == BuildResult.Succeeded;
        results.Add($"{(ok ? "OK    " : "FAILED")} {testName}_{variant}: {report.summary.result}, " +
                    $"{report.summary.totalErrors} errors, {report.summary.totalSize / (1024 * 1024)} MB, {report.summary.totalTime:mm\\:ss} -> {file}");
        return ok;
    }

    /// <summary>Leaves exactly one XR loader active for the build target group.</summary>
    private static void ConfigureLoader(BuildTargetGroup group, string loaderTypeName)
    {
        XRGeneralSettings settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(group);
        if (settings == null)
        {
            var perTarget = AssetDatabase.FindAssets("t:XRGeneralSettingsPerBuildTarget")
                .Select(g => AssetDatabase.LoadAssetAtPath<XRGeneralSettingsPerBuildTarget>(AssetDatabase.GUIDToAssetPath(g)))
                .FirstOrDefault();
            if (perTarget == null) throw new System.Exception("XRGeneralSettingsPerBuildTarget asset not found (Assets/XR).");
            settings = ScriptableObject.CreateInstance<XRGeneralSettings>();
            var manager = ScriptableObject.CreateInstance<XRManagerSettings>();
            settings.Manager = manager;
            AssetDatabase.AddObjectToAsset(settings, perTarget);
            AssetDatabase.AddObjectToAsset(manager, perTarget);
            perTarget.SetSettingsForBuildTarget(group, settings);
        }
        settings.InitManagerOnStart = true;
        XRManagerSettings mgr = settings.AssignedSettings;

        foreach (var l in mgr.activeLoaders.ToList())
        {
            string typeName = l.GetType().FullName;
            if (typeName != loaderTypeName)
            {
                bool removed = XRPackageMetadataStore.RemoveLoader(mgr, typeName, group);
                Debug.Log($"[Builder] {group}: removed loader {typeName} ({removed})");
            }
        }
        if (!mgr.activeLoaders.Any(l => l.GetType().FullName == loaderTypeName))
        {
            bool assigned = XRPackageMetadataStore.AssignLoader(mgr, loaderTypeName, group);
            Debug.Log($"[Builder] {group}: assigned loader {loaderTypeName} ({assigned})");
        }
        EditorUtility.SetDirty(mgr);
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        Debug.Log($"[Builder] {group}: active loaders = " + string.Join(", ", mgr.activeLoaders.Select(l => l.GetType().Name)));
    }

    // ---------------------------------------------------------------------------------
    // ANDROID OPENXR FEATURE SETS PER VARIANT
    // ---------------------------------------------------------------------------------

    // 1) Universal Android OpenXR (runs on both Meta Quest & VIVE Focus Vision without vendor lock-in)
    private static readonly HashSet<string> UniversalAndroidFeaturesOn = new HashSet<string>
    {
        "MetaQuestFeature",               // Meta Quest Support
        "KHRSimpleControllerProfile",     // Khronos Simple Controller
        "OculusTouchControllerProfile",   // Quest 2/3/Pro controllers
        "HandInteractionProfile",         // XR_EXT_hand_interaction
        "HandCommonPosesInteraction",     // Hand Interaction Poses
        "PalmPoseInteraction",            // Palm Pose
        "HandTracking",                   // XR Hands subsystem (XR_EXT_hand_tracking)
        "MetaHandTrackingAim",            // Meta Hand Tracking Aim
        "EyeGazeInteraction",             // optional OpenXR eye gaze
    };

    // 2) HTC VIVE Focus Vision Eye-Tracking + Pupillometry APK (VIVE OpenXR Plugin)
    private static readonly HashSet<string> FocusVisionAndroidFeaturesOn = new HashSet<string>
    {
        "VIVEFocus3Feature",              // VIVE XR Support (manifest + runtime hooks)
        "ViveEnterpriseCommand",          // VIVE XR Enterprise Command (paired with VIVEFocus3Feature)
        "ViveEyeTracker",                 // VIVE XR Eye Tracker (XR_HTC_eye_tracker: gaze + pupillometry in mm)
        "EyeGazeInteraction",             // Eye Gaze Interaction Profile (XR_EXT_eye_gaze_interaction)
        "VIVEFocus3Profile",              // VIVE Focus 3 / Focus Vision Controller Support
        "KHRSimpleControllerProfile",     // Khronos Simple Controller fallback
        "HandInteractionProfile",         // XR_EXT_hand_interaction
        "HandCommonPosesInteraction",     // Hand Interaction Poses
        "PalmPoseInteraction",            // Palm Pose
        "HandTracking",                   // XR Hands subsystem (XR_EXT_hand_tracking)
    };

    // 3) Meta Quest Pro Eye-Tracking APK (Meta Quest OpenXR + Eye Gaze + Quest Pro controller profile)
    private static readonly HashSet<string> QuestProAndroidFeaturesOn = new HashSet<string>
    {
        "MetaQuestFeature",                   // Meta Quest Support (with cambria / Quest Pro enabled)
        "EyeGazeInteraction",                 // Eye Gaze Interaction Profile (XR_EXT_eye_gaze_interaction)
        "MetaQuestTouchProControllerProfile", // Meta Quest Touch Pro Controller Profile
        "OculusTouchControllerProfile",       // Oculus Touch Controller Profile
        "KHRSimpleControllerProfile",         // Khronos Simple Controller fallback
        "HandInteractionProfile",             // XR_EXT_hand_interaction
        "HandCommonPosesInteraction",         // Hand Interaction Poses
        "PalmPoseInteraction",                // Palm Pose
        "HandTracking",                       // XR Hands subsystem (XR_EXT_hand_tracking)
        "MetaHandTrackingAim",                // Meta Hand Tracking Aim
    };

    private static void ConfigureAndroidOpenXRFeatures(HardwareVariant variant)
    {
        // Discover any newly installed OpenXR features (e.g. VIVE OpenXR Plugin) and ensure
        // their ScriptableObject instances exist in OpenXR Package Settings.asset.
        FeatureHelpers.RefreshFeatures(BuildTargetGroup.Android);

        OpenXRSettings settings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
        if (settings == null) throw new System.Exception("No OpenXR settings for Android (Assets/XR/Settings).");

        HashSet<string> wanted = variant == HardwareVariant.FocusVision
            ? FocusVisionAndroidFeaturesOn
            : (variant == HardwareVariant.QuestPro ? QuestProAndroidFeaturesOn : UniversalAndroidFeaturesOn);

        foreach (OpenXRFeature f in settings.GetFeatures())
        {
            if (f == null) continue;
            bool on = wanted.Contains(f.GetType().Name);
            if (f.enabled != on) f.enabled = on;

            if (on && f is MetaQuestFeature mq)
            {
                mq.OnEnable();
            }

            EditorUtility.SetDirty(f);
        }
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        Debug.Log($"[Builder] Android ({variant}) OpenXR features ON: " +
                  string.Join(", ", settings.GetFeatures().Where(f => f != null && f.enabled).Select(f => f.GetType().Name)));
    }

    private static void ConfigureAndroid(string testName, HardwareVariant variant)
    {
        EnsureSpaceFreeAndroidSdk();
        ConfigureAndroidOpenXRFeatures(variant);

        string suffix = variant == HardwareVariant.FocusVision
            ? ".focusvision"
            : (variant == HardwareVariant.QuestPro ? ".questpro" : "");
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, $"com.{Company}.{testName}{suffix}");
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
        PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)30;
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { UnityEngine.Rendering.GraphicsDeviceType.Vulkan });
        PlayerSettings.Android.forceSDCardPermission = false;
        PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
        EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;
        EditorUserBuildSettings.buildAppBundle = false;
        ConfigureJdk8Keystore();
    }

    private static void ConfigureJdk8Keystore()
    {
        string ksPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "debug_jdk8.keystore"));
        if (!File.Exists(ksPath))
        {
            string keytool = Path.Combine(EditorApplication.applicationContentsPath,
                "PlaybackEngines", "AndroidPlayer", "OpenJDK", "bin", "keytool.exe");
            if (File.Exists(keytool))
            {
                var psi = new System.Diagnostics.ProcessStartInfo(keytool,
                    $"-genkeypair -keystore \"{ksPath}\" -storetype JKS -storepass android " +
                    "-alias androiddebugkey -keypass android -keyalg RSA -keysize 2048 " +
                    "-validity 10000 -dname \"CN=Android Debug,O=Android,C=US\"")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                using (var p = System.Diagnostics.Process.Start(psi)) p?.WaitForExit(10000);
            }
        }
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = ksPath;
        PlayerSettings.Android.keystorePass = "android";
        PlayerSettings.Android.keyaliasName = "androiddebugkey";
        PlayerSettings.Android.keyaliasPass = "android";
    }

    private static void EnsureSpaceFreeAndroidSdk()
    {
        string currentSdk = UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath;
        if (string.IsNullOrEmpty(currentSdk) || !currentSdk.Contains(" ")) return;

        string junctionRoot = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Library", "AndroidPlayerNoSpace"));
        if (junctionRoot.Contains(" "))
            junctionRoot = @"C:\Users\panag\.gemini\antigravity\brain\4527c08c-467c-4dd2-a550-fe64b4e6204d\scratch\UnityAndroidPlayer";

        string targetPlayer = Path.Combine(EditorApplication.applicationContentsPath, "PlaybackEngines", "AndroidPlayer");
        if (!Directory.Exists(junctionRoot) && Directory.Exists(targetPlayer))
        {
            var psi = new System.Diagnostics.ProcessStartInfo("cmd.exe", $"/c mklink /J \"{junctionRoot}\" \"{targetPlayer}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            using (var p = System.Diagnostics.Process.Start(psi)) p?.WaitForExit(5000);
        }

        string sdkNoSpace = Path.Combine(junctionRoot, "SDK");
        if (Directory.Exists(sdkNoSpace))
        {
            UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath = sdkNoSpace;
            Debug.Log($"[Builder] Redirected Android SDK root to space-free junction: {sdkNoSpace}");
        }
    }
}

/// <summary>
/// Post-processes the generated Gradle AndroidManifest.xml so that:
///   * QuestPro builds include com.oculus.permission.EYE_TRACKING and oculus.software.eye_tracking (required=true),
///   * FocusVision builds include wave.feature.eyetracking (required=true) and wave.feature.handtracking (required=true)
///     with no leftover Oculus tags,
///   * Universal OpenXR_Android builds do not mark vendor-specific eye tracking as required.
/// Also removes the generated AndroidManifest.xml after the build completes so incremental Bee/Gradle builds
/// always start from a clean template when switching variants.
/// </summary>
internal class AndroidEyeTrackingManifestProcessor : IPostGenerateGradleAndroidProject, IPostprocessBuildWithReport
{
    private const string AndroidNs = "http://schemas.android.com/apk/res/android";
    private static string lastManifestPath;

    public int callbackOrder => 100;

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        string manifestPath = Path.Combine(path, "src", "main", "AndroidManifest.xml");
        lastManifestPath = manifestPath;
        if (!File.Exists(manifestPath)) return;

        var doc = new XmlDocument();
        using (var reader = new XmlTextReader(manifestPath))
        {
            reader.Read();
            doc.Load(reader);
        }

        XmlElement manifest = doc.SelectSingleNode("/manifest") as XmlElement;
        XmlElement application = doc.SelectSingleNode("/manifest/application") as XmlElement;
        if (manifest == null) return;

        HardwareVariant v = CognitiveTestsBuilder.CurrentAndroidVariant;
        switch (v)
        {
            case HardwareVariant.QuestPro:
                RemoveElementsByName(manifest, "uses-feature", "wave.feature.eyetracking");
                RemoveElementsByName(manifest, "uses-feature", "wave.feature.handtracking");
                RemoveElementsByName(manifest, "uses-feature", "wave.feature.tracker");
                EnsureElement(doc, manifest, "uses-permission", "com.oculus.permission.EYE_TRACKING");
                EnsureElement(doc, manifest, "uses-feature", "oculus.software.eye_tracking", ("required", "true"));
                EnsureElement(doc, manifest, "uses-feature", "oculus.software.handtracking", ("required", "false"));
                EnsureElement(doc, manifest, "uses-permission", "com.oculus.permission.HAND_TRACKING");
                Debug.Log("[ManifestProcessor] Applied QuestPro eye-tracking + hand-tracking manifest entries.");
                break;

            case HardwareVariant.FocusVision:
                RemoveElementsByName(manifest, "uses-permission", "com.oculus.permission.EYE_TRACKING");
                RemoveElementsByName(manifest, "uses-feature", "oculus.software.eye_tracking");
                EnsureElement(doc, manifest, "uses-permission", "org.khronos.openxr.permission.OPENXR");
                EnsureElement(doc, manifest, "uses-permission", "org.khronos.openxr.permission.OPENXR_SYSTEM");
                EnsureElement(doc, manifest, "uses-feature", "wave.feature.eyetracking", ("required", "true"));
                EnsureElement(doc, manifest, "uses-feature", "wave.feature.handtracking", ("required", "true"));
                Debug.Log("[ManifestProcessor] Applied FocusVision (VIVE OpenXR) eye-tracking + hand-tracking manifest entries.");
                break;

            default:
                RemoveElementsByName(manifest, "uses-feature", "oculus.software.eye_tracking");
                RemoveElementsByName(manifest, "uses-feature", "wave.feature.eyetracking");
                Debug.Log("[ManifestProcessor] Verified universal OpenXR_Android manifest entries.");
                break;
        }

        using (var writer = new XmlTextWriter(manifestPath, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
        }
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.Android && !string.IsNullOrEmpty(lastManifestPath) && File.Exists(lastManifestPath))
        {
            File.Delete(lastManifestPath);
            lastManifestPath = null;
        }
    }

    private static void RemoveElementsByName(XmlElement parent, string tag, string androidName)
    {
        var nodes = parent.SelectNodes(tag);
        if (nodes == null) return;
        var toRemove = new List<XmlNode>();
        foreach (XmlNode n in nodes)
        {
            var attr = n.Attributes?.GetNamedItem("name", AndroidNs);
            if (attr != null && attr.Value == androidName) toRemove.Add(n);
        }
        foreach (var n in toRemove) parent.RemoveChild(n);
    }

    private static void EnsureElement(XmlDocument doc, XmlElement parent, string tag, string androidName, params (string key, string val)[] extraAttrs)
    {
        // Remove duplicates first so repeated calls stay clean.
        RemoveElementsByName(parent, tag, androidName);
        XmlElement el = doc.CreateElement(tag);
        el.SetAttribute("name", AndroidNs, androidName);
        foreach (var (key, val) in extraAttrs)
            el.SetAttribute(key, AndroidNs, val);
        parent.AppendChild(el);
    }
}
