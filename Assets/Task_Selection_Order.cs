using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Selection_Order : MonoBehaviour
{
    public enum Task
    {
        deary,
        digit,
        corsi
    }
    [Tooltip("Select the interaction mode for the TMT-VR")]
    public Task task;
    [Header("Tasks")]
    public GameObject Deary;
    public GameObject DigitF;
    public GameObject DigitB;
    public GameObject CorsiF;
    public GameObject CorsiB;
    public InstructionManager InstructionManager;
    public GameObject EndofExperiment;
    public GameObject Platform;
    public Score_Manager ScoreManager;
    [Header("EyeTracking")]
    public EyeTracking eyeTracking;
    private bool scoringDone = false;  
    void Start()
    {
        // Plase here the code to assign the value of ParticipantID
        // ParticipantID = ...

        // Determine the order based on ParticipantID
        //order = ParticipantID % 3; // For simplicity, assuming ParticipantIDs are sequential

        StartCoroutine(ExecuteTasks());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!scoringDone)
            {
                StartCoroutine(ExperimentEnd());
            }
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (InstructionManager.instructionsDone == false)
            {
                InstructionManager.videoPlayer.Stop();
                InstructionManager.audioSource.Stop();
            }
        }
    }
    IEnumerator ExecuteTasks()
    {
        yield return new WaitUntil(() => ScoreManager.CSVInitialization);
        if (task == Task.digit)
        {
            eyeTracking.digit = true;
            ScoreManager.TaskNumber = 2;
            yield return StartCoroutine(DigitTask());
        }
        else if (task == Task.corsi)
        {
            ScoreManager.TaskNumber = 1;
            eyeTracking.corsi = true;
            yield return StartCoroutine(CorsiTask());
        }
        else if (task == Task.deary)
        {
            ScoreManager.TaskNumber = 0;
            eyeTracking.deary = true;
            yield return StartCoroutine(DearyTask());
        }
        //yield return StartCoroutine(VarjoCalibration()); // Uncomment this line if you want to calibrate the eye tracker for all interaction modes
        
        StartCoroutine(ExperimentEnd());
    }
    IEnumerator ExperimentEnd()
    {
        yield return StartCoroutine(ScoreManager.SaveScore()); // SCORING
        scoringDone = true;
        EndofExperiment.SetActive(true);
        Platform.SetActive(false);
        yield return null;
    }
    IEnumerator DearyTask()
    {
        StartCoroutine(InstructionManager.DearyInstructions());
        yield return new WaitUntil(() => InstructionManager.instructionsDone);
        Deary.SetActive(true);
        yield return new WaitUntil(() => Deary.GetComponent<Deary_Manager>().DONE);
        Deary.SetActive(false);
        // Implementation for TMT-VR PartA
        yield return null;
    }

    IEnumerator DigitTask()
    {
        StartCoroutine(InstructionManager.DigitFInstructions());
        yield return new WaitUntil(() => InstructionManager.instructionsDone);
        DigitF.SetActive(true);
        yield return new WaitUntil(() => DigitF.GetComponent<DigitRecallManager>().DONE);
        DigitF.SetActive(false);
        StartCoroutine(InstructionManager.DigitBInstructions());
        yield return new WaitUntil(() => InstructionManager.instructionsDone);
        DigitB.SetActive(true);
        yield return new WaitUntil(() => DigitB.GetComponent<DigitRecallManager>().DONE);
        DigitB.SetActive(false);
        // Implementation for TMT-VR PartB
        yield return null;
    }

    IEnumerator CorsiTask()
    {
        StartCoroutine(InstructionManager.CorsiFInstructions());
        yield return new WaitUntil(() => InstructionManager.instructionsDone);
        CorsiF.SetActive(true);
        yield return new WaitUntil(() => CorsiF.GetComponent<CorsiBlockManager>().DONE);
        CorsiF.SetActive(false);
        StartCoroutine(InstructionManager.CorsiBInstructions());
        yield return new WaitUntil(() => InstructionManager.instructionsDone);
        CorsiB.SetActive(true);
        yield return new WaitUntil(() => CorsiB.GetComponent<CorsiBlockManager>().DONE);
        CorsiB.SetActive(false);
        // Implementation for TMT-VR PartB
        yield return null;
    }
}
