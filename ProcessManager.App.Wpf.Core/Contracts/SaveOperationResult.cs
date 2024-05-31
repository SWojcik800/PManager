namespace ProcessManager.App.Wpf.Core.Contracts
{
    public class SaveOperationResult<TPKey>
    {
        private SaveOperationResult()
        {
            
        }
        public TPKey Id { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; set; }

        public static SaveOperationResult<TPKey> Success(TPKey id)
        {
            var instance = new SaveOperationResult<TPKey>();
            instance.Id = id;
            instance.IsSuccess = true;

            return instance;
        }

        public static SaveOperationResult<TPKey> Fail(string errorMsg)
        {
            var instance = new SaveOperationResult<TPKey>();
            instance.ErrorMessage = errorMsg;
            instance.IsSuccess = false;

            return instance;
        }


    }
}
