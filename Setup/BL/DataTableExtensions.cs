using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup.BL
{
    public static class DataTableExtensions
    {

            public static DataTable ContainColumn(this DataTable table, string columnName)
            {
                DataColumnCollection columns = table.Columns;
                if (columns.Contains(columnName))
                {
                System.Data.DataColumn newColumn = new System.Data.DataColumn("BaseImage", typeof(System.String));
                table.Columns.Add(newColumn);
                //return true;
                }

                return table;
            }
        
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
            DataTable newTable = ContainColumn(self, "Image");
            var result = new List<dynamic>(newTable.Rows.Count);
            bool track = false;
            foreach (var row in self.Rows.OfType<DataRow>())
            {
                var expando = new ExpandoObject() as IDictionary<string, object>;
                foreach (var col in row.Table.Columns.OfType<DataColumn>())
                {
                    Dictionary<string, object> _customProperties = new Dictionary<string, object>();
                    object _currentObject;

                   /* if (expando.ContainsKey("Image"))
                    {
                        expando = new ExpandoObject() as IDictionary<string, object>;
                        byte[] byt = (Byte[])(row[col]);

                        string Base64String = Encoding.UTF8.GetString(byt);

                        row.SetField(col.ColumnName, Base64String);
                        expando.Add(col.ColumnName, Base64String);
                    }

                  else*/
                   
                   if (col.ColumnName == "BaseImage")
                    {
                      //  dataTable.Columns.Add("last_updated_user", typeof(string));
                        byte[] byt = (Byte[])(row["Image"]);
                      
                        string Base64String = Encoding.UTF8.GetString(byt);

                        row.SetField(col.ColumnName, Base64String);
                        expando.Add(col.ColumnName, Base64String);
                    }
                    else if (col.ColumnName == "Image")
                    {
                        System.Data.DataColumn newColumn = new System.Data.DataColumn("BaseImage", typeof(System.String));

                    }

                    
                    else
                    {
                        expando.Add(col.ColumnName, row[col]);
                    }
                }
                if(track== true)
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
        public static IEnumerable<dynamic> RowToExpandoObjectList(this DataRow[] self)
        {
            var result = new List<dynamic>(self.Length);
            bool track = false;
            foreach (var row in self)
            {
                var expando = new ExpandoObject() as IDictionary<string, object>;
                foreach (var col in row.Table.Columns.OfType<DataColumn>())
                {

                
                        expando.Add(col.ColumnName, row[col]);
                    
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
