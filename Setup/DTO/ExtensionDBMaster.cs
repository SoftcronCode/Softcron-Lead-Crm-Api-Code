using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Setup.DTO
{
    public class AddTableDTO
    {
        [Required(ErrorMessage = "Required Table Name")]
        [MaxLength(50)]
        public string TableName { get; set; }
        [Required(ErrorMessage = "Required Client ID")]
        public int ClientMasterID { get; set; }

        [Required(ErrorMessage = "Required Table Alias Name")]
        public string TableAliasName { get; set; }

        [Required(ErrorMessage = "Required Table Url")]
        public string TableUrl { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }

    }

    public class AddColumnDTO
    {
        public int ExtensionTableCode { get; set; }

        public string ExtensionTableName { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }

        public int ControlType { get; set; }
        public int InputDataType { get; set; }

        public int Validate_MaxLength { get; set; }
        public int Validate_isRequired { get; set; }
        public int Validate_isUnique { get; set; }
        public int Validate_isReference { get; set; }

        public string ReferenceTableName { get; set; }
      
        public string ReferenceFieldName { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }

    }
}
