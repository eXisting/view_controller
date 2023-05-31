using Component;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Screen
{
    public class CallScreen : Page
    {
        [SerializeField] private Button back;
    
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private GameObject typeMachineGO;
        [SerializeField] private TMP_Text userName;
        [SerializeField] private TMP_Text subtitles;

        private TypeMachine _typeMachine;

        private void Awake()
        {
            videoPlayer.Prepare();
        }

        private void Start()
        {
            _typeMachine = typeMachineGO.GetComponent<TypeMachine>();
      
            var data = Communicator.Signals.Pop();
            Prepare(data.UserId, data.VideoId, data.Message);
      
            back.onClick.AddListener(Back);
            videoPlayer.Play();
            _typeMachine.StartTypewriter();
        }

        private void Prepare(string userName, string videoName, string message)
        {
            this.userName.text = userName;
            subtitles.text = message;
        }
    
        private void Back()
        {
            videoPlayer.Stop();
            Navigator.Open(Enum.Screen.Main);
        }
    }
}