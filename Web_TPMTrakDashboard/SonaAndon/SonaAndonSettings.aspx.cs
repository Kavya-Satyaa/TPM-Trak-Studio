using SonaAndon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SonaAndon
{
	public partial class SonaAndonSettings : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack)
			{
				bindtable();
			}
		}

		private void bindtable()
		{
			List<fontstyling> data = new List<fontstyling>();
			data = DataBase.GetFontstyling();
			ddlfontfamily.SelectedValue = data[0].fontfamily;
			ddlfontsize.SelectedValue = data[0].fontsize.ToString();
			ddlHeaderFontSize.SelectedValue = data[0].Headerfontsize.ToString();
			ddlfontstyle.SelectedValue = data[0].fontstyle;
			ddlrows.SelectedValue = data[0].Rows.ToString();
			headercolor.Text = data[0].HeaderColor.ToString();
			RowBackgroundColor.Text = data[0].RowColor.ToString();
			AlternativeRowBackgroundColor.Text = data[0].AlternativeRowColor.ToString();
		}

		protected void btnsave_Click(object sender, EventArgs e)
		{
			if (ddlfontfamily != null && ddlfontsize != null && ddlfontstyle != null && ddlHeaderFontSize != null && ddlrows != null && !string.IsNullOrEmpty(headercolor.Text) && !string.IsNullOrEmpty(RowBackgroundColor.Text) && !string.IsNullOrEmpty(AlternativeRowBackgroundColor.Text)) 
			{
				DataUpdate(ddlfontfamily.SelectedValue.ToString(),"Font Family");
				DataUpdate(ddlfontsize.SelectedValue.ToString(), "Font Size");
				DataUpdate(ddlfontstyle.SelectedValue.ToString(), "Font Style");
				DataUpdate(ddlHeaderFontSize.SelectedValue.ToString(), "Header Font Style");
				DataUpdate(ddlrows.SelectedValue.ToString(), "Number of Rows");
				DataUpdate(headercolor.Text, "Header Color");
				DataUpdate(RowBackgroundColor.Text, "Row Background Color");
				DataUpdate(AlternativeRowBackgroundColor.Text, "Alternative Row Color");
			}
			bindtable();
		}

		private void DataUpdate(string ValueInText2, string ValueInText)
		{
			DataBase.SaveFontstyling("AndonSonaSettings", ValueInText, ValueInText2);
		}
	}
}