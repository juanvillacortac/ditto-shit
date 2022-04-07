{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
{{ range $key, $dep := ModelDeps .Model -}}
using {{ $namespace }}.API.ViewModels.{{ $dep.Name }};
{{ end }}
namespace {{ $namespace }}.API.ViewModels.{{ .Model.Name | CamelCase }}
{
    public class {{ .Model.Name | CamelCase }}ViewModel
    {
        {{- range .Model.Props}}
        {{ $fmtName := .Name | CamelCase -}}
        {{ $another := Model .Type -}}
        {{- if $another -}}
        public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}ViewModel>{{ else }}{{ .Type }}Dao{{ end }} {{ .Name | CamelCase }} { get; set; }{{ if .IsArray }} = new System.Collections.Generic.List<{{ .Type }}ViewModel>();{{ end }}
        {{- else -}}
        public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}>{{ else }}{{ .Type }}{{ end }} {{ .Name | CamelCase }} { get; set; }{{ if .IsArray }} = new System.Collections.Generic.List<{{ .Type }}>();{{ end }}
        {{- end -}}
        {{- end }}
    }
    {{- if $filter }}{{with Model $filter}}{{if NodeOption . "embedFilter"}}

    public class {{ .Name }}ViewModel
    {
        {{- range .Props}}
        {{ $another := Model .Type -}}
        {{- if $another -}}
        public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}ViewModel>{{ else }}{{ .Type }}Dao{{ end }} {{ .Name | CamelCase }} { get; set; }
        {{- else -}}
        public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}>{{ else }}{{ .Type }}{{ end }} {{ .Name | CamelCase }} { get; set; }
        {{- end -}}
        {{- end }}
    }
    {{- end }}{{ end }}{{ end }}
}