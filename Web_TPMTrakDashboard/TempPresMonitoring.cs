using System;
using System.ComponentModel;

namespace Web_TPMTrakDashboard
{
    public class TempPresMonitoring : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _cycleStart { get; set; }
        private DateTime _cycleEnd { get; set; }
        private double _T1 { get; set; }
        private double _T2 { get; set; }
        private double _T3 { get; set; }
        private double _T4 { get; set; }
        private double _T5 { get; set; }
        private double _T6 { get; set; }
        private double _P1 { get; set; }
        private double _P2 { get; set; }
        private DateTime _updatedTS { get; set; }
        private string _machineID { get; set; }



        public DateTime cycleStart
        {
            get { return _cycleStart; }
            set
            {
                if (value != this._cycleStart)
                {
                    this._cycleStart = value;
                    OnPropertyChanged("cycleStart");
                }
            }
        }
        public DateTime cycleEnd
        {
            get { return _cycleEnd; }
            set
            {
                if (value != this._cycleEnd)
                {
                    this._cycleEnd = value;
                    OnPropertyChanged("cycleEnd");
                }
            }
        }
        public double T1
        {
            get { return _T1; }
            set
            {
                if (value != this._T1)
                {
                    this._T1 = value;
                    OnPropertyChanged("T1");
                }
            }
        }
        public double T2
        {
            get { return _T2; }
            set
            {
                if (value != this._T2)
                {
                    this._T2 = value;
                    OnPropertyChanged("T2");
                }
            }
        }
        public double T3
        {
            get { return _T3; }
            set
            {
                if (value != this._T3)
                {
                    this._T3 = value;
                    OnPropertyChanged("T3");
                }
            }
        }
        public double T4
        {
            get { return _T4; }
            set
            {
                if (value != this._T4)
                {
                    this._T4 = value;
                    OnPropertyChanged("T4");
                }
            }
        }
        public double T5
        {
            get { return _T5; }
            set
            {
                if (value != this._T5)
                {
                    this._T5 = value;
                    OnPropertyChanged("T5");
                }
            }
        }
        public double T6
        {
            get { return _T6; }
            set
            {
                if (value != this._T6)
                {
                    this._T6 = value;
                    OnPropertyChanged("T6");
                }
            }
        }
        public double P1
        {
            get { return _P1; }
            set
            {
                if (value != this._P1)
                {
                    this._P1 = value;
                    OnPropertyChanged("P1");
                }
            }
        }
        public double P2
        {
            get { return _P2; }
            set
            {
                if (value != this._P2)
                {
                    this._P2 = value;
                    OnPropertyChanged("P2");
                }
            }
        }
        public DateTime updatedTS
        {
            get { return _updatedTS; }
            set
            {
                if (value != this._updatedTS)
                {
                    this._updatedTS = value;
                    OnPropertyChanged("updatedTS");
                }
            }
        }
        public string machineID
        {
            get { return _machineID; }
            set
            {
                if (value != this._machineID)
                {
                    this._machineID = value;
                    OnPropertyChanged("machineID");
                }
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}