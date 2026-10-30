// =====================================================================================
//  CsvUtil.cs
// -------------------------------------------------------------------------------------
//  Purpose   : Small, dependency-free helpers shared by every data-writing script of the
//              CognitiveTestsVR project (Corsi VR + Deary-Liewald VR).
//              1) Culture-independent number formatting for CSV files.
//              2) "Safe" averages that never throw on empty lists.
//              3) A single "NA" token for values that could not be measured.
//  Layer     : Runtime utility (no MonoBehaviour, no scene references).
//  Depends on: System.Linq, System.Globalization only.
//  Author    : P. Kourtesis / PsyNet Lab
//  Date      : 2026-10-29
//  Change log:
//    2026-10-29  v1.0  Created for the 4-variant (Varjo / Pro Eye / OpenXR PC / OpenXR
//                      Android) rebuild of Corsi VR and Deary-Liewald VR.
// =====================================================================================
//
//  WHY THIS FILE EXISTS
//  --------------------
//  * float.ToString() uses the *current Windows locale*. On a Greek-locale PC this writes
//    "0,523" instead of "0.523", which silently breaks a comma-separated CSV (one value
//    becomes two columns). Every number that ends up in a CSV must therefore go through
//    CsvUtil.F() / CsvUtil.Format(), which always use InvariantCulture (dot decimal).
//  * System.Linq's Average() throws InvalidOperationException on an empty list. In the
//    original code this happened inside OnDisable() whenever eye tracking was not
//    available (OpenXR builds) or every response was slow, which aborted the rest of the
//    OnDisable() and left e.g. ChoiceReactionTime at 0 in the CSV. SafeAverage() returns
//    NaN instead, and NaN is written to the CSV as "NA".
// =====================================================================================

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public static class CsvUtil
{
    /// <summary>Token written to the CSV when a value could not be measured
    /// (e.g. pupil size on a headset without eye tracking). R/SPSS/pandas all read "NA" as missing.</summary>
    public const string NA = "NA";

    /// <summary>Average that returns NaN (instead of throwing) when the list is null or empty.</summary>
    public static float SafeAverage(List<float> values)
    {
        if (values == null || values.Count == 0) return float.NaN;
        return values.Average();
    }

    /// <summary>Sum / count style mean that returns NaN when the denominator is 0
    /// (used for Corsi "average fixation duration" = total duration / number of fixations).</summary>
    public static float SafeDivide(float numerator, float denominator)
    {
        if (denominator == 0f || float.IsNaN(denominator)) return float.NaN;
        return numerator / denominator;
    }

    /// <summary>Culture-invariant float formatting; NaN / Infinity become "NA".
    /// "R" (round-trip) keeps full precision exactly as the old ToString() did, only with a dot.</summary>
    public static string F(float v)
    {
        if (float.IsNaN(v) || float.IsInfinity(v)) return NA;
        return v.ToString("R", CultureInfo.InvariantCulture);
    }

    /// <summary>Formats any value for the CSV in a culture-invariant way.
    /// Strings are sanitised so that a comma or newline inside a text value cannot break the row.</summary>
    public static string Format(object value)
    {
        switch (value)
        {
            case null: return NA;
            case float f: return F(f);
            case double d: return (double.IsNaN(d) || double.IsInfinity(d)) ? NA : d.ToString("R", CultureInfo.InvariantCulture);
            case int i: return i.ToString(CultureInfo.InvariantCulture);
            case long l: return l.ToString(CultureInfo.InvariantCulture);
            case bool b: return b ? "1" : "0";
            case string s: return Sanitise(s);
            default: return Sanitise(System.Convert.ToString(value, CultureInfo.InvariantCulture));
        }
    }

    /// <summary>Removes characters that would break a simple comma-separated row.</summary>
    public static string Sanitise(string s)
    {
        if (string.IsNullOrEmpty(s)) return s ?? NA;
        return s.Replace(",", ";").Replace("\r", " ").Replace("\n", " ");
    }
}
