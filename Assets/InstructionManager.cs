using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InstructionManager : MonoBehaviour
{
    public GameObject videoWall;
    public VideoPlayer videoPlayer;
    public VideoClip dearyClip;
    public VideoClip corsiClip;
    public VideoClip digitClip;

    public AudioSource audioSource;
    public AudioClip dearyAudio;
    public AudioClip corsiFAudio;
    public AudioClip corsiBAudio;
    public AudioClip digitFAudio;
    public AudioClip digitBAudio;

    public bool instructionsDone = false;
    public IEnumerator DearyInstructions()
    {
        instructionsDone = false;
        videoWall.SetActive(true);
        videoPlayer.clip = dearyClip;
        audioSource.clip = dearyAudio;
        videoPlayer.Play();
        audioSource.Play();
        yield return StartCoroutine(WaitUntilInstructionsDone());
    }
    public IEnumerator CorsiFInstructions()
    {
        instructionsDone = false;
        videoWall.SetActive(true);
        videoPlayer.clip = corsiClip;
        audioSource.clip = corsiFAudio;
        videoPlayer.Play();
        audioSource.Play();
        yield return StartCoroutine(WaitUntilInstructionsDone());
    }
    public IEnumerator DigitFInstructions()
    {
        instructionsDone = false;
        videoWall.SetActive(true);
        videoPlayer.clip = digitClip;
        audioSource.clip = digitFAudio;
        videoPlayer.Play();
        audioSource.Play();
        yield return StartCoroutine(WaitUntilInstructionsDone());
    }
    public IEnumerator CorsiBInstructions()
    {
        instructionsDone = false;
        videoWall.SetActive(true);
        videoPlayer.clip = corsiClip;
        audioSource.clip = corsiBAudio;
        videoPlayer.Play();
        audioSource.Play();
        yield return StartCoroutine(WaitUntilInstructionsDone());
    }
    public IEnumerator DigitBInstructions()
    {
        instructionsDone = false;
        videoWall.SetActive(true);
        videoPlayer.clip = digitClip;
        audioSource.clip = digitBAudio;
        videoPlayer.Play();
        audioSource.Play();
        yield return StartCoroutine(WaitUntilInstructionsDone());
    }
    IEnumerator WaitUntilInstructionsDone()
    {
        //yield return new WaitUntil(() => videoPlayer.isPlaying == false);
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        videoPlayer.Stop();
        videoWall.SetActive(false);
        instructionsDone = true;
    }
}
