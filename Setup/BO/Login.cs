using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.BO
{
    public class ERPLoginResponse
    {

               
     

    }
    public class ERPClientLoginResponse
    {

        public ExpandoObject User { get; set; }
        public IEnumerable<dynamic> Tables { get; set; }


    }


    public static class DataTableExtensions
    {
        #region Method to Convert DataRow to Object
        public static dynamic ToExpandoObject(this DataRow @this)
        {
            dynamic entity = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)entity;

            foreach (DataColumn column in @this.Table.Columns)
            {
                expandoDict.Add(column.ColumnName, @this[column]);
            }

            return expandoDict;
        }
        #endregion
        #region Method to Convert DataRow to ObjectList

        public static IEnumerable<dynamic> ToExpandoObjectList(this DataTable self)
        {
            var result = new List<dynamic>(self.Rows.Count);
            bool track = false;
            foreach (var row in self.Rows.OfType<DataRow>())
            {
                var expando = new ExpandoObject() as IDictionary<string, object>;
                foreach (var col in row.Table.Columns.OfType<DataColumn>())
                {

                    if (col.ColumnName == "StoreStartDate")
                    {
                        DateTime StoreStartDate;
                        StoreStartDate = Convert.ToDateTime(CommonClass.BL.EncodeDecode.Decode(row[col].ToString()));
                        row.SetField(col.ColumnName, StoreStartDate);
                        expando.Add(col.ColumnName, StoreStartDate);
                    }
                    else if (col.ColumnName == "StoreEndDate")
                    {
                        DateTime StoreEndDate;
                        StoreEndDate = Convert.ToDateTime(CommonClass.BL.EncodeDecode.Decode(row[col].ToString()));
                        if (col.ColumnName == "StoreEndDate")
                        {
                            int i = DateTime.Compare(DateTime.Now, StoreEndDate);
                            if (i > 0)
                            {

                                track = true;
                            }
                            else
                            {
                                row.SetField(col.ColumnName, StoreEndDate);
                                expando.Add(col.ColumnName, StoreEndDate);
                            }

                        }

                    }
                    else
                    {
                        expando.Add(col.ColumnName, row[col]);
                    }
                }
                if (track == true)
                {

                }
                else
                {
                    result.Add(expando);
                    track = false;
                }

            }
            return result;
        }

        #endregion
    }
}
