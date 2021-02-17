using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNews.Models
{
    public class TabSelectionVisbility
    {
        private readonly string NoCssClass = string.Empty;
        private readonly string NoDisplayCssClass = "d-none";

        public string ActiveTabCss { get; set; }

        public bool DisplayFeed { get; set; }
        public string DisplayFeedCss => DisplayFeed ? NoCssClass : NoDisplayCssClass;
        public string FeedActiveCss => DisplayFeed ? ActiveTabCss : NoCssClass;

        public bool DisplaySaved { get; set; }
        public string DisplaySavedCss => DisplaySaved ? NoCssClass : NoDisplayCssClass;
        public string SavedActiveCss => DisplaySaved ? ActiveTabCss : NoCssClass;

        public bool DisplaySettings { get; set; }
        public string DisplaySettingsCss => DisplaySettings ? NoCssClass : NoDisplayCssClass;
        public string SettingsActiveCss => DisplaySettings ? ActiveTabCss : NoCssClass;

        public void SetAllVisibility(bool value)
        {
            DisplayFeed = value;
            DisplaySaved = value;
            DisplaySettings = value;
        }
    }
}
