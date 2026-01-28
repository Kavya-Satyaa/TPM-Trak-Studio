using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_TPMTrakDashboard.GenericAndon.Model;

namespace Web_TPMTrakDashboard.VED.Model
{
    public class DTO
    {
    }

    public class ScreenEntityVED
    {
        public string Parameter { get; set; } = string.Empty;
        public string ValueInText { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = false;
        public int SortOrder { get; set; }
    }

    public class AndonSettingEntityVED
    {
        public string FontFamily { get; set; } = "Calibri";
        public string FontStyle { get; set; } = "normal";
        public string AndonTitle { get; set; }
        public int HeaderFontSize { get; set; }
        public int ContentFontSize { get; set; }
        public int DataRefreshInterval { get; set; }
        public int ScreenFlipInterval { get; set; }
        public int ImageEnabled { get; set; }
        public int VideoEnabled { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public int SlideshowInterval { get; set; }

        public int EmojiEnabled { get; set; }
        public int EmojiSize { get; set; }
        public int ShowCurvedBoxes { get; set; }
        public int MsgBlockEnabled { get; set; }
        public int FooterBlockEnabled { get; set; }
        public string ScrollingMsg { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }

        public string orderby { get; set; }
        public string Sortorder { get; set; }
    }


    public class AndonEntity
    {
        public AndonSettingEntityVED andonsetting { get; set; } = new AndonSettingEntityVED();
        public List<ScreenEntityVED> enabledScreenList { get; set; } = new List<ScreenEntityVED>();
         
        public List<HourlyTargetAndonEntity> HourlyTargetData { get; set; } = new List<HourlyTargetAndonEntity>();
         
        public AndonKPIEntity KPIdata { get; set; } = new AndonKPIEntity();

        public DowntimeMainEntity downTimeData { get; set; } = new DowntimeMainEntity();

        public List<CockpitData> genericAndonData { get; set; } = new List<CockpitData>();
    }

    public class AndonKPIEntity
    {
        public string DonutChartFontSize { get; set; }
        public string ColumnChartxAxisFontSize { get; set; }
        public string ColumnChartyAxisFontSize { get; set; }
        public string ColumnChartDataLabelFontSize { get; set; }

        public string EntityID { get; set; }
        public double OEE { get; set; }
        public double AE { get; set; }
        public double PE { get; set; }
        public double QE { get; set; }

        public string OEEColor { get; set; }
        public string AEColor { get; set; }
        public string PEColor { get; set; }
        public string QEColor { get; set; }
        public List<AndonKPIEntity> CellWiseKPIsMonthly { get; set; } = new List<AndonKPIEntity>();
        public List<AndonKPIEntity> MachineWiseKPIsMonthly { get; set; } = new List<AndonKPIEntity>();
    }


    public class DowntimeEntity
    {
        public string dCategory { get; set; }
        public string dCode { get; set; }
        public double downTimeinMin { get; set; }
        public double downTimeinSec { get; set; }

    }

    public class DowntimeMainEntity
    {
        public string PieChartFontSize { get; set; }
        public string ColumnChartxAxisFontSize { get; set; }
        public string ColumnChartyAxisFontSize { get; set; }
        public string ColumnChartDataLabelFontSize { get; set; }
        
        public string ParetoChartxAxisFontSize { get; set; }
        public string ParetoChartyAxisFontSize { get; set; }
        public string ParetoChartColumnDatalabelsFontSize { get; set; }
        public string ParetoChartParetoDatalabelsFontSize { get; set; }

        public List<DowntimeEntity> CategoriesData { get; set; } = new List<DowntimeEntity>();
        public List<DowntimeEntity> downtimeData { get; set; } = new List<DowntimeEntity>();
    }

    public class HourlyTargetAndonEntity
    {
        public string MachineID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftID { get; set; }
        public string HourName { get; set; }
        public string HourID { get; set; }
        public string HourTiminigs { get; set; }
        public string Target { get; set; }
        public string Actual { get; set; }
        public string ActualColorCode { get; set; } = "black";

        public string cssClass { get; set; } = "colspan-1-class";
        public string tdVisibility { get; set; } = "";
        public string td1visible { get; set; } = "";

        public List<HourlyTargetAndonEntity> HourDataByMachine { get; set; } = new List<HourlyTargetAndonEntity>();

        public string fontSize { get; set; }

        public string IsCurrentHour { get; set; }

    }

    public class AndonFontSettingEntity
    {
        public string Parameter { get; set; }
        public string ValueInText { get; set; }
        public string ValueInInt { get; set; }
        public string DeviceName { get; set; }
    }

}