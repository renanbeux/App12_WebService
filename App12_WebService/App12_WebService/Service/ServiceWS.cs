using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using App12_WebService.Model;

namespace App12_WebService.Service
{
    public class ServiceWS
    {
        private static string Enderecobase = "http://ws.spacedu.com.br/xf2018/rest/api";

        public static Usuario GetUsuario(Usuario usuario)
        {
            HttpClient requisicao = new HttpClient();
            HttpResponseMessage resposta = requisicao.PostAsync("Url", "parametros").GetAwaiter().GetResult();

            if(resposta.StatusCode == HttpStatusCode.OK)
            {
                //TODO - Deserealizar, retornar no met e salvar como login.
            }

            return null;
        }
    }
}
