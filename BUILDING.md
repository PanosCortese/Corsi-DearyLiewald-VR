# Building Corsi-VR & Deary-Liewald VR (DLRTT-VR)

## Requirements

- **Unity Editor**: `2021.3.36f1` (LTS)
- **Unity Modules**:
  - **Windows Build Support (Mono / IL2CPP)**
  - **Android Build Support** (with Android SDK & NDK Tools and OpenJDK)

---

## 1. Opening the Project

1. Clone this repository:
   ```bash
   git clone https://github.com/PanosCortese/Corsi-DearyLiewald-VR.git
   ```
2. Open **Unity Hub**, click **Add -> Add project from disk**, and select the repository root folder (`CognitiveTestsVR`).
3. Open the project with **Unity 2021.3.36f1**.

---

## 2. Regenerating the 12 Scenes (Optional)

All 12 scenes are already committed in `Assets/CognitiveTests_2026/Scenes/`:

- `CorsiVR_Varjo.unity`
- `CorsiVR_ProEye.unity`
- `CorsiVR_OpenXR_PC.unity`
- `CorsiVR_OpenXR_Android.unity`
- `CorsiVR_FocusVision.unity`
- `CorsiVR_QuestPro.unity`
- `DearyLiewaldVR_Varjo.unity`
- `DearyLiewaldVR_ProEye.unity`
- `DearyLiewaldVR_OpenXR_PC.unity`
- `DearyLiewaldVR_OpenXR_Android.unity`
- `DearyLiewaldVR_FocusVision.unity`
- `DearyLiewaldVR_QuestPro.unity`

To programmatically regenerate all 12 scenes from the base template scenes (`Assets/CorsiBlockVR_OpenXR.unity` and `Assets/DearyLiewaldVR_OpenXR.unity`), click:
- **Top Menu**: `CognitiveTests -> Generate All 2026 Scenes`

---

## 3. Building from the Unity Editor Menu

Use the **`CognitiveTests`** top menu in the Unity Editor:

- **`CognitiveTests -> Build All Windows (6 EXEs)`**:
  Builds `CorsiVR_Varjo.exe`, `CorsiVR_ProEye.exe`, `CorsiVR_OpenXR_PC.exe`, `DearyLiewaldVR_Varjo.exe`, `DearyLiewaldVR_ProEye.exe`, and `DearyLiewaldVR_OpenXR_PC.exe`.
- **`CognitiveTests -> Build All Android (6 APKs)`**:
  Builds all 6 Standalone Android OpenXR APKs (`OpenXR_Android`, `FocusVision`, and `QuestPro` for both tasks).
- **`CognitiveTests -> Build Android Eye-Tracking Only (4 APKs: FocusVision + QuestPro)`**:
  Builds only the 4 dedicated eye-tracking APKs (`CorsiVR_FocusVision.apk`, `CorsiVR_QuestPro.apk`, `DearyLiewaldVR_FocusVision.apk`, `DearyLiewaldVR_QuestPro.apk`).

All builds are output to `../Builds_2026/CorsiVR/<Variant>/` and `../Builds_2026/DearyLiewaldVR/<Variant>/`.

---

## 4. Command-Line Batch Builds

You can also run headless batch builds directly from PowerShell or Command Prompt:

```powershell
$unity = "C:\Program Files\Unity\Hub\Editor\2021.3.36f1\Editor\Unity.exe"
$proj  = "c:\path\to\CognitiveTestsVR"

# 1. Regenerate all 12 scenes
& $unity -batchmode -nographics -quit -projectPath $proj -executeMethod CognitiveTests_2026.Editor.CognitiveTestsSceneBuilder.GenerateAllScenes -logFile unity_scenegen.log

# 2. Build all 6 Windows PC VR executables
& $unity -batchmode -nographics -quit -projectPath $proj -executeMethod CognitiveTests_2026.Editor.CognitiveTestsBuilder.BuildAllWindows -logFile unity_build_win.log

# 3. Build all 6 Android Standalone APKs
& $unity -batchmode -nographics -quit -buildTarget Android -projectPath $proj -executeMethod CognitiveTests_2026.Editor.CognitiveTestsBuilder.BuildAllAndroid -logFile unity_build_android.log
```
