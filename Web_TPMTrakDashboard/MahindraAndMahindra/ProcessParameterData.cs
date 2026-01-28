using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.MahindraAndMahindra
{
    public class DTO
    {
        private string _ParameterId;
        private string _parameterName;
        private string _minValue;
        private string _maxValue;
        private string _unit;
        private string _templateType;
        private string _backgroundColor;
        private string _parameterValue;
        private string _color;
        private string _Visibility;
        private string _DisplayText;
        private System.Drawing.Color _headerColor;

        public string ParameterId
        {
            get { return _ParameterId; }
            set { _ParameterId = value; }
        }
        public string DisplayText
        {
            get { return _DisplayText; }
            set { _DisplayText = value; }
        }

        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public string MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        public string MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public string TemplateType
        {
            get { return _templateType; }
            set { _templateType = value; }
        }

        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public string ParameterValue
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }
        public string Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; }
        }
        public string ForeColor
        {
            get { return _color; }
            set { _color = value; }
        }
        public System.Drawing.Color HeaderForeColor
        {
            get { return _headerColor; }
            set { _headerColor = value; }
        }
    }

    public class ProcessParameterData
    {
        public List<DTO> lstProcessParameters;
    }
}