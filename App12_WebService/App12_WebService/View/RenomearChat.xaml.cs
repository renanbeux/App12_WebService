using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App12_WebService.Model;

namespace App12_WebService.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RenomearChat : ContentPage
	{
		public RenomearChat (Chat chat)
		{
			InitializeComponent ();

            BindingContext = new ViewModel.RenomearChatViewModel(chat);
		}
	}
}