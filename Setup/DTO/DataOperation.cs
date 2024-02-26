using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.DTO
{
    public class Columns
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public int ColumnDataType { get; set; }
    }

    public class TableOperationDTO
    {
        public string TableName { get; set; }
        public string Action { get; set; }
        public int ID { get; set; }
        public string  primaryColumn { get; set; }
        public string Primarydatatype { get; set; }
        public int primaryColumnValue { get; set; }
        public List<Columns> Columns { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }
    }


    public class OperationDTO {
        public InsertCommonBasicBN ObjCommon { get; set; }
    }
  

    public class TableColumns
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public string ColumnDataType { get; set; }
    }

}
