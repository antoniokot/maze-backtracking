using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maze_backtracking
{

    // Antônio Hideto Borges Kotsubo - 19162 e Matheus Seiji Luna Noda - 19190
    public partial class frmMaze : Form
    {
        Labirinto labirinto;                                                        // Declaração de um objeto da classe Labirinto
        public frmMaze()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)                      // Clique do botão de abrir arquivo
        {
            dgvCaminho.Enabled = false;                                             // Desabilita o dgv de caminho, que se encontra vazio
            dgvCaminho.RowCount = 0;                                                // Limpamos os dgvs
            dgvLabirinto.RowCount = 0;

            if(dlgOpen.ShowDialog() == DialogResult.OK)                             // Aqui verifica-se se o usuário selecionou um arquivo
            {
                btnBuscar.Enabled = true;                                           // Se tiver selecionado, habilita o botão de busca
                labirinto = new Labirinto(dlgOpen.FileName);                        // Bem como instancia o objeto da classe Labirinto
                labirinto.Exibir(dgvLabirinto);                                     // E exibe a matriz confeccionada 
            }

        }

        private void btnFind_Click(object sender, EventArgs e)                      // Clique do botão de buscar caminhos
        {
            btnBuscar.Enabled = false;                                              // Desabilita o botão de busca, para que não se faça mais de uma busca por vez

            if (dgvLabirinto.RowCount == 0)                                         // Se o dgv com o labirinto estiver vazio, o usuário é forçado e escolher um arquivo
                btnAbrir.PerformClick();

            else
            if (labirinto.AchouCaminhos(dgvLabirinto, dgvCaminho))                  // Chama-se o método para achar caminhos e retorna se o mesmo foi bem sucedido
            {
                MessageBox.Show($"O labirinto possui {dgvCaminho.RowCount} caminho(s)!", "Caminhos encontrados", MessageBoxButtons.OK);         // Retorna um MessageBox, caso tenha achado
                dgvCaminho.Enabled = true;                                                                                                      // E habilita o dgv com os caminhos encontrados, que, anteriormente, estava desabilitado
            }

            else
                MessageBox.Show("O labirinto passado não possui caminhos de saída", "Nenhum caminho encontrado", MessageBoxButtons.OK);         // Retorna um MessageBox, caso não tenha encontrado nada
        }

        private void dgvCaminho_CellClick(object sender, DataGridViewCellEventArgs e)                                   // Clique em uma linha do dgv
        {
            dgvCaminho.Enabled = false;                                                                                 // Desativa o mesmo, para que seja feita apenas uma pesquisa por vez

            Random rnd = new Random();                                                                                  // Neste bloco, há a atribuição de uma cor aleatória para que o caminho desejado seja mostrado
            Color cor = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));

            int linha = 0;                                                                                              // Declaramos variáveis linha e coluna
            int coluna = 0;

            for (int i = 0; i < dgvCaminho.Rows[dgvCaminho.CurrentRow.Index].Cells.Count; i++)                          // Este for vai de 0 até o número de células da linha clicada
            {
                string[] coordenada = dgvCaminho.Rows[dgvCaminho.CurrentRow.Index].Cells[i].Value.ToString().Split(',');        // Aqui dividem-se os valores de uma célula separados por ","

                if (coordenada[0] != "" && coordenada[1] != "")
                {
                    linha = int.Parse(coordenada[0].Trim());                                                                // Aqui ocorre a conversão dos valores encontrados para inteiros
                    coluna = int.Parse(coordenada[1].Trim());

                    dgvLabirinto.CurrentCell = dgvLabirinto[coluna, linha];                                                 // Exibe-se passo a passo
                    dgvLabirinto[coluna, linha].Style.BackColor = cor;                                                      // Mudando a cor de fundo da célula atual

                    Thread.Sleep(300);                                                                                      // Um pequeno delay, para deixar os movimentos mais visíveis
                    Application.DoEvents();
                }
            }

            MessageBox.Show("O caminho desejado foi finalizado.", "Caminho escolhido", MessageBoxButtons.OK);           // Exibe-se uma mensagem de sucesso
            dgvCaminho.Enabled = true;                                                                                  // Reativa-se o dgv de caminhos caso o usuário deseje visualizar outro caminho
        }
    }
}
