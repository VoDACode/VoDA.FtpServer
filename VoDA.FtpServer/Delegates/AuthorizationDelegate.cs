namespace VoDA.FtpServer.Delegates
{
    /// <summary>
    /// Called to verify the correctness of the user's password.
    /// </summary>
    /// <param name="username">User name.</param>
    /// <param name="password">User password.</param>
    /// <returns>true if the data is correct, else - false.</returns>
    public delegate bool AuthorizationDelegate(string username, string password);
}
