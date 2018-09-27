using App12_WebService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using App12_WebService.Model;

namespace App12_WebService.ViewModel
{
    public class RenomearChatViewModel : INotifyPropertyChanged
    {
        private Chat chat;

        private string _txtRenomear;
        public string txtRenomear
        {
            get { return _txtRenomear; }
            set { _txtRenomear = value; OnPropertyChanged("txtRenomear"); }
        }
        private string _mensagem;
        public string mensagem
        {
            get { return _mensagem; }
            set { _mensagem = value; OnPropertyChanged("mensagem"); }
        }

        public Command SalvarCommand { get; set; }

        public RenomearChatViewModel(Chat ch)
        {
            chat = ch;
            txtRenomear = ch.nome;
            SalvarCommand = new Command(Salvar);
        }

        private void Salvar()
        {
            Chat chatNovo = chat;
            mensagem = chat.id + " - " + chat.nome;

            chatNovo.nome = txtRenomear;
            if (ServiceWS.RenomearChat(chatNovo))
                mensagem += " - Passou";
            else
                mensagem += " - Erro";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
