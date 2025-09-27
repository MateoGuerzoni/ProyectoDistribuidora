namespace ProyectoDistribuidora.Models.Legacy
{
    public class Auditoria
    {
        public int AuditoriaId { get; set; } // PK autoincremental
        public int LogId { get; set; }
        public string TableName { get; set; }
        public string Operation { get; set; }
        public string RecordId { get; set; }
        public string SecondKey { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsProcessed { get; set; }
    }

}
