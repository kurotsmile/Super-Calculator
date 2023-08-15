using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cal_Element_Item : MonoBehaviour
{
    public Transform area_Numerator;
    public Transform area_Denominator;
    public GameObject obj_n;
    public GameObject obj_question;
    public int num_exponential;
    public Image img_obj;

    private int count_Denominator = 0;
    private List<GameObject> list_Numerator=new List<GameObject>();

    public void load_number(List<GameObject> list_n)
    {
        GameObject.Find("App").GetComponent<App>().carrot.clear_contain(this.area_Numerator);
        for(int i = 0; i < list_n.Count; i++)
        {
            list_n[i].transform.SetParent(this.area_Numerator);
            list_n[i].GetComponent<RectTransform>().sizeDelta = new Vector2(22f, list_n[i].GetComponent<RectTransform>().sizeDelta.y);
        }

        this.list_Numerator = list_n;

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(list_n.Count * 30f, this.GetComponent<RectTransform>().sizeDelta.y);
        GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(false);
        GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(true);
        this.img_obj.color = GameObject.Find("App").GetComponent<Calculator_mode>().color_digit;
    }

    public void add_Denominator(int n)
    {
        Destroy(this.obj_question);
        GameObject o_n = Instantiate(this.obj_n);
        o_n.transform.SetParent(this.area_Denominator);
        o_n.transform.localScale = new Vector3(1f, 1f, 1f);
        o_n.transform.localPosition = new Vector3(o_n.transform.localPosition.x, o_n.transform.localPosition.y, o_n.transform.localPosition.z);
        o_n.GetComponent<n_calculation>().img.sprite = GameObject.Find("App").GetComponent<App>().n_calculation_sprite[n];
        o_n.GetComponent<n_calculation>().img.color = GameObject.Find("App").GetComponent<Calculator_mode>().color_digit;
        o_n.GetComponent<n_calculation>().s_val = n.ToString();
        o_n.GetComponent<RectTransform>().sizeDelta = new Vector3(22f, o_n.GetComponent<RectTransform>().sizeDelta.y);
        this.count_Denominator++;
        if (this.count_Denominator > this.list_Numerator.Count)
        {
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.count_Denominator * 30f, this.GetComponent<RectTransform>().sizeDelta.y);
            GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(false);
            GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(true);
        }
    }

    public string get_result()
    {
        string cal_1 = "";
        string cal_2 = "";

        foreach (Transform c in this.area_Numerator)
        {
            c.GetComponent<RectTransform>().sizeDelta = new Vector2(8f, c.GetComponent<RectTransform>().sizeDelta.y);
            cal_1 = cal_1 + c.GetComponent<n_calculation>().s_val;
        }

        foreach (Transform c in this.area_Denominator)
        {
            c.GetComponent<RectTransform>().sizeDelta = new Vector2(8f, c.GetComponent<RectTransform>().sizeDelta.y);
            cal_2 = cal_2 + c.GetComponent<n_calculation>().s_val;
        }

        if (this.count_Denominator > this.list_Numerator.Count)
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.count_Denominator * 10f, this.GetComponent<RectTransform>().sizeDelta.y);
        else
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.list_Numerator.Count * 10f, this.GetComponent<RectTransform>().sizeDelta.y);

        GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(false);
        GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(true);
        return (int.Parse(cal_1) / int.Parse(cal_2)).ToString();
    }

}
