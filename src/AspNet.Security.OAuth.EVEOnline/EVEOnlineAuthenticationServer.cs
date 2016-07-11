namespace AspNet.Security.OAuth
{
    /// <summary>
    /// Defines a list of available authentication endpoints based on the 
    /// accessed server.
    /// </summary>
    public enum EVEOnlineAuthenticationServer {
        /// <summary>
        /// Live server
        /// </summary>
        Tranquility = 0,

        /// <summary>
        /// Test server
        /// </summary>
        Singularity = 1
    }
}