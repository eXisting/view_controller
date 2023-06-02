using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Calls
{
  public class TypeMachine : MonoBehaviour
  {
    [SerializeField] private Text text;
    [SerializeField] public TMP_Text tmpProText;

    [SerializeField] private float delayBeforeStart;
    [SerializeField] private float timeBtwChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay;

    [Space(10)] [SerializeField] private bool startOnEnable;

    [Header("Collision-Based")] [SerializeField]
    private bool clearAtStart;

    [SerializeField] private bool startOnCollision;
    [SerializeField] private Options collisionExitOptions;

    private Coroutine _coroutine;

    private string _writer;

    private void Awake()
    {
      if (text != null)
        _writer = text.text;

      if (tmpProText != null)
        _writer = tmpProText.text;
    }

    private void Start()
    {
      if (!clearAtStart)
        return;

      if (text != null)
        text.text = "";

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
    }

    private void OnCollisionEnter2D(Collision2D _)
    {
      print("Collision!");
      if (startOnCollision)
        StartTypewriter();
    }

    private void OnCollisionExit2D(Collision2D _)
    {
      if (collisionExitOptions == Options.Complete)
      {
        if (text != null)
          text.text = _writer;

        if (tmpProText != null)
          tmpProText.text = _writer;
      }
      else
      {
        if (text != null)
          text.text = "";

        if (tmpProText != null)
          tmpProText.text = "";
      }

      StopAllCoroutines();
    }


    public void StartTypewriter()
    {
      StopAllCoroutines();

      if (text != null)
      {
        text.text = "";
        StartCoroutine(nameof(TypeWriterText));
      }

      if (tmpProText == null)
        return;

      tmpProText.text = "";
      StartCoroutine(nameof(TypeWriterTMP));
    }

    private IEnumerator TypeWriterText()
    {
      text.text = leadingCharBeforeDelay ? leadingChar : "";

      yield return new WaitForSeconds(delayBeforeStart);

      foreach (var c in _writer)
      {
        if (text.text.Length > 0)
          text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);

        text.text += c;
        text.text += leadingChar;
        yield return new WaitForSeconds(timeBtwChars);
      }

      if (leadingChar != "")
        text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);

      yield return null;
    }

    private IEnumerator TypeWriterTMP()
    {
      tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

      yield return new WaitForSeconds(delayBeforeStart);

      foreach (var c in _writer)
      {
        if (tmpProText.text.Length > 0)
          tmpProText.text = tmpProText.text[..^leadingChar.Length];

        tmpProText.text += c;
        tmpProText.text += leadingChar;
        yield return new WaitForSeconds(timeBtwChars);
      }

      if (leadingChar != "")
        tmpProText.text = tmpProText.text[..^leadingChar.Length];
    }

    private enum Options
    {
      Clear,
      Complete
    }
  }
}