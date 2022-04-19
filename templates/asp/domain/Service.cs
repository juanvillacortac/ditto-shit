{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
{{- $pageItem := (NodeOptionOr .Model "pageItem" .Model.Name) | CamelCase -}}
{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.Domain.{{ .Name | CamelCase }}Domain;
{{- end }}{{ end }}{{ end }}
{{- if ne $pageItem .Model.Name }}{{with Model $pageItem}}
using {{ $namespace }}.Domain.{{ $pageItem }}Domain;
{{- end }}{{ end }}
namespace {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain
{
    public class {{ .Model.Name | CamelCase }}Service : I{{ .Model.Name | CamelCase }}Service
    {
        private readonly I{{ .Model.Name | CamelCase }}Repository _repository;

        public {{ .Model.Name | CamelCase }}Service(I{{ .Model.Name | CamelCase }}Repository repository) =>
            this._repository = repository;
        {{ if and $filter (not (NodeOption .Model "singular")) }}
        public System.Collections.Generic.List<{{ .Model.Name | CamelCase }}> Get{{ .Model.Name | Plural }}List({{ $filter | CamelCase }} filter) =>
            this._repository.Get{{ .Model.Name | Plural }}List(filter);
        {{ end }}

        {{- if NodeOption .Model "singularFromFilter" }}
        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name | CamelCase }}({{ $filter | CamelCase }} filter) =>
            this._repository.Get{{ .Model.Name | CamelCase }}(filter);
        {{- else }}
        {{- $pk := .Model.PKProp }}
        {{- if $pk }}
        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name | CamelCase }}({{ $pk.Type }} {{ $pk.Name }}) =>
            this._repository.Get{{ .Model.Name | CamelCase }}({{ $pk.Name }});
        {{- end }}
        {{- end }}
        {{- if not (NodeOption .Model "notPost") }}
        public long Post{{ .Model.Name | CamelCase }}({{ $pageItem }} model) =>
            this._repository.Post{{ .Model.Name | CamelCase }}(model);
        {{- end }}
    }
}
