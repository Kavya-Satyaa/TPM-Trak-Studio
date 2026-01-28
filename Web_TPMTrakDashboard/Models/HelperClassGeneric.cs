using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.Models
{
    public class HelperClassGeneric
    {

        static string[] formats = new string[] { "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy HH:mm", "dd-MM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy HH:mm", "dd-MMM-yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "dd-MM-yyyyTHH:mm:ss", "dd-MM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm", "dd-MMM-yyyyTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm", "dd-MM-yyyy HH:mm:ss tt", "dd-MM-yyyy H:mm:ss tt", "d-MM-yyyy HH:mm:ss tt", "d-M-yyyy hh:mm:ss tt", "d-M-yyyy HH:mm:ss tt", "MM-dd-yyyy HH:mm:ss tt", "M-d-yyyy HH:mm:ss tt", "yyyy-MM-dd HH:mm:ss tt", "yyyy-M-d HH:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "dd/MM/yyyy HH:mm:ss", "M/dd/yyyy hh:mm:ss tt", "MM/d/yyyy hh:mm:ss tt", "M/d/yyyy hh:mm:ss tt", "M/dd/yyyy h:mm:ss tt", "MM/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm:ss tt", "MM/dd/yyyy h:mm:ss tt", "dd-MM-yyyy HH:mm:ss.fff", "HH:mm:ss", "yyyy-MMM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.fff", "d/MM/yyyy", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "M/d/yyyy", "MM/d/yyyy", "dd-MMM-yy" };
        public static DateTime GetDateTime(string strDatetime)
        {
            DateTime datetime = DateTime.Now;
            if (!DateTime.TryParseExact(strDatetime.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datetime))
            {
                datetime = DateTime.Now;
                Logger.WriteErrorLog(string.Format("Not able to convert datetime string {0} to DateTime", strDatetime));
            }
            return datetime;
        }
        public static void openWarningToastrModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openToastrWarning", "toasterWarningMsg('" + msg + ".','');", true);
        }
        public static void openInsertErrorModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "error", "openErrorModal_1('Failed to insert record.');", true);
        }
        public static void openInsertSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Record saved Successfully.','');", true);
        }
        public static void openUpdateSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Record updated Successfully.','');", true);
        }
        public static void clearModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "confirmModal", "clearAllModalScreen();", true);
        }
        public static void openWarningModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openWarning", "openWarningModal_1('" + msg + ".');", true);
        }
        public static void openErrorModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openWarning", "openErrorModal_1('" + msg + ".');", true);
        }
        public static void openModal(Page page, string modalid, bool isAnimationRequired)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "openModal", "openModal('" + modalid + "', '" + isAnimationRequired + "');", true);
        }
        public static void openSuccessModal(Page page, string msg)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('" + msg + "','');", true);
        }
        public static void openDeleteSuccessModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "successMsg", "showSuccessMsg('Record deleted Successfully.','');", true);
        }
        public static void openDeleteErrorModal(Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "error", "openErrorModal_1('Failed to delete record.');", true);
        }
        public static void setDropdownValue(DropDownList ddl, string value)
        {
            try
            {
                if (ddl != null)
                {
                    if (ddl.Items.FindByValue(value) != null)
                    {
                        ddl.SelectedValue = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public static void setListBoxValue(ListBox listBox, string value)
        {
            try
            {
                if (listBox != null)
                {
                    if (listBox.Items.FindByValue(value) != null)
                    {
                        listBox.SelectedValue = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public static void clearListBoxValue(ListBox listBox)
        {
            try
            {
                if (listBox != null)
                {
                    foreach (ListItem item in listBox.Items)
                    {
                        item.Selected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
        }
        public static string getListBoxValueWithCommaSeparator(ListBox listBox)
        {
            string value = "";
            try
            {
                if (listBox != null)
                {
                    foreach (ListItem item in listBox.Items)
                    {
                        if (item.Selected)
                        {
                            if (value == "")
                                value += "'" + item.Value + "'";
                            else
                                value += ",'" + item.Value + "'";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return value;
        }
        public static string getListBoxValueWithoutSingleQuotes(ListBox listBox)
        {
            string value = "";
            try
            {
                if (listBox != null)
                {
                    foreach (ListItem item in listBox.Items)
                    {
                        if (item.Selected)
                        {
                            if (value == "")
                                value += item.Value;
                            else
                                value += "," + item.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return value;
        }
        public static Double getDoubleValueFromString(string value)
        {
            Double valueInDouble = 0;
            try
            {
                if (value != "")
                {
                    valueInDouble = Convert.ToDouble(value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return valueInDouble;
        }
        public static Int32 getIntValueFromString(string value)
        {
            Int32 valueInInt = 0;
            try
            {
                if (value != "")
                {
                    valueInInt = Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return valueInInt;
        }
        public static bool getCheckboxValue(string value)
        {
            bool isChecked = false;
            try
            {
                if (value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1")
                {
                    isChecked = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return isChecked;
        }
        public static string getAbbreviatedMonthName(string monthNumber)
        {
            string value = "";
            try
            {
                int monthInInt = Convert.ToInt32(monthNumber);
                value = new DateTime(2010, monthInInt, 1).ToString("MMM", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return value;
        }

        public static string getMonthNumberFromAbbMonthName(string MonthName)
        {
            try
            {
                if (MonthName.Equals("Jan", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "01";
                }
                else if (MonthName.Equals("Feb", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "02";
                }
                else if (MonthName.Equals("Mar", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "03";
                }
                else if (MonthName.Equals("Apr", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "04";
                }
                else if (MonthName.Equals("May", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "05";
                }
                else if (MonthName.Equals("Jun", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "06";
                }
                else if (MonthName.Equals("Jul", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "07";
                }
                else if (MonthName.Equals("Aug", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "08";
                }
                else if (MonthName.Equals("Sep", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "09";
                }
                else if (MonthName.Equals("Oct", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "10";
                }
                else if (MonthName.Equals("Nov", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "11";
                }
                else if (MonthName.Equals("Dec", StringComparison.OrdinalIgnoreCase))
                {
                    MonthName = "12";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
            return MonthName;
        }
        public static string getListboxSelectedValue(ListBox listBox)
        {
            string value = "";
            try
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (item.Selected)
                    {
                        if (value == "")
                        {
                            value += item.Value;
                        }
                        else
                        {
                            value += "," + item.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return value;
        }
        internal static string getDropdownValueWithCommaSeparator(DropDownList dropDownList)
        {
            string Value = "";
            try
            {
                if (dropDownList.SelectedValue.ToString().Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ListItem item in dropDownList.Items)
                    {
                        if (item.Value != "ALL" && item.Value != "All")
                        {
                            if (Value == "")
                                Value += "" + item.Value + "";
                            else
                                Value += "," + item.Value + "";
                        }
                    }
                }
                else
                {
                    Value += "" + dropDownList.SelectedValue + "";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("getDropdownValueWithCommaSeparator = " + ex.ToString());
            }
            return Value;
        }

        public static bool IsValueNumeric(string input)
        {
            bool output = false;
            try
            {
                bool isNumeric = int.TryParse(input, out int integerValue); 
                if (isNumeric)
                {
                    output = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("IsValueNumeric" + ex.Message);
            }
            return output;
        }

        public static bool IsValueDecimal(string input)
        {
            bool output = false;
            try
            { 
                bool IsDecimal = decimal.TryParse(input, out decimal decimalValue);
                if ( IsDecimal)
                {
                    output = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("IsValueDecimal" + ex.Message);
            }
            return output;
        }
    }
}