namespace permisionsApp.Entities
{
    public class Permsion
    {
        public string Nazev { get; set; }
        public bool? Grant { get; set; }

        public Permsion(string nazev, bool? grant)
        {
            Nazev = nazev;
            Grant = grant;
        }
    }
}
