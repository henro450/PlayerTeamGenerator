namespace PlayerTeamGenerator.Helpers
{
    public class ManagerResponse<T>
    {
        public bool Succeeded { get; protected set; }
        public T Data { get; protected set; }
        public string Error { get; protected set; } = null;

        public static ManagerResponse<T> Success() =>
            new ManagerResponse<T>
            {
                Succeeded = true
            };

        public static ManagerResponse<T> Success(T respose)
        {
            return new ManagerResponse<T>
            {
                Succeeded = true,
                Data = respose
            };
        }

        public static ManagerResponse<T> Failure(string response)
        {
            return new ManagerResponse<T>
            {
                Succeeded = false,
                Error = response
            };
        }
    }
}
