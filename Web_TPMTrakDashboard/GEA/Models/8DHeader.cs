using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEA_NonMachining.Entities
{
    public class _8DHeader : INotifyPropertyChanged
    {
        private int slno;
        public int SLNO
        {
            get { return slno; }
            set
            {
                slno = value;
                NotifyPropertyChanged("SLNO");
            }
        }

        private int headerid;
        public int headerID
        {
            get { return headerid; }
            set
            {
                headerid = value;
                NotifyPropertyChanged("headerID");
            }
        }

        private bool isenable;
        public bool IsEnable
        {
            get { return isenable; }
            set { isenable = value; NotifyPropertyChanged("IsEnable"); }
        }

        private string richTextSubHeader;
        public string RichTextSubHeader
        {
            get { return richTextSubHeader; }
            set { richTextSubHeader = value; NotifyPropertyChanged("RichTextSubHeader"); }
        }



        private List<_8DreportGridCol> gridData;
        public List<_8DreportGridCol> GridData
        {
            get { return gridData; }
            set { gridData = value; NotifyPropertyChanged("GridData"); }
        }

        private string templateName;
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; NotifyPropertyChanged("TemplateName"); }
        }

        private string gridOrRichText;
        public string GridOrRichText 
        {
            get { return gridOrRichText; }
            set { gridOrRichText = value; NotifyPropertyChanged("GridOrRichText"); }
        }

        private string gridOrRichType;
        public string GridOrRichType
        {
            get { return gridOrRichType; }
            set { gridOrRichType = value; NotifyPropertyChanged("GridOrRichType"); }
        }

        private string gridColumnFirst;
        public string GridColumnFirst
        {
            get { return gridColumnFirst; }
            set { gridColumnFirst = value; NotifyPropertyChanged("GridColumnFirst"); }
        }

        private string gridColumnSecond;
        public string GridColumnSecond
        {
            get { return gridColumnSecond; }
            set { gridColumnSecond = value; NotifyPropertyChanged("GridColumnSecond"); }
        }

        private string gridColumnThird;
        public string GridColumnThird
        {
            get { return gridColumnThird; }
            set { gridColumnThird = value; NotifyPropertyChanged("GridColumnThird"); }
        }

        private string gridColumnThirdType;
        public string GridColumnThirdType
        {
            get { return gridColumnThirdType; }
            set { gridColumnThirdType = value; NotifyPropertyChanged("GridColumnThirdType"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public class _8DreportGridCol :INotifyPropertyChanged
    {
        private string measure;
        public string Measure
        {
            get { return measure; }
            set { measure = value; NotifyPropertyChanged("Measure"); }
        }

        private string responsible;
        public string Responsible
        {
            get { return responsible; }
            set { responsible = value; NotifyPropertyChanged("Responsible"); }
        }

        private string deadline;
        public string Deadline
        {
            get { return deadline; }
            set { deadline = value; NotifyPropertyChanged("Deadline"); }
        }

        private string slno;
        public string Slno
        {
            get { return slno; }
            set { slno = value; NotifyPropertyChanged("Slno"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
