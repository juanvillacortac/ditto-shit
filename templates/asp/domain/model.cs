{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
{{ range $key, $dep := ModelDeps .Model -}}
using {{ $namespace }}.Domain.{{ $dep.Name | CamelCase }}Domain;
{{ end }}
namespace {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain
{
    public class {{ .Model.Name }}
    {
        {{- range .Model.Props}}
        {{ $fmtName := .Name | CamelCase -}}
        public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}>{{ else }}{{ .Type }}{{ end }} {{ $fmtName }} { get; set; }{{ if .IsArray }} = new System.Collections.Generic.List<{{ .Type }}>();{{ end }}
        {{- end }}
    }
    {{- if $filter }}{{with Model $filter}}{{if NodeOption . "embedFilter"}}

    public class {{ .Name }}
    {
        {{- range .Props}}
        {{ $another := Model .Type -}}
        public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}>{{ else }}{{ .Type }}{{ end }} {{ .Name | CamelCase }} { get; set; }
        {{- end }}
    }
    {{- end }}{{ end }}{{ end }}
}