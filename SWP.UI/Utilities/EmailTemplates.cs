using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Utilities
{
    public static class EmailTemplates
    {

        public static string GetConfirmationTempalte(string url)
        {
            return $"<div><hr/><h4 style = padding-bottom: 5pex;> Szanowni Państwo,</h4 ><p> Otrzymaliśmy prośbę rejestracji nowego użytkownika w portalu Systemy Wspomagania Pracy.</p><p> Klikając w poniższy link potwierdzą Państwo chęć rejestracji w portalu.</p><p style = padding: 20px 0px 20px 0px;><a style = color: #0067b8; font-size: 14px; href =\"{url}\">Potwierdzam rejestrację.</a></p><p> Jeśli to nie Państwo skorzystali z formularza rejestracji na naszym portalu prosimy o zignorowanie tej wiadomości.</p><p> Wiadomość została wygenerowana automatycznie.Prosimy na nią nie odpowiadać.</p><p> Z poważaniem,</p><p> Zespół SWP </p><hr/></div>";
        }

        public static string GetPasswordResetTempalte(string url)
        {
            return $"<div><hr/><h4 style = padding-bottom: 5pex;> Szanowny Użytkowniku,</h4 ><p> Otrzymaliśmy prośbę resetu hasła do Twojego konta na portalu Systemy Wspomagania Pracy.</p><p> W celu zresetowania hasła prosimy kliknąć w poniższy link.</p><p style = padding: 20px 0px 20px 0px;><a style = color: #0067b8; font-size: 14px; href =\"{url}\">Zmiana hasła.</a></p><p> Jeśli nie korzystałeś z funkcji 'Zapomniałeś hasła?' na naszym portalu prosimy o zignorowanie tej wiadomości.</p><p> Wiadomość została wygenerowana automatycznie. Prosimy na nią nie odpowiadać.</p><p> Z poważaniem,</p><p> Zespół SWP </p><hr/></div>";
        }
    }
}
