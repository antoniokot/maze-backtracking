using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
                labirinto = new Labirinto(dlgOpen.FileName);
                labirinto.Exibir(dgvLabirinto);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            dgvCaminho.RowCount = 0;

            if(labirinto.AchouCaminhos(dgvLabirinto, dgvCaminho))
                MessageBox.Show($"O labirinto possui {dgvCaminho.RowCount} caminho(s)!", "Caminhos encontrados", MessageBoxButtons.OK);

            else
                MessageBox.Show("O labirinto passado não possui caminhos de saída", "Nenhum caminho encontrado", MessageBoxButtons.OK);


        }
    }
}
