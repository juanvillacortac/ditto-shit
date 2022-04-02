{{- $filter := NodeOption .Model "filter" -}}
{{ $namespace := NodeOption .Root "cs_namespace" -}}
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
        public long Post{{ .Model.Name | CamelCase }}(model: {{ .Model.Name }}, long idUser, long idBusiness);
        {{- end }}
    }
}
