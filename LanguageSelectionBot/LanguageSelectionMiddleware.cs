﻿using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageSelectionBot
{
    public class LanguageSelectionMiddleware : IMiddleware
    {
        private readonly UserState userState;

        public LanguageSelectionMiddleware(UserState userState)
        {
            this.userState = userState;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            var prop = userState.CreateProperty<string>("languagePreference");
            string language = await prop.GetAsync(turnContext);
            if (!string.IsNullOrEmpty(language))
            {
                string newLocale =  language switch
                {
                    "Spanish" => "es",
                    // default to english
                    _ => "en-us"
                };

                turnContext.Activity.Locale = newLocale;
                ((TurnContext)turnContext).Locale = newLocale;
            }

            await next(cancellationToken);
        }
    }
}
