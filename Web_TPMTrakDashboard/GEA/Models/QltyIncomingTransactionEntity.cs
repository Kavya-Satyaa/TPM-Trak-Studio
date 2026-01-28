using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard.GEA.Models
{
    public class QltyIncomingTransactionEntity
    {
        public int ID { get; set; }
        private string _characteristicSlNo;
        private string _characteristicCode;
        private string _lsl;
        private string _usl;
        private string _setValue;
        private string _uom;
        private string _dataType;
        private string _inspectionValueOneA;
        private string _inspectionValueOneB;
        private string _inspectionValueTwoA;
        private string _inspectionValueTwoB;
        private string _inspectionValueThreeA;
        private string _inspectionValueThreeB;
        private string _inspectionValueFourA;
        private string _inspectionValueFourB;
        private string _inspectionValueFiveA;
        private string _inspectionValueFiveB;
        private bool _isMandatory;
        private string _inspectedBy;
        private string _comments;
        private string _inspectedTS;

        public string CharacteristicSlNo
        {
            get { return _characteristicSlNo; }
            set
            {
                if (value != this._characteristicSlNo)
                {
                    this._characteristicSlNo = value;
                }
            }
        }

        public string CharacteristicCode
        {
            get { return _characteristicCode; }
            set
            {
                if (value != this._characteristicCode)
                {
                    this._characteristicCode = value;
                }
            }
        }

        public string LSL
        {
            get { return _lsl; }
            set
            {
                if (value != this._lsl)
                {
                    this._lsl = value;
                }
            }
        }

        public string USL
        {
            get { return _usl; }
            set
            {
                if (value != this._usl)
                {
                    this._usl = value;
                }
            }
        }

        public string SetValue
        {
            get { return _setValue; }
            set
            {
                if (value != this._setValue)
                {
                    this._setValue = value;
                }
            }
        }

        public string UOM
        {
            get { return _uom; }
            set
            {
                if (value != this._uom)
                {
                    this._uom = value;
                }
            }
        }

        public string DataType
        {
            get { return _dataType; }
            set
            {
                if (value != this._dataType)
                {
                    this._dataType = value;
                }
            }
        }

        public string InspectionValueOneA
        {
            get { return _inspectionValueOneA; }
            set
            {
                if (value != this._inspectionValueOneA)
                {
                    this._inspectionValueOneA = value;
                }
            }
        }

        public string InspectionValueOneB
        {
            get { return _inspectionValueOneB; }
            set
            {
                if (value != this._inspectionValueOneB)
                {
                    this._inspectionValueOneB = value;
                }
            }
        }

        public string InspectionValueTwoA
        {
            get { return _inspectionValueTwoA; }
            set
            {
                if (value != this._inspectionValueTwoA)
                {
                    this._inspectionValueTwoA = value;
                }
            }
        }

        public string InspectionValueTwoB
        {
            get { return _inspectionValueTwoB; }
            set
            {
                if (value != this._inspectionValueTwoB)
                {
                    this._inspectionValueTwoB = value;
                }
            }
        }

        public string InspectionValueThreeA
        {
            get { return _inspectionValueThreeA; }
            set
            {
                if (value != this._inspectionValueThreeA)
                {
                    this._inspectionValueThreeA = value;
                }
            }
        }

        public string InspectionValueThreeB
        {
            get { return _inspectionValueThreeB; }
            set
            {
                if (value != this._inspectionValueThreeB)
                {
                    this._inspectionValueThreeB = value;
                }
            }
        }

        public string InspectionValueFourA
        {
            get { return _inspectionValueFourA; }
            set
            {
                if (value != this._inspectionValueFourA)
                {
                    this._inspectionValueFourA = value;
                }
            }
        }

        public string InspectionValueFourB
        {
            get { return _inspectionValueFourB; }
            set
            {
                if (value != this._inspectionValueFourB)
                {
                    this._inspectionValueFourB = value;
                }
            }
        }

        public string InspectionValueFiveA
        {
            get { return _inspectionValueFiveA; }
            set
            {
                if (value != this._inspectionValueFiveA)
                {
                    this._inspectionValueFiveA = value;
                }
            }
        }

        public string InspectionValueFiveB
        {
            get { return _inspectionValueFiveB; }
            set
            {
                if (value != this._inspectionValueFiveB)
                {
                    this._inspectionValueFiveB = value;
                }
            }
        }

        public bool IsMandatory
        {
            get { return _isMandatory; }
            set
            {
                if (value != this._isMandatory)
                {
                    this._isMandatory = value;
                }
            }
        }

        public string InspectedBy
        {
            get { return _inspectedBy; }
            set
            {
                if (value != this._inspectedBy)
                {
                    this._inspectedBy = value;
                }
            }
        }

        public string Comments
        {
            get { return _comments; }
            set
            {
                if (value != this._comments)
                {
                    this._comments = value;
                }
            }
        }

        public string InspectedTS
        {
            get { return _inspectedTS; }
            set
            {
                if (value != this._inspectedTS)
                {
                    this._inspectedTS = value;
                }
            }
        }
    }
}