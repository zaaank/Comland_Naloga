namespace ToDoApp.Data.Entities
{
    public class Task
    {
        public Task() 
        {
            Naslov = string.Empty;
            Opis = string.Empty;
        }
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public DateTime DatumUstvarjanja { get; set; }
        public bool Opravljeno { get; set; }
    }
}
