using CommonClass.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Setup.DTO
{
    public class MasterDataBindingDTO
    {
        [Required(ErrorMessage = "Required Action Type")]
        [MaxLength(50)]
        public string Action { get; set; }
        [MaxLength(20)]
        public string SearchText { get; set; }
        public int FilterID { get; set; }
        public int FilterID1 { get; set; }
        [MaxLength(35)]
        public string FilterID2 { get; set; }
        [MaxLength(35)]
        public string FilterID3 { get; set; }
        public string SearchCriteria { get; set; }
        public InsertCommonBasicBN ObjCommon { get; set; }

    }
}

