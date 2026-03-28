namespace ProjectApi.DTOS
{
    public class PropertySearchDto
    {

        public string? Type { get; set; }
        public string? TypeContract { get; set; }

        public string? Location { get; set; }

        public double? Price { get; set; }

        public string? Keyword { get; set; }

        public double? Area { get; set; }

        public DateTime? Date { get; set; }
    }
}
