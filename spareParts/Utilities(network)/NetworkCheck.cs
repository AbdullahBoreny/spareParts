namespace spareParts.Utilities_network;
public static class NetworkCheck
{
    public static bool IsConnected()
    {
        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
