﻿namespace Tasks.Api;

public class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Tasks
    {
        private const string Base = $"{ApiBase}/tasks";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
}
