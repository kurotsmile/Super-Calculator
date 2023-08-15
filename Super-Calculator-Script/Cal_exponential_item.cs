using UnityEngine;
using UnityEngine.UI;

public class Cal_exponential_item: MonoBehaviour
{
    public Transform area_exponential;
    public GameObject obj_n;
    public GameObject obj_question;
    public int num_exponential;

    public void load_number(string s_number)
    {
        this.num_exponential = int.Parse(s_number);
        this.obj_question.GetComponent<Image>().color = GameObject.Find("App").GetComponent<Calculator_mode>().color_digit;
    }

    public void add_n(int s)
    {
        this.obj_question.SetActive(false);
        GameObject o_n = Instantiate(this.obj_n);
        o_n.transform.SetParent(this.area_exponential);
        o_n.transform.localScale = new Vector3(1f, 1f, 1f);
        o_n.transform.localPosition = new Vector3(o_n.transform.localPosition.x, o_n.transform.localPosition.y, o_n.transform.localPosition.z);
        o_n.GetComponent<n_calculation>().img.sprite = GameObject.Find("App").GetComponent<App>().n_calculation_sprite[s];
        o_n.GetComponent<n_calculation>().img.color = GameObject.Find("App").GetComponent<Calculator_mode>().color_digit;
        o_n.GetComponent<n_calculation>().s_val = s.ToString();
        o_n.GetComponent<RectTransform>().sizeDelta = new Vector3(25f,o_n.GetComponent<RectTransform>().sizeDelta.y);

        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2((o_n.GetComponent<RectTransform>().sizeDelta.x+3f)*this.area_exponential.childCount, this.transform.GetComponent<RectTransform>().sizeDelta.y);
        GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(false);
        GameObject.Find("App").GetComponent<App>().area_Panel_result.gameObject.SetActive(true);
    }

    public string get_result()
    {
        if (this.area_exponential.childCount == 0)
        {
            Destroy(this.gameObject); 
            return "";
        }

        string s_exponential="";
        float size_col = 0f;
        foreach(Transform c in this.area_exponential)
        {
            size_col = size_col + 9f;
            c.GetComponent<RectTransform>().sizeDelta = new Vector2(8f, c.GetComponent<RectTransform>().sizeDelta.y);
            s_exponential = s_exponential+c.GetComponent<n_calculation>().s_val;
        }
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size_col * this.area_exponential.childCount, this.transform.GetComponent<RectTransform>().sizeDelta.y);

        int n_exponential = int.Parse(s_exponential);
        int r_result = 1;
        for (int i= 0;i<n_exponential; i++)
        {
            r_result = r_result * this.num_exponential;
        }

        return r_result.ToString();
    }
}
