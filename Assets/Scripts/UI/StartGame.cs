using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Framework.AudioType;

public class StartGame : MonoBehaviour
{
    public Button startBtn;
    public Button quitBtn;

    private void Start()
    {
        AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.BGM,"BGM2",true));
        startBtn.onClick.AddListener((() =>
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.BGM,"BGM",true));
            ScenesManager.Instance.LoadSceneSync(SceneType.main);
        }));
        
        quitBtn.onClick.AddListener((Application.Quit));
    }
}
