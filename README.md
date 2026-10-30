# Corsi-VR & Deary-Liewald VR (DLRTT-VR) — Open-Source Unity Project

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Docs & Instructions: CC BY 4.0](https://img.shields.io/badge/Instructions-CC%20BY%204.0-lightgrey.svg)](NOTICE.md)
[![Validation DOI](https://img.shields.io/badge/Validation%20DOI-10.1111%2Fjnp.70014-success)](https://doi.org/10.1111/jnp.70014)
[![Unity 2021.3 LTS](https://img.shields.io/badge/Unity-2021.3.36f1-black)](https://unity.com/)

This repository contains the official open-source Unity project for two validated immersive virtual reality neuropsychological assessments developed by **Dr. Panagiotis Kourtesis**:

1. **Corsi Block Test in Virtual Reality (`Corsi-VR`)** — An immersive 3D assessment of **visuospatial short-term and working memory** (27-level adaptive staircase procedure, two trials per span level).
2. **Deary-Liewald Reaction Time Task in Virtual Reality (`DLRTT-VR`)** — An immersive 3D adaptation of the Deary-Liewald paradigm (Deary, Liewald, & Nissan, 2011) assessing **Simple Reaction Time (SRT)** and **Four-Choice Reaction Time (CRT)**, decomposing choice responses into **Attentional Orienting Time (AOT)**, **Motor Execution Time (MET)**, and **Total Reaction Time (TRT)** alongside continuous **binocular pupillometry (mm)**.

Part of the **Open-Access Library of XR Cognitive & Behavioural Assessments**: [https://www.panoskourtesis.com/vr-cognitive-assessments](https://www.panoskourtesis.com/vr-cognitive-assessments)

---

## Pre-Built Executables & Android APKs (2026 Release)

All 12 compiled player packages (6 Windows x64 PC VR `.zip` archives and 6 Standalone Android `.apk` installers) are publicly available on Google Drive:

- **Corsi-VR Builds Folder**: [https://drive.google.com/drive/folders/1lN7nYoDcxFqymoXNDpNMjyO2KWjmgqV-](https://drive.google.com/drive/folders/1lN7nYoDcxFqymoXNDpNMjyO2KWjmgqV-)
- **Deary-Liewald VR Builds Folder**: [https://drive.google.com/drive/folders/1zJ0eLVT1G7vZX_ofbA404x11GRlCytl6](https://drive.google.com/drive/folders/1zJ0eLVT1G7vZX_ofbA404x11GRlCytl6)

| Task | Scene / Build Variant | Platform | Target Headsets | Interaction Modalities | Eye Tracking & Pupillometry |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Corsi-VR** | `CorsiVR_Varjo` | Windows x64 (`.zip`) | Varjo Aero, Varjo XR-3 / VR-3 | Eye Gaze (0.5 s dwell), 6DoF Controller Ray, Hand Tracking | **200 Hz Binocular Gaze + Pupillometry (mm)** (`VarjoEyeTracking`) |
| **Corsi-VR** | `CorsiVR_ProEye` | Windows x64 (`.zip`) | HTC VIVE Pro Eye | Eye Gaze (0.5 s dwell), 6DoF Controller Ray, Hand Tracking | **120 Hz Binocular Gaze + Pupillometry (mm)** (`SRanipal`) |
| **Corsi-VR** | `CorsiVR_OpenXR_PC` | Windows x64 (`.zip`) | Universal PCVR (Quest Link, VIVE, Index, WMR) | Eye/Head Gaze (0.5 s dwell), 6DoF Controller Ray, OpenXR Hand Tracking | OpenXR Eye Gaze (`XR_EXT_eye_gaze_interaction`) |
| **Corsi-VR** | `CorsiVR_FocusVision` | Android (`.apk`) | **HTC VIVE Focus Vision**, Focus 3 (Eye Tracker), XR Elite | Eye Gaze (0.5 s dwell), 6DoF Controller Ray, Hand Tracking | **120 Hz Binocular Gaze + Pupillometry (mm)** (`XR_HTC_eye_tracker`) |
| **Corsi-VR** | `CorsiVR_QuestPro` | Android (`.apk`) | **Meta Quest Pro** | Eye Gaze (0.5 s dwell), 6DoF Controller Ray, Hand Tracking | **Eye Gaze Tracking** (`XR_EXT_eye_gaze_interaction` + `oculus.software.eye_tracking`) |
| **Corsi-VR** | `CorsiVR_OpenXR_Android` | Android (`.apk`) | **Universal Standalone** (Quest 2/3/3S/Pro, Focus Vision/3) | Head/Eye Gaze (0.5 s dwell), 6DoF Controller Ray, OpenXR Hand Tracking | Universal OpenXR Eye/Head Gaze |
| **DLRTT-VR** | `DearyLiewaldVR_Varjo` | Windows x64 (`.zip`) | Varjo Aero, Varjo XR-3 / VR-3 | Eye Gaze (AOT) + Controller Ray / Hand Tracking (MET) | **200 Hz Binocular Gaze (AOT/MET) + Pupillometry (mm)** (`VarjoEyeTracking`) |
| **DLRTT-VR** | `DearyLiewaldVR_ProEye` | Windows x64 (`.zip`) | HTC VIVE Pro Eye | Eye Gaze (AOT) + Controller Ray / Hand Tracking (MET) | **120 Hz Binocular Gaze (AOT/MET) + Pupillometry (mm)** (`SRanipal`) |
| **DLRTT-VR** | `DearyLiewaldVR_OpenXR_PC` | Windows x64 (`.zip`) | Universal PCVR (Quest Link, VIVE, Index, WMR) | Eye/Head Gaze (AOT) + Controller Ray / OpenXR Hand Tracking (MET) | OpenXR Eye Gaze (`XR_EXT_eye_gaze_interaction`) |
| **DLRTT-VR** | `DearyLiewaldVR_FocusVision` | Android (`.apk`) | **HTC VIVE Focus Vision**, Focus 3 (Eye Tracker), XR Elite | Eye Gaze (AOT) + Controller Ray / Hand Tracking (MET) | **120 Hz Binocular Gaze (AOT/MET) + Pupillometry (mm)** (`XR_HTC_eye_tracker`) |
| **DLRTT-VR** | `DearyLiewaldVR_QuestPro` | Android (`.apk`) | **Meta Quest Pro** | Eye Gaze (AOT) + Controller Ray / Hand Tracking (MET) | **Eye Gaze Tracking (AOT/MET)** (`XR_EXT_eye_gaze_interaction`) |
| **DLRTT-VR** | `DearyLiewaldVR_OpenXR_Android` | Android (`.apk`) | **Universal Standalone** (Quest 2/3/3S/Pro, Focus Vision/3) | Head/Eye Gaze (AOT) + Controller Ray / OpenXR Hand Tracking (MET) | Universal OpenXR Eye/Head Gaze |

---

## Project Architecture

- **`Assets/CognitiveTests_2026/Scenes/`**: Contains all **12 generated scenes** (`6` for `CorsiVR` and `6` for `DearyLiewaldVR`).
- **`Assets/CognitiveTests_2026/Scripts/`**:
  - `VariantConfig.cs`: Declares the active `TaskKind` (`Corsi` vs. `DearyLiewald`) and `HardwareTarget` (`Varjo`, `ProEye`, `OpenXR_PC`, `OpenXR_Android`, `FocusVision`, `QuestPro`) for each scene.
  - `GazePointerDriver.cs`: Unified multi-backend eye/head-gaze raycast and binocular pupillometry driver supporting:
    - **Varjo (`VarjoEyeTracking`)**: 200 Hz binocular gaze ray + left/right pupil diameter in mm.
    - **HTC VIVE Pro Eye (`ViveSR.anipal.Eye.SRanipal_Eye_v2`)**: 120 Hz binocular gaze ray + left/right pupil diameter in mm.
    - **HTC VIVE Focus Vision (`VIVE.OpenXR.EyeTracker.XrSingleEyeGazeDataHTC` / `XrSingleEyePupilDataHTC`)**: 120 Hz native OpenXR binocular gaze ray + left/right pupil diameter in mm (`isDiameterValid`).
    - **Meta Quest Pro & Universal OpenXR (`UnityEngine.InputSystem.EyeGazeDevice` / `XR_EXT_eye_gaze_interaction`)**: Standard OpenXR eye-gaze pose with automatic head-gaze fallback.
  - `StandaloneTaskBootstrap.cs`: Configures each scene as a dedicated single-test application (auto-starting either Corsi-VR or Deary-Liewald VR directly) and writes CSV results to `Application.persistentDataPath` on Android and `Application.dataPath` on Windows.
- **`Assets/CognitiveTests_2026/Editor/`**:
  - `CognitiveTestsSceneBuilder.cs`: Programmatically generates all 12 scenes (`CognitiveTests -> Generate All 2026 Scenes`).
  - `CognitiveTestsBuilder.cs`: Configures XR Plug-in Management, OpenXR interaction profiles, eye-tracking features, and Android manifest permissions (`wave.feature.eyetracking`, `com.oculus.permission.EYE_TRACKING`, `oculus.software.eye_tracking`) per build target (`CognitiveTests -> Build All ...`).

See [`BUILDING.md`](BUILDING.md) for step-by-step instructions on opening the project and building the 12 targets in Unity 2021.3.36f1.

---

## Citations & Peer-Reviewed Studies

If you use **Corsi-VR** or **Deary-Liewald VR (DLRTT-VR)** in your research, please cite the **validation study**:

### Validation Study
- **Kourtesis, P., Lizarraga, A., & MacPherson, S. E. (2026).** Immersive virtual reality assessments of working memory and psychomotor skills: A comparison between immersive and non-immersive assessments. *Journal of Neuropsychology*, 20(1), 115–131. [https://doi.org/10.1111/jnp.70014](https://bpspsychub.onlinelibrary.wiley.com/doi/full/10.1111/jnp.70014)

### Empirical Studies Using Corsi-VR & DLRTT-VR
- **Kourtesis, P., Linnell, J., Amir, R., Argelaguet, F., & MacPherson, S. E. (2023).** Cybersickness in Virtual Reality Questionnaire (CSQ-VR): A Validation and Comparison against SSQ and VRSQ. *Virtual Worlds*, 2(1), 16–35. [https://doi.org/10.3390/virtualworlds2010002](https://www.mdpi.com/2813-2084/2/1/2)
- **Kourtesis, P., Amir, R., Linnell, J., Argelaguet, F., & MacPherson, S. E. (2023).** Cybersickness, Cognition, & Motor Skills: The Effects of Music, Gender, and Gaming Experience. *IEEE Transactions on Visualization and Computer Graphics*, 29(5), 2326–2336. [https://doi.org/10.1109/TVCG.2023.3247062](https://ieeexplore.ieee.org/abstract/document/10049731)
- **Kourtesis, P., Papadopoulou, A., & Roussos, P. (2024).** Examination of Cybersickness in Virtual Reality: The Role of Individual Differences, Effects on Cognitive Functions & Motor Skills, and Intensity Differences During and After Immersion. *Virtual Worlds*, 3(1), 62–83. [https://doi.org/10.3390/virtualworlds3010004](https://www.mdpi.com/2813-2084/3/1/4)
- **Papaefthymiou, S., Giannakopoulos, A., Roussos, P., & Kourtesis, P. (2024).** Mitigating Cybersickness in Virtual Reality: Impact of Eye–Hand Coordination Tasks, Immersion, and Gaming Skills. *Virtual Worlds*, 3(4), 506–535. [https://doi.org/10.3390/virtualworlds3040027](https://www.mdpi.com/2813-2084/3/4/27)

---

## Licence

- **Source Code**: Licensed under the **MIT License** (see [`LICENSE`](LICENSE)).
- **Spoken Instructions, Demonstration Videos & Instruction Texts**: Licensed under **CC BY 4.0** (see [`NOTICE.md`](NOTICE.md)).
- **Third-Party SDKs & Packages**: Retain their own licences (see [`THIRD_PARTY_NOTICES.md`](THIRD_PARTY_NOTICES.md)).
