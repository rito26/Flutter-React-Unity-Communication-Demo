using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
//using Codice.CM.Common.Merge;

public class UnityReactCommunication : MonoBehaviour
{
    // 통신 규격
    [DllImport("__Internal")] private static extern void UnityToReact(string action, string data);

    private void ReactToUnity_MoveUp(string data)
    {
        inputToMoveTr.Translate(Vector2.up * 10f);
        DebugReactToUnity("MoveUp", data);
    }

    private void ReactToUnity_MoveDown(string data)
    {
        inputToMoveTr.Translate(Vector2.down * 10f);
        DebugReactToUnity("MoveDown", data);
    }

    private void DebugReactToUnity(string action, string data)
    {
        Debug.Log($"[Unity: React -> Unity] Action: {action}, Data: {data}");
        txtReactToUnity.text = $"[React -> Unity]\n - Action: {action}\n - Data: {data}";
    }

    // -

    public Button unityToReactCallButton;
    public TextMeshProUGUI txtUnityToReact;
    public TextMeshProUGUI txtReactToUnity;
    public Transform inputToMoveTr;

    public void UnityToReactCall<T>(string action, T data)
    {
        string strData = data.ToString();

#if UNITY_WEBGL == true && UNITY_EDITOR == false
        UnityToReact (action, strData);
#endif
        Debug.Log($"[Unity: Unity -> React] Action: {action}, Data: {strData}");
        txtUnityToReact.text = $"[Unity -> React]\n - Action: {action}\n - Data: {data}";
    }

    void Start()
    {
        //StartCoroutine(nameof(HeartBeatRoutine));
        unityToReactCallButton.onClick.AddListener(() => UnityToReactCall("ButtonClicked - float value", "1234.56"));
    }

    private IEnumerator HeartBeatRoutine()
    {
        yield return new WaitForSeconds(5f);

        int sec = 0;
        const int CYCLE = 10;
        while (true)
        {
            yield return new WaitForSeconds(CYCLE);
            sec += CYCLE;
            UnityToReactCall("Heartbeat", sec);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityToReactCall("Keydown", "Space");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            inputToMoveTr.Translate(Vector2.left * 10f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            inputToMoveTr.Translate(Vector2.right * 10f);
            UnityToReactCall("Keydown - D", "D");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UnityToReactCall("Keydown - W", "W");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("[Unity] Just Log");
        }
    }
}
