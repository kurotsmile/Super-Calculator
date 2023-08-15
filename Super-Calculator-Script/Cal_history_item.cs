using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cal_history_item : MonoBehaviour
{
    public Text txt_cal;
    public Text txt_result;

    public void click()
    {
        GameObject.Find("App").GetComponent<App>().show_history_cal(this.txt_cal.text, this.txt_result.text.Replace("=",""));
    }
}
