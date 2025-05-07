using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CookingBook.AppData
{
    internal class AppFrame
    {
        public static Frame frmMain;
    }
    public partial class Recipes
    {
        public string CurrentPhoto { get {
                if (String.IsNullOrEmpty(Image) || String.IsNullOrWhiteSpace(Image))
                        return @"\Images\picture.jpg";
                else return @"\Images\"+Image;
            } }

    }
}