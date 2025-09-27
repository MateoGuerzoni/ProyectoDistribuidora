namespace ProyectoDistribuidora.Models.Legacy
{
    public class Administracion
    {
        public long ID { get; set; }  // bigint, not null (PK)

        public string Nombre { get; set; }  // char, not null

        public long? Dato { get; set; }  // bigint, null
    }
}
