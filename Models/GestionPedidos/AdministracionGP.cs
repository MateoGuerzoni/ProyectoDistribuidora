namespace ProyectoDistribuidora.Models.GestionPedidos
{
    public class AdministracionGP
    {
        public long ID { get; set; }  // bigint, not null (PK)

        public string Nombre { get; set; }  // char, not null

        public long? Dato { get; set; }  // bigint, null
    }
}
