namespace CABASUS.Modelos
{
    public class fotos
    {
        public string foto { get; set; }
        public string fk_diario { get; set; }
    }
    public class updateRul
    {
        public string urlAntigua { get; set; }
        public string urlNueva { get; set; }
    }

    public class url_local
    {
        public string id_caballo { get; set; }
        public string foto_caballo { get; set; }
        public string foto_actualizada { get; set; }
    }
}
