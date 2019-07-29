using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Helpers
{
    #region Result with generic parameter
    public class Result<T>
    {
        public Result()
        {
            Errors = new List<string>();
            Success = false;
        }
        public T Data { get; set; }
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; }
        public string ErrorMessage { get; set; }

        public void AddErrors(List<string> errors)
        {
            if (errors.Count > 0)
            {
                Success = false;
                Errors = errors;
            }
        }
        public void AddError(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

    }
    #endregion

    #region Result without parameter
    public class Result
    {
        public Result()
        {
            Errors = new List<string>();
            Success = false;
        }
        public object Data { get; set; }
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; }
        public string ErrorMessage { get; set; }

        public void AddErrors(List<string> errors)
        {
            if (errors.Count > 0)
            {
                Success = false;
                Errors = errors;
            }
        }
        public void AddError(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

    }
    #endregion
}