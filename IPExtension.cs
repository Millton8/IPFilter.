using System.Net;


namespace IPFilter
{
    //Метод расширения для сравнения IP адресов
    public static class IPExtension
    {
        public static sbyte CompareTo(this IPAddress ip1, IPAddress ip2)
        {

            byte[] b1 = ip1.GetAddressBytes();
            byte[] b2 = ip2.GetAddressBytes();

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] < b2[i])
                    return -1;
                else if (b1[i] > b2[i])
                    return 1;

            }
            return 0;
        }
    }
}
