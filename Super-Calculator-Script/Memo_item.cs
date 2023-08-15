using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memo_item : MonoBehaviour
{
    public Text txt_result;
    public int index;

    public void click()
    {
        GameObject.Find("App").GetComponent<Calculation_history>().show_memo(this.index);
    }

    public void btn_summation()
    {
        GameObject.Find("App").GetComponent<Calculation_history>().memo_summation(this);
    }


    public void btn_subtraction()
    {
        GameObject.Find("App").GetComponent<Calculation_history>().memo_subtraction(this);
    }

    public void btn_delete()
    {
        GameObject.Find("App").GetComponent<Calculation_history>().del_memo(this.index);
    }
}
