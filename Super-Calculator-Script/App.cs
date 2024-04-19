using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [Header("Obj Main")]
    public Carrot.Carrot carrot;
    public Calculator_mode mode;
    public Text txt_result_debug;

    [Header("Cal Prefab")]
    public GameObject n_calculation_prefab;
    public GameObject exponential_cal_prefab;
    public GameObject Element_cal_prefab;

    [Header("Obj Calulation")]
    public Sprite[] n_calculation_sprite;
    public Sprite[] n_func_sprite;
    public Button[] btn_block_func_x;
    public Transform area_Panel_result;
    public Transform area_Panel_calculation;
    public GameObject obj_result_error;

    [Header("Sound")]
    public AudioSource sound_background;

    private List<GameObject> n_1 = new List<GameObject>();
    private bool is_zero = true;
    private bool is_negative = false;
    private bool is_exponential = false;
    private bool is_element = false;
    private Cal_exponential_item exponential_temp;
    private Cal_Element_Item element_temp;

    private const double Tau = 6.2831853071795862;

    private void Start()
    {
        this.carrot.Load_Carrot();
        this.carrot.shop.onCarrotPaySuccess += this.mode.pay_success;
        this.carrot.game.load_bk_music(this.sound_background);

        this.mode.load();
        this.mode.check_roate_scene();
        this.carrot.clear_contain(this.area_Panel_calculation);
        this.carrot.clear_contain(this.area_Panel_result);
        this.add_number(0);
        this.obj_result_error.SetActive(false);

        if (this.carrot.model_app == Carrot.ModelApp.Develope)
            this.txt_result_debug.gameObject.SetActive(true);
        else
            this.txt_result_debug.gameObject.SetActive(false);
    }


    public void btn_add_number(int n)
    {
        this.obj_result_error.SetActive(false);

        if (this.is_exponential)
        {
            this.exponential_temp.add_n(n);
        }
        else if (this.is_element)
        {
            this.element_temp.add_Denominator(n);
        }
        else
        {
            if (this.is_zero)
            {
                this.carrot.clear_contain(this.area_Panel_result);
                this.n_1 = new List<GameObject>();
            }

            if (n != 0) this.is_zero = false;

            this.add_number(n);
        }
        this.GetComponent<Calculator_mode>().play_sound(0);
    }

    private void add_number(int n)
    {
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_result);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[n];
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        n_cal_obj.GetComponent<n_calculation>().s_val = n.ToString();
        this.n_1.Add(n_cal_obj);
    }

    public void show_func_cal(string s_func)
    {
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_calculation);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
        if (s_func == "+") n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[0];
        if (s_func == "-") n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[1];
        if (s_func == "*") n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[2];
        if (s_func == "/") n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[3];
        n_cal_obj.GetComponent<n_calculation>().s_val = s_func;
        n_cal_obj.GetComponent<n_calculation>().is_func = true;
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        this.check_math_return();
        this.block_btn_func_x(true);
    }

    public void check_math_return()
    {
        int c_cal = 0;
        foreach (Transform c in this.area_Panel_calculation) if(c.GetComponent<n_calculation>()!=null)if (c.GetComponent<n_calculation>().is_func) c_cal++;

        if (c_cal >= 2)
        {
            string s_cal_1 = "";
            foreach (Transform c in this.area_Panel_calculation)
            {
                if (c.GetComponent<Cal_exponential_item>() != null)
                    s_cal_1 = s_cal_1 + c.GetComponent<Cal_exponential_item>().get_result();
                else if (c.GetComponent<Cal_Element_Item>() != null)
                    s_cal_1 = s_cal_1 + c.GetComponent<Cal_Element_Item>().get_result();
                else
                    s_cal_1 = s_cal_1 + c.GetComponent<n_calculation>().s_val;
            }
            s_cal_1 = s_cal_1.Substring(0, s_cal_1.Length - 1);

            this.show_result(Evaluate(s_cal_1).ToString());
        }
        else
        {
            this.n_1 = new List<GameObject>();
            this.carrot.clear_contain(this.area_Panel_result);
            this.add_number(0);
        }
        this.is_zero = true;
    }

    public void btn_add_func(string s_func)
    {
        if (this.is_zero) this.carrot.clear_contain(this.area_Panel_calculation);

        this.obj_result_error.SetActive(false);
        if (this.is_exponential)
        {
            for (int i = 0; i < this.n_1.Count; i++)
            {
                GameObject n_cal_obj = Instantiate(this.n_1[i]);
                n_cal_obj.transform.SetParent(this.area_Panel_calculation);
                n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
                if (n_cal_obj.GetComponent<n_calculation>().s_val == ".")
                    n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 35f);
                else
                    n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
            }
            this.exponential_temp.transform.SetParent(this.area_Panel_calculation);
            this.show_result(this.exponential_temp.get_result());
            this.is_exponential = false;
        }else if (this.is_element)
        {
            this.element_temp.transform.SetParent(this.area_Panel_calculation);
            this.show_result(this.element_temp.get_result());
            this.is_element = false;
        }
        else
        {
            for (int i = 0; i < this.n_1.Count; i++)
            {
                if (this.n_1[i] != null)
                {
                    GameObject n_cal_obj = Instantiate(this.n_1[i]);
                    n_cal_obj.transform.SetParent(this.area_Panel_calculation);
                    n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
                    if (n_cal_obj.GetComponent<n_calculation>().s_val == ".")
                        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 35f);
                    else
                        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
                }
            }
        }
        this.show_func_cal(s_func);
        this.mode.play_sound(1);
    }


    public void btn_return_results()
    {
        if (this.area_Panel_calculation.childCount == 0)
        {

            string s_cal_1 = "";
            foreach (Transform c in this.area_Panel_result)
            {
                if (c.GetComponent<Cal_exponential_item>() != null)
                {
                    s_cal_1 = s_cal_1 + c.GetComponent<Cal_exponential_item>().get_result();
                }else if (c.GetComponent<Cal_Element_Item>() != null)
                {
                    s_cal_1 = s_cal_1 + c.GetComponent<Cal_Element_Item>().get_result();
                }
                else
                {
                    GameObject n_cal_obj = Instantiate(c.gameObject);
                    n_cal_obj.transform.SetParent(this.area_Panel_calculation);
                    n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
                    if (n_cal_obj.GetComponent<n_calculation>().s_val == ".")
                        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 35f);
                    else
                        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
                    s_cal_1 = s_cal_1 + c.GetComponent<n_calculation>().s_val;
                }
            }

            if(this.exponential_temp!=null) this.exponential_temp.transform.SetParent(this.area_Panel_calculation);
            if(this.element_temp!=null) this.element_temp.transform.SetParent(this.area_Panel_calculation);

            string s_result = s_cal_1;
            this.show_result(s_result);
            this.GetComponent<Calculation_history>().add_history(s_cal_1, s_result);
        }
        else
        {
            string s_cal_1 = "";


                foreach (Transform c in this.area_Panel_calculation)
                {
                    if (c.GetComponent<Cal_exponential_item>() != null)
                        s_cal_1 = s_cal_1 + c.GetComponent<Cal_exponential_item>().get_result();
                    else if (c.GetComponent<Cal_Element_Item>() != null)
                        s_cal_1 = s_cal_1 + c.GetComponent<Cal_Element_Item>().get_result();
                    else
                        s_cal_1 = s_cal_1 + c.GetComponent<n_calculation>().s_val;
                }

            foreach (Transform c in this.area_Panel_result)
            {
                if (c.GetComponent<Cal_exponential_item>() != null)
                    s_cal_1 = s_cal_1 + c.GetComponent<Cal_exponential_item>().get_result();
                else if (c.GetComponent<Cal_Element_Item>() != null)
                    s_cal_1 = s_cal_1 + c.GetComponent<Cal_Element_Item>().get_result();
                else
                    s_cal_1 = s_cal_1 + c.GetComponent<n_calculation>().s_val;
            }

            try
            {
                string s_result = Evaluate(s_cal_1).ToString().Replace(".", ",");
                this.show_result(s_result);
                this.GetComponent<Calculation_history>().add_history(s_cal_1, s_result);
            }
            catch
            {
                this.is_zero = true;
                this.carrot.clear_contain(this.area_Panel_result);
                this.add_number(0);
                this.cal_error();
            }
            
            
            this.carrot.clear_contain(this.area_Panel_calculation);
        }

        this.is_zero = true;
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.carrot.ads.show_ads_Interstitial();
        this.block_btn_func_x(true);
    }


    public void show_result(string s_result, bool is_clear = true)
    {
        if (this.carrot.model_app == Carrot.ModelApp.Develope)
        {
            this.txt_result_debug.text = s_result;
            Debug.Log("Ket qua:" + s_result);
        }
            
        if (s_result == "Infinity"||s_result== "NaN"|| s_result == "-Infinity") {
            this.is_zero = true;
            this.carrot.clear_contain(this.area_Panel_result);
            if(s_result == "Infinity")
            {
                GameObject n_cal_obj = Instantiate(n_calculation_prefab);
                n_cal_obj.transform.SetParent(this.area_Panel_result);
                n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[13];
                n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
                n_cal_obj.GetComponent<n_calculation>().s_val = "0";
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
                this.n_1.Add(n_cal_obj);
            }
            else if(s_result== "-Infinity")
            {
                GameObject n_cal_obj = Instantiate(n_calculation_prefab);
                n_cal_obj.transform.SetParent(this.area_Panel_result);
                n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[14];
                n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
                n_cal_obj.GetComponent<n_calculation>().s_val = "0";
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
                this.n_1.Add(n_cal_obj);
            }
            else
            {
                this.add_number(0);
            }
            this.cal_error();
            return;
        }

        this.obj_result_error.SetActive(false);
        if (is_clear) this.carrot.clear_contain(this.area_Panel_result);

        this.n_1 = new List<GameObject>();
        for (int i = 0; i < s_result.Length; i++)
        {
            if (s_result[i].ToString() == ".")
                this.add_dots();
            else if (s_result[i].ToString() == ",")
                this.add_dots();
            else if (s_result[i].ToString() == "-")
                this.add_negative_number();
            else if (s_result[i].ToString() == "E")
                this.add_e();
            else if (s_result[i].ToString() == "+")
            {
                GameObject n_cal_obj = Instantiate(n_calculation_prefab);
                n_cal_obj.transform.SetParent(this.area_Panel_result);
                n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
                n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[0];
                n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
                n_cal_obj.GetComponent<n_calculation>().s_val = "+";
            }
            else
            {
                int index_n = int.Parse(s_result[i] + "");
                this.add_number(index_n);
            }
        }
    }

    public void btn_c()
    {
        this.carrot.clear_contain(this.area_Panel_calculation);
        this.btn_ce();
    }

    public void btn_ce()
    {
        this.obj_result_error.SetActive(false);
        this.block_btn_func_x(true);
        this.n_1 = new List<GameObject>();
        this.carrot.clear_contain(this.area_Panel_result);
        this.is_zero = true;
        this.is_negative = false;
        this.is_exponential = false;
        this.is_element = false;
        this.add_number(0);
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void btn_del()
    {
        this.obj_result_error.SetActive(false);
        int last_index = this.area_Panel_result.transform.childCount - 1;
        Destroy(this.area_Panel_result.transform.GetChild(last_index).gameObject);
        this.n_1.RemoveAt(last_index);
        if (this.area_Panel_result.childCount == 1)
        {
            this.is_negative = false;
            this.is_zero = true;
            this.add_number(0);
            return;
        }
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void btn_negative_number()
    {
        if (this.is_zero) return;

        if (this.is_negative)
        {
            this.is_negative = false;
            this.show_result(this.get_s_result().Replace("-",""));
        }
        else
        {
            this.is_negative = true;
            this.carrot.clear_contain(this.area_Panel_result);
            this.show_result("-"+this.get_s_result(),false);
        }
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    private void add_negative_number()
    {
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_result);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[5];
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        n_cal_obj.GetComponent<n_calculation>().s_val = "-";
        this.n_1.Add(n_cal_obj);
    }

    private void add_e()
    {
        if (this.is_zero) this.carrot.clear_contain(this.area_Panel_result);
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_result);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[11];
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        n_cal_obj.GetComponent<n_calculation>().s_val = Math.E.ToString();
        this.n_1.Add(n_cal_obj);
    }

    public void btn_add_dots()
    {
        this.add_dots();
        this.is_zero = false;
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    private void add_dots()
    {
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_result);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[4];
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        n_cal_obj.GetComponent<n_calculation>().s_val = ".";
        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
        this.n_1.Add(n_cal_obj);
    }

    static double Evaluate(string expression)
    {
        var loDataTable = new DataTable();
        var loDataColumn = new DataColumn("Eval", typeof(double), expression);
        loDataTable.Columns.Add(loDataColumn);
        loDataTable.Rows.Add(0);
        return (double)(loDataTable.Rows[0]["Eval"]);
    }

    public void show_history_cal(string s_cal,string s_rel)
    {
        this.carrot.clear_contain(this.area_Panel_calculation);
        for (int i = 0; i < s_cal.Length; i++)
        {
            GameObject n_cal_obj = Instantiate(n_calculation_prefab);
            
            n_cal_obj.transform.SetParent(this.area_Panel_calculation);
            n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
            n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);

            if (s_cal[i].ToString() == ".")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[4];
                n_cal_obj.GetComponent<n_calculation>().s_val = ".";
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
            }
            else if (s_cal[i].ToString() == ",")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[4];
                n_cal_obj.GetComponent<n_calculation>().s_val = ".";
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
            }
            else if (s_cal[i].ToString() == "-")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[5];
                n_cal_obj.GetComponent<n_calculation>().s_val = "-";
            }
            else if (s_cal[i].ToString() == "+")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[0];
                n_cal_obj.GetComponent<n_calculation>().s_val = "+";
                n_cal_obj.GetComponent<n_calculation>().is_func = true;
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
            }
            else if (s_cal[i].ToString() == "-")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[1];
                n_cal_obj.GetComponent<n_calculation>().s_val = "-";
                n_cal_obj.GetComponent<n_calculation>().is_func = true;
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
            }
            else if (s_cal[i].ToString() == "*")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[2];
                n_cal_obj.GetComponent<n_calculation>().s_val = "*";
                n_cal_obj.GetComponent<n_calculation>().is_func = true;
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
            }
            else if (s_cal[i].ToString() == "/")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[3];
                n_cal_obj.GetComponent<n_calculation>().s_val = "/";
                n_cal_obj.GetComponent<n_calculation>().is_func = true;
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
            }
            else if (s_cal[i].ToString() == "E")
            {
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_func_sprite[11];
                n_cal_obj.GetComponent<n_calculation>().s_val = ".";
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
            }
            else
            {
                int index_n = int.Parse(s_cal[i] + "");
                n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[index_n];
                n_cal_obj.GetComponent<n_calculation>().s_val = index_n.ToString();
                n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 35f);
            }
        }
        this.show_result(s_rel);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.carrot.close();
    }

    public void btn_exponential()
    {
        if (this.is_zero) return;
        this.is_exponential = true;
        GameObject obj_exponential = Instantiate(this.exponential_cal_prefab);
        obj_exponential.transform.SetParent(this.area_Panel_result);
        obj_exponential.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_exponential.transform.localPosition = new Vector3(obj_exponential.transform.localPosition.x, obj_exponential.transform.localPosition.y, obj_exponential.transform.localPosition.z);
        obj_exponential.GetComponent<Cal_exponential_item>().load_number(this.get_s_result());
        this.exponential_temp=obj_exponential.GetComponent<Cal_exponential_item>();
        this.GetComponent<Calculator_mode>().play_sound(1);
        this.block_btn_func_x(false);
    }

    public string get_s_result_for_memo()
    {
        string s_result="";
        foreach (Transform child in this.area_Panel_result)
        {
            if (child.GetComponent<n_calculation>() != null) s_result = s_result + child.GetComponent<n_calculation>().s_val;
        }
        return s_result;
    }

    public string get_s_result()
    {
        string s_result = "";
        foreach (Transform child in this.area_Panel_result)
        {
            if (child.GetComponent<n_calculation>() != null)
            {
                s_result = s_result + child.GetComponent<n_calculation>().s_val;
                child.GetComponent<n_calculation>().s_val = "";
            }
        }
        return s_result;
    }

    public void btn_round()
    {
        string s=Mathf.Round(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_cos()
    {
        string s = Mathf.Cos(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_sin()
    {
        string s = Mathf.Sin(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_tan()
    {
        string s = Mathf.Tan(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_Element()
    {
        if (this.is_zero) return;
        this.is_element = true;
        GameObject obj_element = Instantiate(this.Element_cal_prefab);
        obj_element.transform.SetParent(this.area_Panel_result);
        obj_element.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_element.transform.localPosition = new Vector3(obj_element.transform.localPosition.x, obj_element.transform.localPosition.y, obj_element.transform.localPosition.z);
        obj_element.GetComponent<Cal_Element_Item>().load_number(this.n_1);
        this.element_temp = obj_element.GetComponent<Cal_Element_Item>();
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.block_btn_func_x(false);
    }

    public void btn_Abs()
    {
        string s = Mathf.Abs(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_log()
    {
        string s = Mathf.Log(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_Tau()
    {
        if (this.is_zero) this.carrot.clear_contain(this.area_Panel_result);
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_result);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[12];
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        n_cal_obj.GetComponent<n_calculation>().s_val = Tau.ToString();
        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector3(40f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
        this.n_1.Add(n_cal_obj);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_E()
    {
        this.add_e();
        this.is_zero = false;
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void btn_Asinh()
    {
        string s = System.Math.Asin(double.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_Cosh()
    {
        string s = System.Math.Cosh(double.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_Tanh()
    {
        string s = System.Math.Tanh(double.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_Atan2()
    {
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_Sign()
    {
        string s = Mathf.Sign(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_log10()
    {
        string s = Mathf.Log10(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
    }

    public void btn_Floor()
    {
        string s = Mathf.Floor(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_Cbrt()
    {
        string s = Mathf.Sqrt(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_Exp()
    {
        string s = Mathf.Exp(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_Ceil()
    {
        string s = Mathf.Ceil(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_acos()
    {
        string s = Mathf.Acos(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_asin()
    {
        string s = Mathf.Asin(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_atan()
    {
        string s = Mathf.Atan(float.Parse(this.get_s_result())).ToString();
        this.show_result(s);
        this.GetComponent<Calculator_mode>().play_sound(2);
        this.is_zero = true;
    }

    public void btn_pi()
    {
        if (this.is_zero) this.carrot.clear_contain(this.area_Panel_result);
        GameObject n_cal_obj = Instantiate(n_calculation_prefab);
        n_cal_obj.transform.SetParent(this.area_Panel_result);
        n_cal_obj.transform.localScale = new Vector3(1f, 1f, 1f);
        n_cal_obj.transform.localPosition = new Vector3(n_cal_obj.transform.localPosition.x, n_cal_obj.transform.localPosition.y, n_cal_obj.transform.localPosition.z);
        n_cal_obj.GetComponent<n_calculation>().img.sprite = this.n_calculation_sprite[10];
        n_cal_obj.GetComponent<n_calculation>().img.color = this.GetComponent<Calculator_mode>().color_digit;
        n_cal_obj.GetComponent<n_calculation>().s_val = Mathf.PI.ToString();
        n_cal_obj.GetComponent<RectTransform>().sizeDelta = new Vector3(40f, n_cal_obj.GetComponent<RectTransform>().sizeDelta.y);
        this.n_1.Add(n_cal_obj);
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    private void block_btn_func_x(bool is_act)
    {
        for (int i = 0; i < this.btn_block_func_x.Length; i++) this.btn_block_func_x[i].interactable = is_act;
    }

    public void set_input_zero()
    {
        this.is_zero = true;
    }

    [ContextMenu("Delete all data")]
    public void delete_all_data()
    {
        PlayerPrefs.DeleteAll();
        this.carrot.Delete_all_data();
    }

    public void set_roate_scene(bool is_portrait)
    {
        this.GetComponent<Calculation_history>().set_btn_memo_roate_scene(is_portrait);
    }

    public void cal_error()
    {
        this.obj_result_error.SetActive(true);
        this.obj_result_error.GetComponent<Image>().color = this.GetComponent<Calculator_mode>().color_digit;
        this.GetComponent<Calculator_mode>().play_sound(3);
        this.carrot.play_vibrate();
    }
}
