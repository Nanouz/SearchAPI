namespace SAG.Uitpas.Helpers.Interfaces
{
    internal interface IOAuthService
    {
        string GetAuthorizationHeader2Legged(string uri, string method);
        string GetAuthorizationHeader(string uri, string method);
    }
}