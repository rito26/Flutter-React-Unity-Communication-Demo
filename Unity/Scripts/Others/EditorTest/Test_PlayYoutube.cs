using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;
using Rito.ut23;

public class Test_PlayYoutube : MonoBehaviour
{
    private VideoPlayer _vp;

    [SerializeField] private RaycastButton _playButton;
    [SerializeField] private RaycastButton _pauseButton;
    [SerializeField] private RaycastButton _stopButton;
    [SerializeField] private LayerMask _buttonLayer;
    [SerializeField] private string _url = "https://www.youtube.com/watch?v=1PuGuqpHQGo";

    private void Awake()
    {
        TryGetComponent(out _vp);
    }

    private void Start()
    {
        InitVideo();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        if (Rito.ut23.Raycaster.Cast.CamToMouse(out var hit, 9999f, _buttonLayer))
        {
            bool h = hit.transform.TryGetComponent(out RaycastButton targetButton);
            if (h)
            {
                if (targetButton == _playButton && _vp.isPrepared)
                {
                    _vp.Play();
                    Debug.Log("Play");
                }
                else if (targetButton == _pauseButton)
                {
                    _vp.Pause();
                    Debug.Log("Pause");
                }
                else if (targetButton == _stopButton)
                {
                    _vp.Stop();
                    Debug.Log("Stop");
                    _vp.Prepare();
                }
            }
        }
    }

    private async void InitVideo()
    {
        await _vp.PlayYoutubeVideoAsync(_url);
        _vp.Stop();
        _vp.Prepare();
    }

}
