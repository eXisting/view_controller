using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class CallNotification : MonoBehaviour
  {
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip callRingtone;
    [SerializeField] private Button answer;
    
    private float _stopWatch;
    
    private void OnEnable()
    {
      StartCoroutine(nameof(PlayRingtone));
    }

    private void Start()
    {
      answer.onClick.AddListener(Answer);
    }

    private void OnDestroy()
    {
      answer.onClick.RemoveListener(Answer);
    }
    
    private void Answer()
    {
      StopCoroutine(nameof(PlayRingtone));
      
      Component.Navigator.Open(Enum.Screen.Call);
      source.Stop();
      gameObject.SetActive(false);
    }
    
    private IEnumerator PlayRingtone()
    {
      source.Play();
      _stopWatch = Time.time;

      while (Time.time - _stopWatch < callRingtone.length)
      {
        Handheld.Vibrate();
        Handheld.Vibrate();
        
        yield return new WaitForSeconds(.8f);
      }
      
      gameObject.SetActive(false);
    }
  }
}