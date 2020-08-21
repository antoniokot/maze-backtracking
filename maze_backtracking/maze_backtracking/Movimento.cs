using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCidadesBacktracking
{
    class Movimento : IComparable<Movimento>	// Classe alterada para auxilio no projeto
    {
		private int linha, coluna, index;		// Coordenadas da matriz
		public Movimento(int lin, int col, int i)
		{
			linha = lin;
			coluna = col;
			index = i;
		}
		public Movimento(int lin, int col)
		{
			linha = lin;
			coluna = col;
			index = 0;
		}

		public int Linha
		{
			get => linha;
			set => linha = value;
		}
		public int Coluna
		{
			get => coluna;
			set => coluna = value;
		}
        public int Index 
		{ 
			get => index; 
			set => index = value; 
		}

        public override String ToString()
		{
			return linha + ", "+ coluna;
		}

		public int CompareTo(Movimento outro)   // para compatibilizar com ListaSimples e NoLista
		{
			return 0;
		}
	}
}
