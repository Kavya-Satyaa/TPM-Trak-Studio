using BusinessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_TPMTrakDashboard.KTASpindle
{
    public partial class AndonScreen : System.Web.UI.Page
    {
        List<MachineEntity> entities = new List<MachineEntity>();
        MachineEntity machineEntity = new MachineEntity();
        public static int refreshInterval = 0;
        int rows = 0;
        int count = 0;
        int flips = 0;
        int rowstotake = 0;
        public int HeaderFontsize = 20;
        public int ContentFontsize = 19;
        public int topDowncode = 0;
        public string displaytype = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["connectionString"] == null)
                Response.Redirect("../SignIn.aspx", false);
            else
            {
                if (!IsPostBack)
                {
                    BindPlantID();
                    Binddata();
                    timer.Interval = refreshInterval;
                    timer.Enabled = true;
                }
            }
        }

        private void BindPlantID()
        {
            try
            {
                List<string> lstPlantData = BindCockpitView.ViewPlantToDisplay();
                ddlPlant.DataSource = lstPlantData;
                ddlPlant.DataBind();
                ddlPlant_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }


        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string plant = ddlCell.SelectedValue.ToString() == "All" ? "" : ddlCell.SelectedValue.ToString();
                List<string> CellID = BindCockpitView.ViewCellsToDisplay(plant);
                CellID.Insert(0, "CellAll");
                if (CellID != null && CellID.Count > 0)
                {
                    ddlCell.DataSource = CellID;
                    ddlCell.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        private void Binddata()
        {
            try
            {
                string ShiftStart = "", ShiftEnd = "";
                DataTable totaldataTable = new DataTable();
                int remainder = 0;
                rows = 2;
                List<SettingsEntity> settingentity = new List<SettingsEntity>();
                settingentity = DBAccess.GetSettingsData();
                HeaderFontsize = settingentity[0].HeaderFontSize;
                ContentFontsize = settingentity[0].ContentFontSize;
                flips = settingentity[0].FlipInterval;
                refreshInterval = flips * 1000;
                topDowncode = settingentity[0].TopDownCode;
                displaytype = settingentity[0].DisplayType.ToString();
                ShiftStart = DBAccess.GetShiftstart(out ShiftEnd);
                ShiftStart = Convert.ToDateTime(ShiftStart).ToString("yyyy-MM-dd HH:mm:ss");
                ShiftEnd = Convert.ToDateTime(ShiftEnd).ToString("yyyy-MM-dd HH:mm:ss");
                machineEntity = DBAccess.GetAndonData(ddlPlant.SelectedValue.ToString(), ddlCell.SelectedValue.ToString(), "2021-11-10 14:00:00.000", "2021-11-10 22:00:00.000", out totaldataTable);
                txtRefreshTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (machineEntity != null)
                {
                    List<string> machIDList = machineEntity.CompList.AsEnumerable().Select(x => x.MachineID).Distinct().ToList();


                    foreach (string machineID in machIDList)

                    {
                        MachineEntity entitylist = new MachineEntity();
                        entitylist.MachineID = machineID;
                        entitylist.CompList = machineEntity.CompList.AsEnumerable().Where(x => x.MachineID == machineID).ToList();
                        entitylist.TargetList = machineEntity.TargetList.AsEnumerable().Where(x => x.MachineID == machineID).ToList();
                        //entitylist.DownList = machineEntity.DownList.AsEnumerable().Where(x => x.MachineID == machineID).ToList();
                        List<DownCodeData> downdata= machineEntity.DownList.AsEnumerable().Where(x => x.MachineID == machineID).ToList();
                        var totaltime = totaldataTable.AsEnumerable().Where(x => x.Field<string>("MachineID").Equals(machineID)).Select(x => x.Field<string>("total")).FirstOrDefault();
                        if (downdata.Count>0)
                        {
                            downdata.Add(new DownCodeData { DownCode = "Shift Total DownCode", Time=totaltime});
                        }
                        entitylist.DownList = downdata;
                        entities.Add(entitylist);
                    }

                }

                lvMachineData.DataSource = entities;
                lvMachineData.DataBind();
                //List<ComponentData> componentDatas = new List<ComponentData>();
                //componentDatas.Add(new ComponentData { OEE = "80%", PE = "90%", QE = "100%", AE = "80%", ActualCount = "40", Operator = "Govinnd", LastCycleEnd = "13-30 12/3/20", Operation = "10", Component = "Bush 1", UtilizedTime = "7:20", DownTime = "01:10(HH:MM)", });

                //List<TargetData> targetDatas = new List<TargetData>();
                //targetDatas.Add(new TargetData { Hr = 1, ActualQty = 10 });
                //targetDatas.Add(new TargetData { Hr = 2, ActualQty = 60 });
                //targetDatas.Add(new TargetData { Hr = 3, ActualQty = 20 });
                //targetDatas.Add(new TargetData { Hr = 4, ActualQty = 30 });
                //targetDatas.Add(new TargetData { Hr = 5, ActualQty = 40 });
                //targetDatas.Add(new TargetData { Hr = 6, ActualQty = 50 });

                //List<DownCodeData> downCodeDatas = new List<DownCodeData>();
                //downCodeDatas.Add(new DownCodeData { DownCode = "Speed Loss", Time = "01:10:22" });
                //downCodeDatas.Add(new DownCodeData { DownCode = "Dressing Cycle", Time = "12:10:22" });
                //downCodeDatas.Add(new DownCodeData { DownCode = "No Load", Time = "05:10:22" });
                //downCodeDatas.Add(new DownCodeData { DownCode = "Tea Break", Time = "01:10:22" });
                //if(downCodeDatas.Count>0)
                //{
                //    downCodeDatas.Add(new DownCodeData { DownCode="TotalTime", Time = "07:11:22" });
                //}


                //entities.Add(new MachineEntity { MachineID = "VMC-11", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-12", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-13", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-14", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-15", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-16", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-17", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });
                //entities.Add(new MachineEntity { MachineID = "VMC-18", CompList = componentDatas, TargetList = targetDatas, DownList = downCodeDatas });

                if (entities != null && entities.Count > 0)
                {
                    if(displaytype.Equals("OneTypeView"))
                    {
                        rows = 1;
                    }
                    else
                    {
                        rows = 2;
                    }
                    count = entities.Count;
                    remainder = count % rows;
                    flips = remainder != 0 ? ((count / rows) + 1) : (count / rows);
                    rowstotake = 1;
                    IEnumerable<MachineEntity> datas = entities.Take(rows * rowstotake);
                    lvMachineData.DataSource = datas;
                    lvMachineData.DataBind();
                    Session["Flips"] = flips;
                    Session["MachineData"] = entities;
                    Session["rowstotake"] = rowstotake;
                    Session["Rows"] = rows;
                    rowstotake++;
                }


            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            if (Session["Flips"] == null)
            {
                Binddata();
            }
            int.TryParse(Session["Flips"].ToString(), out flips);
            if (flips > 1)
            {
                int.TryParse(Session["rowstotake"].ToString(), out rowstotake);
                int.TryParse(Session["Rows"].ToString(), out rows);
                entities = (List<MachineEntity>)Session["MachineData"];
                int skiprows = rows * rowstotake;
                IEnumerable<MachineEntity> data = entities.Skip(skiprows).Take(rows);
                lvMachineData.DataSource = entities.Skip(skiprows).Take(rows);
                lvMachineData.DataBind();
                flips--;
                rowstotake++;
                Session["Flips"] = flips;
                Session["rowstotake"] = rowstotake;
            }
            else
            {
                Binddata();
            }
        }

        protected void ddlCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Binddata();
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex.Message);
            }
        }
    }
}