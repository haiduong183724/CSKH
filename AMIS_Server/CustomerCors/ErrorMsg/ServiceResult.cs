using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.ErrorMsg
{
    public class ServiceResult
    {
        /// <summary>
        /// Trạng thái của request
        /// </summary>
        public bool isValid { get; set; } = true;
        /// <summary>
        /// Danh sách các lỗi trả về
        /// </summary>
        public List<string> ErrorMsg { get; set; } = new List<string>();
        public List<object> Data { get; set; } = new List<object>();
    }
}
