using System;
using System.Collections.Generic;
using Component;
using Component.Communicators;
using DTO;
using Enum;
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
        [SerializeField] private TMP_Text userName;
        [SerializeField] private TMP_Text subtitles;

        private ViewSignal _viewSignal = new(ViewOperation.Call, "Creator", "Enjoy your life");

        private readonly Dictionary<string, VideoClip> _videoCalls = new();

        private void Awake()
        {
            var videoCallsArray = Resources.LoadAll<VideoClip>("VideoCalls");

            foreach (var video in videoCallsArray) 
                _videoCalls.Add(video.name, video);
        }

        private void OnEnable()
        {
            if (HeadControl.Instance.Communicator.Calls.Count != 0)
                _viewSignal = HeadControl.Instance.Communicator.Calls.Pop();
            Prepare(_viewSignal.UserName, _viewSignal.VideoId, _viewSignal.Message);
            
            videoPlayer.Play();
            subtitles.gameObject.SetActive(true);
        }

        private void Start()
        {
            back.onClick.AddListener(Back);
        }

        private void OnDisable() => 
            subtitles.gameObject.SetActive(false);

        private void Prepare(string userName, string videoId, string message)
        {
            if (videoId != null && _videoCalls.TryGetValue(videoId, out var videoClip))
                videoPlayer.clip = videoClip;
            videoPlayer.Prepare();

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