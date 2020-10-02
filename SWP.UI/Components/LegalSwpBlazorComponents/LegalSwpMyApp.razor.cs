﻿using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpMyApp
    {
        [Parameter]
        public LegalBlazorApp App { get; set; }
        [Parameter]
        public EventCallback<LegalBlazorApp> AppChanged { get; set; }

        private string FormatAsPLN(object value) => ((double)value).ToString(CultureInfo.CreateSpecificCulture("pl"));

        private string FormatAsTime(object value)
        {
            var stringValue = value.ToString();

            if (stringValue.Contains('.') ||
                stringValue.Contains(','))
            {
                if (value.ToString().Split('.', ',')[1].Length == 1)
                {
                    return $"{value.ToString().Split('.', ',')[0]}:{value.ToString().Split('.', ',')[1]}0";
                }
                else
                {
                    return $"{value.ToString().Split('.', ',')[0]}:{value.ToString().Split('.', ',')[1]}";
                }
            }
            else
            {
                return stringValue;
            }
        }

        private string RelatedUsersFilterValue = "";


    }
}