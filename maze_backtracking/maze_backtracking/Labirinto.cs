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

                char marca = lab[i + lin[index], j + col[index]];                       // Verifica-se o valor do caracter na próxima possível posição

                if (marca == 'S')                                                       // Se for igual a S
                {
                    var novaPilha = new PilhaLista<Movimento>();                        // Cria-se uma instância da classe PilhaLista
                    pilhaCaminho.Empilhar(new Movimento(i, j, index));                  // Empilha-se o movimento
                        
                    DefinirZero();                                                      // Colca-se um zero na posição atual

                    cor = CorAleatoria();                                               // Adquire-se uma cor aleatória para repintar o labirinto


                    while (!pilhaCaminho.EstaVazia)                                     
                    {
                        var mov = pilhaCaminho.Desempilhar();                           // Aqui ocorre a passagem dos movimentos para uma nova pilha
                        novaPilha.Empilhar(mov);                                        // de trás para frente
                    }

                    novaPilha.Exibir(dgvResultado);                                     // Exibe-se esta nova pilha no outro dgv
                    qtsCaminhos++;

                    while (!novaPilha.EstaVazia)
                    {
                        var mov = novaPilha.Desempilhar();                              // Aqui os movimentos são devolvidos para a pilha original, 
                        pilhaCaminho.Empilhar(mov);                                     // na ordem original
                    }

                    index++;                                                           
                }

                else
                if (marca != '#' && marca != '0' && marca != 'I')                     
                {
                    if(!EstouNoInicio())                                                // Se o cursor não estiver no inicio
                        DefinirZero();                                                  // Coloca-se um zero na posição atual

                    pilhaCaminho.Empilhar(new Movimento(i, j, index));                  // Empilha-se o movimento atual

                    i += lin[index];                                                    // Os valores de linha e coluna são alterados
                    j += col[index];                                                    // de acordo com o índice do movimento

                    ExibirPasso();                                                      // Exibe-se um passo
                    index = 0;                                                          // O índice se torna zero para movimentos futuros
                }

                else
                if (index == 7)
                {
                    if (!pilhaCaminho.EstaVazia)
                    {
                        if(!EstouNoInicio())                                            // Se o cursor não estiver no inicio     
                            DefinirZero();                                              // Coloca-se um zero na posição atual

                        var mov = pilhaCaminho.Desempilhar();                           // Desempilha-se o último movimento realizado 

                        if (mov.Direcao == 7)                                       
                            index = 0;

                        else
                            index = mov.Direcao + 1;                                    // O índice recebe o índice do último movimento mais um,
                                                                                        // para que ele não avance para o mesmo lugar
                        i = mov.Linha;                                                  // A posição do cursor passa a ser a do último movimento,
                        j = mov.Coluna;                                                 // ou posição anterior

                        ExibirPasso();                                                  // Exibe-se um passo                                                
                    }
                }

                else
                    index++;
            }
            if (qtsCaminhos == 0)                                                       // Retorna falso caso a quantidade de caminhos encontrados
                return false;                                                           // seja igual a zero

            return true;                                                                // Do contrário, retorna true

            void ExibirPasso()                                                          // Este método é responsável por exibir um movimento
            {                                                                           // no dgv 
                dgvLab.CurrentCell = dgvLab[j, i];
                dgvLab[j, i].Style.BackColor = cor;

                Thread.Sleep(300);
                Application.DoEvents();
            }

            void DefinirZero()                                                          // Este método é responsável por colocar um zero  
            {                                                                           // na posição atual
                dgvLab[j, i].Value = '0';
                lab[i, j] = '0';
            }

            Color CorAleatoria()                                                        // Este método retorna um cor aleatória
            {
                return Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            }

            Boolean EstouNoInicio()                                                     // Este método verifica se o cursor do dgv está na posição [1,1]
            {
                return dgvLab[j, i].Value.ToString()[0] == 'I';
            }
        }
    }
}
