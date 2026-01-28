using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
    public class SettingsGUI
    {
        private AppUISettings generalSettings = null;
        private TableUISetting tableSettings = null;
        private ColorUISetting EffiColor = null;
        private IconicUISetting iconicSettings = null;
        private string user = string.Empty;

        public SettingsGUI()
        {
            user = (HttpContext.Current.Session == null || HttpContext.Current.Session["UserName"] == null) ? "none" : HttpContext.Current.Session["UserName"].ToString();
        }
        public AppUISettings AppUISettings
        {
            get
            {
                if (HttpContext.Current.Session["GeneralSettings"] == null)
                {
                    HttpContext.Current.Session["GeneralSettings"] = generalSettings = AndonCockpitView.ViewApplicationUISettings(user);
                }
                else
                {
                    generalSettings = HttpContext.Current.Session["GeneralSettings"] as AppUISettings;
                }
                return generalSettings;
            }
        }

        public IconicUISetting IconicUISetting
        {
            get
            {
                if (HttpContext.Current.Session["IconicViewSettings"] == null)
                {
                    HttpContext.Current.Session["IconicViewSettings"] = iconicSettings = AndonCockpitView.ViewIconicUISettings(user);
                }
                else
                {
                    iconicSettings = HttpContext.Current.Session["IconicViewSettings"] as IconicUISetting;
                }
                return iconicSettings;
            }
        }

        public TableUISetting TableUISetting
        {
            get
            {
                if (HttpContext.Current.Session["TableViewSettings"] == null)
                {
                    HttpContext.Current.Session["TableViewSettings"] = tableSettings = AndonCockpitView.ViewTableUISettings(user);
                }
                else
                {
                    tableSettings = HttpContext.Current.Session["TableViewSettings"] as TableUISetting;
                }
                return tableSettings;
            }
        }

        public ColorUISetting ColorUISetting
        {
            get
            {
                if (HttpContext.Current.Session["ColorViewSettings"] == null)
                {
                    HttpContext.Current.Session["ColorViewSettings"] = EffiColor = AndonCockpitView.ViewColorSettingData(user);
                }
                else
                {
                    EffiColor = HttpContext.Current.Session["ColorViewSettings"] as ColorUISetting;
                }
                return EffiColor;
            }
        }
       

        public void RefreshSettings()
        {
            HttpContext.Current.Session["GeneralSettings"] = generalSettings = AndonCockpitView.ViewApplicationUISettings(user);
            HttpContext.Current.Session["TableViewSettings"] = tableSettings = AndonCockpitView.ViewTableUISettings(user);
            HttpContext.Current.Session["ColorViewSettings"] = EffiColor = AndonCockpitView.ViewColorSettingData(user);
            HttpContext.Current.Session["IconicViewSettings"] = iconicSettings = AndonCockpitView.ViewIconicUISettings(user);
        }

    }
}