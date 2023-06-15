using System;
using System.Collections;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Messages
{
  public class MessageNotification : MonoBehaviour
  {
    public event Action<string> Clicked;

    [SerializeField] private TMP_Text userName;
    [SerializeField] private TMP_Text message;
    
    [SerializeField] private AudioClip messageRingtone;
    [SerializeField] private Button open;
    
    private const float Duration = 0.3f;
    
    private Vector3 _activePosition;
    private Vector3 _inactivePosition;
    private Vector3 _initialPosition;

    private MessageData _messageData;
    
    private float _elapsedTime;
    private bool _notificationActive;

    private void Start()
    {
      gameObject.SetActive(false);

      var movementArea = GetComponent<RectTransform>().rect.height * 1.5f;

      var position = transform.position;
      _inactivePosition = position + new Vector3(0, movementArea);
      _activePosition = position - new Vector3(0, movementArea);
      
      open.onClick.AddListener(OpenMessage);
    }

    public void Show(MessageData messageData)
    {
      _messageData = messageData;
      
      if (_notificationActive)
      {
        StopAllCoroutines();
        
        gameObject.SetActive(true);
        StartCoroutine(nameof(Reset));
        
        return;
      }

      gameObject.SetActive(true);
      
      StartCoroutine(nameof(ActivateAndHide));
    }

    private IEnumerator Reset()
    {
      yield return StartCoroutine(nameof(Hide));

      StartCoroutine(nameof(ActivateAndHide));
    }
    
    private IEnumerator ActivateAndHide()
    {
      _notificationActive = true;
      _initialPosition = transform.position;
      
      Handheld.Vibrate();
      AudioSource.PlayClipAtPoint(messageRingtone, _initialPosition);

      userName.text = _messageData.UserName;
      message.text = _messageData.Text;

      _elapsedTime = 0;
      while (_elapsedTime < Duration)
      {
        _elapsedTime += Time.deltaTime;
        
        var t = _elapsedTime / Duration;
        var newPosition = Vector3.Lerp(_initialPosition, _activePosition, t);
        transform.position = newPosition;

        yield return null;
      }
      
      yield return new WaitForSeconds(5);
      
      StartCoroutine(nameof(Hide));
    }

    private IEnumerator Hide()
    {
      _initialPosition = transform.position;
      
      _elapsedTime = 0;
      while (_elapsedTime < Duration)
      {
        _elapsedTime += Time.deltaTime;
        
        var t = _elapsedTime / Duration;
        var newPosition = Vector3.Lerp(_initialPosition, _inactivePosition, t);
        transform.position = newPosition;

        yield return null;
      }
      
      _notificationActive = false;
    }
    
    private void OpenMessage()
    {
      StopAllCoroutines();

      StartCoroutine(nameof(Hide));
      
      Clicked?.Invoke(_messageData.UserName);
    }
  }
}