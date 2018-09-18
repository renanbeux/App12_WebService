using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using App12_WebService.Model;
using App12_WebService.Service;
using Xamarin.Forms;
using System.Linq;
using App12_WebService.Util;

namespace App12_WebService.ViewModel
{
    public class MensagemViewModel:INotifyPropertyChanged
    {
        private StackLayout SL;
        private List<Mensagem> _mensagens;
        public List<Mensagem> Mensagens
        {
            get { return _mensagens; }
            set { _mensagens = value; OnPropertyChanged("Mensagens"); ShowOnScreen(); }
        }

        public MensagemViewModel(Chat chat, StackLayout SLMensagemContainer)
        {
            SL = SLMensagemContainer; 
            Mensagens = ServiceWS.GetMensagensChat(chat);
        }

        private void ShowOnScreen()
        {
            var usuario = UsuarioUtil.GetUsuarioLogado();
            SL.Children.Clear();
            foreach(var msg in Mensagens)
            {
                if (msg.usuario.id == usuario.id)
                    SL.Children.Add(CriarMensagemPropria(msg));
                else
                    SL.Children.Add(CriarMensagemOutrosUsuarios(msg));
            }
        }

        private Xamarin.Forms.View CriarMensagemPropria(Mensagem mensagem)
        {
            var layout = new StackLayout() { Padding = 5, BackgroundColor = Color.FromHex("#5ED055"), HorizontalOptions = LayoutOptions.End };
            var label = new Label() { Text = mensagem.mensagem, TextColor = Color.White };

            layout.Children.Add(label);
            return layout;
        }
        private Xamarin.Forms.View CriarMensagemOutrosUsuarios(Mensagem mensagem)
        {
            var labelNome = new Label() { Text=mensagem.usuario.nome, FontSize = 10, TextColor = Color.FromHex("#5ED055") };
            var labelMensagem = new Label() { Text=mensagem.mensagem, TextColor = Color.FromHex("#5ED055") };

            var SL = new StackLayout();
            SL.Children.Add(labelNome);
            SL.Children.Add(labelMensagem);

            var frame = new Frame() { BorderColor = Color.FromHex("#5ED055"), CornerRadius=0, HorizontalOptions = LayoutOptions.Start };
            frame.Content = SL;
            return frame;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
