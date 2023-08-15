using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Calculator_mode : MonoBehaviour
{
    [Header("Ads")]
    public string ID_ads_unity = "4486311";
    public string ID_ads_app_window_vungle= "61b84e22b28013ba23e272c3";
    public string ID_ads_app_android_vungle= "61daef67f2e0e2c30164f5aa";
    public string ID_ads_placement_window_vungle = "DEFAULT-8252319";
    public string ID_ads_placement_android_vungle = "DEFAULT-5913801";
    public string ID_ads_placement_unity = "Interstitial_Android";
    private int count_ads = 0;

    [Header("Mode Obj")]
    public GameObject panel_setting;
    public GameObject panel_memo_bar_portaint;
    public GameObject panel_memo_bar_landspace;
    public GameObject[] btn_mode;
    public GameObject[] btn_cal_full;
    public GameObject[] btn_cal_supper;
    public GridLayoutGroup gridLayout_cal;
    public AudioSource[] sound;

    [Header("Setting")]
    public Sprite sp_sound_on;
    public Sprite sp_sound_off;
    public Sprite sp_light_on;
    public Sprite sp_light_off;
    public Sprite sp_memo_on;
    public Sprite sp_memo_off;
    public Text txt_setting_status_sound;
    public Text txt_setting_status_light;
    public Text txt_setting_status_memo;
    public Image img_setting_status_sound;
    public Image img_setting_status_light;
    public Image img_setting_status_memo;
    public GameObject panel_setting_remove_ads;

    [Header("Theme")]
    public GameObject panel_theme;
    public Theme_Item[] theme_item;
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
    private Carrot.Carrot carrot;
    private bool is_sound=true;
    private bool is_light=true;
    private bool is_ads = true;
    private bool is_memo = true;

    private void Start()
    {
        if (PlayerPrefs.GetInt("is_ads", 0) == 0)
        {
            this.is_ads = true;

#if UNITY_ANDROID
            Advertisement.Initialize(this.ID_ads_unity, false);
            Vungle.init(this.ID_ads_app_android_vungle);
#elif UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
            Vungle.init(this.ID_ads_app_window_vungle);
#endif

            this.panel_setting_remove_ads.SetActive(true);
            this.btn_mode[7].SetActive(true);
            this.btn_mode[8].SetActive(false);
        }
        else
        {
            this.panel_setting_remove_ads.SetActive(false);
            this.btn_mode[7].SetActive(false);
            this.btn_mode[8].SetActive(true);
            this.is_ads = false;
        }

        this.sel_mode = PlayerPrefs.GetInt("sel_mode", 0);

        this.sel_index_theme = PlayerPrefs.GetInt("sel_index_theme", 0);
        this.sel_theme(this.sel_index_theme);

        if (PlayerPrefs.GetInt("is_sound", 0) == 0) this.is_sound = true; else this.is_sound = false;
        this.check_status_sound();

        if (PlayerPrefs.GetInt("is_light", 0) == 0) this.is_light = true; else this.is_light = false;
        this.check_status_light();

        if (PlayerPrefs.GetInt("is_memo", 1) == 0) this.is_memo = true; else this.is_memo = false;
        this.check_status_memo();

        this.carrot = this.GetComponent<App>().carrot;
        this.panel_setting.SetActive(false);
        this.panel_theme.SetActive(false);
        
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
        this.play_sound(1);
        this.panel_setting.SetActive(true);
    }

    public void close_setting()
    {
        this.play_sound(1);
        this.panel_setting.SetActive(false);
    }

    public void app_rate()
    {
        this.carrot.show_rate();
    }

    public void app_list_other()
    {
        this.carrot.show_list_carrot_app();
    }

    public void app_share()
    {
        this.carrot.show_share();
    }

    public void play_sound(int index_sound)
    {
        if(this.is_sound) this.sound[index_sound].Play();
    }

    public void change_status_sound()
    {
        if (this.is_sound)
        {
            PlayerPrefs.SetInt("is_sound", 1);
            this.is_sound = false;
        }
        else
        {
            PlayerPrefs.SetInt("is_sound", 0);
            this.is_sound = true;
            this.play_sound(1);
        }

        this.check_status_sound();
    }

    private void check_status_sound()
    {
        if (this.is_sound)
        {
            this.img_setting_status_sound.sprite = this.sp_sound_on;
            this.txt_setting_status_sound.text = "On";
        }
        else
        {
            this.img_setting_status_sound.sprite = this.sp_sound_off;
            this.txt_setting_status_sound.text = "Off";
        }
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
            this.img_setting_status_light.sprite = this.sp_light_on;
            this.txt_setting_status_light.text = "On";
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        }
        else
        {
            this.img_setting_status_light.sprite = this.sp_light_off;
            this.txt_setting_status_light.text = "Off";
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
            this.img_setting_status_memo.sprite = this.sp_memo_on;
            this.txt_setting_status_memo.text = "On";
            this.panel_memo_bar_portaint.SetActive(true);
            this.panel_memo_bar_landspace.SetActive(true);
            if (this.sel_mode == 2) this.check_mode();
        }
        else
        {
            this.img_setting_status_memo.sprite = this.sp_memo_off;
            this.txt_setting_status_memo.text = "Off";
            this.panel_memo_bar_portaint.SetActive(false);
            this.panel_memo_bar_landspace.SetActive(false);
        }
    }

    public void btn_buy_product(int index_p)
    {
        this.carrot.buy_product(index_p);
        this.play_sound(1);
    }

    public void check_and_show_ads()
    {
        if (this.is_ads)
        {
            this.count_ads++;
            if (this.count_ads >= 15)
            {
#if UNITY_ANDROID
                if(Advertisement.IsReady(this.ID_ads_placement_unity))
                    Advertisement.Show(this.ID_ads_placement_unity);
                else
                    Vungle.playAd(this.ID_ads_placement_android_vungle);

#elif UNITY_WSA_10_0 || UNITY_WINRT_8_1 || UNITY_METRO
                Vungle.playAd(this.ID_ads_placement_window_vungle);
#endif
                this.count_ads++;
            }
        }
    }

    public void check_exit_app()
    {
        if (this.panel_theme.activeInHierarchy)
        {
            this.close_theme();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_setting.activeInHierarchy)
        {
            this.close_setting();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void on_success_carrot_pay(string s_id)
    {
        if (s_id == this.carrot.shop.get_id_by_index(0))
        {
            this.carrot.show_msg(PlayerPrefs.GetString("shop", "Shop"), "Remove ads successfully");
            this.in_app_removeAds();
        }
    }

    public void on_success_carrot_restore(string[] arr_id)
    {
        for (int i = 0; i < arr_id.Length; i++)
        {
            string s_id_p = arr_id[i];
            if (s_id_p == this.carrot.shop.get_id_by_index(0)) this.in_app_removeAds();
        }
    }

    private void in_app_removeAds()
    {
        this.is_ads = false;
        PlayerPrefs.SetInt("is_ads", 1);
        this.panel_setting_remove_ads.SetActive(true);
        this.btn_mode[7].SetActive(false);
        this.btn_mode[8].SetActive(true);
    }

    public void buy_success(Product product)
    {
        this.on_success_carrot_pay(product.definition.id);
    }

    public void app_restore()
    {
        this.carrot.shop.restore_product();
        this.play_sound(1);
    }

    public void show_theme()
    {
        this.panel_theme.SetActive(true);
        this.play_sound(1);
    }

    public void close_theme()
    {
        this.panel_theme.SetActive(false);
        this.play_sound(1);
    }

    public void btn_sel_theme(int index_theme)
    {
        PlayerPrefs.SetInt("sel_index_theme",index_theme);
        this.sel_theme(index_theme);
        this.play_sound(1);
        this.panel_theme.SetActive(false);
    }

    public void sel_theme(int index_theme)
    {
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
            r.offsetMax = new Vector2(r.offsetMax.x, -55f);
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
}
