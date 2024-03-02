﻿using IBM.Watson.LanguageTranslator.v3;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.LanguageTranslator.v3.Model;

namespace Translate.Services;

public class Translator
{
    private LanguageTranslatorService _translator;

    public IEnumerable<Language> Languages =>
        _translator.ListLanguages().Result._Languages;

    public string LanguagesJson => _translator.ListLanguages().Response;

    public Translator(IConfiguration config)
    {
        _translator = new("2018-05-01", new IamAuthenticator(
            apikey: config["Translator:ApiKey"]));
        _translator.SetServiceUrl(config["Translator:Url"]);
        _translator.WithHeader("X-Watson-Learning-Opt-Out", "true");
    }

    public TranslationResult? Translate(string text, string sourceId, string targetId)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(targetId))
            return null;

        var result = _translator.Translate(
            text: new() { text },
            source: sourceId,
            target: targetId).Result;

        return result;
    }

    public string Identify(string text) => _translator.Identify(text).Response;
}
