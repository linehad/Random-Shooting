using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour
{
    public BTNType currentType;
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.Tutorial:
                Debug.Log("튜토리얼");
                break;
            case BTNType.Easy:
                Debug.Log("쉬움 난이도");
                break;
            case BTNType.Normal:
                SceneManager.LoadScene(1);
                break;
            case BTNType.Hard:
                Debug.Log("어려움 난이도");
                break;
        }
    }
}
