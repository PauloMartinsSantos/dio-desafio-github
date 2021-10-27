using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BarcodeLib;

namespace CadastroDeClientes
{
    public partial class Form1 : Form
    {
        //criação do arquivo de dados .Txt no sistema operacional
        static string pasta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//atributo "SpecialFolder" permite escolher pasta especifica do S.O que contem em todos usuarios
        static string arquivo = "registros.txt";
        string local = Path.Combine(pasta, arquivo); // variável para junção do caminho de destino e o arquivo .txt
        int id = 1;

        public Form1()
        {
            InitializeComponent();
        }      


        #region Inicialização
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            //Tratativa de erro: verificação de existencia do arquivo registros.txt
            try 
            {
                id = File.ReadLines(local).Count() + 1; //inicialização do campo id com ultima posição com base nos registros           
                txtId.Text = Convert.ToString(id);
            }

            /*quando o programa é executado a primeira vez o arquivo não existe então o catch atribui o valor "1"
             * ao txtId para a execução do programa continuar*/
            catch
            {
                txtId.Text = Convert.ToString(1);
            }
            
        }
        #endregion

        #region Botão de cadastro do cliente
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtNome.TextLength < 3)//validação comprimento mínimo  do nome
            {
                MessageBox.Show(" Digite um nome valido ");
            }
            else

            if (txtCpf.TextLength != 11)//validação comprimento do CPF
            {
                MessageBox.Show("Digite um CPF valido somente números");
            }
            else

            if (txtRg.TextLength < 8 | txtRg.TextLength > 9 ) //validação comprimento RG
            {
                MessageBox.Show(" Digite um RG válido ");
            }

            else
            {
                StreamWriter sw = new StreamWriter(local, true); // propiedade true serve de append text
                sw.WriteLine( txtId.Text + "-" + txtNome.Text + "-" + txtCpf.Text + "-" + txtRg.Text);
                sw.Close();
                MessageBox.Show("Cadastro realizado com sucesso!");

                //limpa textos dos campos de cadastro
                txtNome.Clear();
                txtCpf.Clear();
                txtRg.Clear();

                id++;
                txtId.Text = Convert.ToString(id);

            }
        }
        #endregion

        #region Botão de atualizar
        private void button1_Click(object sender, EventArgs e) // botão de atualizar
        {

            if (id > 1) // verificação de registros na inicialização do app
            {
                int numLinhas = 0;

                //variavel para criar linhas no dataGridView
                int qtd = File.ReadLines(local).Count(); //conta a quantidade de linhas no arquivo ("local" que é caminho do arquivo unido)

                // laço FOR para criação das linhas

                dataGridView1.Rows.Clear(); // limpa a leitura do dataGrid para não inserir linhas vazias baseado na quantidade de linhas preenchidas

                for (int i = 0; i < qtd; i++) // insere linhas no dataGrid
                {
                    dataGridView1.Rows.Add();
                }
                
                StreamReader sr = new StreamReader(local); //varialvel local é a junção do nome e conteudo do arquivo
                while (!sr.EndOfStream) // enquanto o fim do arquivo for false (sr.EndOfStream == false) ! é a negação da frase
                {
                    string linhaCompleta = sr.ReadLine(); // Recebe o conteudo da linha completa
                    string[] campos = linhaCompleta.Split('-'); // recebe o conteudo da linha e divide com Split guardando no array

                    //Inserir dados do array no datagridView
                    dataGridView1.Rows[numLinhas].Cells[0].Value = campos[0];
                    dataGridView1.Rows[numLinhas].Cells[1].Value = campos[1];
                    dataGridView1.Rows[numLinhas].Cells[2].Value = campos[2];
                    dataGridView1.Rows[numLinhas].Cells[3].Value = campos[3];

                    numLinhas++;

                }
                sr.Close();
            }
            else
                MessageBox.Show("Ainda não existem registros cadastrados");
        }

        #endregion

        #region Gerar código de Barras
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try 
            {
                string nomeCadastro = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
                string cpfCadastro = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
                textBox1.Text = nomeCadastro;
                textBox2.Text = cpfCadastro;
            }
            catch 
            {
                
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)//botão gerar código de barras
        {
            if (textBox1.Text != string.Empty)
            {

                BarcodeLib.Barcode codigoBarras = new BarcodeLib.Barcode();
                codigoBarras.IncludeLabel = true; // exibe texto do código de barras
                panel1.BackgroundImage = codigoBarras.Encode(BarcodeLib.TYPE.CODE128, textBox1.Text + " " + textBox2.Text + " " + textBox3.Text, Color.Black, Color.White, 500, 125);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }


            else
                MessageBox.Show("Nenhum registro selecionado");
        }
        
    }
}

    

        
       

