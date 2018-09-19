using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using App12_WebService.Model;
using Newtonsoft.Json;

namespace App12_WebService.Service
{
    public class ServiceWS
    {
        private static string Enderecobase = "http://ws.spacedu.com.br/xf2018/rest/api";

        public static Usuario GetUsuario(Usuario usuario)
        {
            var URL = Enderecobase + "/usuario";

            //QueryString ex: ?q=Futebol&tipo=imagem
            //StringContent param = new StringContent(string.Format("?nome={0}&password={1}",usuario.nome,usuario.password));
            //Este de baixo faz a mesma coisa, só que já formata automático
            FormUrlEncodedContent param = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("nome",usuario.nome),
                new KeyValuePair<string,string>("password",usuario.password)
            });

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.PostAsync(URL, param).GetAwaiter().GetResult();

            if(resposta.StatusCode == HttpStatusCode.OK)
            {
                string conteudo = resposta.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Usuario>(conteudo);
            }

            return null;
        }

        public static List<Chat> GetChats()
        {
            var URL = Enderecobase + "/chats";
            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.GetAsync(URL).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
            {
                string conteudo = resposta.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (conteudo.Length > 2) // quando vem em branco, ele retorna com 1 registro, por isso o 2
                {
                    List<Chat> lista = JsonConvert.DeserializeObject<List<Chat>>(conteudo);
                    return lista;
                }
                else
                    return null;
            }
            else
               return null;
        }

        public static bool InsertChat(Chat chat)
        {
            var URL = Enderecobase + "/chat";
                        
            FormUrlEncodedContent param = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("nome",chat.nome)
            });

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.PostAsync(URL, param).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
                return true;
            else
               return false;
        }

        public static bool RenomearChat(Chat chat)
        {
            var URL = Enderecobase + "/chat/" + chat.id;

            FormUrlEncodedContent param = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("nome",chat.nome)
            });

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.PutAsync(URL, param).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public static bool DeletarChat(Chat chat)
        {
            var URL = Enderecobase + "/chat/delete/" + chat.id;

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.DeleteAsync(URL).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public static List<Mensagem> GetMensagensChat(Chat chat)
        {
            var URL = Enderecobase + "/chat/" + chat.id + "/msg" ;

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.GetAsync(URL).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
            {
                string conteudo = resposta.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (conteudo.Length > 2)
                {
                    List<Mensagem> lista = JsonConvert.DeserializeObject<List<Mensagem>>(conteudo);
                    return lista;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public static bool InsertMensagem(Mensagem mensagem)
        {
            var URL = Enderecobase + "/chat/" + mensagem.id_chat + "/msg";

            FormUrlEncodedContent param = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("mensagem",mensagem.mensagem),
                new KeyValuePair<string,string>("id_usuario",mensagem.id_usuario.ToString())
            });

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.PostAsync(URL, param).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public static bool DeletarMensagem(Mensagem mensagem)
        {
            var URL = Enderecobase + "/chat/" + mensagem.id_chat + "/delete/" + mensagem.id;

            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.DeleteAsync(URL).GetAwaiter().GetResult();

            if (resposta.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }
    }
}
