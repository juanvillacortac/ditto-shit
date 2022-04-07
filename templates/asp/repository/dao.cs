{{- define "model"}}
public class {{ .Name }}Dao
{
    {{- range .Props}}
    [JsonProperty("{{ NodeOptionOr . "alt" (.Name | CamelCase) }}")]
    {{ $another := Model .Type -}}
    {{- if $another -}}
    public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}Dao>{{ else }}{{ .Type }}Dao{{ end }} {{ .Name | CamelCase }} { get; set; }{{ if .IsArray }} = new System.Collections.Generic.List<{{ .Type }}Dao>();{{ end }}
    {{- else -}}
    public {{ if .IsArray }}System.Collections.Generic.List<{{ .Type }}>{{ else }}{{ .Type }}{{ end }} {{ .Name | CamelCase }} { get; set; }{{ if .IsArray }} = new System.Collections.Generic.List<{{ .Type }}>();{{ end }}
    {{- end -}}
    {{- end }}
}
{{- end -}}
{{- $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
using Newtonsoft.Json;
{{ range $key, $dep := ModelDeps .Model -}}
using {{ $namespace }}.Repository.Dao.{{ $dep.Name }};
{{ end }}
namespace {{ $namespace }}.Repository.Dao.{{ .Model.Name }}
{
    {{- ExecTmpl "model" .Model | indent 4 }}
    {{- if $filter }}{{with Model $filter}}{{if NodeOption . "embedFilter"}}
    {{ ExecTmpl "model" . | indent 4}}
    {{- end }}{{ end }}{{ end }}
}