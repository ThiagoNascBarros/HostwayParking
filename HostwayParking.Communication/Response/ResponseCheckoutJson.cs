namespace HostwayParking.Communication.Response
{
    public class ResponseCheckoutJson
    {
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        // Data e hora de entrada original
        public DateTime EntryTime { get; set; }

        // Data e hora da saída (momento atual do checkout)
        public DateTime ExitTime { get; set; }

        // Tempo total de permanência formatado (ex: "02:30:00")
        public string TimeSpent { get; set; } = string.Empty;

        // Valor total a ser pago
        public decimal TotalPrice { get; set; }

        // Moeda (útil para o requisito de Brasil/Argentina)
        public string Currency { get; set; } = "R$";
    }
}
