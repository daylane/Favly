namespace Favly.Domain.Common.Enums
{
    public static class UnidadeMedidaExtensions
    {
        /// <summary>
        /// Retorna true se a unidade aceita quantidades decimais (ex: 0,500 kg).
        /// Retorna false se apenas inteiros são válidos (ex: 2 unidades).
        /// </summary>
        public static bool PermiteDecimal(this UnidadeMedida unidade) => unidade switch
        {
            UnidadeMedida.Kilograma => true,
            UnidadeMedida.Litro     => true,
            _                       => false  // Unidade, Caixa, Pacote, Duzia, Cartela
        };

        /// <summary>
        /// Número de casas decimais significativas para exibição.
        /// </summary>
        public static int CasasDecimais(this UnidadeMedida unidade) => unidade switch
        {
            UnidadeMedida.Kilograma => 3,   // 0,500 kg
            UnidadeMedida.Litro     => 3,   // 0,500 L
            _                       => 0    // inteiros
        };

        /// <summary>
        /// Sigla curta para exibição na interface.
        /// </summary>
        public static string Sigla(this UnidadeMedida unidade) => unidade switch
        {
            UnidadeMedida.Unidade   => "un",
            UnidadeMedida.Kilograma => "kg",
            UnidadeMedida.Litro     => "L",
            UnidadeMedida.Caixa     => "cx",
            UnidadeMedida.Pacote    => "pct",
            UnidadeMedida.Duzia     => "dz",
            UnidadeMedida.Cartela   => "ct",
            _                       => "?"
        };
    }
}
