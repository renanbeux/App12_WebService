using System;
using System.Collections.Generic;
using System.Text;
using App12_WebService.Model;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace App12_WebService.Util
{
    public class UsuarioUtil
    {
        public static void SetUsuarioLogado(Usuario usuario)
        {
            App.Current.Properties["LOGIN"] = JsonConvert.SerializeObject(usuario);
        }
        public static Usuario GetUsuarioLogado()
        {
            if (App.Current.Properties.ContainsKey("LOGIN"))
            {
                return JsonConvert.DeserializeObject<Usuario>((string)App.Current.Properties["LOGIN"]);
            }
            else
                return null;
        }
    }
}
