namespace permisionsApp.Entities
{
    public class MoukaVajckoStrouha
    {
        public Subject Subject { get; set; }
        public Byte[] Obrazek { get; set; }

        public MoukaVajckoStrouha(Subject subject, byte[] obrazek)
        {
            Subject = subject;
            Obrazek = obrazek;
        }
    }
}
