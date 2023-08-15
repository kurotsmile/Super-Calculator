using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculation_history : MonoBehaviour
{
    [Header("Obj Main")]
    public App app;

    [Header("History")]
    public Sprite icon_history;
    public Image img_icon_history;
    public GameObject prefab_cal_history;

    private List<string> h_s_cal=new List<string>();
    private List<string> h_s_rel=new List<string>();

    [Header("Memo")]
    public Transform area_body_menu_portrait;
    public Transform area_body_menu_landspace;
    public GameObject memo_item_prefab;
    public Sprite icon_memo;
    public Button[] btn_memo;

    private int leng_memo = 0;
    private int sel_index_memo = -1;
    private Carrot.Carrot_Box box;

    void Start()
    {
        this.img_icon_history.color = Color.gray;
        this.leng_memo = PlayerPrefs.GetInt("leng_memo", 0);
        if (this.leng_memo==0)
        {
            this.sel_index_memo = 0;
            this.act_btn_mc(false);
        }
        else
            this.act_btn_mc(true);
    }

    public void show_history()
    {
        this.box=this.app.carrot.Create_Box("Calculation History", this.icon_history);
        this.box.set_icon(this.icon_history);
        for (int i = this.h_s_cal.Count-1; i>=0; i--)
        {
            Carrot.Carrot_Box_Item item_history = this.box.create_item("item_" + i);
            item_history.set_title(this.h_s_cal[i]);
            item_history.set_tip("=" + this.h_s_rel[i]);
        }
    }

    public void add_history(string s_cal,string s_result)
    {
        this.h_s_cal.Add(s_cal);
        this.h_s_rel.Add(s_result);
        this.img_icon_history.color = Color.black;
    }


    public void show_list_memo()
    {
        this.box=this.app.carrot.Create_Box("Memo", this.icon_memo);
        for(int i = this.leng_memo; i>=0; i--)
        {
            if (PlayerPrefs.GetString("memo_" + i, "") != "")
            {
                Carrot.Carrot_Box_Item item_memo = this.box.create_item("item_memo_" + i);
                item_memo.set_title(PlayerPrefs.GetString("memo_" + i));
                /*
                obj_memo.GetComponent<Memo_item>().txt_result.text = ;
                obj_memo.GetComponent<Memo_item>().index = i;
                */
            }
        }
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void memo_mc()
    {
        this.act_btn_mc(false);
        this.leng_memo = 0;
        PlayerPrefs.DeleteKey("leng_memo");
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void memo_ms()
    {
        string s_result = this.GetComponent<App>().get_s_result_for_memo();

        PlayerPrefs.SetString("memo_" + this.leng_memo, s_result);
        this.sel_index_memo = this.leng_memo;
        this.leng_memo++;
        PlayerPrefs.SetInt("leng_memo", this.leng_memo);
        this.act_btn_mc(true);
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void show_memo(int index_show)
    {
        this.sel_index_memo = index_show;
        this.GetComponent<App>().show_result(PlayerPrefs.GetString("memo_" +index_show));
        this.box.close();
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void del_memo(int index_memo)
    {
        PlayerPrefs.DeleteKey("memo_" + index_memo);
        this.show_list_memo();
    }

    public void memo_summation(Memo_item mm_item)
    {
        string s = PlayerPrefs.GetString("memo_" + mm_item.index);
        string s_cal_result = this.GetComponent<App>().get_s_result_for_memo();
        string s_new_result = (int.Parse(s) + int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + mm_item.index, s_new_result);
        mm_item.txt_result.text = s_new_result;
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void memo_subtraction(Memo_item mm_item)
    {
        string s = PlayerPrefs.GetString("memo_" + mm_item.index);
        string s_cal_result = this.GetComponent<App>().get_s_result_for_memo();
        string s_new_result = (int.Parse(s) - int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + mm_item.index, s_new_result);
        mm_item.txt_result.text = s_new_result;
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void btn_memo_summation()
    {
        int last_index_memo = this.leng_memo-1;
        if (this.sel_index_memo != -1) last_index_memo = this.sel_index_memo;

        string s = PlayerPrefs.GetString("memo_" + last_index_memo,"0");
        string s_cal_result = this.GetComponent<App>().get_s_result_for_memo();
        string s_new_result = (int.Parse(s) + int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + last_index_memo, s_new_result);
        this.act_btn_mc(true);
        this.GetComponent<Calculator_mode>().play_sound(1);
    }


    public void btn_memo_subtraction()
    {
        int last_index_memo = this.leng_memo - 1;
        if (this.sel_index_memo != -1) last_index_memo = this.sel_index_memo;
        string s = PlayerPrefs.GetString("memo_" + last_index_memo,"0");
        string s_cal_result = this.GetComponent<App>().get_s_result_for_memo();
        string s_new_result = (int.Parse(s) - int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + last_index_memo, s_new_result);
        this.act_btn_mc(true);
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    public void btn_memo_recall()
    {
        this.GetComponent<App>().show_result(PlayerPrefs.GetString("memo_" + (this.leng_memo-1)));
        this.GetComponent<Calculator_mode>().play_sound(1);
    }

    private void act_btn_mc(bool is_act)
    {
        this.btn_memo[0].interactable = is_act;
        this.btn_memo[1].interactable = is_act;
        this.btn_memo[5].interactable = is_act;
    }


    public void set_btn_memo_roate_scene(bool is_portrait)
    {
        if (is_portrait)
            for (int i = 0; i < this.btn_memo.Length; i++) this.btn_memo[i].transform.SetParent(this.area_body_menu_portrait);
        else
            for (int i = 0; i < this.btn_memo.Length; i++) this.btn_memo[i].transform.SetParent(this.area_body_menu_landspace);

    }
}