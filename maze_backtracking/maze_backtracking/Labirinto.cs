using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace maze_backtracking
{
    // Antônio Hideto Borges Kotsubo - 19162 e Matheus Seiji Luna Noda - 19190
    class Labirinto
    {
        char[,] lab;                                                // Declaração dos atributos
        int linha;
        int coluna;

        public Labirinto(string nomeArq)                            // Construtor
        {
            var arq = new StreamReader(nomeArq);                    // Recebe o nome do arquivo
            coluna = int.Parse(arq.ReadLine());                     // Instancia as variáveis de acordo com os dados do arquivo
            linha = int.Parse(arq.ReadLine());
            lab = new char[linha, coluna];

            for (int lin = 0; lin < linha; lin++)                   // Estrutura a matriz
            {
                string umaLinha = arq.ReadLine();
                for (int col = 0; col < coluna; col++)
                    lab[lin, col] = umaLinha[col];
            }
        }

        public char[,] Lab                                          // Propriedades
        {
            get => lab;
            set => lab = value;
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

        public void Exibir(DataGridView dgv)                        // Este método exibe a matriz do labirinto em um dgv de escolha
        {
            dgv.RowCount = linha;
            dgv.ColumnCount = coluna;

            for (int lin = 0; lin < linha; lin++)
            {

                for (int col = 0; col < coluna; col++)
                {
                    dgv[col, lin].Value = lab[lin, col];
                }
            }

            for (int col = 0; col < coluna; col++)
            {
                dgv.Columns[col].HeaderText = (col).ToString();
                dgv.Columns[col].Width = 15;
            }

            for (int lin = 0; lin < linha; lin++)
                dgv.Rows[lin].HeaderCell.Value = (lin).ToString();

            dgv.CurrentCell = dgv[1, 1];                             // Posiciona a célula atual na posição [1,1]
        }

        public bool AchouCaminhos(DataGridView dgvLab, DataGridView dgvResultado)       // Este método é responsável por achar os caminhos de um labirinto
        {
            int[] col = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };                       // Insere-se, à mão, os valores nos vetores que serão utilizados para realizar um movimento 
            int[] lin = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };
            int i = 1;                                                                  // Declara-se variáveis para linha (i) e coluna (j)
            int j = 1;
            int qtsCaminhos = 0;                                                        // Declara-se uma variável que contra o número de caminhos
            int index = 0;                                                              // E outra para o índice da direção

            Random rnd = new Random();                                                  // Instancia-se um objeto da classe Random
            Color cor = CorAleatoria();                                                 // Chama-se o método interno CorAleatoria, que retorna um cor com valores aleatórios

            dgvLab[1, 1].Style.BackColor = cor;                                         // Colore o fundo da célula inicial com a cor retornada acima

            var pilhaCaminho = new PilhaLista<Movimento>();                             // Instancia-se um objeto da classe PilhaLista

            bool naoPodeVoltarMais = false;                                             // E declara-se uma variável controladora boolean

            while (!naoPodeVoltarMais)
            {
                naoPodeVoltarMais = i == 1 && j == 1 && index == 7;                     // Confere-se se a posição atual é a inicial (1,1) e não existe direção possível (index = 7)

                char marca = lab[i + lin[index], j + col[index]];

                if (marca == 'S')
                {
                    var novaPilha = new PilhaLista<Movimento>();
                    pilhaCaminho.Empilhar(new Movimento(i, j, index));

                    DefinirZero();

                    cor = CorAleatoria();


                    while (!pilhaCaminho.EstaVazia)
                    {
                        var mov = pilhaCaminho.Desempilhar();
                        novaPilha.Empilhar(mov);
                    }

                    novaPilha.Exibir(dgvResultado);
                    qtsCaminhos++;

                    while (!novaPilha.EstaVazia)
                    {
                        var mov = novaPilha.Desempilhar();
                        pilhaCaminho.Empilhar(mov);
                    }
                    index++;
                }

                else
                if (marca != '#' && marca != '0' && marca != 'I')
                {
                    if(!EstouNoInicio())
                        DefinirZero();

                    pilhaCaminho.Empilhar(new Movimento(i, j, index));

                    i += lin[index];
                    j += col[index];

                    ExibirPasso();
                    index = 0;
                }

                else
                if (index == 7)
                {
                    if (!pilhaCaminho.EstaVazia)
                    {
                        if(!EstouNoInicio())
                            DefinirZero();

                        var mov = pilhaCaminho.Desempilhar();

                        if (mov.Direcao == 7)
                            index = 0;

                        else
                            index = mov.Direcao + 1;

                        i = mov.Linha;
                        j = mov.Coluna;
                        ExibirPasso();
                    }
                }

                else
                    index++;
            }
            if (qtsCaminhos == 0)
                return false;

            return true;

            void ExibirPasso()
            {
                dgvLab.CurrentCell = dgvLab[j, i];
                dgvLab[j, i].Style.BackColor = cor;

                Thread.Sleep(300);
                Application.DoEvents();
            }

            void DefinirZero()
            {
                dgvLab[j, i].Value = '0';
                lab[i, j] = '0';
            }

            Color CorAleatoria()
            {
                return Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            }

            Boolean EstouNoInicio()
            {
                return dgvLab[j, i].Value.ToString()[0] == 'I';
            }
        }
    }
}
