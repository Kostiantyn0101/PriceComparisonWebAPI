namespace Domain.Models.Response
{
    public class OperationResultModel<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? ErrorMessage { get; }
        public Exception? Exception { get; }

        private OperationResultModel(T data)
        {
            IsSuccess = true;
            Data = data;
            ErrorMessage = null;
            Exception = null;
        }

        private OperationResultModel(string errorMessage, Exception? exception)
        {
            IsSuccess = false;
            Data = default;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static OperationResultModel<T> Success(T data = default) => new(data);
        public static OperationResultModel<T> Failure(string errorMessage, Exception? exception = null) => new(errorMessage, exception);
    }
}
