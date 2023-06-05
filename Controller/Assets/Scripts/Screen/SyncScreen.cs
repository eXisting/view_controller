using Component;
using Component.Communicators;
using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screen
{
    public class SyncScreen : Page
    {
        [SerializeField] private Button connect;
        [SerializeField] private TMP_InputField address;

        private void Start()
        {
            if (HeadControl.Instance.communicatorType == CommunicatorType.Server)
            {
                address.text = ((Server)HeadControl.Instance.Communicator).GetLocalIPAddress();
                address.DeactivateInputField();
                connect.GetComponentInChildren<TMP_Text>().text = "Ready";
            }

            connect.onClick.AddListener(Connect);
        }

        private void Connect()
        {
            if (HeadControl.Instance.communicatorType == CommunicatorType.Client)
                HeadControl.Instance.Communicator.Start(address.text);
            Navigator.Open(Enum.Screen.Main);
        }

        private void OnDestroy() => 
            connect.onClick.RemoveListener(Connect);
    }
}
