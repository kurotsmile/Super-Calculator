using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class n_calculation : MonoBehaviour
{
    public Image img;
    public string s_val;
    public bool is_func=false;

    public void click()
    {
        Destroy(this.gameObject);
    }
}
