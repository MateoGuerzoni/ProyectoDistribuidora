namespace ProyectoDistribuidora.Models.Legacy
{
    public class Log
    {
        public int LogId { get; set; }             // Identificador único del log
        public string TableName { get; set; }      // Nombre de la tabla afectada
        public string Operation { get; set; }      // Tipo de operación (INSERT, UPDATE, DELETE)
        public string RecordId { get; set; }          // Identificador del registro afectado
        public string? SecondKey { get; set; }
        public DateTime Timestamp { get; set; }    // Fecha y hora del evento
        public bool IsProcessed { get; set; }
    }
}

