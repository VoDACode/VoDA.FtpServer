namespace VoDA.FtpServer.Contexts
{
    public abstract class AuthorizationOptionsContext
    {
        /// <summary>
        /// Enables or disables the use of authorization.
        /// </summary>
        public abstract bool UseAuthorization { get; set; }

        /// <summary>
        /// Called to verify the correctness of the username.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>true if the data is correct, else - false.</returns>
        public abstract bool TryUsernameVerification(string username);

        /// <summary>
        /// Called to verify the correctness of the user's password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">User password.</param>
        /// <returns>true if the data is correct, else - false.</returns>
        public abstract bool TryPasswordVerification(string username, string password);
    }
}
