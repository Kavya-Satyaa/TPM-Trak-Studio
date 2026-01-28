using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_TPMTrakDashboard.Models;

namespace Web_TPMTrakDashboard
{
    public partial class DownCatagoryInfo : System.Web.UI.Page
    {
        protected int SuccMessage
        {
            get;
            private set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDownCatagory();
            }
        }

        #region "Bind Down Catagory Information"
        private void BindDownCatagory()
        {
            try
            {
                string description;
                List<Tuple<string, string>> list= DownCodeInfoDataBase.GetDownCategoryInformation("", out description);
                int flag = 0;
                if (list.Count == 0)
                {
                    flag = 1;
                    list.Add(new Tuple<string, string>("", ""));
                }
                gridviewDownCat.DataSource = list;
                gridviewDownCat.DataBind();
                if (flag == 1)
                {
                    gridviewDownCat.Rows[0].Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        #region "Add Down Catagory"
        protected void AddNewDownCat(object sender, EventArgs e)
        {
            try
            {
                string DownCat = ((TextBox)gridviewDownCat.FooterRow.FindControl("txtAddDownCat")).Text;
                string DownDesc = ((TextBox)gridviewDownCat.FooterRow.FindControl("txtAddDownDesc")).Text;
                DownCodeInfoDataBase.InsertDeleteDownCategoryInformation(DownCat.ToString(), DownDesc.ToString(), "save");
                SuccMessage = 1;
                //lblMessages.ForeColor = System.Drawing.Color.Green;
                //lblMessages.Text = "Details added/updated successfully !!";
                BindDownCatagory();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }
        #endregion

        protected void UpdateDownCat(object sender, EventArgs e)
        {
            try
            {

                foreach (GridViewRow row in gridviewDownCat.Rows)
                {

                    string DownCat = ((Label)row.FindControl("lblDownCate")).Text;
                    string DownDesc = ((TextBox)row
                                        .FindControl("txtEditDownDesc")).Text;
                    string hideDownDesc = ((HiddenField)row.FindControl("hdfDownDesc")).Value;
                    if (DownDesc != hideDownDesc)
                        DownCodeInfoDataBase.InsertDeleteDownCategoryInformation(DownCat.ToString(), DownDesc.ToString(), "save");
                }
                //lblMessages.ForeColor = System.Drawing.Color.Green;
                //lblMessages.Text  = "Details added/updated successfully !!";
                SuccMessage = 1;
                gridviewDownCat.EditIndex = -1;
                BindDownCatagory();
            }

            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void DeleteDownCat(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkRemove = (LinkButton)sender;
                if (DownCodeInfoDataBase.CheckDownCategoryExist(lnkRemove.CommandArgument.ToString()))
                {
                    //lblMessages.Text = " There are some DownIDs belonging to this category. " + Environment.NewLine + " Please change the DownIDs category before deletion.";
                    //lblMessages.ForeColor = System.Drawing.Color.Red;
                    SuccMessage = 2;
                }
                else
                {
                    DownCodeInfoDataBase.InsertDeleteDownCategoryInformation(lnkRemove.CommandArgument.ToString(), lnkRemove.CommandName.ToString(), "delete");
                    SuccMessage = 3;
                    //lblMessages.Text = "Details deleted successfully.";
                    //lblMessages.ForeColor = System.Drawing.Color.Green;
                }
                BindDownCatagory();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMessages.Text = ex.ToString();
            }
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindDownCatagory();
            gridviewDownCat.PageIndex = e.NewPageIndex;
            gridviewDownCat.DataBind();
        }


    }
}