namespace Boost.Admin
{
    /// <summary>
    /// A generic class used to return the result of an operation
    /// 
    /// Example Usage
    /// <code>
    /// public <c>Result</c>><List<Example> GetExamples()
    /// {
    ///     try
    ///     {
    ///         var result = _db.GetValues();
    ///         
    ///         return result is null ? <c>Result</c>.Fail<List<Example>>("No Items found") : <c>Result</c>.Ok(result);
    ///     }
    ///     catch // without logging and reporting the exception
    ///     {
    ///         return <c>Result</c>.Fail<List<Example>>("Error completing operation",)     
    ///     }
    ///     catch(Exception ex)
    ///     {
    ///         return <c>Result</c>.Fail<List<Example>>("Error completing operation",ex)
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">Expected return value if operation completes successfully</typeparam>
    public class LogicResult<T> : LogicResult
    {
        /// <summary>
        /// Creates a class holding the result or failure of an operation
        /// </summary>
        /// <param name="value">The value that will be returned</param>
        /// <param name="success">If the operation was successfull</param>
        /// <param name="error">The reason the operation failed</param>
        protected internal LogicResult(T value, bool success, string error)
            : base(success, error)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a class holding the result or failure of an operation
        /// </summary>
        /// <param name="value">The value that will be returned</param>
        /// <param name="success">If the operation was successfull</param>
        /// <param name="error">The reason the operation failed</param>
        /// <param name="exception">The Exception thrown if any</param>
        protected internal LogicResult(T value, bool success, string error, Exception exception)
          : base(success, error)
        {
            Value = value;

            if (exception != null)
            {
                Exceptions.Add(exception);
            }
        }

        /// <summary>
        /// Creates a class holding the result or failure of an operation
        /// </summary>
        /// <param name="value">The value that will be returned</param>
        /// <param name="success">If the operation was successfull</param>
        /// <param name="error">The reason the operation failed</param>
        /// <param name="exceptions">The Exceptions thrown if any</param>
        protected internal LogicResult(T value, bool success, string error, List<Exception> exceptions)
          : base(success, error)
        {
            Value = value;

            if (exceptions != null)
            {
                Exceptions = exceptions;
            }
        }

        /// <summary>
        /// Creates a class holding the failure of an operation
        /// </summary>
        /// <param name="value">The value that will be returned</param>
        /// <param name="exceptions">The Exceptions thrown if any</param>
        protected internal LogicResult(T value, Exception exception)
          : base(exception)
        {
            Value = value;

            if (exception != null)
            {
                Exceptions.Add(exception);
            }
        }

        public T Value { get; }
    }

    /// <summary>
    /// A non-generic class used to return the result of an operation
    /// </summary>
    public class LogicResult
    {
        /// <summary>
        /// Indicates if the operation completed successfully or not
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// The reason the operation failed
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// The Exceptions raised that caused the operation to fail
        /// </summary>
        public List<Exception> Exceptions { get; set; } = new List<Exception>();

        /// <summary>
        /// Whether the operation was successfull
        /// </summary>
        public bool IsFailure => !Success;

        protected LogicResult(bool success, string error)
        {
            if (success && error != string.Empty)
                throw new InvalidOperationException();

            if (!success && error == string.Empty)
                throw new InvalidOperationException();

            Success = success;
            ErrorMessage = error;
        }

        protected LogicResult(Exception exception)
        {
            if(exception == null)
                throw new ArgumentNullException("exception");

            Exceptions.Add(exception);
            ErrorMessage = "";
            Success = false;
        }

        protected LogicResult(bool success, string error, Exception? exception = null)
        {
            if (success && error != string.Empty)
                throw new InvalidOperationException();

            if (!success && error == string.Empty)
                throw new InvalidOperationException();

            if (exception != null)
                Exceptions.Add(exception);

            Success = success;
            ErrorMessage = error;
        }

        protected LogicResult(bool success, string error, List<Exception> exceptions = null)
        {
            if (success && error != string.Empty)
                throw new InvalidOperationException();

            if (!success && error == string.Empty)
                throw new InvalidOperationException();

            if (exceptions != null)
                Exceptions = exceptions;

            Success = success;
            ErrorMessage = error;
        }

        /// <summary>
        /// Creates a Result object containing the reason the operation failed
        /// </summary>
        /// <param name="message">The reason the operation failed</param>
        /// <returns></returns>
        public static LogicResult Fail(string message)
        {
            return new LogicResult(false, message);
        }

        /// <summary>
        /// Creates a Result object containing the reason the operation failed
        /// </summary>
        /// <param name="message">The reason the operation failed</param>
        /// <param name="exception">The Exception that caused the operation to fail</param>
        /// <returns></returns>
        public static LogicResult Fail(string message, Exception exception)
        {
            return new LogicResult(false, message, exception);
        }

        /// <summary>
        /// Creates a Result object containing the Exception that caused the operation to fail
        /// </summary>
        /// <param name="exception">The Exception that caused the operation to fail</param>
        /// <returns></returns>
        public static LogicResult Fail(Exception exception)
        {
            return new LogicResult(exception);
        }

        /// <summary>
        /// Creates a Result object containing the reason the operation failed
        /// </summary>
        /// <param name="message">The reason the operation failed</param>
        /// <param name="exceptions">The List<Exception> that caused the operation to fail</param>
        /// <returns></returns>
        public static LogicResult Fail(string message, List<Exception> exceptions)
        {
            return new LogicResult(false, message, exceptions);
        }

        /// <summary>
        /// Creates a Result object with the specified return type for the calling method with the reason the operation failed
        /// </summary>
        /// <param name="message">The reason the operation failed</param>
        /// <returns></returns>
        public static LogicResult<T> Fail<T>(string message)
        {
            return new LogicResult<T>(default(T), false, message);
        }

        /// <summary>
        /// Creates a Result object with the specified return type for the calling method with the reason the operation failed
        /// </summary>
        /// <param name="exception">The exception that caused the operation to fail</param>
        /// <returns></returns>
        public static LogicResult<T> Fail<T>(Exception exception)
        {
            return new LogicResult<T>(default(T), exception);
        }

        /// <summary>
        /// Creates a Result object with the specified return type for the calling method with the reason the operation failed and the exception that was thrown
        /// </summary>
        /// <param name="message">The reason the operation failed</param>
        /// <param name="exception">The Exception that caused the operation to fail</param>
        /// <returns></returns>
        public static LogicResult<T> Fail<T>(string message, Exception exception)
        {
            return new LogicResult<T>(default(T), false, message, exception);
        }

        /// <summary>
        /// Creates a Result object with the specified return type for the calling method with the reason the operation failed and the exception that was thrown
        /// </summary>
        /// <param name="message">The reason the operation failed</param>
        /// <param name="exceptions">The List of Exception that caused the operation to fail</param>
        /// <returns></returns>
        public static LogicResult<T> Fail<T>(string message, List<Exception> exceptions)
        {
            return new LogicResult<T>(default(T), false, message, exceptions);
        }

        /// <summary>
        /// Creates a Return object with the Success property set to true
        /// </summary>
        /// <returns></returns>
        public static LogicResult Ok()
        {
            return new LogicResult(true, string.Empty);
        }

        /// <summary>
        /// Creates a Return object with the specified return type for the calling method and the Success property set to true
        /// </summary>
        /// <returns></returns>
        public static LogicResult<T> Ok<T>(T value)
        {
            return new LogicResult<T>(value, true, string.Empty);
        }
    }
}
