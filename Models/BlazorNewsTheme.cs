using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNews.Models
{
    public class BlazorNewsTheme
    {
        public string TextColor { get; set; }
        public string MainBackground { get; set; }
        public string CardBackground { get; set; }
        public string ActiveTab { get; set; }

        /// <summary>
        /// true for dark, false for light
        /// </summary>
        public void ToggleTheme(bool isDark)
        {
            if (isDark)
            {
                MainBackground = "main-dark-background";
                CardBackground = "card-background-dark";
                TextColor = "text-light";
                ActiveTab = "active-pill-dark";
            }
            else
            {
                MainBackground = "bg-light";
                CardBackground = "bg-light";
                TextColor = "text-dark";
                ActiveTab = "active-pill-light";
            }
        }
    }
}
