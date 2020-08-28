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
    public partial class frmMaze : Form
    {
        Labirinto labirinto;
        public frmMaze()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(dlgOpen.ShowDialog() == DialogResult.OK)
            {
                btnBuscar.Enabled = true;
                labirinto = new Labirinto(dlgOpen.FileName);
                labirinto.Exibir(dgvLabirinto);
            }

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            btnBuscar.Enabled = false;

            if (dgvLabirinto.RowCount == 0)
                btnAbrir.PerformClick();

            else
            if (labirinto.AchouCaminhos(dgvLabirinto, dgvCaminho))
            {
                MessageBox.Show($"O labirinto possui {dgvCaminho.RowCount} caminho(s)!", "Caminhos encontrados", MessageBoxButtons.OK);
                dgvCaminho.Enabled = true;
            }

            else
                MessageBox.Show("O labirinto passado não possui caminhos de saída", "Nenhum caminho encontrado", MessageBoxButtons.OK);
        }

        private void dgvCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvCaminho.Enabled = false;

            Random rnd = new Random();
            Color cor = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));

            int linha = 0;
            int coluna = 0;

            for (int i = 0; i < dgvCaminho.Rows[dgvCaminho.CurrentRow.Index].Cells.Count; i++)
            {
                string[] coordenada = dgvCaminho.Rows[dgvCaminho.CurrentRow.Index].Cells[i].Value.ToString().Split(',');

                linha = int.Parse(coordenada[0].Trim());
                coluna = int.Parse(coordenada[1].Trim());
                
                dgvLabirinto.CurrentCell = dgvLabirinto[coluna, linha];
                dgvLabirinto[coluna, linha].Style.BackColor = cor;

                Thread.Sleep(300);
                Application.DoEvents();
            }

            MessageBox.Show("O caminho desejado foi finalizado.", "Caminho escolhido", MessageBoxButtons.OK);
            dgvCaminho.Enabled = true;
        }
    }
}
