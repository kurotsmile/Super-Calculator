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

    private List<string> h_s_cal=new List<string>();
    private List<string> h_s_rel=new List<string>();

    [Header("Memo")]
    public Transform area_body_menu_portrait;
    public Transform area_body_menu_landspace;
    public Sprite icon_memo;
    public Sprite icon_memo_summation;
    public Sprite icon_memo_subtraction;
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
        if (this.box != null) this.box.close();
        this.box=this.app.carrot.Create_Box("Calculation History", this.icon_history);
        this.box.set_icon(this.icon_history);
        for (int i = this.h_s_cal.Count-1; i>=0; i--)
        {
            Carrot.Carrot_Box_Item item_history = this.box.create_item("item_" + i);
            item_history.set_title(this.h_s_cal[i]);
            item_history.set_tip("=" + this.h_s_rel[i]);
            item_history.set_icon(this.icon_history);

            string s_cal = this.h_s_cal[i];
            string s_result = this.h_s_rel[i];
            item_history.set_act(() => this.app.show_history_cal(s_cal, s_result));
        }
        this.box.update_color_table_row();
    }

    public void add_history(string s_cal,string s_result)
    {
        this.h_s_cal.Add(s_cal);
        this.h_s_rel.Add(s_result);
        this.img_icon_history.color = Color.black;
    }


    public void show_list_memo()
    {
        if (this.box != null) this.box.close();
        this.box=this.app.carrot.Create_Box("Memo", this.icon_memo);
        for(int i = this.leng_memo; i>=0; i--)
        {
            if (PlayerPrefs.GetString("memo_" + i, "") != "")
            {
                var index_memo = i;
                Carrot.Carrot_Box_Item item_memo = this.box.create_item("item_memo_" + i);
                item_memo.set_title(PlayerPrefs.GetString("memo_" + i));
                item_memo.set_tip("Results have been saved");
                item_memo.set_icon(this.icon_memo);
                item_memo.set_act(() => this.show_memo(index_memo));

                Carrot.Carrot_Box_Btn_Item btn_summation = item_memo.create_item();
                btn_summation.set_icon(this.icon_memo_summation);
                btn_summation.set_color(this.app.carrot.color_highlight);
                btn_summation.set_act(() => this.memo_summation(index_memo, item_memo));

                Carrot.Carrot_Box_Btn_Item btn_subtraction = item_memo.create_item();
                btn_subtraction.set_icon(this.icon_memo_subtraction);
                btn_subtraction.set_color(this.app.carrot.color_highlight);
                btn_subtraction.set_act(() => this.memo_subtraction(index_memo, item_memo));

                Carrot.Carrot_Box_Btn_Item btn_del=item_memo.create_item();
                btn_del.set_icon(this.app.carrot.sp_icon_del_data);
                btn_del.set_act(() => this.del_memo(index_memo));
                btn_del.set_color(this.app.carrot.color_highlight);
            }
        }
        this.app.mode.play_sound(1);
        this.box.update_color_table_row();
    }

    public void memo_mc()
    {
        this.act_btn_mc(false);
        this.leng_memo = 0;
        PlayerPrefs.DeleteKey("leng_memo");
        this.app.mode.play_sound(1);
    }

    public void memo_ms()
    {
        string s_result = this.app.get_s_result_for_memo();

        PlayerPrefs.SetString("memo_" + this.leng_memo, s_result);
        this.sel_index_memo = this.leng_memo;
        this.leng_memo++;
        PlayerPrefs.SetInt("leng_memo", this.leng_memo);
        this.act_btn_mc(true);
        this.app.mode.play_sound(1);
    }

    public void show_memo(int index_show)
    {
        this.sel_index_memo = index_show;
        this.GetComponent<App>().show_result(PlayerPrefs.GetString("memo_" +index_show));
        if(this.box!=null) this.box.close();
        this.app.mode.play_sound(1);
    }

    public void del_memo(int index_memo)
    {
        PlayerPrefs.DeleteKey("memo_" + index_memo);
        this.show_list_memo();
    }

    public void memo_summation(int index,Carrot.Carrot_Box_Item item_change)
    {
        string s = PlayerPrefs.GetString("memo_" + index);
        string s_cal_result = this.app.get_s_result_for_memo();
        string s_new_result = (int.Parse(s) + int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + index, s_new_result);
        item_change.set_title(s_new_result);
        this.app.mode.play_sound(1);
    }

    public void memo_subtraction(int index, Carrot.Carrot_Box_Item item_change)
    {
        string s = PlayerPrefs.GetString("memo_" + index);
        string s_cal_result = this.app.get_s_result_for_memo();
        string s_new_result = (int.Parse(s) - int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + index, s_new_result);
        item_change.set_title(s_new_result);
        this.app.mode.play_sound(1);
    }

    public void btn_memo_summation()
    {
        int last_index_memo = this.leng_memo-1;
        if (this.sel_index_memo != -1) last_index_memo = this.sel_index_memo;

        string s = PlayerPrefs.GetString("memo_" + last_index_memo,"0");
        string s_cal_result = this.app.get_s_result_for_memo();
        string s_new_result = (int.Parse(s) + int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + last_index_memo, s_new_result);
        this.act_btn_mc(true);
        this.app.mode.play_sound(1);
    }


    public void btn_memo_subtraction()
    {
        int last_index_memo = this.leng_memo - 1;
        if (this.sel_index_memo != -1) last_index_memo = this.sel_index_memo;
        string s = PlayerPrefs.GetString("memo_" + last_index_memo,"0");
        string s_cal_result = this.app.get_s_result_for_memo();
        string s_new_result = (int.Parse(s) - int.Parse(s_cal_result)).ToString();
        PlayerPrefs.SetString("memo_" + last_index_memo, s_new_result);
        this.act_btn_mc(true);
        this.app.mode.play_sound(1);
    }

    public void btn_memo_recall()
    {
        this.app.show_result(PlayerPrefs.GetString("memo_" + (this.leng_memo-1)));
        this.app.mode.play_sound(1);
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
