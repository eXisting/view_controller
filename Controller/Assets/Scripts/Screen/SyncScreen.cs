using Component;
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
            connect.onClick.AddListener(Connect);
        }

        private void Connect()
        {
            Communicator.Connect(address.text);
            Navigator.Open(Enum.Screen.Main);
        }

        private void OnDestroy() => 
            connect.onClick.RemoveListener(Connect);
    }
}
