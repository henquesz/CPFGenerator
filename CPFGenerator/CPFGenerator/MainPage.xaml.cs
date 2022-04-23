using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace CPFGenerator
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void btn_gerar_Clicked(object sender, EventArgs e)
        {

            //Código para a geração dos números do cpf através de array
            var random = new Random();

            int soma = 0;
            int resto = 0;
            int[] multiplicadores = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string semente;

            do
            {
                semente = random.Next(1, 999999999).ToString().PadLeft(9, '0');
            }
            while (
                semente == "000000000"
                || semente == "111111111"
                || semente == "222222222"
                || semente == "333333333"
                || semente == "444444444"
                || semente == "555555555"
                || semente == "666666666"
                || semente == "777777777"
                || semente == "888888888"
                || semente == "999999999"
            );

            for (int i = 1; i < multiplicadores.Count(); i++)
                soma += int.Parse(semente[i - 1].ToString()) * multiplicadores[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente += resto;
            soma = 0;

            for (int i = 0; i < multiplicadores.Count(); i++)
                soma += int.Parse(semente[i].ToString()) * multiplicadores[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            semente = semente + resto;

            semente = Convert.ToUInt64(semente).ToString(@"000\.000\.000\-00");

            lbl_resultado.Text = semente;

            //Refresh de cores e texto da label quando um novo cpf é gerado
            lbl_resultado.TextColor = Color.White;
            lbl_info.Text = "-";
            lbl_info.TextColor = Color.White;
        }

        private async void btn_copy_Clicked(object sender, EventArgs e)
        {
            //Displayalert para confirmação de copia para a area de transferencia
            if (await DisplayAlert("Atenção", "você realmente deseja copiar o CPF gerado?", "yes", "no"))
            {
                //metodo nativo de cópia
                await Clipboard.SetTextAsync(lbl_resultado.Text);
            }
            else
            {
                await DisplayAlert("Atenção", "Você não quis copiar o cpf gerado", "Prosseguir para a tela inicial");
            }
        }

        private void btn_valida_Clicked(object sender, EventArgs e)
        {
            //Instanciamento da classe de validação
            ValidaCPF vldcpf = new ValidaCPF();

            //string para tratamento de validação atraves do label de resultado
            string valor = lbl_resultado.Text;


           //Possivel tratamento de erro em caso de entrada = null na string valor
           // if (CPFGenerator.ValidaCPF.isCpf( valor = null)){DisplayAlert("Atenção", "Você não gerou um cpf para que seja validado", "Prosseguir para a tela inicial");}

            //if de validação caso o cpf seja valido
            if (CPFGenerator.ValidaCPF.isCpf(valor))
            {
                lbl_resultado.TextColor = Color.Green;
                lbl_info.Text = "Válido";
                lbl_info.TextColor = Color.Green;
            }
            //if de validação caso o cpf seja invalido
            else
            {
                lbl_resultado.TextColor = Color.Red;
                lbl_info.Text = "Inválido";
                lbl_info.TextColor = Color.Red;
            }
        }
    }

 }
