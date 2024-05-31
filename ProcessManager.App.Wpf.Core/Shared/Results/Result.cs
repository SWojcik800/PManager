using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.App.Wpf.Core.Shared.Results
{
    public class Result<T>
    {
        private Result()
        {
            
        }

        public static Result<T> Success(T value)
        {
            var instance = new Result<T>();
            instance.IsSuccess = true;
            instance.Value = value;

            return instance;
        }

        public static Result<T> Failure(T value, string errorMessage)
        {
            var instance = new Result<T>();
            instance.IsSuccess = false;
            instance.Value = value;
            instance.ErrorMessage = errorMessage;

            return instance;
        }
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }
        public string ErrorMessage { get; private set; }
    }

    public class Result
    {
        private Result()
        {

        }

        public static Result Success()
        {
            var instance = new Result();
            instance.IsSuccess = true;

            return instance;
        }

        public static Result Failure( string errorMessage)
        {
            var instance = new Result();
            instance.IsSuccess = false;
            instance.ErrorMessage = errorMessage;

            return instance;
        }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
    }



}
