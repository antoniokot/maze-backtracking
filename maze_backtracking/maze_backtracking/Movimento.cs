using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Movimento : IComparable<Movimento>    // Classe alterada para auxilio no projeto
{
    private int linha, coluna, direcao;     // Coordenadas da matriz
    public Movimento(int lin, int col, int dir)
    {
        linha = lin;
        coluna = col;
        direcao = dir;
    }
    public Movimento(int lin, int col)
    {
        linha = lin;
        coluna = col;
        direcao = 0;
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
    public int Direcao
    {
        get => direcao;
        set => direcao = value;
    }

    public override String ToString()
    {
        return linha + ", " + coluna;
    }

    public int CompareTo(Movimento outro)   // para compatibilizar com ListaSimples e NoLista
    {
        return 0;
    }
}

