namespace permisionsApp.Entities
{
    public class Subject
    {
        public List<Permsion> Permsions {  get; set; }
        public string Jmeno { get; set; }
        public string Heslo {  get; set; }
        public bool IsAdmin { get; set; }
        public string kodUzivatele { get; set; }

        public Subject(List<Permsion> permsions, string jmeno, string heslo, bool isAdmin)
        {
            Permsions = permsions;
            Jmeno = jmeno;
            Heslo = heslo;
            IsAdmin = isAdmin;
            kodUzivatele = null;
        }
        public Subject() 
        {
            Permsions = new List<Permsion>();
            Jmeno = "";
            Heslo = "";
            IsAdmin = false;
            kodUzivatele = null;
        }
    }
}
