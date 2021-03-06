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
    public interface I{{ .Model.Name | CamelCase }}Repository
    {
        {{- if and $filter (not (NodeOption .Model "singular")) }}
        public System.Collections.Generic.List<{{ .Model.Name | CamelCase }}> Get{{ .Model.Name | Plural }}List({{ $filter | CamelCase }} filter);
        {{ end }}

        {{- if NodeOption .Model "singularFromFilter" }}
        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name }}({{ $filter | CamelCase }} filter);
        {{- else }}
        {{- $pk := .Model.PKProp }}
        {{- if $pk }}
        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name }}({{ $pk.Type }} {{ $pk.Name }});
        {{- end }}
        {{- end }}
        {{- if not (NodeOption .Model "notPost") }}
        public long Post{{ .Model.Name | CamelCase }}({{ $pageItem }} model);
        {{- end }}
    }
}
