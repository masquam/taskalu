using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace Taskalu
{
    class MainWindowUtil
    {
        public static void setLanguageSettings()
        {
            string settingvalue = Properties.Settings.Default.Language_Setting;

            if (settingvalue == "ja-JP")
            {
                CultureInfo.CurrentCulture = new CultureInfo("ja-JP");
                CultureInfo.CurrentUICulture = new CultureInfo("ja-JP");
            }
            else if (settingvalue == "en-US")
            {
                CultureInfo.CurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            }
            else
            {
                if (CultureInfo.CurrentUICulture.Name == "ja-JP")
                {
                    CultureInfo.CurrentCulture = new CultureInfo("ja-JP");
                    CultureInfo.CurrentUICulture = new CultureInfo("ja-JP");
                    AddUpdateAppSettings("Language_Setting", "ja-JP");
                }
                else
                {
                    CultureInfo.CurrentCulture = new CultureInfo("en-US");
                    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                    AddUpdateAppSettings("Language_Setting", "en-US");
                }
            }
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }


    }
}
