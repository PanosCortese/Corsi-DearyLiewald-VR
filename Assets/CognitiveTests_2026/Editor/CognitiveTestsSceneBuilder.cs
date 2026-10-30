// =====================================================================================
//  CognitiveTestsSceneBuilder.cs   (Editor only)
// -------------------------------------------------------------------------------------
//  Purpose   : Generates the 12 variant scenes (2 tests x 6 hardware variants) from the two
//              base scenes copied from ACG_Final - 2025:
//                  Assets/CorsiBlockVR_OpenXR.unity      -> CorsiVR_<Variant>.unity
//                  Assets/DearyLiewaldVR_OpenXR.unity    -> DearyLiewaldVR_<Variant>.unity
//              into Assets/CognitiveTests_2026/Scenes/. The base scenes are opened, modified
//              IN MEMORY and saved under the new name; the base files are never re-saved.
//              Re-running the builder regenerates the scenes from scratch (idempotent).
//
//              What it does to every variant:
//                * removes components whose script is missing (SteamVR leftovers),
//                * removes the old OpenXRGazePointer (it silently fell back to HEAD direction
//                  when no eye tracker existed, producing fake "attention times"),
//                * adds VariantConfig, GazePointerDriver, ParticipantIdKeypad,
//                * sets the CSV column list of Score_Manager (original columns first, new
//                  columns appended),
//                * sets the 'ProEye' (= pupillometry available) flags of the managers.
//              Variant-specific:
//                * ProEye      : adds the SRanipal Eye Framework prefab, enables EyeTracking.cs
//                                and the SRanipal ET_Pointer.cs (original Pro Eye setup).
//                * Varjo       : adds EyeCalibrationGate (Varjo calibration before the task).
//                * FocusVision : enables pupillometry flag ('ProEye') because VIVE OpenXR
//                                XR_HTC_eye_tracker provides pupil diameter in mm in addition
//                                to binocular eye gaze.
//                * QuestPro    : uses OpenXR eye gaze (XR_EXT_eye_gaze_interaction) via
//                                GazePointerDriver for attention/fixation metrics.
//
//  Menu      : PsyNet Lab > Cognitive Tests > Generate Variant Scenes
//  Batch mode: Unity.exe -batchmode -quit -projectPath <p>
//                        -executeMethod CognitiveTestsSceneBuilder.GenerateAllBatch
//  Layer     : Editor tooling.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created (8 scenes: Varjo, ProEye, OpenXR_PC, OpenXR_Android).
//    2026-10-29  v1.1  Added FocusVision and QuestPro eye-tracking scenes (12 scenes total).
// =====================================================================================

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CognitiveTestsSceneBuilder
{
    public const string OutputFolder = "Assets/CognitiveTests_2026/Scenes";
    private const string CorsiBase = "Assets/CorsiBlockVR_OpenXR.unity";
    private const string DearyBase = "Assets/DearyLiewaldVR_OpenXR.unity";
    private const string SRanipalPrefab = "Assets/ViveSR/Prefabs/SRanipal Eye Framework.prefab";

    // ------------------------------------------------------------------ CSV columns
    // Original column names/order are kept (Pro Eye version, which included pupil size);
    // new columns are appended at the end so old analysis scripts keep working.
    private static readonly string[] MetaColumns =
        { "Test", "Variant", "Platform", "EyeTracker", "GazeSource", "ETCalibration", "Completed", "SessionStart", "AppVersion" };

    private static readonly string[] DearyColumns =
    {
        "ParticipantID", "SingleReactionTime", "ChoiceReactionTime", "AttentionTime", "MotorTime", "DearyPupilSize",
        "SingleCorrect", "SingleSlow", "ChoiceCorrect", "ChoiceErrors", "ChoiceSlow"
    };

    private static readonly string[] CorsiColumns =
    {
        "ParticipantID",
        "CorsiForwardTotalScore", "CorsiForwardSpan", "CorsiForwardFixationTimes", "CorsiForwardFixationDuration", "CorsiForwardPupilSize",
        "CorsiBackwardTotalScore", "CorsiBackwardSpan", "CorsiBackwardFixationTimes", "CorsiBackwardFixationDuration", "CorsiBackwardPupilSize",
        "CorsiForwardAvgTrialTime", "CorsiForwardAvgPlanningTime", "CorsiForwardAvgMotorTime",
        "CorsiBackwardAvgTrialTime", "CorsiBackwardAvgPlanningTime", "CorsiBackwardAvgMotorTime"
    };

    public static string ScenePath(CognitiveTest test, HardwareVariant variant)
    {
        string name = test == CognitiveTest.Corsi ? "CorsiVR" : "DearyLiewaldVR";
        return $"{OutputFolder}/{name}_{variant}.unity";
    }

    [MenuItem("PsyNet Lab/Cognitive Tests/Generate Variant Scenes")]
    public static void GenerateAll()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        GenerateAllInternal();
    }

    /// <summary>Entry point for -executeMethod (no dialogs).</summary>
    public static void GenerateAllBatch()
    {
        bool ok = GenerateAllInternal();
        if (Application.isBatchMode) EditorApplication.Exit(ok ? 0 : 1);
    }

    private static bool GenerateAllInternal()
    {
        System.IO.Directory.CreateDirectory(OutputFolder);
        bool ok = true;
        foreach (CognitiveTest test in System.Enum.GetValues(typeof(CognitiveTest)))
            foreach (HardwareVariant variant in System.Enum.GetValues(typeof(HardwareVariant)))
                ok &= Generate(test, variant);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[SceneBuilder] Finished. Success = " + ok);
        return ok;
    }

    public static bool Generate(CognitiveTest test, HardwareVariant variant)
    {
        string basePath = test == CognitiveTest.Corsi ? CorsiBase : DearyBase;
        string outPath = ScenePath(test, variant);
        Debug.Log($"[SceneBuilder] ===== {test} / {variant}  ({basePath} -> {outPath})");

        Scene scene = EditorSceneManager.OpenScene(basePath, OpenSceneMode.Single);
        var log = new List<string>();
        try
        {
            // ---- 1) remove missing-script components (SteamVR leftovers) on ALL objects, incl. inactive
            int removed = 0;
            foreach (var t in AllTransforms(scene))
                removed += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(t.gameObject);
            log.Add($"removed {removed} missing-script components");

            // ---- 2) core references
            var taskOrder = Find<Task_Selection_Order>(scene);
            var score = Find<Score_Manager>(scene);
            if (taskOrder == null || score == null)
                throw new System.Exception("Task_Selection_Order / Score_Manager not found in " + basePath);
            GameObject manager = taskOrder.gameObject;

            // Task selection (enum: deary = 0, digit = 1, corsi = 2) - make sure it matches the test.
            taskOrder.task = test == CognitiveTest.Corsi ? Task_Selection_Order.Task.corsi : Task_Selection_Order.Task.deary;

            // XR camera: take it from the old gaze pointer before removing it, else the MainCamera.
            Camera xrCamera = null;
            var oldPointer = manager.GetComponent<OpenXRGazePointer>();
            if (oldPointer != null)
            {
                xrCamera = oldPointer.xrCamera;
                Object.DestroyImmediate(oldPointer);
                log.Add("removed OpenXRGazePointer (head-gaze fallback)");
            }
            if (xrCamera == null)
                xrCamera = AllComponents<Camera>(scene).FirstOrDefault(c => c.CompareTag("MainCamera") && c.gameObject.activeInHierarchy);
            log.Add("XR camera: " + (xrCamera != null ? PathOf(xrCamera.transform) + " active=" + xrCamera.gameObject.activeInHierarchy : "NOT FOUND"));

            // ET pointer (kinematic trigger sphere tagged ET_Pointer)
            Transform etPointer = AllTransforms(scene).FirstOrDefault(t => t.name == "ET_Pointer" && t.CompareTag("ET_Pointer"));
            if (etPointer == null) throw new System.Exception("ET_Pointer object not found");
            etPointer.gameObject.SetActive(true);

            // EyeTracking component = pupil list "sink" used by Deary_Manager / Corsi_ET_Manager.
            EyeTracking eyeTracking = taskOrder.eyeTracking;
            if (eyeTracking == null) eyeTracking = AllComponents<EyeTracking>(scene).FirstOrDefault(e => e.gameObject.name == "Eye_Tracking");
            if (eyeTracking == null) throw new System.Exception("EyeTracking component not found");
            taskOrder.eyeTracking = eyeTracking;

            // ---- 3) new components on the ExperimentManager
            var vc = GetOrAdd<VariantConfig>(manager);
            vc.test = test;
            vc.variant = variant;

            var driver = GetOrAdd<GazePointerDriver>(manager);
            driver.gazePointer = etPointer;
            driver.xrCamera = xrCamera;
            driver.pupilSink = eyeTracking;

            var keypad = GetOrAdd<ParticipantIdKeypad>(manager);
            keypad.xrCamera = xrCamera;

            // ---- 4) CSV columns + pupillometry flags
            // Varjo, ProEye, and FocusVision (XR_HTC_eye_tracker) all provide pupil diameter in mm.
            bool pupil = variant == HardwareVariant.Varjo ||
                         variant == HardwareVariant.ProEye ||
                         variant == HardwareVariant.FocusVision;
            score.variables = BuildColumns(test == CognitiveTest.Corsi ? CorsiColumns : DearyColumns);
            score.ProEye = pupil;
            foreach (var dm in AllComponents<Deary_Manager>(scene)) { dm.ProEye = pupil; dm.ET = eyeTracking; EditorUtility.SetDirty(dm); }
            foreach (var cm in AllComponents<Corsi_ET_Manager>(scene)) { cm.ProEye = pupil; cm.ET = eyeTracking; EditorUtility.SetDirty(cm); }

            // ---- 5) variant-specific eye tracking setup
            var srPointer = etPointer.GetComponent<ET_Pointer>();
            switch (variant)
            {
                case HardwareVariant.ProEye:
                    // Original Pro Eye setup: SRanipal framework + EyeTracking.cs + SRanipal ET_Pointer.cs
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(SRanipalPrefab);
                    if (prefab == null) throw new System.Exception("SRanipal prefab missing: " + SRanipalPrefab);
                    var fw = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                    fw.name = "SRanipal Eye Framework";
                    eyeTracking.gameObject.SetActive(true);
                    eyeTracking.enabled = true;
                    if (srPointer != null) srPointer.enabled = true;
                    log.Add("ProEye: SRanipal framework added, EyeTracking + ET_Pointer(SRanipal) enabled");
                    break;

                case HardwareVariant.Varjo:
                    GetOrAdd<EyeCalibrationGate>(manager);
                    eyeTracking.gameObject.SetActive(false); // only used as pupil-list sink
                    eyeTracking.enabled = false;             // SRanipal code must not run on Varjo
                    if (srPointer != null) srPointer.enabled = false;
                    log.Add("Varjo: EyeCalibrationGate added, GazePointerDriver drives ET_Pointer + pupil");
                    break;

                case HardwareVariant.FocusVision:
                    eyeTracking.gameObject.SetActive(false); // used as pupil-list sink for XR_HTC_eye_tracker
                    eyeTracking.enabled = false;             // SRanipal code must not run on Android
                    if (srPointer != null) srPointer.enabled = false;
                    log.Add("FocusVision: VIVE OpenXR XR_HTC_eye_tracker gaze + pupillometry via GazePointerDriver");
                    break;

                case HardwareVariant.QuestPro:
                    eyeTracking.gameObject.SetActive(false);
                    eyeTracking.enabled = false;
                    if (srPointer != null) srPointer.enabled = false;
                    log.Add("QuestPro: Meta Quest Pro OpenXR eye gaze via GazePointerDriver");
                    break;

                default:
                    eyeTracking.gameObject.SetActive(false);
                    eyeTracking.enabled = false;
                    if (srPointer != null) srPointer.enabled = false;
                    log.Add("OpenXR: no required eye tracker (optional OpenXR eye gaze only)");
                    break;
            }

            // ---- 6) diagnostics: cameras + finger colliders
            foreach (var cam in AllComponents<Camera>(scene))
                log.Add($"camera: {PathOf(cam.transform)} activeInHierarchy={cam.gameObject.activeInHierarchy} tag={cam.tag}");
            foreach (var t in AllTransforms(scene).Where(t => t.CompareTag("IndexFinger")))
            {
                var fi = t.GetComponent<FollowIndex>();
                log.Add($"finger collider: {PathOf(t)} active={t.gameObject.activeInHierarchy} follows={(fi != null && fi.target != null ? PathOf(fi.target.transform) : "NONE")}");
            }

            EditorUtility.SetDirty(manager);
            EditorUtility.SetDirty(score);
            EditorUtility.SetDirty(taskOrder);
            EditorSceneManager.MarkSceneDirty(scene);
            bool saved = EditorSceneManager.SaveScene(scene, outPath, false);
            foreach (var l in log) Debug.Log("[SceneBuilder]   " + l);
            Debug.Log($"[SceneBuilder]   saved={saved} -> {outPath}");
            return saved;
        }
        catch (System.Exception e)
        {
            foreach (var l in log) Debug.Log("[SceneBuilder]   " + l);
            Debug.LogError($"[SceneBuilder] FAILED {test}/{variant}: {e}");
            return false;
        }
    }

    // ------------------------------------------------------------------ helpers

    private static List<Score_Manager.VariableEntry> BuildColumns(string[] taskColumns)
    {
        var list = new List<Score_Manager.VariableEntry>();
        foreach (var c in taskColumns.Concat(MetaColumns))
        {
            var type = c == "ParticipantID" || c.EndsWith("Score") || c.EndsWith("Span") || c.EndsWith("Correct") ||
                       c.EndsWith("Slow") || c.EndsWith("Errors") || c == "Completed"
                ? Score_Manager.VariableEntry.VariableType.Int
                : (System.Array.IndexOf(MetaColumns, c) >= 0 ? Score_Manager.VariableEntry.VariableType.String
                                                              : Score_Manager.VariableEntry.VariableType.Float);
            list.Add(new Score_Manager.VariableEntry { variableName = c, variableType = type });
        }
        return list;
    }

    private static IEnumerable<Transform> AllTransforms(Scene scene)
    {
        foreach (var root in scene.GetRootGameObjects())
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
                yield return t;
    }

    private static IEnumerable<T> AllComponents<T>(Scene scene) where T : Component
    {
        foreach (var root in scene.GetRootGameObjects())
            foreach (var c in root.GetComponentsInChildren<T>(true))
                yield return c;
    }

    private static T Find<T>(Scene scene) where T : Component => AllComponents<T>(scene).FirstOrDefault();

    private static T GetOrAdd<T>(GameObject go) where T : Component
    {
        var c = go.GetComponent<T>();
        return c != null ? c : go.AddComponent<T>();
    }

    private static string PathOf(Transform t)
    {
        string p = t.name;
        while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }
}
