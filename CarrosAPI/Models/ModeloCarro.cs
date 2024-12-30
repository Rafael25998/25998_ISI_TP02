namespace CarrosAPI.Models
{   public class ModeloCarro
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public double Preco { get; set; }
        public string Imagem { get; set; } = string.Empty;
    }
}
