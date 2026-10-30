# Third-Party Notices

**Corsi-VR** and **Deary-Liewald VR (DLRTT-VR)** build upon Unity packages, OpenXR and vendor eye-tracking SDKs, and third-party environmental assets. Those components remain under their own licences. This document summarises the third-party components and reproduces the required notices; it grants no licence itself. The licences for the project's own source code and instructional content are set out in `LICENSE` and `NOTICE.md`.

## Included Third-Party Components & Licences

| Component | Location in Repository / Manifest | Licence & Terms |
| :--- | :--- | :--- |
| **Varjo XR Plugin (`com.varjo.xr` v3.6.0)** | `Packages/com.varjo.xr/` (`Packages/com.varjo.xr/LICENSE.md`) | Varjo SDK License Agreement (`Copyright © 2021 Varjo. All Rights Reserved.`). Used for Varjo Aero / XR-3 / VR-3 200 Hz binocular eye tracking and pupillometry. |
| **VIVE OpenXR Plugin (`com.htc.upm.vive.openxr` v2.5.1)** | `Packages/com.htc.upm.vive.openxr/` (`Packages/com.htc.upm.vive.openxr/LICENSE.txt`) | HTC VIVE SDK License Agreement (`Copyright © HTC Corporation, LLC and its affiliates. All rights reserved.`). Used for HTC VIVE Focus Vision / Focus 3 (`XR_HTC_eye_tracker`) 120 Hz binocular eye tracking and pupillometry. |
| **HTC VIVE SRanipal SDK (`ViveSR`)** | `Assets/ViveSR/` | HTC VIVE SRanipal SDK License Agreement (`Copyright © HTC Corporation`). Used in the `ProEye` PC VR scenes for HTC VIVE Pro Eye 120 Hz eye tracking and pupillometry. |
| **Unity OpenXR Plugin (`com.unity.xr.openxr` v1.10.0)** | `Packages/manifest.json`, `Assets/Samples/OpenXR Plugin/` | Unity Companion License (UCL) / Unity Package Distribution License (UPDL). Includes `XR_EXT_eye_gaze_interaction` and Meta Quest (`com.oculus.permission.EYE_TRACKING`) support. |
| **Unity XR Interaction Toolkit (`com.unity.xr.interaction.toolkit` v2.5.3)** | `Packages/manifest.json`, `Assets/Samples/XR Interaction Toolkit/` | Unity Companion License (UCL). |
| **Unity XR Hands (`com.unity.xr.hands` v1.4.1)** | `Packages/manifest.json`, `Assets/Samples/XR Hands/` | Unity Package Distribution License (UPDL) / Unity Companion License (UCL). |
| **TextMesh Pro (`com.unity.textmeshpro` v3.0.6) & Liberation Sans** | `Assets/TextMesh Pro/` | Unity Companion License (UCL); Liberation Sans font under the SIL Open Font License 1.1 (`Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt`). |
| **SkySeries Freebie HDRI Skyboxes** | `Assets/SkySeries Freebie/` | Standard Unity Asset Store EULA. |

---

## Varjo SDK Redistribution Disclaimer

In accordance with Section 1.1 of the Varjo SDK License Agreement (`Packages/com.varjo.xr/LICENSE.md`), redistributions incorporating the Varjo XR Plugin include the following disclaimer:

> THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
>
> Copyright © 2021 Varjo. All Rights Reserved.
