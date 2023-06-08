using System;
using System.Collections.Generic;
using Component;
using Component.Communicators;
using DTO;
using Enum;
using TMPro;
using UI;
using UI.Calls;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Screen
{
    public class CallScreen : Page
    {
        [SerializeField] private Button back;
    
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private TMP_Text userName;

        [SerializeField] private TypeMachine typeMachine;

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
            Prepare(_viewSignal);
            
            videoPlayer.Play();
            typeMachine.gameObject.SetActive(true);
        }

        private void Start()
        {
            back.onClick.AddListener(Back);
        }

        private void OnDisable() => 
            typeMachine.gameObject.SetActive(false);

        private void Prepare(ViewSignal signal)
        {
            if (signal.VideoId != null && _videoCalls.TryGetValue(signal.VideoId, out var videoClip))
                videoPlayer.clip = videoClip;
            videoPlayer.Prepare();
            videoPlayer.SetDirectAudioMute(0, signal.MuteVideo);
            videoPlayer.isLooping = signal.LoopVideo;

            userName.text = signal.UserName;
            typeMachine.PrepareSubtitles(signal.Subtitles);
        }
    
        private void Back()
        {
            videoPlayer.Stop();
            Navigator.Open(Enum.Screen.Main);
        }
    }
}