using UnityEngine;
using UnityEngine.UI;

public class Calculator_mode : MonoBehaviour
{
    [Header("Main obj")]
    public App app;

    [Header("Setting")]
    public Sprite icon_light_on;
    public Sprite icon_light_off;
    public Sprite icon_theme;
    public Sprite icon_theme_item;
    public Sprite icon_memo_on;
    public Sprite icon_memo_off;
    private Carrot.Carrot_Box_Item item_light = null;
    private Carrot.Carrot_Box_Item item_memo = null;

    [Header("Mode Obj")]
    public GameObject panel_memo_bar_portaint;
    public GameObject panel_memo_bar_landspace;
    public GameObject[] btn_mode;
    public GameObject[] btn_cal_full;
    public GameObject[] btn_cal_supper;
    public GridLayoutGroup gridLayout_cal;
    public AudioSource[] sound;

    [Header("Theme")]
    public Sprite icon_cart;
    public Theme_Item[] theme_item;
    public string[] theme_name;
    public string[] theme_tip;
    public Color32 color_btn_mode_nomal;
    public Color32 color_btn_mode_select;
    public Color32 color_btn_mode_txt;
    public Color32 color_bk;
    public Color32 color_menu_bar;
    public Color32 color_dashboard;
    public Color32 color_btn;
    public Color32 color_btn_txt;
    public Color32 color_btn_txt_tip;
    public Color32 color_bk_show;
    public Color32 color_bk_calculation;
    public Color32 color_bk_result;
    public Color32 color_digit;
    public Color32 color_btn_result;
    public Color32 color_btn_c;
    public Color32 color_btn_number;
    private int sel_index_theme=0;

    [Header("Theme Obj change")]
    public Image Obj_img_bk;
    public Image Obj_img_menu_bar;
    public Image Obj_img_dashboard;
    public Image[] Obj_img_btn;
    public Image[] Obj_img_btn_number;
    public Image[] Obj_txt_icon_btn;
    public Text[] Obj_txt_btn;
    public Text[] Obj_txt_tip_btn;
    public Image Obj_cal_show;
    public Image Obj_cal_calculation;
    public Image Obj_cal_result;
    public Image Obj_btn_result;
    public Image Obj_btn_c;

    [Header("Object Roate Scene")]
    public Transform area_main_landspace_left;
    public Transform area_dashboard_landspace_left;
    public Transform area_dashboard_landspace_right;
    public Transform area_dashboard_portaint;

    private int sel_mode = 0;
    private bool is_light=true;
    private bool is_memo = true;
    private int index_buy_theme = -1;

    private Carrot.Carrot_Box box_theme = null;

    public void load()
    {
        this.sel_mode = PlayerPrefs.GetInt("sel_mode", 0);

        this.sel_index_theme = PlayerPrefs.GetInt("sel_index_theme", 0);
        this.sel_theme(this.sel_index_theme);

        if (PlayerPrefs.GetInt("is_light", 0) == 0) this.is_light = true; else this.is_light = false;
        this.check_status_light();

        if (PlayerPrefs.GetInt("is_memo", 1) == 0) this.is_memo = true; else this.is_memo = false;
        this.check_status_memo();
    }

    public void select_mod(int index_mode)
    {
        this.play_sound(1);
        this.sel_mode = index_mode;
        PlayerPrefs.SetInt("sel_mode", index_mode);
        this.check_mode();
    }

    private void check_mode()
    {
        for (int i = 0; i < this.btn_mode.Length; i++)this.btn_mode[i].GetComponent<Image>().color =this.color_btn_mode_nomal;
        this.btn_mode[this.sel_mode].GetComponent<Image>().color = this.color_btn_mode_select;

        if (this.Obj_img_bk.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < this.btn_cal_full.Length; i++) this.btn_cal_full[i].SetActive(false);
            for (int i = 0; i < this.btn_cal_supper.Length; i++) this.btn_cal_supper[i].SetActive(false);
        }

        if (this.sel_mode == 0)
        {
            this.gridLayout_cal.cellSize = new Vector2(110f,100f);
        }
       
        if(this.sel_mode==1)
        {
            for (int i = 0; i < this.btn_cal_full.Length; i++) this.btn_cal_full[i].SetActive(true);
            this.gridLayout_cal.cellSize = new Vector2(110f,68f);
        }

        if (this.sel_mode==2)
        {
            for (int i = 0; i < this.btn_cal_full.Length; i++) this.btn_cal_full[i].SetActive(true);
            for (int i = 0; i < this.btn_cal_supper.Length; i++) this.btn_cal_supper[i].SetActive(true);
            if(this.is_memo)
                this.gridLayout_cal.cellSize = new Vector2(110f, 45f);
            else
                this.gridLayout_cal.cellSize = new Vector2(110f, 50f);
        }
    }

    public void show_setting()
    {
        Carrot.Carrot_Box box_setting=this.app.carrot.Create_Setting();

        this.item_light=box_setting.create_item("light_item");
        if(this.is_light)
            item_light.set_icon(this.icon_light_on);
        else
            item_light.set_icon(this.icon_light_off);
        item_light.set_title("Backlight");
        item_light.set_tip("Turn off or disable always wake screen mode");
        item_light.set_act(() => this.change_status_light());
        item_light.transform.SetSiblingIndex(0);

        Carrot.Carrot_Box_Item item_theme = box_setting.create_item("item_theme");
        item_theme.set_icon(this.icon_theme);
        item_theme.set_title("Pocket calculator theme");
        item_theme.set_tip("Change the look of the app to your liking");
        item_theme.set_act(()=>this.show_theme());
        item_theme.transform.SetSiblingIndex(1);

        this.item_memo= box_setting.create_item("item_memo");
        if (this.is_memo)
            item_memo.set_icon(this.icon_memo_on);
        else
            item_memo.set_icon(this.icon_memo_off);

        item_memo.set_title("Memo toolbar");
        item_memo.set_tip("Enable or disable the use of the memo bar");
        item_memo.set_act(() => this.change_status_memo());
        item_memo.transform.SetSiblingIndex(2);
    }

    public void play_sound(int index_sound)
    {
        if(this.app.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void change_status_light()
    {
        if (this.is_light)
        {
            PlayerPrefs.SetInt("is_light", 1);
            this.is_light = false;
        }
        else
        {
            PlayerPrefs.SetInt("is_light", 0);
            this.is_light = true;
            
        }
        this.play_sound(1);
        this.check_status_light();
    }

    private void check_status_light()
    {
        if (this.is_light)
        {
            if (this.item_light != null) this.item_light.set_icon(this.icon_light_on);
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        }
        else
        {
            if (this.item_light != null) this.item_light.set_icon(this.icon_light_off);
            Screen.sleepTimeout = 5000;
        }
            
    }

    public void change_status_memo()
    {
        if (this.is_memo)
        {
            PlayerPrefs.SetInt("is_memo", 1);
            this.is_memo = false;
        }
        else
        {
            PlayerPrefs.SetInt("is_memo", 0);
            this.is_memo = true;

        }
        this.play_sound(1);
        this.check_status_memo();
    }

    private void check_status_memo()
    {
        if (this.is_memo)
        {
            this.panel_memo_bar_portaint.SetActive(true);
            this.panel_memo_bar_landspace.SetActive(true);
            if (this.sel_mode == 2) this.check_mode();
            if (this.item_memo != null) this.item_memo.set_icon(this.icon_memo_on);
        }
        else
        {
            this.panel_memo_bar_portaint.SetActive(false);
            this.panel_memo_bar_landspace.SetActive(false);
            if (this.item_memo != null) this.item_memo.set_icon(this.icon_memo_off);
        }
    }

    public void show_theme()
    {
        this.play_sound(1);
        this.box_theme = this.app.carrot.Create_Box();
        this.box_theme.set_title("Pocket Calculator Theme");
        this.box_theme.set_icon(this.icon_theme);

        for(int i = 0; i < this.theme_name.Length; i++)
        {
            var index_theme = i;
            Carrot.Carrot_Box_Item item_theme=this.box_theme.create_item("item_theme_" + i);
            item_theme.set_title(this.theme_name[i]);
            item_theme.set_tip(this.theme_tip[i]);
            item_theme.set_icon(this.icon_theme_item);

            if(PlayerPrefs.GetInt("buy_theme_" + i) != 1)
            {
                if (this.theme_item[i].is_buy)
                {
                    item_theme.set_act(() => this.sel_item_buy_theme(index_theme));

                    Carrot.Carrot_Box_Btn_Item btn_buy = item_theme.create_item();
                    btn_buy.set_icon(this.icon_cart);
                    btn_buy.set_icon_color(Color.white);
                    btn_buy.set_color(this.app.carrot.color_highlight);
                    Destroy(btn_buy.GetComponent<Button>());
                }
                else
                {
                    item_theme.set_act(() => this.item_sel_theme(index_theme));
                }
            }
            else
            {
                item_theme.set_act(() => this.item_sel_theme(index_theme));
            }

            if (this.sel_index_theme == i)
            {
                Carrot.Carrot_Box_Btn_Item btn_check = item_theme.create_item();
                btn_check.set_color(this.app.carrot.color_highlight);
                btn_check.set_icon(this.app.carrot.icon_carrot_done);
                Destroy(btn_check.GetComponent<Button>());
            }
   
        }
    }

    private void item_sel_theme(int index_theme)
    {
        PlayerPrefs.SetInt("sel_index_theme",index_theme);
        this.sel_theme(index_theme);
        this.play_sound(1);
        if (this.box_theme != null) this.box_theme.close();
    }

    public void sel_theme(int index_theme)
    {
        this.app.carrot.set_color(theme_item[index_theme].color_btn_mode_select);
        this.color_btn_mode_nomal = this.theme_item[index_theme].color_btn_mode_nomal;
        this.color_btn_mode_select = this.theme_item[index_theme].color_btn_mode_select;
        this.color_bk = this.theme_item[index_theme].color_bk;
        this.color_menu_bar = this.theme_item[index_theme].color_menu_bar;
        this.color_dashboard = this.theme_item[index_theme].color_dashboard;
        this.color_btn = this.theme_item[index_theme].color_btn;
        this.color_btn_txt = this.theme_item[index_theme].color_btn_txt;
        this.color_bk_show= this.theme_item[index_theme].color_bk_show;
        this.color_bk_calculation= this.theme_item[index_theme].color_bk_calculation;
        this.color_bk_result= this.theme_item[index_theme].color_bk_result;
        this.color_digit = this.theme_item[index_theme].color_digit;
        this.color_btn_txt_tip = this.theme_item[index_theme].color_btn_txt_tip;
        this.color_btn_result= this.theme_item[index_theme].color_btn_result;
        this.color_btn_c = this.theme_item[index_theme].color_btn_c;
        this.color_btn_number = this.theme_item[index_theme].color_btn_number;
        this.check_mode();
        this.check_theme();
    }

    private void check_theme()
    {
        this.Obj_img_bk.color = this.color_bk;
        this.GetComponent<Carrot.Carrot_DeviceOrientationChange>().emp_Landscape[0].GetComponent<Image>().color = this.color_bk;
        this.Obj_img_menu_bar.color = this.color_menu_bar;
        this.Obj_img_dashboard.color = this.color_dashboard;
        this.Obj_cal_show.color = this.color_bk_show;
        this.Obj_cal_result.color = this.color_bk_result;
        this.Obj_cal_calculation.color = this.color_bk_calculation;
        this.Obj_btn_result.color = this.color_btn_result;
        this.Obj_btn_c.color = this.color_btn_c;

        for(int i=0;i<this.Obj_img_btn.Length;i++) this.Obj_img_btn[i].color = this.color_btn;
        for(int i = 0;i<this.Obj_txt_btn.Length;i++) this.Obj_txt_btn[i].color = this.color_btn_txt;
        for (int i = 0; i < this.Obj_txt_icon_btn.Length; i++) this.Obj_txt_icon_btn[i].color = this.color_btn_txt;
        for (int i = 0; i < this.Obj_txt_tip_btn.Length; i++) this.Obj_txt_tip_btn[i].color = this.color_btn_txt_tip;
        for (int i = 0; i < this.Obj_img_btn_number.Length; i++) this.Obj_img_btn_number[i].color = this.color_btn_number;

        foreach(Transform c in this.GetComponent<App>().area_Panel_result)if (c.GetComponent<n_calculation>() != null)c.GetComponent<n_calculation>().img.color = this.color_digit;
        foreach (Transform c in this.GetComponent<App>().area_Panel_calculation) if (c.GetComponent<n_calculation>() != null) c.GetComponent<n_calculation>().img.color = this.color_digit;
    }


    public void check_roate_scene()
    {
        this.GetComponent<App>().carrot.delay_function(0.2f, this.change_rotate_scene);
    }

    private void change_rotate_scene()
    {
        bool is_portain = this.Obj_img_bk.gameObject.activeInHierarchy;
        this.GetComponent<App>().set_roate_scene(is_portain);
        if (is_portain)
        {
            this.Obj_cal_show.transform.SetParent(this.Obj_img_bk.transform);
            this.Obj_cal_show.transform.SetSiblingIndex(0);

            RectTransform r = this.Obj_cal_show.GetComponent<RectTransform>();

            r.anchorMin = new Vector2(0f, 1f);
            r.pivot = new Vector2(0.5f, 1f);
            r.offsetMin = new Vector2(5, r.offsetMin.y);
            r.offsetMax = new Vector2(-5, r.offsetMax.y);
            r.offsetMax = new Vector2(r.offsetMax.x, -68f);
            r.offsetMin = new Vector2(r.offsetMin.x, -213.4f);

            for (int i = 0; i < this.Obj_img_btn.Length; i++) this.Obj_img_btn[i].transform.SetParent(this.area_dashboard_portaint);
            for (int i = 0; i < this.Obj_img_btn_number.Length; i++) this.Obj_img_btn_number[i].transform.SetParent(this.area_dashboard_portaint);

            this.Obj_btn_c.transform.SetParent(this.area_dashboard_portaint);
            this.Obj_btn_c.transform.SetSiblingIndex(24);

            this.Obj_img_btn[31].transform.SetSiblingIndex(25);

            this.Obj_img_btn_number[0].transform.SetSiblingIndex(28);
            this.Obj_img_btn_number[1].transform.SetSiblingIndex(29);
            this.Obj_img_btn_number[2].transform.SetSiblingIndex(30);
            this.Obj_img_btn[30].transform.SetSiblingIndex(31);
            this.Obj_img_btn_number[3].transform.SetSiblingIndex(32);
            this.Obj_img_btn_number[4].transform.SetSiblingIndex(33);
            this.Obj_img_btn_number[5].transform.SetSiblingIndex(34);
            this.Obj_img_btn[26].transform.SetSiblingIndex(35);
            this.Obj_img_btn_number[6].transform.SetSiblingIndex(36);
            this.Obj_img_btn_number[7].transform.SetSiblingIndex(37);
            this.Obj_img_btn_number[8].transform.SetSiblingIndex(38);
            this.Obj_img_btn[27].transform.SetSiblingIndex(39);
            this.Obj_img_btn[28].transform.SetSiblingIndex(40);
            this.Obj_img_btn_number[9].transform.SetSiblingIndex(41);
            this.Obj_img_btn[29].transform.SetSiblingIndex(42);
            this.Obj_btn_result.transform.SetParent(this.area_dashboard_portaint);
            this.Obj_btn_result.transform.SetSiblingIndex(43);

            this.check_mode();
        }
        else
        {
            this.Obj_cal_show.transform.SetParent(this.area_main_landspace_left);
            this.Obj_cal_show.transform.SetSiblingIndex(0);

            RectTransform r = this.Obj_cal_show.GetComponent<RectTransform>();

            r.anchorMin = new Vector2(0f, 1f);
            r.pivot = new Vector2(0.5f, 1f);
            r.offsetMin = new Vector2(0, r.offsetMin.y);
            r.offsetMax = new Vector2(0, r.offsetMax.y);
            r.offsetMax = new Vector2(r.offsetMax.x, -20f);
            r.offsetMin = new Vector2(r.offsetMin.x, -178f);

            for (int i = 0; i < this.Obj_img_btn_number.Length; i++)
            {
                this.Obj_img_btn_number[i].gameObject.SetActive(true);
                this.Obj_img_btn_number[i].transform.SetParent(this.area_dashboard_landspace_left);
            }
            for (int i = 0; i < this.Obj_img_btn.Length; i++)
            {
                this.Obj_img_btn[i].gameObject.SetActive(true);
                this.Obj_img_btn[i].transform.SetParent(this.area_dashboard_landspace_right);
            }

            this.Obj_img_btn[25].transform.SetParent(this.area_dashboard_landspace_left);
            this.Obj_img_btn[25].transform.SetSiblingIndex(3);
            this.Obj_img_btn[28].transform.SetParent(this.area_dashboard_landspace_left);
            this.Obj_img_btn[28].transform.SetSiblingIndex(13);
            this.Obj_img_btn[29].transform.SetParent(this.area_dashboard_landspace_left);
            this.Obj_img_btn[29].transform.SetSiblingIndex(14);
            this.Obj_img_btn[26].transform.SetParent(this.area_dashboard_landspace_left);
            this.Obj_img_btn[26].transform.SetSiblingIndex(10);
            this.Obj_img_btn[30].transform.SetParent(this.area_dashboard_landspace_left);
            this.Obj_img_btn[30].transform.SetSiblingIndex(7);
            this.Obj_img_btn[27].transform.SetParent(this.area_dashboard_landspace_left);
            this.Obj_img_btn[27].transform.SetSiblingIndex(15);
            this.Obj_img_btn_number[9].transform.SetSiblingIndex(13);

            this.Obj_btn_result.transform.SetParent(this.area_dashboard_landspace_right);
            this.Obj_btn_result.transform.SetSiblingIndex(24);
            this.Obj_btn_c.transform.SetParent(this.area_dashboard_landspace_right);
            this.Obj_btn_c.transform.SetSiblingIndex(25);
        }
    }

    public void btn_rate()
    {
        this.app.carrot.show_rate();
    }

    public void btn_share()
    {
        this.app.carrot.show_share();
    }

    public void btn_remove_ads()
    {
        this.app.carrot.buy_inapp_removeads();
    }

    private void sel_item_buy_theme(int index_theme)
    {
        this.index_buy_theme = index_theme;
        this.app.carrot.buy_product(2);
    }

    public void pay_success(string s_id)
    {
        if (s_id == this.app.carrot.shop.get_id_by_index(2))
        {
            if (this.index_buy_theme != -1)
            {
                PlayerPrefs.SetInt("buy_theme_" + this.index_buy_theme, 1);
                this.sel_theme(this.index_buy_theme);
                this.app.carrot.show_msg("Theme", "Buy skins for pocket calculator successfully!", Carrot.Msg_Icon.Success);
                this.index_buy_theme = -1;
                if (this.box_theme != null) this.box_theme.close();
            }
        }
    }
}
