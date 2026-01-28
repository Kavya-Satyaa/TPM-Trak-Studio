using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.Models
{
	public class Lookup
	{
		#region private fileds
		private string _name;
		private string _value;
		#endregion

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}