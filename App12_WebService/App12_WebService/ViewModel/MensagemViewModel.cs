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
        private Page page;
        private StackLayout SL;
        private ScrollView BarraRolagem;
        private Chat chat;
        private List<Mensagem> _mensagens;
        public List<Mensagem> Mensagens
        {
            get { return _mensagens; }
            set {
                    _mensagens = value;
                    OnPropertyChanged("Mensagens");
                    /*if (value != null)
                    {
                        ShowOnScreen();
                    }*/
                }
        }
        private List<Mensagem> _mensagensNovas;
        public List<Mensagem> MensagensNovas
        {
            get { return _mensagensNovas; }
            set
            {
                _mensagensNovas = value;
                OnPropertyChanged("MensagensNovas");
            }
        }

        private string _txtMensagem;
        public string TxtMensagem
        {
            get { return _txtMensagem; }
            set
            {
                _txtMensagem = value;
                OnPropertyChanged("TxtMensagem");
            }
        }
        private string _msgErro;
        public string msgErro
        {
            get { return _msgErro; }
            set
            {
                _msgErro = value;
                OnPropertyChanged("msgErro");
            }
        }
        
        public Command BtnEnviarCommand { get; set; }
        public Command AtualizarCommand { get; set; }
        public Command RenomearCommand { get; set; }
        public Command ExcluirCommand { get; set; }

        public MensagemViewModel(Chat chat, StackLayout SLMensagemContainer, Page pag, ScrollView scroll)
        {
            page = pag;
            this.chat = chat;
            SL = SLMensagemContainer;
            BarraRolagem = scroll;
            Mensagens = ServiceWS.GetMensagensChat(chat);
            ShowOnScreen();
            BtnEnviarCommand = new Command(BtnEnviar);
            AtualizarCommand = new Command(Atualizar);
            RenomearCommand = new Command(Renomear);
            ExcluirCommand = new Command(Excluir);
            
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                Atualizar();
                return true;
            });
        }

        private void Atualizar()
        {
            MensagensNovas = ServiceWS.GetMensagensChat(chat);
            
            if ((Mensagens != null) && (MensagensNovas != null) && (Mensagens.Count != MensagensNovas.Count))
            {
                Mensagens = MensagensNovas;
                ShowOnScreen();                                
            }
        }
        private void Renomear()
        {
            ((NavigationPage)App.Current.MainPage).Navigation.PushAsync(new View.RenomearChat(chat));
        }
        private void Excluir()
        {
            ShowMsgConfirma("Apagar chat?");
            if (ServiceWS.DeletarChat(chat))
                ShowMsg("Chat deletado com sucesso!");
            else
                ShowMsg("Erro ao deletar Chat!");
            
            ((NavigationPage)App.Current.MainPage).Navigation.PopAsync();
        }

        private async void ShowMsgConfirma(string text)
        {
            await page.DisplayAlert("AVISO", text, "VAI FUNDO", "CANCELAR");
        }

        private async void ShowMsg(string text)
        {
            await page.DisplayAlert("AVISO", text, "OK");
        }

        private void BtnEnviar()
        {
            var msg = new Mensagem()
            {
                id_usuario = UsuarioUtil.GetUsuarioLogado().id,
                mensagem = TxtMensagem,
                id_chat = chat.id
            };
            ServiceWS.InsertMensagem(msg);
            Atualizar();
            TxtMensagem = string.Empty;
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
            BarraRolagem.ScrollToAsync(SL, ScrollToPosition.End, false);
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
