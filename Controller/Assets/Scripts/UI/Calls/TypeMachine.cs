using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using Screen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Calls
{
  public class TypeMachine : MonoBehaviour
  {
    [SerializeField] public TMP_Text tmpProText;

    [SerializeField] private float delayBeforeStart;
    [SerializeField] private float timeBtwChars = 0.1f;
    [SerializeField] private bool leadingCharBeforeDelay;

    [Space(10)] [SerializeField] private bool startOnEnable;

    [Header("Collision-Based")] [SerializeField]
    private bool clearAtStart;

    [SerializeField] private bool startOnCollision;
    [SerializeField] private Options collisionExitOptions;

    private Coroutine _coroutine;

    private string _writer;
    private List<SubtitlePart> _subtitles;

    private void Awake()
    {
      if (tmpProText != null)
        _writer = tmpProText.text;
    }

    private void Start()
    {
      if (!clearAtStart)
        return;

      if (tmpProText != null)
        tmpProText.text = "";
    }

    private void OnEnable()
    {
      print("On Enable!");
      if (startOnEnable)
        StartTypewriter();
    }

    private void OnDisable()
    {
      StopAllCoroutines();
     
      _subtitles.Clear();
    }

    public void PrepareSubtitles(List<SubtitlePart> subtitles)
    {
      _subtitles = subtitles;
    }

    private void StartTypewriter()
    {
      StopAllCoroutines();

      if (tmpProText == null)
        return;

      tmpProText.text = "";
      StartCoroutine(nameof(TypeWriterTMP));
    }

    private IEnumerator TypeWriterTMP()
    {
      tmpProText.text = string.Empty;

      for (var i = 0; i < _subtitles.Count; i++)
      {
        _writer = _subtitles[i].Text;
        timeBtwChars = (_subtitles[i].Finish - _subtitles[i].Start) / _subtitles[i].Text.Length;
        
        yield return new WaitForSeconds(i == 0 ? _subtitles[i].Start : _subtitles[i].Start - _subtitles[i - 1].Finish);
        
        tmpProText.text = string.Empty;

        foreach (var c in _writer)
        {
          tmpProText.text += c;
          yield return new WaitForSeconds(timeBtwChars);
        }
      }
    }

    private enum Options
    {
      Clear,
      Complete
    }
  }
}