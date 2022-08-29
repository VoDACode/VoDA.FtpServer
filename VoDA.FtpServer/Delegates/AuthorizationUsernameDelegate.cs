namespace VoDA.FtpServer.Delegates
{
    /// <summary>
    /// Called to verify the correctness of the username.
    /// </summary>
    /// <param name="username">User name.</param>
    /// <returns>true if the data is correct, else - false.</returns>
    public delegate bool AuthorizationUsernameDelegate(string username);
}
